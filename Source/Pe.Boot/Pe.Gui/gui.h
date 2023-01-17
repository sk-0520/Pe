#pragma once
#include "../Pe.Library/memory.h"

/// <summary>
/// GUIライブラリの根っこリソース。
/// <para>こいつをもとにあれこれ作られる。</para>
/// </summary>
typedef struct tag_GUI_ROOT_RESOURCE
{
    struct
    {
        HINSTANCE instance_handle;
        const MEMORY_ARENA_RESOURCE* memory_arena_resource;
    } library;
} GUI_ROOT_RESOURCE;

/// <summary>
/// <see cref="GUI_ROOT_RESOURCE" /> の生成。
/// </summary>
/// <param name="instance_handle"></param>
/// <param name="memory_arena_resource"></param>
/// <returns>解放が必要。</returns>
GUI_ROOT_RESOURCE create_gui_root_resource(HINSTANCE instance_handle, MEMORY_ARENA_RESOURCE* memory_arena_resource);

/// <summary>
/// <see cref="GUI_ROOT_RESOURCE" /> は有効か。
/// </summary>
/// <param name="gui_root_resource"></param>
/// <returns></returns>
bool is_enabled_gui_root_resource(const GUI_ROOT_RESOURCE* gui_root_resource);

/// <summary>
/// <see cref="GUI_ROOT_RESOURCE" /> の開放。
/// </summary>
/// <param name="gui_root_resource"></param>
/// <returns></returns>
bool release_gui_root_resource(GUI_ROOT_RESOURCE* gui_root_resource);
