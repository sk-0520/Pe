#pragma once
#include <stddef.h>

#include "../Pe.Library/text.h"

#include "token.h"

typedef struct tag_TOKEN_RESULT
{
    TEXT* file_path;
    OBJECT_LIST/*TOKEN*/ token;
    OBJECT_LIST/*COMPILE_RESULT*/ result;
} TOKEN_RESULT;


TOKEN_RESULT analyze(const TEXT* file_path, const TEXT* source, const PROJECT_SETTING* setting);

void free_token_result(TOKEN_RESULT* token_result);
