#include "lexical_analyzer.h"


SOURCE_DATA parse_source_data(const TEXT* path, const TEXT* source)
{
    SOURCE_DATA result = {
        .path = clone_text(path),
        .tokens = NULL,
        .token_count = 0,
    };

    return result;
}

bool free_source_data(SOURCE_DATA* source_data)
{
    if (!source_data) {
        return false;
    }

    if (!free_text(&source_data->path)) {
        return false;
    }
    source_data->path = create_invalid_text();

    bool token_release = true;
    for (size_t i = 0; i < source_data->token_count; i++) {
        token_release &= free_text(&source_data->tokens[i].value);
    }
    if (!token_release) {
        return false;
    }

    if (!free_memory(source_data->tokens)) {
        return false;
    }

    if (!free_memory(source_data->tokens)) {
        return false;
    }
    source_data->tokens = NULL;
    source_data->token_count = 0;

    return true;
}
