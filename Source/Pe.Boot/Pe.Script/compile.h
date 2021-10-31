#pragma once

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
    COMPILE_CODE_NOT_CLOSE_COMMENT,
    COMPILE_CODE_NOT_CLOSE_STRING,
    COMPILE_CODE_UNKNOWN_ESCAPE_SEQUENCE,
    COMPILE_CODE_INVALID_NUMBER,
} COMPILE_CODE;
