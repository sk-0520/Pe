#include "../Pe.Library/debug.h"
#include "../Pe.Library/logging.h"

#include "syntax_analyzer.h"
#include "syntax_analyzer.z.parse.h"

void analyze_syntax(const TOKEN_RESULT* token_result, const PROJECT_SETTING* setting)
{
    SYNTAXES syntaxes = new_default_syntaxes();

    const TOKEN* tokens = reference_value_object_list(TOKEN, token_result->token);
    size_t current_index = 0;
    while (current_index < token_result->token.length) {
        const TOKEN* token = tokens + current_index++;

        for (size_t i = 0; i < syntaxes.defines->length; i++) {
            for (size_t j = 0; j < syntaxes.defines[i].length; j++) {
                const SYNTAX_ELEMENTS* elements = syntaxes.defines[i].elements + j;
                for (size_t k = 0; k < elements->length; k++) {
                    const SYNTAX_ELEMENT* element = elements->data + k;
                    switch (element->type) {
                        case SYNTAX_ELEMENT_TYPE_TOKEN:
                            {
                                logger_format_info(_T("%d"), token->kind);
                                goto NEXT;
                            }
                            break;

                        case SYNTAX_ELEMENT_TYPE_DEFINE:
                            break;


                        case SYNTAX_ELEMENT_TYPE_ELEMENTS:
                            assert(false);
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

    release_syntaxes(&syntaxes);
}
