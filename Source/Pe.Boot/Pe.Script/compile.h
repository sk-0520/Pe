#pragma once
#include <stddef.h>

#include "../Pe.Library/text.h"
#include "../Pe.Library/object_list.h"

#include "source.h"

/// <summary>
/// コンパイル段階。
/// </summary>
typedef enum tag_COMPILE_STAGE
{
    COMPILE_STAGE_NONE,
    /// <summary>
    /// 字句解析。
    /// </summary>
    COMPILE_STAGE_LEX,
} COMPILE_STAGE;

/// <summary>
/// エラーコード。
/// </summary>
typedef enum tag_COMPILE_CODE
{
    COMPILE_CODE_NONE,
    COMPILE_CODE_NOT_IMPLEMENT,
    COMPILE_CODE_NOT_IMPLEMENT_DECIMAL,
    COMPILE_CODE_NOT_IMPLEMENT_KEYWORD,
    COMPILE_CODE_NOT_CLOSE_COMMENT,
    COMPILE_CODE_NOT_CLOSE_STRING,
    COMPILE_CODE_UNKNOWN_ESCAPE_SEQUENCE,
    COMPILE_CODE_INVALID_NUMBER,
    COMPILE_CODE_PAESE_ERROR_NUMBER,
    COMPILE_CODE_INVALID_WORD,
} COMPILE_CODE;

/// <summary>
/// コンパイル結果種別。
/// </summary>
typedef enum tag_COMPILE_RESULT_KIND
{
    COMPILE_RESULT_KIND_INFORMATION,
    COMPILE_RESULT_KIND_WARNING,
    COMPILE_RESULT_KIND_ERROR,
} COMPILE_RESULT_KIND;

/// <summary>
/// コンパイル結果。
/// </summary>
typedef struct tag_COMPILE_RESULT
{
    COMPILE_STAGE stage;
    COMPILE_RESULT_KIND kind;
    SOURCE_POSITION position;
    COMPILE_CODE code;
    /// <summary>
    /// 解放が必要。
    /// </summary>
    TEXT remark;
} COMPILE_RESULT;

/// <summary>
///
/// </summary>
/// <param name="compile_results"><c>COMPILE_RESULT</c>の<c>OBJECT_LIST</c></param>
/// <param name="kind"></param>
/// <param name="code"></param>
/// <param name="remark"></param>
/// <param name="column_position"></param>
/// <param name="line_number"></param>
void add_compile_result(OBJECT_LIST* compile_results, COMPILE_RESULT_KIND kind, COMPILE_CODE code, const TEXT* remark, const SOURCE_POSITION* source_position);
