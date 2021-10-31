#include "../Pe.Library/object_list.h"
#include "token.h"

void add_token_kind(OBJECT_LIST* tokens, TOKEN_KIND kind, size_t column_position, size_t line_number)
{
    TOKEN token = {
        .kind = kind,
        .position = {
            .column_position = column_position,
            .line_number = line_number,
        },
        .type = TOKEN_VALUE_TYPE_NONE,
        .value = {
            .none = NULL,
        },
    };

    push_object_list(tokens, &token);
}

static void add_token_value_core(OBJECT_LIST* tokens, TOKEN_KIND kind, TOKEN_VALUE_TYPE type, TOKEN_VALUE value, size_t column_position, size_t line_number)
{
    TOKEN token = {
        .kind = kind,
        .position = {
            .column_position = column_position,
            .line_number = line_number,
        },
        .type = type,
        .value = value,
    };

    push_object_list(tokens, &token);
}

void add_token_word(OBJECT_LIST* tokens, TOKEN_KIND kind, const TEXT* word, size_t column_position, size_t line_number)
{
    TOKEN_VALUE token_value = {
        .word = clone_text(word),
    };
    add_token_value_core(tokens, kind, TOKEN_VALUE_TYPE_STRING, token_value, column_position, line_number);
}

void add_token_integer(OBJECT_LIST* tokens, TOKEN_KIND kind, ssize_t value, size_t column_position, size_t line_number)
{
    TOKEN_VALUE token_value = {
        .integer = value,
    };
    add_token_value_core(tokens, kind, TOKEN_VALUE_TYPE_INTEGER, token_value, column_position, line_number);
}

void add_token_decimal(OBJECT_LIST* tokens, TOKEN_KIND kind, double value, size_t column_position, size_t line_number)
{
    TOKEN_VALUE token_value = {
        .decimal = value,
    };
    add_token_value_core(tokens, kind, TOKEN_VALUE_TYPE_DECIMAL, token_value, column_position, line_number);
}
