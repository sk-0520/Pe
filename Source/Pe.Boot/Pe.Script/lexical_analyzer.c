#include "lexical_analyzer.h"

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

typedef struct tag_ANALYZE_DATA
{
    TOKEN_RESULT* result;
    PROJECT_SETTING* setting;
} ANALYZE_DATA;

void analyze_line(TOKEN_RESULT* result, size_t line_number, const TEXT* line, const PROJECT_SETTING* setting)
{

}

bool foreach_analyze_line(const void* value, size_t index, size_t length, void* data)
{
    TEXT* line = (TEXT*)value;
    ANALYZE_DATA* analyze_data = (ANALYZE_DATA*)data;
    analyze_line(analyze_data->result, index + 1, line, analyze_data->setting);
    return true;
}

TOKEN_RESULT analyze(const TEXT* source, const PROJECT_SETTING* setting)
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

void free_token_result(TOKEN_RESULT* token_result)
{
    free_object_list(&token_result->token);
    free_object_list(&token_result->result);
}
