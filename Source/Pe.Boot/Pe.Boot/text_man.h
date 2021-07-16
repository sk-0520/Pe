﻿#pragma once
#include "text.h"

typedef enum tag_TEXT_COMPARE_MODE
{
    /// <summary>
    /// 大文字と小文字を区別しない。
    /// </summary>
    TEXT_COMPARE_MODE_IGNORE_CASE = NORM_IGNORECASE,
    /// <summary>
    /// ひらがなとカタカナを区別しない。
    /// </summary>
    TEXT_COMPARE_MODE_IGNORE_KANA = NORM_IGNOREKANATYPE,
    /// <summary>
    /// アクセントとかを区別しない。
    /// </summary>
    TEXT_COMPARE_MODE_IGNORE_NONSPACE = NORM_IGNORENONSPACE,
    /// <summary>
    /// 記号を区別しない。
    /// </summary>
    TEXT_COMPARE_MODE_IGNORE_SYMBOLS = NORM_IGNORESYMBOLS,
    /// <summary>
    /// 全角半角を区別しない。
    /// </summary>
    TEXT_COMPARE_MODE_IGNORE_WIDTH = NORM_IGNOREWIDTH,
    /// <summary>
    /// 区切り記号を記号として扱う。
    /// </summary>
    TEXT_COMPARE_MODE_STRING_SORT = SORT_STRINGSORT,
} TEXT_COMPARE_MODE;

typedef struct tag_TEXT_COMPARE_RESULT
{
    int compare;
    bool success;
} TEXT_COMPARE_RESULT;

/// <summary>
/// 空テキストの無視設定
/// </summary>
typedef enum tag_IGNORE_EMPTY
{
    /// <summary>
    /// 無視しない。
    /// </summary>
    IGNORE_EMPTY_NONE,
    /// <summary>
    /// 空を無視する。
    /// </summary>
    IGNORE_EMPTY_ONLY,
    /// <summary>
    /// 空白文字を無視する。
    /// </summary>
    IGNORE_EMPTY_WHITESPACE,
} IGNORE_EMPTY;

/// <summary>
/// 数値(64bit幅)変換結果。
/// </summary>
typedef struct tag_TEXT_PARSED_INT32_RESULT
{
    /// <summary>
    /// 変換値。
    /// <para>successが真の場合に有効値が設定される。</para>
    /// </summary>
    __int32 value;
    /// <summary>
    /// 変換成功状態。
    /// </summary>
    bool success;
} TEXT_PARSED_INT32_RESULT;

/// <summary>
/// 数値(64bit幅)変換結果。
/// </summary>
typedef struct tag_TEXT_PARSED_INT64_RESULT
{
    /// <summary>
    /// 変換値。
    /// <para>successが真の場合に有効値が設定される。</para>
    /// </summary>
    __int64 value;
    /// <summary>
    /// 変換成功状態。
    /// </summary>
    bool success;
} TEXT_PARSED_INT64_RESULT;

/// <summary>
/// テキスト長の取得。
/// <para>TEXT.length でもいいけどこちらの方が安全。</para>
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>長さ。文字列格納領域の長さであるため文字数とは違う。</returns>
size_t getTextLength(const TEXT* text);

/// <summary>
/// テキスト検索。
/// </summary>
/// <param name="haystack">検索対象テキスト。</param>
/// <param name="needle">検索テキスト。</param>
/// <param name="ignoreCase">大文字小文字を無視するか。</param>
/// <returns>見つかったテキストを開始とする参照テキスト、見つからない場合は無効テキスト。解放不要。</returns>
TEXT findText(const TEXT* haystack, const TEXT* needle, bool ignoreCase);

/// <summary>
/// テキスト検索。
/// </summary>
/// <param name="haystack">検索対象テキスト。</param>
/// <param name="needle">検索文字。</param>
/// <returns>見つかったテキストを開始とする参照テキスト、見つからない場合は無効テキスト。解放不要。</returns>
TEXT findCharacter2(const TEXT* haystack, TCHAR needle);

/// <summary>
/// テキスト内の文字位置を検索。
/// </summary>
/// <param name="haystack">検索対象テキスト。</param>
/// <param name="needle">検索文字。</param>
/// <returns>一致文字のインデックス。見つからない場合は0未満。</returns>
ssize_t indexOfCharacter(const TEXT* haystack, TCHAR needle);

/// <summary>
/// テキスト比較。
/// </summary>
/// <param name="a">比較対象テキスト1。</param>
/// <param name="b">比較対象テキスト2。</param>
/// <param name="ignoreCase">大文字小文字を無視するか。</param>
/// <returns>a &lt; b: 負, a = b: 0, a &gt; b: 正。</returns>
int compareText(const TEXT* a, const TEXT* b, bool ignoreCase);

TEXT_COMPARE_RESULT compareTextDetail(const TEXT* a, const TEXT* b, ssize_t width, LOCALE_TYPE locale, TEXT_COMPARE_MODE mode);

/// <summary>
/// 指定のテキストで始まるか。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <param name="word">検索テキスト。</param>
/// <returns>始まる場合に真。</returns>
bool startsWithText(const TEXT* text, const TEXT* word);

/// <summary>
/// テキストを数値(32bit幅)に変換。
/// </summary>
/// <param name="input">入力テキスト。</param>
/// <param name="supportHex">16進数(0x)を考慮するか</param>
/// <returns>結果データ。</returns>
TEXT_PARSED_INT32_RESULT parseInteger(const TEXT* input, bool supportHex);
/// <summary>
/// テキストを数値(64bit幅)に変換。
/// </summary>
/// <param name="input">入力テキスト。</param>
/// <param name="supportHex">16進数(0x)を考慮するか</param>
/// <returns>結果データ。</returns>
TEXT_PARSED_INT64_RESULT parseLong(const TEXT* input, bool supportHex);

/// <summary>
/// テキスト追加。
/// </summary>
/// <param name="source">追加元テキスト。</param>
/// <param name="text">追加対象テキスト。</param>
/// <returns>追加済みテキスト。解放が必要。</returns>
TEXT addText(const TEXT* source, const TEXT* text);

/// <summary>
/// テキスト結合。
/// </summary>
/// <param name="separator">セパレータ。</param>
/// <param name="texts">結合するテキスト。</param>
/// <param name="count">textsの個数。</param>
/// <param name="ignoreEmpty">空要素を無視するか。</param>
/// <returns>結合済みテキスト。解放が必要。</returns>
TEXT joinText(const TEXT* separator, const TEXT texts[], size_t count, IGNORE_EMPTY ignoreEmpty);

/// <summary>
/// 空テキストか。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>空の場合に真。</returns>
bool isEmptyText(const TEXT* text);

/// <summary>
/// 空か空白文字で構成されたテキストか。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>空か空白文字で構成されている場合に真。</returns>
bool isWhiteSpaceText(const TEXT* text);

