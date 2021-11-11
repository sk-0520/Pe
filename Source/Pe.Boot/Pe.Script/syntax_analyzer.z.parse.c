﻿/* 自動生成: syntax_analyzer.z.parse.c.tt */
#include "../Pe.Library/debug.h"
#include "../Pe.Library/logging.h"
#include "../Pe.Library/memory.h"

#include "syntax_analyzer.z.parse.h"

SYNTAX_DEFINE script__syntax_defines[SYNTAX_DEFINE_LENGTH];
static bool script__initialized_syntax_defines = false;

void script__initialize_syntax_defines(void)
{
    SYNTAX_DEFINE define_0_more_arguments = {
        .kind = SYNTAX_DEFINE_KIND_MORE_ARGUMENTS,
        .elements = allocate_memory(sizeof(SYNTAX_ELEMENTS) * 1, false),
        .length = 1,
    };
    define_0_more_arguments.elements[0].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 2, false),
    define_0_more_arguments.elements[0].length = 2;
    SYNTAX_ELEMENT define_0_more_arguments_element_0_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_OP_COMMA,
        }
    };
    define_0_more_arguments.elements[0].data[0] = define_0_more_arguments_element_0_data_0_value;

    SYNTAX_ELEMENT define_0_more_arguments_element_0_data_1_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_EXPRESSION,
        }
    };
    define_0_more_arguments.elements[0].data[1] = define_0_more_arguments_element_0_data_1_value;

    SYNTAX_DEFINE define_1_arguments = {
        .kind = SYNTAX_DEFINE_KIND_ARGUMENTS,
        .elements = allocate_memory(sizeof(SYNTAX_ELEMENTS) * 1, false),
        .length = 1,
    };
    define_1_arguments.elements[0].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 2, false),
    define_1_arguments.elements[0].length = 2;
    SYNTAX_ELEMENT define_1_arguments_element_0_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_EXPRESSION,
        }
    };
    define_1_arguments.elements[0].data[0] = define_1_arguments_element_0_data_0_value;

    SYNTAX_ELEMENT define_1_arguments_element_0_data_1_value = {
        .rule = SYNTAX_RULE_MORE_0,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_MORE_ARGUMENTS,
        }
    };
    define_1_arguments.elements[0].data[1] = define_1_arguments_element_0_data_1_value;

    SYNTAX_DEFINE define_2_expression = {
        .kind = SYNTAX_DEFINE_KIND_EXPRESSION,
        .elements = allocate_memory(sizeof(SYNTAX_ELEMENTS) * 3, false),
        .length = 3,
    };
    define_2_expression.elements[0].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 3, false),
    define_2_expression.elements[0].length = 3;
    SYNTAX_ELEMENT define_2_expression_element_0_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_TERM,
        }
    };
    define_2_expression.elements[0].data[0] = define_2_expression_element_0_data_0_value;

    SYNTAX_ELEMENT define_2_expression_element_0_data_1_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_OP_PLUS,
        }
    };
    define_2_expression.elements[0].data[1] = define_2_expression_element_0_data_1_value;

    SYNTAX_ELEMENT define_2_expression_element_0_data_2_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_TERM,
        }
    };
    define_2_expression.elements[0].data[2] = define_2_expression_element_0_data_2_value;

    define_2_expression.elements[1].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 3, false),
    define_2_expression.elements[1].length = 3;
    SYNTAX_ELEMENT define_2_expression_element_1_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_TERM,
        }
    };
    define_2_expression.elements[1].data[0] = define_2_expression_element_1_data_0_value;

    SYNTAX_ELEMENT define_2_expression_element_1_data_1_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_OP_MINUS,
        }
    };
    define_2_expression.elements[1].data[1] = define_2_expression_element_1_data_1_value;

    SYNTAX_ELEMENT define_2_expression_element_1_data_2_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_TERM,
        }
    };
    define_2_expression.elements[1].data[2] = define_2_expression_element_1_data_2_value;

    define_2_expression.elements[2].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 1, false),
    define_2_expression.elements[2].length = 1;
    SYNTAX_ELEMENT define_2_expression_element_2_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_PRIMARY_EXPRESSION,
        }
    };
    define_2_expression.elements[2].data[0] = define_2_expression_element_2_data_0_value;

    SYNTAX_DEFINE define_3_primary_expression = {
        .kind = SYNTAX_DEFINE_KIND_PRIMARY_EXPRESSION,
        .elements = allocate_memory(sizeof(SYNTAX_ELEMENTS) * 4, false),
        .length = 4,
    };
    define_3_primary_expression.elements[0].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 1, false),
    define_3_primary_expression.elements[0].length = 1;
    SYNTAX_ELEMENT define_3_primary_expression_element_0_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_WORD,
        }
    };
    define_3_primary_expression.elements[0].data[0] = define_3_primary_expression_element_0_data_0_value;

    define_3_primary_expression.elements[1].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 1, false),
    define_3_primary_expression.elements[1].length = 1;
    SYNTAX_ELEMENT define_3_primary_expression_element_1_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_LITERAL_INTEGER,
        }
    };
    define_3_primary_expression.elements[1].data[0] = define_3_primary_expression_element_1_data_0_value;

    define_3_primary_expression.elements[2].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 1, false),
    define_3_primary_expression.elements[2].length = 1;
    SYNTAX_ELEMENT define_3_primary_expression_element_2_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_LITERAL_DECIMAL,
        }
    };
    define_3_primary_expression.elements[2].data[0] = define_3_primary_expression_element_2_data_0_value;

    define_3_primary_expression.elements[3].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 1, false),
    define_3_primary_expression.elements[3].length = 1;
    SYNTAX_ELEMENT define_3_primary_expression_element_3_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_STRING,
        }
    };
    define_3_primary_expression.elements[3].data[0] = define_3_primary_expression_element_3_data_0_value;

    SYNTAX_DEFINE define_4_string = {
        .kind = SYNTAX_DEFINE_KIND_STRING,
        .elements = allocate_memory(sizeof(SYNTAX_ELEMENTS) * 3, false),
        .length = 3,
    };
    define_4_string.elements[0].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 1, false),
    define_4_string.elements[0].length = 1;
    SYNTAX_ELEMENT define_4_string_element_0_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_LITERAL_SSTRING,
        }
    };
    define_4_string.elements[0].data[0] = define_4_string_element_0_data_0_value;

    define_4_string.elements[1].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 1, false),
    define_4_string.elements[1].length = 1;
    SYNTAX_ELEMENT define_4_string_element_1_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_LITERAL_DSTRING,
        }
    };
    define_4_string.elements[1].data[0] = define_4_string_element_1_data_0_value;

    define_4_string.elements[2].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 1, false),
    define_4_string.elements[2].length = 1;
    SYNTAX_ELEMENT define_4_string_element_2_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_LITERAL_BSTRING,
        }
    };
    define_4_string.elements[2].data[0] = define_4_string_element_2_data_0_value;

    SYNTAX_DEFINE define_5_term = {
        .kind = SYNTAX_DEFINE_KIND_TERM,
        .elements = allocate_memory(sizeof(SYNTAX_ELEMENTS) * 3, false),
        .length = 3,
    };
    define_5_term.elements[0].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 3, false),
    define_5_term.elements[0].length = 3;
    SYNTAX_ELEMENT define_5_term_element_0_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_EXPRESSION,
        }
    };
    define_5_term.elements[0].data[0] = define_5_term_element_0_data_0_value;

    SYNTAX_ELEMENT define_5_term_element_0_data_1_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_OP_STAR,
        }
    };
    define_5_term.elements[0].data[1] = define_5_term_element_0_data_1_value;

    SYNTAX_ELEMENT define_5_term_element_0_data_2_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_EXPRESSION,
        }
    };
    define_5_term.elements[0].data[2] = define_5_term_element_0_data_2_value;

    define_5_term.elements[1].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 3, false),
    define_5_term.elements[1].length = 3;
    SYNTAX_ELEMENT define_5_term_element_1_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_EXPRESSION,
        }
    };
    define_5_term.elements[1].data[0] = define_5_term_element_1_data_0_value;

    SYNTAX_ELEMENT define_5_term_element_1_data_1_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_OP_SLASH,
        }
    };
    define_5_term.elements[1].data[1] = define_5_term_element_1_data_1_value;

    SYNTAX_ELEMENT define_5_term_element_1_data_2_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_EXPRESSION,
        }
    };
    define_5_term.elements[1].data[2] = define_5_term_element_1_data_2_value;

    define_5_term.elements[2].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 1, false),
    define_5_term.elements[2].length = 1;
    SYNTAX_ELEMENT define_5_term_element_2_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_FACTOR,
        }
    };
    define_5_term.elements[2].data[0] = define_5_term_element_2_data_0_value;

    SYNTAX_DEFINE define_6_factor = {
        .kind = SYNTAX_DEFINE_KIND_FACTOR,
        .elements = allocate_memory(sizeof(SYNTAX_ELEMENTS) * 1, false),
        .length = 1,
    };
    define_6_factor.elements[0].data = allocate_memory(sizeof(SYNTAX_ELEMENT) * 3, false),
    define_6_factor.elements[0].length = 3;
    SYNTAX_ELEMENT define_6_factor_element_0_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_OP_LPAREN,
        }
    };
    define_6_factor.elements[0].data[0] = define_6_factor_element_0_data_0_value;

    SYNTAX_ELEMENT define_6_factor_element_0_data_1_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_EXPRESSION,
        }
    };
    define_6_factor.elements[0].data[1] = define_6_factor_element_0_data_1_value;

    SYNTAX_ELEMENT define_6_factor_element_0_data_2_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_OP_RPAREN,
        }
    };
    define_6_factor.elements[0].data[2] = define_6_factor_element_0_data_2_value;


    script__syntax_defines[0] = define_0_more_arguments;
    script__syntax_defines[1] = define_1_arguments;
    script__syntax_defines[2] = define_2_expression;
    script__syntax_defines[3] = define_3_primary_expression;
    script__syntax_defines[4] = define_4_string;
    script__syntax_defines[5] = define_5_term;
    script__syntax_defines[6] = define_6_factor;
    script__initialized_syntax_defines = true;
}

void script__uninitialize_syntax_defines(void)
{
    if (!script__initialized_syntax_defines) {
        return;
    }

    for (size_t i = 0; i < SYNTAX_DEFINE_LENGTH; i++) {
        for (size_t j = 0; j < script__syntax_defines[i].length; j++) {
            free_memory(script__syntax_defines[i].elements[j].data);
        }
        free_memory(script__syntax_defines[i].elements);
    }

    script__initialized_syntax_defines = false;
}


