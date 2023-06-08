#include "gui_test.h"

void gui_test(HINSTANCE hInstance)
{
    MEMORY_ARENA_RESOURCE memory_arena_resource = new_memory_arena_resource(MEMORY_ARENA_AUTO_INITIAL_SIZE, MEMORY_ARENA_EXTENDABLE_MAXIMUM_SIZE);
    GUI_ROOT_RESOURCE gcr = create_gui_root_resource(hInstance, &memory_arena_resource);

    loop_message_gui(NULL, &gcr);

    release_gui_root_resource(&gcr);
    release_memory_arena_resource(&memory_arena_resource);
}
