#pragma once
#include "lexical_analyzer.h"

bool is_number_start(TCHAR c);

size_t read_number_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, const SOURCE_POSITION* source_position, const PROJECT_SETTING* project_setting);
