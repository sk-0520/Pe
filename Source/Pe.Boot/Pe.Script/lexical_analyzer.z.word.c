#include "../Pe.Library/primitive_list.h"

#include "lexical_analyzer.h"
#include "lexical_analyzer.z.word.h"

static bool is_word_char(TCHAR c)
{
    return
        ('0' <= c && c <= '9')
        ||
        is_word_start(c)
        ;
}

static TOKEN_KIND get_word_token_kind(const TEXT* word)
{
    return TOKEN_KIND_WORD;
}

bool is_word_start(TCHAR c)
{
    return
        ('a' <= c && c <= 'z')
        ||
        ('A' <= c && c <= 'Z')
        ||
        (c == '_')
#ifdef UNICODE
        // 一旦勘弁してくれ。unicodeのカテゴリ云々詳しくない
#endif
        ;
}


size_t read_word_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, const SOURCE_POSITION* source_position, const PROJECT_SETTING* project_setting)
{
    PRIMITIVE_LIST_TCHAR character_list = new_primitive_list(PRIMITIVE_LIST_TYPE_TCHAR, 256);
    push_list_tchar(&character_list, source->value[start_index]);

    size_t read_length = 1;
    for (size_t current_index = start_index + 1, i = 0; current_index < source->length; current_index++, i++, read_length++) {
        TCHAR current_character = source->value[current_index];

        if (is_word_boundary(current_character)) {
            break;
        }

        if (!is_word_char(current_character)) {
            add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_INVALID_WORD, NULL, source_position);
            read_length = 0;
            goto EXIT;
        }

        push_list_tchar(&character_list, current_character);
    }

    const TCHAR* raw_word = reference_list_tchar(&character_list);
    TEXT word = wrap_text_with_length(raw_word, character_list.length, false);
    TOKEN_KIND word_token_kind = get_word_token_kind(&word);
    if (word_token_kind == TOKEN_KIND_WORD) {
        add_token_word(&token_result->token, TOKEN_KIND_WORD, &word, source_position);
    } else {
        add_token_kind(&token_result->token, word_token_kind, source_position);
    }

EXIT:
    free_primitive_list(&character_list);

    return read_length;
}



