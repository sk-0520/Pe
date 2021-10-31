﻿#pragma once
#include "text.h"
#include "object_list.h"


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
///
/// </summary>
/// <para name="source">元データ。</para>
/// <para name="next_index">sourceから見た次の開始インデックス。</para>
/// <returns>分割後の解放不要テキスト。無効の際に処理終了。</returns>
typedef struct tag_TEXT (*func_split_text)(const TEXT* source, size_t* next_index);

/// <summary>
/// テキスト長の取得。
/// <para>TEXT.length でもいいけどこちらの方が安全。</para>
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>長さ。文字列格納領域の長さであるため文字数とは違う。</returns>
size_t get_text_length(const TEXT* text);

/// <summary>
/// テキスト追加。
/// </summary>
/// <param name="source">追加元テキスト。</param>
/// <param name="text">追加対象テキスト。</param>
/// <returns>追加済みテキスト。解放が必要。</returns>
TEXT add_text(const TEXT* source, const TEXT* text);

/// <summary>
/// テキスト結合。
/// </summary>
/// <param name="separator">セパレータ。</param>
/// <param name="texts">結合するテキスト。</param>
/// <param name="count">textsの個数。</param>
/// <param name="ignore_empty">空要素を無視するか。</param>
/// <returns>結合済みテキスト。解放が必要。</returns>
TEXT join_text(const TEXT* separator, const TEXT_LIST texts, size_t count, IGNORE_EMPTY ignore_empty);

/// <summary>
/// 空テキストか。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>空の場合に真。</returns>
bool is_empty_text(const TEXT* text);

/// <summary>
/// 空か空白文字で構成されたテキストか。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>空か空白文字で構成されている場合に真。</returns>
bool is_whitespace_text(const TEXT* text);

/// <summary>
/// テキストのトリム処理。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <param name="start">先頭をトリム対象とするか。</param>
/// <param name="end">終端をトリム対象とするか。</param>
/// <param name="characters">トリム対象文字一覧。</param>
/// <param name="count">charactersの個数。</param>
/// <returns>トリムされたテキスト。解放が必要。</returns>
TEXT trim_text(const TEXT* text, bool start, bool end, const TCHAR* characters, size_t count);

/// <summary>
/// 両端のホワイトスペースをトリム。
/// <para><c>trimText</c>のラッパー。</para>
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>トリムされたテキスト。解放が必要。</returns>
TEXT trim_whitespace_text(const TEXT* text);

/// <summary>
/// テキスト分割。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <param name="function">処理。</param>
/// <returns>分割後のテキスト一覧。解放が必要、各テキストの解放は不要。</returns>
OBJECT_LIST RC_HEAP_FUNC(split_text, const TEXT* text, func_split_text function);
#ifdef RES_CHECK
#   define split_text(text, function) RC_HEAP_WRAP(split_text, text, function)
#endif

/// <summary>
/// 改行で分割。
/// </summary>
/// <param name="text"></param>
/// <returns></returns>
OBJECT_LIST RC_HEAP_FUNC(split_newline_text, const TEXT* text);
#ifdef RES_CHECK
#   define split_newline_text(text) RC_HEAP_WRAP(split_newline_text, text)
#endif

