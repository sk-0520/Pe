#pragma once
#include <stddef.h>

/// <summary>
/// ソース位置。
/// </summary>
typedef struct tag_SOURCE_POSITION
{
    /// <summary>
    /// 行番号(1始まり)。
    /// </summary>
    size_t line_number;
    /// <summary>
    /// カラム位置(0始まり)。
    /// </summary>
    size_t column_position;
} SOURCE_POSITION;

