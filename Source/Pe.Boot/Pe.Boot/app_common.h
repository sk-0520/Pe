﻿#pragma once

#define OPTION_APP_MODE_KEY _T("_mode")

#define OPTION_APP_BOOT_WAIT_KEY _T("_boot-wait")
/// #737 互換用処理
#define OPTION_APP_BOOT_WAIT_KEY_issue_737 _T("wait")

/// <summary>
/// 終了コード。
/// </summary>
typedef enum tag_EXIT_CODE
{
    /// <summary>
    /// 正常終了。
    /// </summary>
    EXIT_CODE_SUCCESS = 0,
    /// <summary>
    /// 実行モードが不明。
    /// </summary>
    EXIT_CODE_UNKNOWN_EXECUTE_MODE = 1,
} EXIT_CODE;

/// <summary>
/// 実行モード。
/// </summary>
typedef enum tag_EXECUTE_MODE
{
    /// <summary>
    /// 通常起動。
    /// </summary>
    EXECUTE_MODE_BOOT,
    /// <summary>
    /// 何もせずに本体も終わらす
    /// </summary>
    EXECUTE_MODE_DRY_RUN,
    /// <summary>
    /// コンソール起動。
    /// </summary>
    EXECUTE_MODE_CONSOLE,
} EXECUTE_MODE;
