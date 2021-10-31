#include "lexical_analyzer.h"
#include "../Pe.Library/primitive_list.h"

typedef struct tag_ANALYZE_DATA
{
    TEXT* file_path;
    TOKEN_RESULT* result;
    PROJECT_SETTING* setting;
} ANALYZE_DATA;

#define MULTI_TOKEN_FIRST (0)
#define MULTI_TOKEN_SECOND (1)
#define MULTI_TOKEN_COUNT (2)
static struct tag_MULTI_TOKEN
{
    TCHAR characters[MULTI_TOKEN_COUNT];
    /// <summary>
    /// MULTI_TOKEN_FIRST: characters[MULTI_TOKEN_FIRST] のみのトークン
    /// MULTI_TOKEN_SECOND: characters[MULTI_TOKEN_FIRST][MULTI_TOKEN_SECOND]のトークン
    /// </summary>
    TOKEN_KIND kinds[MULTI_TOKEN_COUNT];
    /// <summary>
    /// 真の場合にコメント中は無視する
    /// </summary>
    bool skip_comments[MULTI_TOKEN_COUNT];
} library__multi_tokens[] = {
    // +
    {
        .characters = { '+', '=' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_ADD_ASSIGN },
        .skip_comments = { true, true },
    },
    {
        .characters = { '+', '+' },
        .kinds = { TOKEN_KIND_OP_PLUS, TOKEN_KIND_OP_INCREMENT },
        .skip_comments = { true, true },
    },
    // -
    {
        .characters = { '-', '=' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_SUB_ASSIGN },
        .skip_comments = { true, true },
    },
    {
        .characters = { '-', '-' },
        .kinds = { TOKEN_KIND_OP_MINUS, TOKEN_KIND_OP_DECREMENT },
        .skip_comments = { true, true },
    },
    // *
    {
        .characters = { '*', '=' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_MUL_ASSIGN },
        .skip_comments = { true, true },
    },
    {
        .characters = { '*', '/' },
        .kinds = { TOKEN_KIND_OP_STAR, TOKEN_KIND_COMMENT_BLOCK_END },
        .skip_comments = { true, false },
    },
    // /
    {
        .characters = { '/', '/' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_COMMENT_LINE },
        .skip_comments = { false, false },
    },
    {
        .characters = { '/', '*' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_COMMENT_BLOCK_BEGIN },
        .skip_comments = { false, false },
    },
    {
        .characters = { '/', '=' },
        .kinds = { TOKEN_KIND_OP_SLASH, TOKEN_KIND_OP_DIV_ASSIGN },
        .skip_comments = { false, false },
    },
    // %
    {
        .characters = { '%', '=' },
        .kinds = { TOKEN_KIND_OP_PERCENT, TOKEN_KIND_OP_REM_ASSIGN },
        .skip_comments = { false, false },
    },
    // =
    {
        .characters = { '=', '>' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_LAMBDA },
        .skip_comments = { false, false },
    },
    {
        .characters = { '=', '=' },
        .kinds = { TOKEN_KIND_OP_ASSIGN, TOKEN_KIND_OP_EQUALS },
        .skip_comments = { false, false },
    },
    // <
    {
        .characters = { '<', '=' },
        .kinds = { TOKEN_KIND_OP_LESS, TOKEN_KIND_OP_LESS_EQUAL },
        .skip_comments = { false, false },
    },
    // >
    {
        .characters = { '>', '=' },
        .kinds = { TOKEN_KIND_OP_GREATER, TOKEN_KIND_OP_GREATER_EQUAL },
        .skip_comments = { false, false },
    },
    // !
    {
        .characters = { '!', '=' },
        .kinds = { TOKEN_KIND_OP_EXCLAMATION, TOKEN_KIND_OP_NOT_EQUALS },
        .skip_comments = { false, false },
    },
    // &
    {
        .characters = { '&', '&' },
        .kinds = { TOKEN_KIND_OP_AMPERSAND, TOKEN_KIND_OP_AND },
        .skip_comments = { false, false },
    },
    // |
    {
        .characters = { '|', '|' },
        .kinds = { TOKEN_KIND_OP_VERTICALBAR, TOKEN_KIND_OP_OR },
        .skip_comments = { false, false },
    },
};

static struct tag_SINGLE_CHAR_TOKEN
{
    TCHAR character;
    TOKEN_KIND kind;
} library__single_character_tokens[] = {
    {
        .character = ',',
        .kind = TOKEN_KIND_OP_COMMA,
    },
    {
        .character = '.',
        .kind = TOKEN_KIND_OP_DOT,
    },
    {
        .character = ';',
        .kind = TOKEN_KIND_OP_SEMICOLON,
    },
    {
        .character = ':',
        .kind = TOKEN_KIND_OP_COLON,
    },
    {
        .character = '?',
        .kind = TOKEN_KIND_OP_QUESTION,
    },
    {
        .character = '\\',
        .kind = TOKEN_KIND_OP_BACKSLASH,
    },
    {
        .character = '~',
        .kind = TOKEN_KIND_OP_TILDE,
    },
    {
        .character = '@',
        .kind = TOKEN_KIND_OP_AT,
    },
    {
        .character = '$',
        .kind = TOKEN_KIND_OP_DOLLAR,
    },
    {
        .character = '#',
        .kind = TOKEN_KIND_OP_HASH,
    },
    {
        .character = '(',
        .kind = TOKEN_KIND_OP_LPAREN,
    },
    {
        .character = ')',
        .kind = TOKEN_KIND_OP_RPAREN,
    },
    {
        .character = '{',
        .kind = TOKEN_KIND_OP_LBRACE,
    },
    {
        .character = '}',
        .kind = TOKEN_KIND_OP_RBRACE,
    },
    {
        .character = '[',
        .kind = TOKEN_KIND_OP_LBRACKET,
    },
    {
        .character = ']',
        .kind = TOKEN_KIND_OP_RBRACKET,
    },
};

static bool is_whitespace_character(TCHAR c)
{
    return c == _T(' ') || c == _T('\t');
}

static bool is_string_start(TCHAR c)
{
    return c == '\'' || c == '\"' || c == '`';
}


static void add_token_kind(OBJECT_LIST* tokens, TOKEN_KIND kind, size_t column_position, size_t line_number)
{
    TOKEN token = {
        .kind = kind,
        .position = {
            .column_position = column_position,
            .line_number = line_number,
    },
    .word = create_invalid_text()
    };

    push_object_list(tokens, &token);
}

static void add_compile_result(OBJECT_LIST* compile_results, COMPILE_RESULT_KIND kind, COMPILE_CODE code, TEXT remark, size_t column_position, size_t line_number)
{
    COMPILE_RESULT compile_result = {
        .stage = COMPILE_STAGE_LEX,
        .kind = kind,
        .code = code,
        .remark = clone_text(&remark),
        .position = {
            .column_position = column_position,
            .line_number = line_number,
        },
    };

    push_object_list(compile_results, &compile_result);
}

void add_index(size_t* current_index, size_t* column_position, size_t add_value)
{
    *current_index += add_value;
    *column_position += add_value;
}

bool is_comment(TOKEN_KIND kind)
{
    return
        kind == TOKEN_KIND_COMMENT_LINE
        ||
        kind == TOKEN_KIND_COMMENT_BLOCK_BEGIN
        ;
}

static struct tag_SINGLE_CHAR_TOKEN* find_single_character_token(TCHAR c)
{
    for (size_t i = 0; i < sizeof(library__single_character_tokens) / sizeof(library__single_character_tokens[0]); i++) {
        if (library__single_character_tokens[i].character == c) {
            return library__single_character_tokens + i;
        }
    }

    return NULL;
}

void analyze_core(TOKEN_RESULT* token_result, const TEXT* source, ANALYZE_DATA* analyze_data)
{
    TOKEN_RESULT* result = analyze_data->result;

    if (!source->length) {
        return;
    }

    //PRIMITIVE_LIST_TCHAR stock = new_primitive_list(PRIMITIVE_LIST_TYPE_TCHAR, line->length);

    size_t current_index = 0;
    size_t line_number = 1;
    size_t column_position = 0;
    TOKEN_KIND last_token_kind = TOKEN_KIND_NONE;

    while (current_index < source->length) {
        TCHAR current_character = source->value[current_index];
        TCHAR next_character = 0;
        if (current_index + 1 < source->length) {
            next_character = source->value[current_index + 1];
        }

        // 改行処理
        if (current_character == '\r' || current_character == '\n') {
            if (current_character == '\r' && next_character == '\n') {
                current_index += 2;
            } else {
                current_index += 1;
            }
            if (last_token_kind == TOKEN_KIND_COMMENT_LINE) {
                last_token_kind = TOKEN_KIND_NONE;
            }
            column_position = 0;
            line_number += 1;
            continue;
        }

        if (is_whitespace_character(current_character)) {
            add_index(&current_index, &column_position, 1);
            continue;
        }

        // 単一トークンと合わせ技のトークンを処理
        bool processed_multi_token = false;
        for (size_t i = 0; !processed_multi_token && i < sizeof(library__multi_tokens) / sizeof(library__multi_tokens[0]); i++) {
            const struct tag_MULTI_TOKEN* multi_token = library__multi_tokens + i;
            if (current_character == multi_token->characters[MULTI_TOKEN_FIRST]) {
                if (next_character && next_character == multi_token->characters[MULTI_TOKEN_SECOND] && multi_token->kinds[MULTI_TOKEN_SECOND] != TOKEN_KIND_NONE) {
                    processed_multi_token = true;
                    if (!is_comment(last_token_kind) || !multi_token->skip_comments[MULTI_TOKEN_SECOND]) {
                        add_token_kind(&result->token, multi_token->kinds[MULTI_TOKEN_SECOND], column_position, line_number);
                    }
                    add_index(&current_index, &column_position, 2);
                } else if (multi_token->kinds[MULTI_TOKEN_FIRST] != TOKEN_KIND_NONE) {
                    processed_multi_token = true;
                    if (!is_comment(last_token_kind) || !multi_token->skip_comments[MULTI_TOKEN_FIRST]) {
                        add_token_kind(&result->token, multi_token->kinds[MULTI_TOKEN_FIRST], column_position, line_number);
                    }
                    add_index(&current_index, &column_position, 1);
                }
            }
        }
        if (processed_multi_token) {
            TOKEN* token = peek_object_list(&result->token);
            last_token_kind = token->kind;

            continue;
        }

        // 以降はコメント中には処理しない
        if (is_comment(last_token_kind)) {
            current_index += 1;
            continue;
        }

        // 1文字トークンの処理
        struct tag_SINGLE_CHAR_TOKEN* single_char_token = find_single_character_token(current_character);
        if (single_char_token) {
            add_token_kind(&result->token, single_char_token->kind, column_position, line_number);
            add_index(&current_index, &column_position, 1);
            continue;
        }

        if (is_string_start(current_character)) {
            if (!next_character) {
                add_compile_result(&result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_CLOSE_STRING, wrap_text(_T("文字列が閉じられていない")), column_position, line_number);
                break;
            }
        }

        current_index += 1;
    }

    if (last_token_kind == TOKEN_KIND_COMMENT_BLOCK_BEGIN) {
        TOKEN* token = peek_object_list(&result->token);
        add_compile_result(&result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_CLOSE_COMMENT, wrap_text(_T("コメントが閉じられていない")), token->position.column_position, token->position.line_number);
    }
}

TOKEN_RESULT analyze(const TEXT* file_path, const TEXT* source, const PROJECT_SETTING* setting)
{
    assert(setting);

    TOKEN_RESULT token_result = {
        .file_path = (TEXT*)file_path,
        .token = create_object_list(sizeof(TOKEN), 1024, compare_object_list_value_null, free_object_list_value_null),
        .result = create_object_list(sizeof(COMPILE_RESULT), OBJECT_LIST_DEFAULT_CAPACITY_COUNT, compare_object_list_value_null, free_object_list_value_null),
    };

    if (!source || !source->length) {
        return token_result;
    }

    //OBJECT_LIST lines = split_newline_text(source);

    ANALYZE_DATA analyze_data = {
        .file_path = (TEXT*)file_path,
        .result = &token_result,
        .setting = (PROJECT_SETTING*)setting, //TODO: とりあえずの回避。Cって構造体メンバにconst使えんの？
    };
    analyze_core(&token_result, source, &analyze_data);
    //foreach_object_list(&lines, foreach_analyze_line, &analyze_data);

    //free_object_list(&lines);

    return token_result;
}

static bool free_token_result_token(const void* value, size_t index, size_t length, void* data)
{
    TOKEN* token = (TOKEN*)value;
    free_text(&token->word);
    return true;
}

void free_token_result(TOKEN_RESULT* token_result)
{
    foreach_object_list(&token_result->token, free_token_result_token, NULL);
    free_object_list(&token_result->token);
    free_object_list(&token_result->result);
}
