﻿#include "fsio.h"

static FILE_POINTER _createInvalidFile()
{
    FILE_POINTER result = {
        .path = createEmptyText(),
        .handle = NULL,
    };
    return result;
}

static FILE_POINTER _openFileCore(const TEXT* path, FILE_ACCESS_MODE accessMode, FILE_SHARE_MODE sharedMode, FILE_OPEN_MODE openMode, DWORD attributes)
{
    if (!path) {
        return _createInvalidFile();
    }
    if (!path->value) {
        return _createInvalidFile();
    }

    HANDLE handle = CreateFile(path->value, accessMode, sharedMode, NULL, openMode, attributes, NULL);
    if (!handle) {
        return _createInvalidFile();
    }

    FILE_POINTER result = {
        .path = cloneText(path),
        .handle = handle
    };

    return result;
}

FILE_POINTER createFile(const TEXT* path)
{
    return _openFileCore(path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_NEW, 0);
}

FILE_POINTER openFile(const TEXT* path)
{
    return _openFileCore(path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_OPEN, 0);
}

FILE_POINTER openOrCreateFile(const TEXT* path)
{
    return _openFileCore(path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_OPEN_OR_CREATE, 0);
}

bool closeFile(FILE_POINTER* file)
{
    if (!file) {
        return false;
    }

    if (!isEnabledFile(file)) {
        return false;
    }

    freeText(&file->path);
    bool result = CloseHandle(file->handle);
    file->handle = NULL;

    return result;
}

bool isEnabledFile(const FILE_POINTER* file)
{
    if (!file) {
        return false;
    }

    if (!isEnabledText(&file->path)) {
        return false;
    }

    if (!file->handle) {
        return false;
    }

    return true;
}

