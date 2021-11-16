#include "script.h"

static const MEMORY_RESOURCE* script__memory_resource;

void set_script_memory_resource(const MEMORY_RESOURCE* memory_resource)
{
    script__memory_resource = memory_resource;
}
const MEMORY_RESOURCE* get_script_memory_resource()
{
    return script__memory_resource;
}
