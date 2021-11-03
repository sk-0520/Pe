#include "../Pe.Library/primitive_list.h"

#include "lexical_analyzer.h"
#include "lexical_analyzer.z.word.h"

typedef struct tag_KEYWORD_TOKEN
{
    TOKEN_KIND kind;
    bool implement;
    TEXT word;
} KEYWORD_TOKEN;

KEYWORD_TOKEN script__keyword_tokens[] = {
    {
        .kind = TOKEN_KIND_KEYWORD_IF,
        .implement = true,
        .word = static_text("if"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_ELSE,
        .implement = true,
        .word = static_text("else"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_FOR,
        .implement = true,
        .word = static_text("for"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_FOREACH,
        .implement = true,
        .word = static_text("foreach"),
    },
};

static bool is_word_char(TCHAR c)
{
    return
        ('0' <= c && c <= '9')
        ||
        is_word_start(c)
        ;
}

static const KEYWORD_TOKEN* get_word_token_kind(const TEXT* word)
{
    for (size_t i = 0; i < SIZEOF_ARRAY(script__keyword_tokens); i++) {
        if (!compare_text(word, &script__keyword_tokens[i].word, false)) {
            return script__keyword_tokens + i;
        }
    }

    return NULL;
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
    const KEYWORD_TOKEN* keyword_token = get_word_token_kind(&word);
    if (keyword_token) {
        if (!keyword_token->implement) {
            add_compile_result(&token_result->result, COMPILE_RESULT_KIND_ERROR, COMPILE_CODE_NOT_IMPLEMENT_KEYWORD, NULL, source_position);
        }
        add_token_kind(&token_result->token, keyword_token->kind, source_position);
    } else {
        add_token_word(&token_result->token, TOKEN_KIND_WORD, &word, source_position);
    }

EXIT:
    free_primitive_list(&character_list);

    return read_length;
}



