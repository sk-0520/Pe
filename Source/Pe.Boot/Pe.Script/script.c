#include "script.h"

static const MEMORY_ARENA_RESOURCE* script_memory_arena_resource;

void set_script_memory_arena_resource(const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    script_memory_arena_resource = memory_arena_resource;
}
const MEMORY_ARENA_RESOURCE* get_script_memory_arena_resource()
{
    return script_memory_arena_resource;
}
