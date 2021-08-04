#pragma once
#include <stddef.h>
#include "../Pe.Library/text.h"

/// <summary>
/// ソース読み込み時のトークン。
/// </summary>
typedef struct tag_SOURCE_TOKEN
{
    /// <summary>
    /// 行番号(1始まり)
    /// </summary>
    size_t line_number;
    /// <summary>
    /// 文字の開始位置(0始まり)。
    /// </summary>
    size_t column_position;
} SOURCE_TOKEN;

typedef SOURCE_TOKEN* SOURCE_TOKENS;

typedef struct tag_SOURCE_FILE
{
    TEXT path;
    SOURCE_TOKENS tokens;
    size_t token_count;
} SOURCE_FILE;
