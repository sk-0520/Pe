#include "gui.h"

GUI_ROOT_RESOURCE create_gui_root_resource(HINSTANCE instance_handle, MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    GUI_ROOT_RESOURCE result = {
        .library = {
            .instance_handle = instance_handle,
            .memory_arena_resource = memory_arena_resource,
    }
    };

    return result;
}

bool is_enabled_gui_root_resource(const GUI_ROOT_RESOURCE* gui_root_resource)
{
    if (!gui_root_resource) {
        return false;
    }

    if (!gui_root_resource->library.memory_arena_resource) {
        return false;
    }

    return true;
}

bool release_gui_root_resource(GUI_ROOT_RESOURCE* gui_root_resource)
{
    if (!is_enabled_gui_root_resource(gui_root_resource)) {
        return false;
    }

    gui_root_resource->library.instance_handle = NULL;
    gui_root_resource->library.memory_arena_resource = NULL;

    return true;

}
