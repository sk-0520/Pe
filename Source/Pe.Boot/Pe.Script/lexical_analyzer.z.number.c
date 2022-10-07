﻿#include "../Pe.Library/tcharacter.h"
#include "../Pe.Library/primitive_list.gen.h"

#include "lexical_analyzer.z.number.h"
#include "lexical_analyzer.z.token.h"

static bool is_number_int(TCHAR c)
{
    return is_digit_character(c);
}
static bool is_number_hex(TCHAR c)
{
    return is_digit_character(c) || ('A' <= c && c <= 'F') || ('a' <= c && c <= 'f');
}
static bool is_number_bin(TCHAR c)
{
    return '0' == c || c == '1';
}

bool is_number_start(TCHAR c)
{
    return is_number_int(c);
}

size_t read_number_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, const SOURCE_POSITION* source_position, const PROJECT_SETTING* project_setting)
{
    assert(token_result);

    PRIMITIVE_LIST_TCHAR character_list = new_primitive_list(PRIMITIVE_LIST_TYPE_TCHAR, 256, SCRIPT_MEMORY);

    TCHAR start_digit = source->value[start_index];
    push_list_tchar(&character_list, start_digit);
    enum
    {
        MODE_INT,
        MODE_DEC,
        MODE_HEX,
        MODE_BIN,
    } mode = MODE_INT;

    size_t read_length = 1;
    for (size_t current_index = start_index + 1, i = 0; current_index < source->length; current_index++, i++, read_length++) {
        TCHAR current_character = source->value[current_index];
        TCHAR next_character = get_next_character(source, current_index);

        // 最初だけちょっと特殊処理
        if (!i && start_digit == '0' && (current_character == 'x' || current_character == 'b')) {
            if (!next_character) {
                add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_INVALID_NUMBER, NULL, source_position);
                read_length = 0;
                goto EXIT;
            }
            switch (current_character) {
                case 'x':
                    if (!is_number_hex(next_character)) {
                        add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_INVALID_NUMBER, NULL, source_position);
                        read_length = 0;
                        goto EXIT;
                    }
                    mode = MODE_HEX;
                    push_list_tchar(&character_list, current_character);
                    continue;

                case 'b':
                    if (!is_number_bin(next_character)) {
                        add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_INVALID_NUMBER, NULL, source_position);
                        read_length = 0;
                        goto EXIT;
                    }
                    mode = MODE_BIN;
                    push_list_tchar(&character_list, current_character);
                    continue;

                default:
                    break;
            }
        }

        // 区切り文字は無視
        if (current_character == '_') {
            continue;
        }

        if (current_character == '.') {
            // すでに少数制御中は解析終了
            if (mode == MODE_DEC) {
                //current_index -= 1;
                break;
            }
            // 16,2進数も解析終了
            if (mode == MODE_HEX || mode == MODE_BIN) {
                //current_index -= 1;
                break;
            }
            // 次の文字が数値でない場合はもう死んでくれ
            if (!is_number_int(next_character)) {
                add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_INVALID_NUMBER, NULL, source_position);
                read_length = 0;
                goto EXIT;
            }
            // (使えんけど)初回のみ少数に切り替え可能
            mode = MODE_DEC;
            continue;
        }

        if (mode == MODE_INT || mode == MODE_DEC) {
            if (!is_number_int(current_character)) {
                if (!is_word_boundary(current_character)) {
                    add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_INVALID_NUMBER, NULL, source_position);
                    read_length = 0;
                    goto EXIT;
                }
                break;
            }
            push_list_tchar(&character_list, current_character);
            continue;
        }

        if (mode == MODE_HEX) {
            if (!is_number_hex(current_character)) {
                if (!is_word_boundary(current_character)) {
                    add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_INVALID_NUMBER, NULL, source_position);
                    read_length = 0;
                    goto EXIT;
                }
                break;
            }
            push_list_tchar(&character_list, current_character);
            continue;
        }

        if (mode == MODE_BIN) {
            if (!is_number_bin(current_character)) {
                if (!is_word_boundary(current_character)) {
                    add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_INVALID_NUMBER, NULL, source_position);
                    read_length = 0;
                    goto EXIT;
                }
                break;
            }
            push_list_tchar(&character_list, current_character);
            continue;
        }

    }

    assert(read_length);

    TEXT word = wrap_text_with_length(reference_list_tchar(&character_list), character_list.length, false, SCRIPT_MEMORY);
    TOKEN_KIND number_token_kind = TOKEN_KIND_LITERAL_INTEGER;
    ssize_t converted_integer = 0;
    //double converted_decimal = 0;
    TEXT_PARSED_SSIZE_RESULT parsed_result;
    TEXT reference_text;
    switch (mode) {
        case MODE_INT:
            parsed_result = parse_ssize_from_text(&word, PARSE_BASE_NUMBER_D);

            if (parsed_result.success) {
                converted_integer = (ssize_t)parsed_result.value;
            } else {
                add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_PAESE_ERROR_NUMBER, NULL, source_position);
                goto EXIT;
            }
            break;

        case MODE_DEC:
            number_token_kind = TOKEN_KIND_LITERAL_DECIMAL;
            break;

        case MODE_HEX:
            parsed_result = parse_ssize_from_text(&word, PARSE_BASE_NUMBER_X);

            if (parsed_result.success) {
                converted_integer = (ssize_t)parsed_result.value;
            } else {
                add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_PAESE_ERROR_NUMBER, NULL, source_position);
                goto EXIT;
            }
            break;

        case MODE_BIN:
            reference_text = wrap_text_with_length(word.value + 2, word.length - 2, false, SCRIPT_MEMORY);
            parsed_result = parse_i64_from_text(&reference_text, PARSE_BASE_NUMBER_B);
            if (parsed_result.success) {
                converted_integer = (ssize_t)parsed_result.value;
            } else {
                add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_PAESE_ERROR_NUMBER, NULL, source_position);
                goto EXIT;
            }
            break;

        default:
            assert(false);
    }

    if (mode == MODE_DEC) {
        // 整数しかない世界
        TEXT decimal_remark = wrap_text(_T("整数のみ使用可能"));
        add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_IMPLEMENT_DECIMAL, &decimal_remark, source_position);
        //add_token_decimal(&token_result->token, number_token_kind, converted_decimal, source_position);
    } else {
        add_token_integer(&token_result->token, number_token_kind, converted_integer, source_position);
    }


EXIT:
    release_primitive_list(&character_list);

    return read_length;
}
