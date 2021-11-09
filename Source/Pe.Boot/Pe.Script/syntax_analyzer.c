#include "../Pe.Library/debug.h"
#include "../Pe.Library/logging.h"

#include "syntax_analyzer.h"

SYNTAX_DEFINES script__syntax_define = {
    .defines = {
        {
            .kind = SYNTAX_DEFINE_KIND_MORE_ARGUMENTS,
            .elements = {
                { { { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_TOKEN, TOKEN_KIND_OP_COMMA }, { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION } }, 2 },
            },
            .length = 1,
        },
        {
            .kind = SYNTAX_DEFINE_KIND_MORE_ARGUMENTS,
            .elements = {
                { { { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION }, { SYNTAX_RULE_MORE_0, SYNTAX_ELEMENT_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION } }, 2 },
            },
            .length = 1,
        },
        {
            .kind = SYNTAX_DEFINE_KIND_EXPRESSION,
            .elements = {
                { { { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_DEFINE, SYNTAX_DEFINE_KIND_PRIMARY_EXPRESSION } }, 1 },
                { { { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION }, { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_TOKEN, TOKEN_KIND_OP_PLUS }, { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION } }, 3 },
                { { { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION }, { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_TOKEN, TOKEN_KIND_OP_MINUS }, { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION } }, 3 },
                { { { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION }, { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_TOKEN, TOKEN_KIND_OP_STAR }, { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION } }, 3 },
                { { { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION }, { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_TOKEN, TOKEN_KIND_OP_SLASH }, { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_DEFINE, SYNTAX_DEFINE_KIND_EXPRESSION } }, 3 },
            },
            .length = 5,
        },
        {
            .kind = SYNTAX_DEFINE_KIND_PRIMARY_EXPRESSION,
            .elements = {
                { { { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_TOKEN, TOKEN_KIND_LITERAL_INTEGER } }, 1 },
                { { { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_TOKEN, TOKEN_KIND_LITERAL_DECIMAL } }, 1 },
                { { { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_TOKEN, TOKEN_KIND_LITERAL_SSTRING } }, 1 },
                { { { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_TOKEN, TOKEN_KIND_LITERAL_DSTRING } }, 1 },
                { { { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_TOKEN, TOKEN_KIND_LITERAL_BSTRING } }, 1 },
                { { { SYNTAX_RULE_NORMAL, SYNTAX_ELEMENT_TYPE_TOKEN, TOKEN_KIND_WORD } }, 1 },
            },
            .length = 6,
        },
    },
    .length = 2,
};

void analyze_syntax(const TOKEN_RESULT* token_result, const PROJECT_SETTING* setting)
{
    const TOKEN* tokens = reference_value_object_list(TOKEN, token_result->token);
    size_t current_index = 0;
    while (current_index < token_result->token.length) {
        const TOKEN* token = tokens + current_index++;
        
        for (size_t i = 0; i < script__syntax_define.length; i++) {
            for (size_t j = 0; i < script__syntax_define.defines[i].length; j++) {
                const SYNTAX_ELEMENTS* elements = script__syntax_define.defines[i].elements + j;
                for (size_t k = 0; k < elements->length; k++) {
                    const SYNTAX_ELEMENT* element = elements->data + i;
                    switch (element->type) {
                        case SYNTAX_ELEMENT_TYPE_DEFINE:
                            break;
                        case SYNTAX_ELEMENT_TYPE_TOKEN:
                        {
                            logger_format_information(_T("%d"), token->kind);
                            goto NEXT;
                        }
                            break;

                        case SYNTAX_ELEMENT_TYPE_ELEMENTS:
                            break;

                        default:
                            assert(false);
                    }
                }
            }
        }

    NEXT:
        continue;
    }
}
