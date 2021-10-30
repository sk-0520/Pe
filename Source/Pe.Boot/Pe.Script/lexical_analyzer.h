#pragma once
#include <stddef.h>

#include "../Pe.Library/text.h"

#include "token.h"

typedef struct tag_TOKEN_RESULT
{
    OBJECT_LIST token;
    OBJECT_LIST result;
} TOKEN_RESULT;


size_t allocate_token_pairs(TOKEN_PAIR** pairs);

bool free_token_pairs(TOKEN_PAIR pairs[], size_t length);

TOKEN_RESULT analyze(const TEXT* file_path, const TEXT* source, const PROJECT_SETTING* setting);

void free_token_result(TOKEN_RESULT* token_result);
