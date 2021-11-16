#pragma once
#include <stddef.h>
#include <stdbool.h>

#include "../Pe.Library/text.h"
#include "../Pe.Library/res_check.h"

#include "compile.h"
#include "source.h"
#include "lexical_token.h"

typedef struct tag_TOKEN_RESULT
{
    const TEXT* file_path;
    OBJECT_LIST/*TOKEN*/ token;
    OBJECT_LIST/*COMPILE_RESULT*/ result;
} TOKEN_RESULT;

typedef struct tag_ANALYZE_DATA
{
    const TEXT* file_path;
    TOKEN_RESULT* result;
    const PROJECT_SETTING* setting;
} LEXICAL_ANALYZE_DATA;

/// <summary>
/// ホワイトスペースとして扱う文字か。
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
bool is_whitespace_character(TCHAR c);

/// <summary>
/// 単語境界として扱える文字か。
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
bool is_word_boundary(TCHAR c);

/// <summary>
/// 文字列開始文字か。
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
bool is_string_start(TCHAR c);

TOKEN_RESULT RC_HEAP_FUNC(analyze_lexical, const TEXT* file_path, const TEXT* source, const PROJECT_SETTING* setting);
#ifdef RES_CHECK
#   define analyze_lexical(file_path, source, setting) RC_HEAP_WRAP(analyze_lexical, file_path, source, setting)
#endif

void RC_HEAP_FUNC(release_token_result, TOKEN_RESULT* token_result);
#ifdef RES_CHECK
#   define release_token_result(token_result) RC_HEAP_WRAP(release_token_result, token_result)
#endif

