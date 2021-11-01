#include "../Pe.Library/primitive_list.h"

#include "lexical_analyzer.z.string.h"


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

size_t read_string_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, const SOURCE_POSITION* source_position, const PROJECT_SETTING* project_setting)
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
            add_token_word(&token_result->token, string_token_kind, &word, source_position);
            read_length = current_index + 1;
            break;
        }
        TCHAR next_character = get_next_character(source, current_index);

        switch (string_token_kind) {
            case TOKEN_KIND_LITERAL_SSTRING:
                // ' の場合は \' のみをエスケープ対象にする
                if (current_character == '\\' && next_character == '\'') {
                    current_index += 2;
                    push_list_tchar(&character_list, '\'');
                } else if (is_newline_character(current_character) || !next_character) {
                    add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_CLOSE_STRING, NULL, source_position);
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

                    add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_UNKNOWN_ESCAPE_SEQUENCE, NULL, source_position);
                    goto EXIT;
                } else if (is_newline_character(current_character) || !next_character) {
                    add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_CLOSE_STRING, NULL, source_position);
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
