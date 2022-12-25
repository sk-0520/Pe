﻿#pragma once
#ifdef NDEBUG
#   include <windows.h>
#endif

#include "common.h"

/// @brief デバッグ時のみ使用可能なログ出力。
/// @param s DEBUGレベルで動かすので設定レベルに依存。
void output_debug(const TCHAR * s);

#ifdef NDEBUG
#   define assert(expr)
#else
#   define assert(expr) \
do { \
    if (!(expr)) { \
        output_debug(_T("ASSERT: ") _T(#expr) _T(" -> ") _T(__FILE__) _T(":") _T(TO_STRING(__LINE__))); \
        DebugBreak(); \
        ExitProcess(9); \
    } \
} while(0)
#endif

