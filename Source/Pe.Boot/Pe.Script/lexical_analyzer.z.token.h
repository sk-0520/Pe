#pragma once
#include <stdbool.h>

#include "token.h"

typedef struct tag_SINGLE_CHAR_TOKEN
{
    TCHAR character;
    TOKEN_KIND kind;
} SINGLE_CHAR_TOKEN;

#define MULTI_CHAR_TOKEN_FIRST (0)
#define MULTI_CHAR_TOKEN_SECOND (1)
#define MULTI_CHAR_TOKEN_COUNT (2)
typedef struct tag_MULTI_CHAR_TOKEN
{
    TCHAR characters[MULTI_CHAR_TOKEN_COUNT];
    /// <summary>
    /// MULTI_CHAR_TOKEN_FIRST: characters[MULTI_CHAR_TOKEN_FIRST] のみのトークン
    /// MULTI_CHAR_TOKEN_SECOND: characters[MULTI_CHAR_TOKEN_FIRST][MULTI_CHAR_TOKEN_SECOND]のトークン
    /// </summary>
    TOKEN_KIND kinds[MULTI_CHAR_TOKEN_COUNT];
    /// <summary>
    /// 真の場合にコメント中は無視する
    /// </summary>
    bool skip_comments[MULTI_CHAR_TOKEN_COUNT];
} MULTI_CHAR_TOKEN;

bool is_comment_token_kind(TOKEN_KIND kind);

SINGLE_CHAR_TOKEN* find_single_character_token(TCHAR c);

size_t read_multi_character_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, TOKEN_KIND last_token_kind, const SOURCE_POSITION* source_position, const PROJECT_SETTING* project_setting);
