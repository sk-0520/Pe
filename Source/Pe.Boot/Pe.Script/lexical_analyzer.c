#include "lexical_analyzer.h"
#include "../Pe.Library/primitive_list.h"

typedef struct tag_ANALYZE_DATA
{
    TEXT* file_path;
    TOKEN_RESULT* result;
    PROJECT_SETTING* setting;
} ANALYZE_DATA;

size_t allocate_token_pairs(TOKEN_PAIR** pairs)
{
    TOKEN_PAIR token_pairs[] = {
        { TOKEN_KIND_OP_ASSIGN, new_text(_T("=")) },
    };

    size_t token_length = sizeof(token_pairs) / sizeof(token_pairs[0]);
    byte_t tokens_bytes = sizeof(token_pairs[0]) * token_length;
    TOKEN_PAIR* result = allocate_memory(tokens_bytes, false);
    copy_memory(pairs, result, tokens_bytes);

    return token_length;
}

bool free_token_pairs(TOKEN_PAIR pairs[], size_t length)
{
    if (!pairs) {
        return false;
    }

    for (size_t i = 0; i < length; i++) {
        free_text(&pairs[i].word);
    }

    return true;
}

static bool is_whitespace_character(TCHAR c)
{
    return c == _T(' ') || c == _T('\t');
}


static void add_token_kind(OBJECT_LIST* tokens, TOKEN_KIND kind, size_t column_index, size_t line_number, const TEXT* file_path)
{
    TOKEN token = {
        .kind = kind,
        .position = {
            .column_index = column_index,
            .line_number = line_number,
            .file_path = (TEXT*)file_path,
    },
    .word = create_invalid_text()
    };

    push_object_list(tokens, &token);
}
//
//static TOKEN create_token_word()
//{
//
//}


static void analyze_line(TOKEN_RESULT* result, size_t line_number, const TEXT* line, const TEXT* file_path, const PROJECT_SETTING* setting)
{
    if (!line->length) {
        return;
    }

    //PRIMITIVE_LIST_TCHAR stock = new_primitive_list(PRIMITIVE_LIST_TYPE_TCHAR, line->length);

    size_t column_index = 0;
    while (column_index < line->length) {

        TCHAR current_character = line->value[column_index];
        if (is_whitespace_character(current_character)) {
            column_index += 1;
            continue;
        }

        TCHAR next_character = 0;
        if (column_index + 1 < line->length) {
            next_character = line->value[column_index + 1];
        }

        switch (current_character) {
            case _T('+'):
                if (next_character) {
                    if (next_character == _T('+')) {
                        column_index += 2;
                        add_token_kind(&result->token, TOKEN_KIND_OP_INCREMENT, column_index, line_number, file_path);
                        continue;
                    }
                }
                add_token_kind(&result->token, TOKEN_KIND_OP_PLUS, column_index, line_number, file_path);
                column_index += 1;
                continue;

            default:
                break;
        }

        column_index += 1;

    }
}

static bool foreach_analyze_line(const void* value, size_t index, size_t length, void* data)
{
    TEXT* line = (TEXT*)value;
    ANALYZE_DATA* analyze_data = (ANALYZE_DATA*)data;
    analyze_line(analyze_data->result, index + 1, line, analyze_data->file_path, analyze_data->setting);
    return true;
}

TOKEN_RESULT analyze(const TEXT* file_path, const TEXT* source, const PROJECT_SETTING* setting)
{
    assert(setting);

    TOKEN_RESULT token_result = {
        .token = create_object_list(sizeof(TOKEN), 1024, compare_object_list_value_null, free_object_list_value_null),
        .result = create_object_list(sizeof(COMPILE_RESULT), OBJECT_LIST_DEFAULT_CAPACITY_COUNT, compare_object_list_value_null, free_object_list_value_null),
    };

    if (!source || !source->length) {
        return token_result;
    }

    OBJECT_LIST lines = split_newline_text(source);

    ANALYZE_DATA data = {
        .result = &token_result,
        .setting = (PROJECT_SETTING*)setting, //TODO: とりあえずの回避。Cって構造体メンバにconst使えんの？
    };
    foreach_object_list(&lines, foreach_analyze_line, &data);

    free_object_list(&lines);

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
