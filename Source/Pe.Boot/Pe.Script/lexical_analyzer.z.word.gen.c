/* 自動生成: lexical_analyzer.z.word.gen.c.tt */
#include "../Pe.Library/tcharacter.h"
#include "../Pe.Library/primitive_list.gen.h"

#include "lexical_analyzer.h"
#include "lexical_analyzer.z.word.h"

typedef struct tag_KEYWORD_TOKEN
{
    TOKEN_KIND kind;
    bool implement;
    TEXT word;
} KEYWORD_TOKEN;

static const KEYWORD_TOKEN script__keyword_tokens[] = {
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
    {
        .kind = TOKEN_KIND_KEYWORD_DO,
        .implement = true,
        .word = static_text("do"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_WHILE,
        .implement = true,
        .word = static_text("while"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_BREAK,
        .implement = true,
        .word = static_text("break"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_CONTINUE,
        .implement = true,
        .word = static_text("continue"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_GOTO,
        .implement = true,
        .word = static_text("goto"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_SWITCH,
        .implement = true,
        .word = static_text("switch"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_DEFAULT,
        .implement = true,
        .word = static_text("default"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_CASE,
        .implement = true,
        .word = static_text("case"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_VAR,
        .implement = true,
        .word = static_text("var"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_VAL,
        .implement = true,
        .word = static_text("val"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_CONST,
        .implement = true,
        .word = static_text("const"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_STATIC,
        .implement = true,
        .word = static_text("static"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_FUNCTION,
        .implement = true,
        .word = static_text("function"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_RETURN,
        .implement = true,
        .word = static_text("return"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_SCOPE,
        .implement = true,
        .word = static_text("scope"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_TRY,
        .implement = true,
        .word = static_text("try"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_CATCH,
        .implement = true,
        .word = static_text("catch"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_FINALLY,
        .implement = true,
        .word = static_text("finally"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_THROW,
        .implement = true,
        .word = static_text("throw"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_IMPORT,
        .implement = true,
        .word = static_text("import"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_TRUE,
        .implement = true,
        .word = static_text("true"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_FALSE,
        .implement = true,
        .word = static_text("false"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_STRUCT,
        .implement = true,
        .word = static_text("struct"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_INTERFACE,
        .implement = true,
        .word = static_text("interface"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_PRIVATE,
        .implement = true,
        .word = static_text("private"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_IN,
        .implement = true,
        .word = static_text("in"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_OUT,
        .implement = true,
        .word = static_text("out"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_PROPERTY,
        .implement = true,
        .word = static_text("property"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_GET,
        .implement = true,
        .word = static_text("get"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_SET,
        .implement = true,
        .word = static_text("set"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_VALUE,
        .implement = true,
        .word = static_text("value"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_ARG,
        .implement = true,
        .word = static_text("arg"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_TYPE_VOID,
        .implement = true,
        .word = static_text("void"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_TYPE_INTEGER,
        .implement = true,
        .word = static_text("integer"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_TYPE_DECIMAL,
        .implement = true,
        .word = static_text("decimal"),
    },
    {
        .kind = TOKEN_KIND_KEYWORD_TYPE_STRING,
        .implement = true,
        .word = static_text("string"),
    },
};

static bool is_word_char(TCHAR c)
{
    return
        is_word_start(c)
        ||
        is_digit_character(c)
        ;
}

static const KEYWORD_TOKEN* get_word_token_kind(const TEXT* word)
{
    for (size_t i = 0; i < SIZEOF_ARRAY(script__keyword_tokens); i++) {
        if (is_equals_text(word, &script__keyword_tokens[i].word, false)) {
            return script__keyword_tokens + i;
        }
    }

    return NULL;
}

bool is_word_start(TCHAR c)
{
    return
        is_alphabet_character(c)
        ||
        (c == '_')
#ifdef UNICODE
        // 一旦勘弁してくれ。unicodeのカテゴリ云々詳しくない
#endif
        ;
}


size_t read_word_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, const SOURCE_POSITION* source_position, const PROJECT_SETTING* project_setting)
{
    PRIMITIVE_LIST_TCHAR character_list = new_primitive_list(PRIMITIVE_LIST_TYPE_TCHAR, 256, SCRIPT_MEMORY);
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
    TEXT word = wrap_text_with_length(raw_word, character_list.length, false, SCRIPT_MEMORY);
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
    release_primitive_list(&character_list);

    return read_length;
}


