﻿#include <windows.h>

#include "path.h"
#include "logging.h"

size_t getApplicationPath(HINSTANCE hInstance, TCHAR* result)
{
    TCHAR appRawPath[MAX_PATH];
    GetModuleFileName(hInstance, appRawPath, MAX_PATH);
    // 正規化しておく
    PathCanonicalize(result, appRawPath);
    outputDebug(result);
    return lstrlen(result);
}

size_t getParentDirectoryPath(TCHAR* result, const TCHAR* path)
{
    lstrcpy(result, path);
    PathRemoveFileSpec(result);
    outputDebug(result);
    return lstrlen(result);
}

size_t getMainModulePath(TCHAR* result, const TCHAR* rootDirPath)
{
    TCHAR binPath[MAX_PATH];
    binPath[0] = 0;
    PathCombine(binPath, rootDirPath, _T("bin"));
    PathCombine(result, binPath, _T("Pe.Main.exe"));
    outputDebug(result);
    return lstrlen(result);
}

void getAppPathItems(HMODULE hInstance, APP_PATH_ITEMS* result)
{
    result->applicationLength = getApplicationPath(hInstance, result->application);

    result->rootDirectoryLength = getParentDirectoryPath(result->rootDirectory, result->application);

    result->mainModuleLength = getMainModulePath(result->mainModule, result->rootDirectory);
}
