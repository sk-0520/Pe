#include "../Pe.Library/object_list.h"
#include "token.h"

void add_token_kind(OBJECT_LIST* tokens, TOKEN_KIND kind, const SOURCE_POSITION* source_position)
{
    TOKEN token = {
        .kind = kind,
        .position = *source_position,
        .type = TOKEN_VALUE_TYPE_NONE,
        .value = {
            .none = NULL,
        },
    };

    push_object_list(tokens, &token);
}

static void add_token_value_core(OBJECT_LIST* tokens, TOKEN_KIND kind, TOKEN_VALUE_TYPE type, TOKEN_VALUE value, const SOURCE_POSITION* source_position)
{
    TOKEN token = {
        .kind = kind,
        .position = *source_position,
        .type = type,
        .value = value,
    };

    push_object_list(tokens, &token);
}

void add_token_word(OBJECT_LIST* tokens, TOKEN_KIND kind, const TEXT* word, const SOURCE_POSITION* source_position)
{
    TOKEN_VALUE token_value = {
        .word = clone_text(word),
    };
    add_token_value_core(tokens, kind, TOKEN_VALUE_TYPE_STRING, token_value, source_position);
}

void add_token_integer(OBJECT_LIST* tokens, TOKEN_KIND kind, ssize_t value, const SOURCE_POSITION* source_position)
{
    TOKEN_VALUE token_value = {
        .integer = value,
    };
    add_token_value_core(tokens, kind, TOKEN_VALUE_TYPE_INTEGER, token_value, source_position);
}

void add_token_decimal(OBJECT_LIST* tokens, TOKEN_KIND kind, double value, const SOURCE_POSITION* source_position)
{
    TOKEN_VALUE token_value = {
        .decimal = value,
    };
    add_token_value_core(tokens, kind, TOKEN_VALUE_TYPE_DECIMAL, token_value, source_position);
}
