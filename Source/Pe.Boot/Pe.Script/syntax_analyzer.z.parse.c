/* 自動生成: syntax_analyzer.z.parse.c.tt */
#include "../Pe.Library/debug.h"
#include "../Pe.Library/logging.h"

#include "syntax_analyzer.z.parse.h"

SYNTAX_DEFINE script__syntax_defines[SYNTAX_DEFINE_LENGTH];
static bool script__initialized_syntax_defines = false;

void script__initialize_syntax_defines(void)
{
    SYNTAX_DEFINE define_more_arguments = {
        .kind = SYNTAX_DEFINE_KIND_MORE_ARGUMENTS,
    };
    script__syntax_defines[0] = define_more_arguments;

    SYNTAX_DEFINE define_arguments = {
        .kind = SYNTAX_DEFINE_KIND_ARGUMENTS,
    };
    script__syntax_defines[1] = define_arguments;

    SYNTAX_DEFINE define_expression = {
        .kind = SYNTAX_DEFINE_KIND_EXPRESSION,
    };
    script__syntax_defines[2] = define_expression;

    SYNTAX_DEFINE define_primary_expression = {
        .kind = SYNTAX_DEFINE_KIND_PRIMARY_EXPRESSION,
    };
    script__syntax_defines[3] = define_primary_expression;

}



