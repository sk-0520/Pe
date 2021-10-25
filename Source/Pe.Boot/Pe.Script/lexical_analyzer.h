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
    /// <summary>
    /// トークンデータ。
    /// </summary>
    TEXT value;
} SOURCE_TOKEN;

typedef SOURCE_TOKEN* SOURCE_TOKENS;

/// <summary>
/// ソースデータ情報。
/// </summary>
typedef struct tag_SOURCE_DATA
{
    /// <summary>
    /// ソースファイルパス。
    /// 解放が必要(free_source_dataで巻き取る)。
    /// </summary>
    TEXT path;
    /// <summary>
    /// ソース読み込み時のトークン一覧。
    /// </summary>
    SOURCE_TOKENS tokens;
    /// <summary>
    /// ソース読み込み時のトークン数。
    /// 解放が必要(free_source_dataで巻き取る)。
    /// </summary>
    size_t token_count;
} SOURCE_DATA;

typedef struct tag_PROJECT_SETTING
{
    TEXT base;
} PROJECT_SETTING;


/// <summary>
///
/// </summary>
/// <param name="path"></param>
/// <param name="source"></param>
/// <returns><see cref="free_source_data(SOURCE_DATA*)" />が必要。</returns>
SOURCE_DATA parse_source_data(const TEXT* path, const TEXT* source);

bool free_source_data(SOURCE_DATA* source_data);

