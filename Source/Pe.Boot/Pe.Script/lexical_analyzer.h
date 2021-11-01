#pragma once
#include <stddef.h>
#include <stdbool.h>

#include "../Pe.Library/text.h"
#include "../Pe.Library/res_check.h"

#include "compile.h"
#include "source.h"
#include "token.h"

typedef struct tag_TOKEN_RESULT
{
    TEXT* file_path;
    OBJECT_LIST/*TOKEN*/ token;
    OBJECT_LIST/*COMPILE_RESULT*/ result;
} TOKEN_RESULT;

typedef struct tag_ANALYZE_DATA
{
    TEXT* file_path;
    TOKEN_RESULT* result;
    PROJECT_SETTING* setting;
} LEXICAL_ANALYZE_DATA;

/// <summary>
/// テキストの位置からN進めた文字を取得。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <param name="base_index">ベースインデックス。</param>
/// <param name="next_position">進める量。</param>
/// <returns>進めた先にある文字。進められない場合は \0 。</returns>
TCHAR get_relative_character(const TEXT* text, size_t base_index, ssize_t next_position);
/// <summary>
/// テキストの位置から一文字進めた文字を取得。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <param name="base_index">ベースインデックス。</param>
/// <returns>1文字先の文字。進められない場合は \0 。</returns>
#define get_next_character(text, base_index) get_relative_character(text, (base_index), 1)

/// <summary>
/// ホワイトスペースとして扱う文字か。
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
bool is_whitespace_character(TCHAR c);

/// <summary>
/// 改行として扱う文字か。
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
bool is_newline_character(TCHAR c);

TOKEN_RESULT RC_HEAP_FUNC(lexical_analyze, const TEXT* file_path, const TEXT* source, const PROJECT_SETTING* setting);
#ifdef RES_CHECK
#   define lexical_analyze(file_path, source, setting) RC_HEAP_WRAP(lexical_analyze, file_path, source, setting)
#endif

void RC_HEAP_FUNC(free_token_result, TOKEN_RESULT* token_result);
#ifdef RES_CHECK
#   define free_token_result(token_result) RC_HEAP_WRAP(free_token_result, token_result)
#endif

