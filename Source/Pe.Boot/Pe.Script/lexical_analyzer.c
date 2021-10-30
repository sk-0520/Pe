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
static struct
{
    TCHAR characters[MULTI_TOKEN_COUNT];
    /// <summary>
    /// MULTI_TOKEN_FIRST: characters[MULTI_TOKEN_FIRST] のみのトークン
    /// MULTI_TOKEN_SECOND: characters[MULTI_TOKEN_FIRST][MULTI_TOKEN_SECOND]のトークン
    /// </summary>
    TOKEN_KIND kinds[MULTI_TOKEN_COUNT];
} library__multi_tokens[] = {
    // +
    {
        .characters = { _T('+'), _T('=') },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_ADD_ASSIGN },
    },
    {
        .characters = { _T('+'), _T('+') },
        .kinds = { TOKEN_KIND_OP_PLUS, TOKEN_KIND_OP_INCREMENT },
    },
    // -
    {
        .characters = { _T('-'), _T('=') },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_SUB_ASSIGN },
    },
    {
        .characters = { _T('-'), _T('-') },
        .kinds = { TOKEN_KIND_OP_MINUS, TOKEN_KIND_OP_DECREMENT },
    },
    // *
    {
        .characters = { _T('*'), _T('=') },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_MUL_ASSIGN },
    },
    {
        .characters = { _T('*'), _T('/') },
        .kinds = { TOKEN_KIND_OP_STAR, TOKEN_KIND_COMMENT_BLOCK_END },
    },
    // /
    {
        .characters = { _T('/'), _T('/') },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_COMMENT_LINE },
    },
    {
        .characters = { _T('/'), _T('=') },
        .kinds = { TOKEN_KIND_OP_SLASH, TOKEN_KIND_OP_DIV_ASSIGN },
    },
    // =
    {
        .characters = { _T('='), _T('>') },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_LAMBDA },
    },
    {
        .characters = { _T('='), _T('=') },
        .kinds = { TOKEN_KIND_OP_ASSIGN, TOKEN_KIND_OP_EQUALS },
    },
    // <
    {
        .characters = { _T('<'), _T('=') },
        .kinds = { TOKEN_KIND_OP_LESS, TOKEN_KIND_OP_LESS_EQUAL },
    },
    // >
    {
        .characters = { _T('>'), _T('=') },
        .kinds = { TOKEN_KIND_OP_GREATER, TOKEN_KIND_OP_GREATER_EQUAL },
    },
};


static bool is_whitespace_character(TCHAR c)
{
    return c == _T(' ') || c == _T('\t');
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



static void analyze_line(size_t line_number, const TEXT* line, ANALYZE_DATA* analyze_data)
{
    TOKEN_RESULT* result = analyze_data->result;

    if (!line->length) {
        return;
    }

    //PRIMITIVE_LIST_TCHAR stock = new_primitive_list(PRIMITIVE_LIST_TYPE_TCHAR, line->length);

    size_t column_position = 0;
    while (column_position < line->length) {

        TCHAR current_character = line->value[column_position];
        if (is_whitespace_character(current_character)) {
            column_position += 1;
            continue;
        }

        TCHAR next_character = 0;
        if (column_position + 1 < line->length) {
            next_character = line->value[column_position + 1];
        }

        // 単一トークンと合わせ技のトークンを処理
        bool processed_multi_token = false;
        for (size_t i = 0; !processed_multi_token && i < sizeof(library__multi_tokens) / sizeof(library__multi_tokens[0]); i++) {
            if (current_character == library__multi_tokens[i].characters[MULTI_TOKEN_FIRST]) {
                if (next_character && next_character == library__multi_tokens[i].characters[MULTI_TOKEN_SECOND] && library__multi_tokens[i].kinds[MULTI_TOKEN_SECOND] != TOKEN_KIND_NONE) {
                    column_position += 2;
                    processed_multi_token = true;
                    add_token_kind(&result->token, library__multi_tokens[i].kinds[MULTI_TOKEN_SECOND], column_position, line_number);
                } else if (library__multi_tokens[i].kinds[MULTI_TOKEN_FIRST] != TOKEN_KIND_NONE) {
                    column_position += 1;
                    processed_multi_token = true;
                    add_token_kind(&result->token, library__multi_tokens[i].kinds[MULTI_TOKEN_FIRST], column_position, line_number);
                }
            }
        }
        if (processed_multi_token) {
            continue;
        }

        //switch (current_character) {
        //    case _T('+'):
        //        if (next_character) {
        //            if (next_character == _T('+')) {
        //                column_position += 2;
        //                add_token_kind(&result->token, TOKEN_KIND_OP_INCREMENT, column_position, line_number, file_path);
        //                continue;
        //            }
        //        }
        //        add_token_kind(&result->token, TOKEN_KIND_OP_PLUS, column_position, line_number, file_path);
        //        column_position += 1;
        //        continue;

        //    default:
        //        break;
        //}

        column_position += 1;

    }
}

static bool foreach_analyze_line(const void* value, size_t index, size_t length, void* data)
{
    TEXT* line = (TEXT*)value;
    analyze_line(index + 1, line, data);
    return true;
}

void add_index(size_t* current_index, size_t* column_position, size_t add_value)
{
    *current_index += add_value;
    *column_position += add_value;
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
            if (current_character == library__multi_tokens[i].characters[MULTI_TOKEN_FIRST]) {
                if (next_character && next_character == library__multi_tokens[i].characters[MULTI_TOKEN_SECOND] && library__multi_tokens[i].kinds[MULTI_TOKEN_SECOND] != TOKEN_KIND_NONE) {
                    processed_multi_token = true;
                    add_token_kind(&result->token, library__multi_tokens[i].kinds[MULTI_TOKEN_SECOND], column_position, line_number);
                    add_index(&current_index, &column_position, 2);
                } else if (library__multi_tokens[i].kinds[MULTI_TOKEN_FIRST] != TOKEN_KIND_NONE) {
                    processed_multi_token = true;
                    add_token_kind(&result->token, library__multi_tokens[i].kinds[MULTI_TOKEN_FIRST], column_position, line_number);
                    add_index(&current_index, &column_position, 1);
                }
            }
        }
        if (processed_multi_token) {
            continue;
        }

        current_index += 1;
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
