#include "../Pe.Library/tcharacter.h"
#include "../Pe.Library/primitive_list.gen.h"
#include "../Pe.Library/debug.h"

#include "lexical_analyzer.h"
#include "lexical_analyzer.z.token.h"
#include "lexical_analyzer.z.string.h"
#include "lexical_analyzer.z.number.h"
#include "lexical_analyzer.z.word.h"


static void add_index(size_t* current_index, size_t* column_position, size_t add_value)
{
    *current_index += add_value;
    *column_position += add_value;
}

bool is_whitespace_character(TCHAR c)
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

bool is_word_boundary(TCHAR c)
{
    return
        is_whitespace_character(c)
        ||
        is_newline_character(c)
        ||
        is_synbol_token(c)
        ;
}

static void analyze_lexical_core(TOKEN_RESULT* token_result, const TEXT* source, LEXICAL_ANALYZE_DATA* lexical_analyze_data)
{
    TOKEN_RESULT* result = lexical_analyze_data->result;
    const PROJECT_SETTING* project_setting = lexical_analyze_data->setting;

    if (!source->length) {
        return;
    }

    //PRIMITIVE_LIST_TCHAR stock = new_primitive_list(PRIMITIVE_LIST_TYPE_TCHAR, line->length);

    size_t current_index = 0;
    //size_t line_number = 1;
    //size_t column_position = 0;
    TOKEN_KIND last_token_kind = TOKEN_KIND_NONE;
    SOURCE_POSITION source_position = {
        .column_position = 0,
        .line_number = 1,
    };

    while (current_index < source->length) {
        TCHAR current_character = source->value[current_index];
        TCHAR next_character = get_next_character(source, current_index);

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
            source_position.column_position = 0;
            source_position.line_number += 1;
            continue;
        }

        // 行コメント中はなんもしない
        if (last_token_kind == TOKEN_KIND_COMMENT_LINE) {
            current_index += 1;
            continue;
        }

        // 空白無視
        if (is_whitespace_character(current_character)) {
            add_index(&current_index, &source_position.column_position, 1);
            continue;
        }

        // 単一記号トークンと合わせ技の記号トークンを処理
        size_t multi_symbol_token_length = read_multi_symbol_token(result, source, current_index, last_token_kind, &source_position, project_setting);
        if (multi_symbol_token_length) {
            TOKEN* token = peek_object_list(&result->token);
            last_token_kind = token->kind;

            add_index(&current_index, &source_position.column_position, multi_symbol_token_length);

            continue;
        }

        // 以降はコメント中には処理しない
        if (is_comment_token_kind(last_token_kind)) {
            current_index += 1;
            continue;
        }

        // 単一記号トークンの処理
        const SINGLE_SYMBOL_TOKEN* single_symbol_token = find_single_symbol_token(current_character);
        if (single_symbol_token) {
            add_token_kind(&result->token, single_symbol_token->kind, &source_position);
            add_index(&current_index, &source_position.column_position, 1);
            continue;
        }

        // 文字列処理
        if (is_string_start(current_character)) {
            if (current_character == '`') {
                TEXT remark = wrap_text(_T("文字列 ` は未実装"));
                add_compile_result(&result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_IMPLEMENT, &remark, &source_position);
                break;
            }
            if (!next_character) {
                add_compile_result(&result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_CLOSE_STRING, NULL, &source_position);
                break;
            }
            size_t read_length = read_string_token(result, source, current_index, &source_position, project_setting);
            if (!read_length) {
                break;
            }
            current_index += read_length;
            continue;
        }

        // 数値処理
        if (is_number_start(current_character)) {
            size_t read_length = read_number_token(result, source, current_index, &source_position, project_setting);
            if (!read_length) {
                break;
            }
            current_index += read_length;
            continue;
        }

        // 単語処理
        if (is_word_start(current_character)) {
            size_t read_length = read_word_token(result, source, current_index, &source_position, project_setting);
            if (!read_length) {
                break;
            }
            current_index += read_length;
            continue;
        }

        current_index += 1;
    }

    if (last_token_kind == TOKEN_KIND_COMMENT_BLOCK_BEGIN) {
        TOKEN* token = peek_object_list(&result->token);
        add_compile_result(&result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_CLOSE_COMMENT, NULL, &token->position);
    }
}

TOKEN_RESULT RC_HEAP_FUNC(analyze_lexical, const TEXT* file_path, const TEXT* source, const PROJECT_SETTING* setting)
{
    assert(setting);

    TOKEN_RESULT token_result = {
        .file_path = file_path,
        .token = RC_HEAP_CALL(new_object_list, sizeof(TOKEN), 1024, NULL, compare_object_list_value_null, release_object_list_value_null, SCRIPT_MEMORY),
        .result = RC_HEAP_CALL(new_object_list, sizeof(COMPILE_RESULT), OBJECT_LIST_DEFAULT_CAPACITY_COUNT, NULL, compare_object_list_value_null, release_object_list_value_null, SCRIPT_MEMORY),
    };

    if (!source || !source->length) {
        return token_result;
    }

    LEXICAL_ANALYZE_DATA lexical_analyze_data = {
        .file_path = file_path,
        .result = &token_result,
        .setting = setting,
    };
    analyze_lexical_core(&token_result, source, &lexical_analyze_data);

    return token_result;
}

void RC_HEAP_FUNC(release_token_result, TOKEN_RESULT* token_result)
{
    const TOKEN* tokens = reference_value_object_list(TOKEN, token_result->token);
    for (size_t i = 0; i < token_result->token.length; i++) {
        TOKEN* token = (TOKEN*)(tokens + i);
        if (token->type == TOKEN_VALUE_TYPE_STRING) {
            release_text(&token->value.word);
        }
    }

    const COMPILE_RESULT* compile_results = reference_value_object_list(COMPILE_RESULT, token_result->result);
    for (size_t i = 0; i < token_result->result.length; i++) {
        COMPILE_RESULT* compile_result = (COMPILE_RESULT*)(compile_results + i);
        release_text(&compile_result->remark);
    }

    release_object_list(&token_result->token, true);
    release_object_list(&token_result->result, true);
}
