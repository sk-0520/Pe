﻿#include <shlwapi.h>

#include "tstring.h"
#include "memory.h"

TCHAR* findString(const TCHAR* haystack, const TCHAR* needle)
{
    return StrStr(haystack, needle);
}

TCHAR* findStringCase(const TCHAR* haystack, const TCHAR* needle)
{
    return StrStrI(haystack, needle);
}

size_t getStringLength(const TCHAR* s)
{
    return lstrlen(s);
}

TCHAR* findCharacter(const TCHAR* haystack, TCHAR needle)
{
    while (*haystack != needle) {
        if (!*haystack) {
            return NULL;
        }
        haystack++;
    }

    return (TCHAR*)haystack;
}

SSIZE_T indexCharacter(const TCHAR* haystack, TCHAR needle)
{
    TCHAR* p = findCharacter(haystack, needle);
    if (!p) {
        return -1;
    }

    return p - haystack;
}

static bool tryParseIntegerCore(int* result, const TCHAR* input, bool hex)
{
    return StrToIntEx(input, hex ? STIF_SUPPORT_HEX: STIF_DEFAULT, result);
}

bool tryParseInteger(int* result, const TCHAR* input)
{
    return tryParseIntegerCore(result, input, false);
}

bool tryParseHexOrInteger(int* result, const TCHAR* input)
{
    return tryParseIntegerCore(result, input, true);
}

static bool tryParseLongCore(long long* result, const TCHAR* input, bool hex)
{
    return StrToInt64Ex(input, hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, result);
}

bool tryParseLong(long long* result, const TCHAR* input)
{
    return tryParseLongCore(result, input, false);
}

bool tryParseHexOrLong(long long* result, const TCHAR* input)
{
    return tryParseLongCore(result, input, true);
}



TCHAR* concatString(TCHAR* target, const TCHAR* value)
{
    return lstrcat(target, value);
}

TCHAR* copyString(TCHAR* result, const TCHAR* value)
{
    return lstrcpy(result, value);
}

TCHAR* cloneString(const TCHAR* source)
{
    if (!source) {
        return NULL;
    }

    size_t length = getStringLength(source);
    TCHAR* result = allocateMemory((length * sizeof(TCHAR)) + sizeof(TCHAR), false);
    copyMemory(result, (void*)source, length * sizeof(TCHAR));
    result[length] = 0;

    return result;
}


