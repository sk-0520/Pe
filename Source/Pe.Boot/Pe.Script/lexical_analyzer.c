#include "lexical_analyzer.h"

size_t allocate_token_pairs(TOKEN_PAIR** pairs)
{
    TOKEN_PAIR token_pairs[] = {
        { TOKEN_KIND_OP_ASSIGN, new_text(_T("=")) },
    };

    size_t token_length = sizeof(token_pairs) / sizeof(token_pairs[0]);
    byte_t tokens_bytes = sizeof(token_pairs[0]) * token_length;
    TOKEN_PAIR* result = allocate_memory(tokens_bytes, false);
    copy_memory(pairs, result, tokens_bytes);

    return token_length;
}

bool free_token_pairs(TOKEN_PAIR pairs[], size_t length)
{
    if (!pairs) {
        return false;
    }

    for (size_t i = 0; i < length; i++) {
        free_text(&pairs[i].word);
    }

    return true;
}
