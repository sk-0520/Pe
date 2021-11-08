#include "syntax_analyzer.h"

SYNTAX_DEFINES aaa = {
    .defines = {
        {
            .kind = SYNTAX_DEFINE_KIND_EXPRESSIONS_IF_EXISTS,
            .elements = {
                { { { SYNTAX_TYPE_TOKEN, TOKEN_KIND_OP_COMMA }, { SYNTAX_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION } }, 2 },
            },
            .length = 1,
        },
        {
            .kind = SYNTAX_DEFINE_KIND_EXPRESSION,
            .elements = {
                { { { SYNTAX_TYPE_DEFINE, SYNTAX_DEFINE_KIND_PRIMARY_EXPRESSION } }, 1 },
                { { { SYNTAX_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION }, { SYNTAX_TYPE_TOKEN, TOKEN_KIND_OP_PLUS }, { SYNTAX_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION } }, 3 },
                { { { SYNTAX_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION }, { SYNTAX_TYPE_TOKEN, TOKEN_KIND_OP_MINUS }, { SYNTAX_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION } }, 3 },
                { { { SYNTAX_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION }, { SYNTAX_TYPE_TOKEN, TOKEN_KIND_OP_STAR }, { SYNTAX_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION } }, 3 },
                { { { SYNTAX_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION }, { SYNTAX_TYPE_TOKEN, TOKEN_KIND_OP_SLASH }, { SYNTAX_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION } }, 3 },
            },
            .length = 5,
        },
        {
            .kind = SYNTAX_DEFINE_KIND_PRIMARY_EXPRESSION,
            .elements = {
                { { { SYNTAX_TYPE_TOKEN, TOKEN_KIND_LITERAL_INTEGER } }, 1 },
                { { { SYNTAX_TYPE_TOKEN, TOKEN_KIND_LITERAL_DECIMAL } }, 1 },
                { { { SYNTAX_TYPE_TOKEN, TOKEN_KIND_LITERAL_SSTRING } }, 1 },
                { { { SYNTAX_TYPE_TOKEN, TOKEN_KIND_LITERAL_DSTRING } }, 1 },
                { { { SYNTAX_TYPE_TOKEN, TOKEN_KIND_LITERAL_BSTRING } }, 1 },
                { { { SYNTAX_TYPE_TOKEN, TOKEN_KIND_WORD } }, 1 },
            },
            .length = 6,
        },
    },
    .length = 2,
};

