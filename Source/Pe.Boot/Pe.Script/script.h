#pragma once
#include "../Pe.Library/text.h"
#include "compile.h"

/// <summary>
/// プロジェクト設定。
/// </summary>
typedef struct tag_PROJECT_SETTING
{
    /// <summary>
    /// 警告はエラーとして扱うか。
    /// </summary>
    bool warning_is_error;
} PROJECT_SETTING;

/// <summary>
/// プロジェクト情報。
/// 解析時に使用される。
/// </summary>
typedef struct tag_PROJECT_INFO
{
    /// <summary>
    /// スクリプトのベースディレクトリパス。
    /// </summary>
    TEXT base_directory_path;
    /// <summary>
    /// プロジェクト設定。
    /// </summary>
    PROJECT_SETTING setting;
} PROJECT_INFO;

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
void add_compile_result(OBJECT_LIST* compile_results, COMPILE_RESULT_KIND kind, COMPILE_CODE code, const TEXT* remark, size_t column_position, size_t line_number);

