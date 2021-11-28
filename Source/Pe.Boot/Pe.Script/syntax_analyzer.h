#pragma once
#include <stddef.h>

#include "lexical_analyzer.h"
#include "lexical_analyzer.z.token.h"
#include "syntax_analyzer.z.parse.h"

void analyze_syntax(const TOKEN_RESULT* token_result, const PROJECT_SETTING* setting);
