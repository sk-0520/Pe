#pragma once
#include "../Pe.Library/text.h"

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

void set_script_memory_resource(const MEMORY_RESOURCE* memory_resource);
const MEMORY_RESOURCE* get_script_memory_resource();

#define SCRIPT_MEMORY get_script_memory_resource()
