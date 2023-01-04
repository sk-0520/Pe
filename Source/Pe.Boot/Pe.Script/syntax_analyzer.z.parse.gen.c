/* 自動生成: syntax_analyzer.z.default_parse.c.gen.tt */
#include "../Pe.Library/debug.h"
#include "../Pe.Library/logging.h"
#include "../Pe.Library/memory.h"

#include "syntax_analyzer.z.parse.gen.h"

#define DEFAULT_SYNTAX_DEFINE_LENGTH (5)

static const TEXT script_syntax_define_kind_member_names[] = {
    static_text("SYNTAX_DEFINE_KIND_EXPRESSION"),
    static_text("SYNTAX_DEFINE_KIND_PRIMARY_EXPRESSION"),
    static_text("SYNTAX_DEFINE_KIND_STRING"),
    static_text("SYNTAX_DEFINE_KIND_TERM"),
    static_text("SYNTAX_DEFINE_KIND_FACTOR"),
};

const TEXT* get_member_name_by_syntax_define_kind(SYNTAX_DEFINE_KIND syntax_define_kind)
{
    assert(syntax_define_kind < SIZEOF_ARRAY(script_syntax_define_kind_member_names));

    return script_syntax_define_kind_member_names + syntax_define_kind;
}

SYNTAXES new_default_syntaxes(void)
{
    const MEMORY_ARENA_RESOURCE* memory_resource = SCRIPT_MEMORY;
    SYNTAX_DEFINE* syntax_defines = new_memory(DEFAULT_SYNTAX_DEFINE_LENGTH, sizeof(SYNTAX_DEFINE), memory_resource);

    SYNTAXES result = {
        .defines = syntax_defines,
        .length = DEFAULT_SYNTAX_DEFINE_LENGTH,
        .script = {
            .memory_resource = memory_resource,
        },
    };

    SYNTAX_DEFINE define_0_expression = {
        .kind = SYNTAX_DEFINE_KIND_EXPRESSION,
        .elements = allocate_raw_memory(sizeof(SYNTAX_ELEMENTS) * 3, false, memory_resource),
        .length = 3,
    };
    define_0_expression.elements[0].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 3, false, memory_resource),
    define_0_expression.elements[0].length = 3;
    SYNTAX_ELEMENT define_0_expression_element_0_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_TERM,
        }
    };
    define_0_expression.elements[0].data[0] = define_0_expression_element_0_data_0_value;

    SYNTAX_ELEMENT define_0_expression_element_0_data_1_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_OP_PLUS,
        }
    };
    define_0_expression.elements[0].data[1] = define_0_expression_element_0_data_1_value;

    SYNTAX_ELEMENT define_0_expression_element_0_data_2_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_TERM,
        }
    };
    define_0_expression.elements[0].data[2] = define_0_expression_element_0_data_2_value;

    define_0_expression.elements[1].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 3, false, memory_resource),
    define_0_expression.elements[1].length = 3;
    SYNTAX_ELEMENT define_0_expression_element_1_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_TERM,
        }
    };
    define_0_expression.elements[1].data[0] = define_0_expression_element_1_data_0_value;

    SYNTAX_ELEMENT define_0_expression_element_1_data_1_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_OP_MINUS,
        }
    };
    define_0_expression.elements[1].data[1] = define_0_expression_element_1_data_1_value;

    SYNTAX_ELEMENT define_0_expression_element_1_data_2_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_TERM,
        }
    };
    define_0_expression.elements[1].data[2] = define_0_expression_element_1_data_2_value;

    define_0_expression.elements[2].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 1, false, memory_resource),
    define_0_expression.elements[2].length = 1;
    SYNTAX_ELEMENT define_0_expression_element_2_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_PRIMARY_EXPRESSION,
        }
    };
    define_0_expression.elements[2].data[0] = define_0_expression_element_2_data_0_value;

    SYNTAX_DEFINE define_1_primary_expression = {
        .kind = SYNTAX_DEFINE_KIND_PRIMARY_EXPRESSION,
        .elements = allocate_raw_memory(sizeof(SYNTAX_ELEMENTS) * 4, false, memory_resource),
        .length = 4,
    };
    define_1_primary_expression.elements[0].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 1, false, memory_resource),
    define_1_primary_expression.elements[0].length = 1;
    SYNTAX_ELEMENT define_1_primary_expression_element_0_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_WORD,
        }
    };
    define_1_primary_expression.elements[0].data[0] = define_1_primary_expression_element_0_data_0_value;

    define_1_primary_expression.elements[1].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 1, false, memory_resource),
    define_1_primary_expression.elements[1].length = 1;
    SYNTAX_ELEMENT define_1_primary_expression_element_1_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_LITERAL_INTEGER,
        }
    };
    define_1_primary_expression.elements[1].data[0] = define_1_primary_expression_element_1_data_0_value;

    define_1_primary_expression.elements[2].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 1, false, memory_resource),
    define_1_primary_expression.elements[2].length = 1;
    SYNTAX_ELEMENT define_1_primary_expression_element_2_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_LITERAL_DECIMAL,
        }
    };
    define_1_primary_expression.elements[2].data[0] = define_1_primary_expression_element_2_data_0_value;

    define_1_primary_expression.elements[3].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 1, false, memory_resource),
    define_1_primary_expression.elements[3].length = 1;
    SYNTAX_ELEMENT define_1_primary_expression_element_3_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_STRING,
        }
    };
    define_1_primary_expression.elements[3].data[0] = define_1_primary_expression_element_3_data_0_value;

    SYNTAX_DEFINE define_2_string = {
        .kind = SYNTAX_DEFINE_KIND_STRING,
        .elements = allocate_raw_memory(sizeof(SYNTAX_ELEMENTS) * 3, false, memory_resource),
        .length = 3,
    };
    define_2_string.elements[0].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 1, false, memory_resource),
    define_2_string.elements[0].length = 1;
    SYNTAX_ELEMENT define_2_string_element_0_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_LITERAL_SSTRING,
        }
    };
    define_2_string.elements[0].data[0] = define_2_string_element_0_data_0_value;

    define_2_string.elements[1].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 1, false, memory_resource),
    define_2_string.elements[1].length = 1;
    SYNTAX_ELEMENT define_2_string_element_1_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_LITERAL_DSTRING,
        }
    };
    define_2_string.elements[1].data[0] = define_2_string_element_1_data_0_value;

    define_2_string.elements[2].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 1, false, memory_resource),
    define_2_string.elements[2].length = 1;
    SYNTAX_ELEMENT define_2_string_element_2_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_LITERAL_BSTRING,
        }
    };
    define_2_string.elements[2].data[0] = define_2_string_element_2_data_0_value;

    SYNTAX_DEFINE define_3_term = {
        .kind = SYNTAX_DEFINE_KIND_TERM,
        .elements = allocate_raw_memory(sizeof(SYNTAX_ELEMENTS) * 3, false, memory_resource),
        .length = 3,
    };
    define_3_term.elements[0].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 3, false, memory_resource),
    define_3_term.elements[0].length = 3;
    SYNTAX_ELEMENT define_3_term_element_0_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_EXPRESSION,
        }
    };
    define_3_term.elements[0].data[0] = define_3_term_element_0_data_0_value;

    SYNTAX_ELEMENT define_3_term_element_0_data_1_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_OP_STAR,
        }
    };
    define_3_term.elements[0].data[1] = define_3_term_element_0_data_1_value;

    SYNTAX_ELEMENT define_3_term_element_0_data_2_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_EXPRESSION,
        }
    };
    define_3_term.elements[0].data[2] = define_3_term_element_0_data_2_value;

    define_3_term.elements[1].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 3, false, memory_resource),
    define_3_term.elements[1].length = 3;
    SYNTAX_ELEMENT define_3_term_element_1_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_EXPRESSION,
        }
    };
    define_3_term.elements[1].data[0] = define_3_term_element_1_data_0_value;

    SYNTAX_ELEMENT define_3_term_element_1_data_1_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_OP_SLASH,
        }
    };
    define_3_term.elements[1].data[1] = define_3_term_element_1_data_1_value;

    SYNTAX_ELEMENT define_3_term_element_1_data_2_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_EXPRESSION,
        }
    };
    define_3_term.elements[1].data[2] = define_3_term_element_1_data_2_value;

    define_3_term.elements[2].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 1, false, memory_resource),
    define_3_term.elements[2].length = 1;
    SYNTAX_ELEMENT define_3_term_element_2_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_FACTOR,
        }
    };
    define_3_term.elements[2].data[0] = define_3_term_element_2_data_0_value;

    SYNTAX_DEFINE define_4_factor = {
        .kind = SYNTAX_DEFINE_KIND_FACTOR,
        .elements = allocate_raw_memory(sizeof(SYNTAX_ELEMENTS) * 1, false, memory_resource),
        .length = 1,
    };
    define_4_factor.elements[0].data = allocate_raw_memory(sizeof(SYNTAX_ELEMENT) * 3, false, memory_resource),
    define_4_factor.elements[0].length = 3;
    SYNTAX_ELEMENT define_4_factor_element_0_data_0_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_BRACKET_LPAREN,
        }
    };
    define_4_factor.elements[0].data[0] = define_4_factor_element_0_data_0_value;

    SYNTAX_ELEMENT define_4_factor_element_0_data_1_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_DEFINE,
        .item = {
            .define = SYNTAX_DEFINE_KIND_EXPRESSION,
        }
    };
    define_4_factor.elements[0].data[1] = define_4_factor_element_0_data_1_value;

    SYNTAX_ELEMENT define_4_factor_element_0_data_2_value = {
        .rule = SYNTAX_RULE_NORMAL,
        .type = SYNTAX_ELEMENT_TYPE_TOKEN,
        .item = {
            .token = TOKEN_KIND_BRACKET_RPAREN,
        }
    };
    define_4_factor.elements[0].data[2] = define_4_factor_element_0_data_2_value;


    syntax_defines[0] = define_0_expression;
    syntax_defines[1] = define_1_primary_expression;
    syntax_defines[2] = define_2_string;
    syntax_defines[3] = define_3_term;
    syntax_defines[4] = define_4_factor;

    return result;
}



