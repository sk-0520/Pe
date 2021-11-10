#pragma once
#include <stddef.h>

#include "lexical_analyzer.h"
#include "lexical_analyzer.z.token.h"
//#include "lexical_analyzer.z.string.h"
//#include "lexical_analyzer.z.number.h"
//#include "lexical_analyzer.z.word.h"

void analyze_syntax(const TOKEN_RESULT* token_result, const PROJECT_SETTING* setting);
