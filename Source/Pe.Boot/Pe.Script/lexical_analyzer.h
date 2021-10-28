#pragma once
#include <stddef.h>

#include "../Pe.Library/text.h"

#include "token.h"

size_t allocate_token_pairs(TOKEN_PAIR** pairs);

bool free_token_pairs(TOKEN_PAIR pairs[], size_t length);
