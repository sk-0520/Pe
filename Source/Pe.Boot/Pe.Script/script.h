#pragma once
#include "../Pe.Library/text.h"

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
    /// 警告はエラーとして扱うか。
    /// </summary>
    bool warning_is_error;
} PROJECT_INFO;



