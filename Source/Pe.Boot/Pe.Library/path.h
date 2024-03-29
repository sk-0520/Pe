﻿#pragma once
#include <tchar.h>

#include "text.h"

/// ディレクトリの区切り文字
#define DIRECTORY_SEPARATOR_CHARACTER _T('\\')
/// ディレクトリの代替区切り文字
#define ALT_DIRECTORY_SEPARATOR_CHARACTER _T('/')

/// <summary>
/// パスデータ。
/// </summary>
typedef struct tag_PATH_INFO
{
    /// <summary>
    /// 親パス。
    /// </summary>
    TEXT parent;
    /// <summary>
    /// 名前。
    /// </summary>
    TEXT name;
    /// <summary>
    /// 拡張子を省いた名前。
    /// </summary>
    TEXT name_without_extension;
    /// <summary>
    /// 拡張子(.なし)。
    /// </summary>
    TEXT extension;

    struct
    {
        bool need_release;
    } library;
} PATH_INFO;

/// <summary>
/// ディレクトリ区切りか。
/// </summary>
/// <param name="c"></param>
/// <returns>ディレクトリ区切り文字の場合に真。</returns>
bool is_directory_separator(TCHAR c);

/// <summary>
/// ルートが含まれているか。
/// <para>Windowsの絶対パス難しいから適当にやる</para>
/// </summary>
/// <param name="text"></param>
/// <returns>ルートが含まれている場合に真。</returns>
bool has_root_path(const TEXT* text);

/// <summary>
/// パスから親ディレクトリパスを取得。
/// </summary>
/// <param name="path">対象パス。</param>
/// <returns>親ディレクトリパス。解放が必要。</returns>
TEXT RC_HEAP_FUNC(get_parent_directory_path, const TEXT* path, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define get_parent_directory_path(path, memory_arena_resource) RC_HEAP_WRAP(get_parent_directory_path, (path), memory_arena_resource)
#endif

/// <summary>
/// パスをディレクトリ区切りで分割。
/// </summary>
/// <param name="path"></param>
/// <param name="memory_arena_resource"></param>
/// <returns>分割後ディレクトリ・ファイル名のリスト。解放が必要。</returns>
OBJECT_LIST RC_HEAP_FUNC(split_path, const TEXT* path, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define split_path(path, memory_arena_resource) RC_HEAP_WRAP(split_path, (path), memory_arena_resource)
#endif

/// <summary>
/// パスの正規化。
/// </summary>
/// <param name="path"></param>
/// <param name="memory_arena_resource"></param>
/// <returns>正規化されたパス。</returns>
TEXT RC_HEAP_FUNC(canonicalize_path, const TEXT* path, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define canonicalize_path(path, memory_arena_resource) RC_HEAP_WRAP(canonicalize_path, (path), memory_arena_resource)
#endif

/// <summary>
/// パスを結合する。
/// </summary>
/// <param name="base_path">ベースのパス。</param>
/// <param name="relative_path">結合するパス。</param>
/// <returns>結合パス。解放が必要。</returns>
TEXT RC_HEAP_FUNC(combine_path, const TEXT* base_path, const TEXT* relative_path, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define combine_path(base_path, relative_path, memory_arena_resource) RC_HEAP_WRAP(combine_path, (base_path), (relative_path), memory_arena_resource)
#endif

/// <summary>
/// パスを結合する。
/// </summary>
/// <param name="base_path">ベースのパス。</param>
/// <param name="paths">結合するパス。</param>
/// <param name="count">結合するパスの個数。</param>
/// <returns>結合パス。解放が必要。</returns>
TEXT RC_HEAP_FUNC(join_path, const TEXT* base_path, const TEXT_LIST paths, size_t count, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define join_path(base_path, paths, count, memory_arena_resource) RC_HEAP_WRAP(join_path, (base_path), (paths), (count), memory_arena_resource)
#endif

/// <summary>
/// 実行中モジュールパスの取得
/// </summary>
/// <param name="hInstance">実行モジュールインスタンスハンドル。</param>
/// <returns>実行中モジュールパス。解放が必要。</returns>
TEXT RC_HEAP_FUNC(get_module_path, HINSTANCE hInstance, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define get_module_path(hInstance, memory_arena_resource) RC_HEAP_WRAP(get_module_path, (hInstance), memory_arena_resource)
#endif

/// <summary>
/// フルパスからパス情報を取得。
/// </summary>
/// <param name="path"></param>
/// <returns>解放は不要。持ちまわす場合は<c>clone_path_info</c>で複製すること。</returns>
PATH_INFO get_path_info_stack(const TEXT* path);

/// <summary>
/// パス情報の複製。
/// </summary>
/// <param name="path_info"></param>
/// <param name="memory_arena_resource"></param>
/// <returns>解放が必要。<c>release_path_info</c></returns>
PATH_INFO RC_HEAP_FUNC(clone_path_info, const PATH_INFO* path_info, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define clone_path_info(path_info, memory_arena_resource) RC_HEAP_WRAP(clone_path_info, path_info, memory_arena_resource)
#endif

/// <summary>
/// パス情報を解放。
/// </summary>
/// <param name=""></param>
/// <param name="path_info"></param>
/// <returns></returns>
bool RC_HEAP_FUNC(release_path_info, PATH_INFO* path_info);
#ifdef RES_CHECK
#   define release_path_info(path_info) RC_HEAP_WRAP(release_path_info, (path_info))
#endif
