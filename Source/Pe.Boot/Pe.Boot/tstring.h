﻿#pragma once
#include <tchar.h>

/// <summary>
/// 文字列 haystack の先頭から文字列 needle を探し、見つかったときにはその位置をポインタで返却し、見つからなかったときにはNULLを返却。
/// </summary>
/// <param name="haystack">検索対象文字列</param>
/// <param name="needle">検索文字列</param>
/// <returns>一致文字のアドレス, 見つからない場合は <c>NULL</c></returns>
TCHAR* tstrstr(const TCHAR* haystack, const TCHAR* needle);
/// <summary>
/// 文字列 haystack の先頭から文字列 needle を探し、見つかったときにはその位置をポインタで返却し、見つからなかったときにはNULLを返却。
/// 大文字小文字を区別しない。
/// </summary>
/// <param name="haystack">検索対象文字列</param>
/// <param name="needle">検索文字列</param>
/// <returns>一致文字のアドレス, 見つからない場合は <c>NULL</c></returns>
TCHAR* tstrstri(const TCHAR* haystack, const TCHAR* needle);

/// <summary>
/// 文字列長を取得。
/// </summary>
/// <param name="s">対象文字列。</param>
/// <returns>長さ。</returns>
size_t tstrlen(const TCHAR* s);

/// <summary>
/// 文字列中の文字を検索。
/// </summary>
/// <param name="haystack">検索対象文字列。</param>
/// <param name="needle">検索文字。</param>
/// <returns>一致文字のアドレス, 見つからない場合は <c>NULL</c></returns>
TCHAR* tstrchr(const TCHAR* haystack, TCHAR needle);


