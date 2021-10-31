#include "lexical_analyzer.h"
#include "../Pe.Library/primitive_list.h"
#include "../Pe.Library/debug.h"

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

static bool is_string_start(TCHAR c)
{
    return c == '\'' || c == '\"' || c == '`';
}


static bool is_whitespace_character(TCHAR c)
{
    return
        c == ' '
        || c == '\t'
        || c == '\v'
        || c == '\f'
#if UNICODE
        || c == _T('　')
#endif // UNICODE まぁこれしか考慮してないけどさ！
        ;
}

static bool is_newline_character(TCHAR c)
{
    return c == '\r' || c == '\n';
}



static void add_index(size_t* current_index, size_t* column_position, size_t add_value)
{
    *current_index += add_value;
    *column_position += add_value;
}

static bool is_comment(TOKEN_KIND kind)
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

/// <summary>
/// 2文字構成エスケープシーケンス(\c)に対する出力を取得。
/// </summary>
/// <param name="target_character"></param>
/// <returns>該当しない場合は 0 。</returns>
static TCHAR get_simple_escape_sequence(TCHAR target_character)
{
    switch (target_character) {
        // 独断でよく使いそうなの
        case '\\':
            return '\\';
        case 'r':
            return '\r';
        case 'n':
            return '\n';
        case '\"':
            return '\"';
        case '\'':
            return '\'';
        case 't':
            return '\t';

            // あんま使わなさそうなの
        case 'a':
            return '\a';
        case 'b':
            return '\b';
        case 'f':
            return '\f';
        case 'v':
            return '\v';

        default:
            return 0;
    }
}

/// <summary>
/// 文字列を読み込み。
/// 呼ばれる時点で最初の文字は文字列トークン判定済みなので初回の次文字読み込みは絶対に成功する。
/// </summary>
/// <param name="token_result">結果格納</param>
/// <param name="source">ソース全体</param>
/// <param name="start_index">文字列トークンとしての開始点</param>
/// <returns>読み込み成功後に飛ばす長さ(start_indexからの相対位置)。0の場合は失敗しているので後続不要。</returns>
static size_t read_string_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, size_t column_position, size_t line_number)
{
    assert(token_result);

    TCHAR string_type = source->value[start_index];
    size_t current_index = start_index + 1;
    size_t read_length = 0;
    TOKEN_KIND string_token_kind = string_type == '\''
        ? TOKEN_KIND_LITERAL_SSTRING
        : string_type == '\"'
        ? TOKEN_KIND_LITERAL_DSTRING
        : string_type == '`'
        ? TOKEN_KIND_LITERAL_BSTRING
        : TOKEN_KIND_NONE
        ;
    assert(string_token_kind != TOKEN_KIND_NONE);

    PRIMITIVE_LIST_TCHAR character_list = new_primitive_list(PRIMITIVE_LIST_TYPE_TCHAR, 256);

    while (current_index < source->length) {
        TCHAR current_character = source->value[current_index];
        if (current_character == string_type) {
            // 文字列はここまで!
            TEXT word = wrap_text_with_length(reference_list_tchar(&character_list), character_list.length, false);
            add_token_word(&token_result->token, string_token_kind, &word, column_position, line_number);
            read_length = current_index + 1;
            break;
        }

        TCHAR next_character = 0;
        if (current_index + 1 < source->length) {
            next_character = source->value[current_index + 1];
        }

        switch (string_token_kind) {
            case TOKEN_KIND_LITERAL_SSTRING:
                // ' の場合は \' のみをエスケープ対象にする
                if (current_character == '\\' && next_character == '\'') {
                    current_index += 2;
                    push_list_tchar(&character_list, '\'');
                } else if (is_newline_character(current_character) || !next_character) {
                    add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_CLOSE_STRING, NULL, column_position, line_number);
                    goto EXIT;
                } else {
                    current_index += 1;
                    push_list_tchar(&character_list, current_character);
                }
                continue;

            case TOKEN_KIND_LITERAL_DSTRING:
                // " はやること多いいでぇ
                if (current_character == '\\' && next_character) {
                    TCHAR simple_escape = get_simple_escape_sequence(next_character);
                    if (simple_escape) {
                        current_index += 2;
                        push_list_tchar(&character_list, simple_escape);
                        continue;
                    }

                    add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_UNKNOWN_ESCAPE_SEQUENCE, NULL, column_position, line_number);
                    goto EXIT;
                } else if (is_newline_character(current_character) || !next_character) {
                    add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_CLOSE_STRING, NULL, column_position, line_number);
                    goto EXIT;
                } else {
                    current_index += 1;
                    push_list_tchar(&character_list, current_character);
                }
                continue;

            default:
                assert(false);
                break;
        }
    }

EXIT:
    free_primitive_list(&character_list);

    return read_length;
}

static void analyze_core(TOKEN_RESULT* token_result, const TEXT* source, ANALYZE_DATA* analyze_data)
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
        if (is_newline_character(current_character)) {
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

        // 空白無視
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

        // 文字列処理
        if (is_string_start(current_character)) {
            if (current_character == '`') {
                TEXT remark = wrap_text(_T("文字列 ` は未実装"));
                add_compile_result(&result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_IMPLEMENT, &remark, column_position, line_number);
                break;
            }
            if (!next_character) {
                add_compile_result(&result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_CLOSE_STRING, NULL, column_position, line_number);
                break;
            }
            size_t read_length = read_string_token(result, source, current_index, column_position, line_number);
            if (!read_length) {
                break;
            }
            current_index += read_length;
            continue;
        }

        // 数値処理
        if (is_number_start(current_character)) {
            size_t read_length = read_number_token(result, source, current_index, column_position, line_number);
            assert(read_length);
            current_index += read_length;
            continue;
        }

        current_index += 1;
    }

    if (last_token_kind == TOKEN_KIND_COMMENT_BLOCK_BEGIN) {
        TOKEN* token = peek_object_list(&result->token);
        add_compile_result(&result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_CLOSE_COMMENT, NULL, token->position.column_position, token->position.line_number);
    }
}

TOKEN_RESULT RC_HEAP_FUNC(analyze, const TEXT* file_path, const TEXT* source, const PROJECT_SETTING* setting)
{
    assert(setting);

    TOKEN_RESULT token_result = {
        .file_path = (TEXT*)file_path,
        .token = RC_HEAP_CALL(create_object_list, sizeof(TOKEN), 1024, compare_object_list_value_null, free_object_list_value_null),
        .result = RC_HEAP_CALL(create_object_list, sizeof(COMPILE_RESULT), OBJECT_LIST_DEFAULT_CAPACITY_COUNT, compare_object_list_value_null, free_object_list_value_null),
    };

    if (!source || !source->length) {
        return token_result;
    }

    ANALYZE_DATA analyze_data = {
        .file_path = (TEXT*)file_path,
        .result = &token_result,
        .setting = (PROJECT_SETTING*)setting, //TODO: とりあえずの回避。Cって構造体メンバにconst使えんの？
    };
    analyze_core(&token_result, source, &analyze_data);

    return token_result;
}

static bool free_token_result_token(const void* value, size_t index, size_t length, void* data)
{
    TOKEN* token = (TOKEN*)value;
    if (token->type == TOKEN_VALUE_TYPE_STRING) {
        free_text(&token->value.word);
    }
    return true;
}

static bool free_token_result_result(const void* value, size_t index, size_t length, void* data)
{
    COMPILE_RESULT* cr = (COMPILE_RESULT*)value;
    free_text(&cr->remark);
    return true;
}

void RC_HEAP_FUNC(free_token_result, TOKEN_RESULT* token_result)
{
    foreach_object_list(&token_result->token, free_token_result_token, NULL);
    foreach_object_list(&token_result->result, free_token_result_result, NULL);
    free_object_list(&token_result->token);
    free_object_list(&token_result->result);
}
