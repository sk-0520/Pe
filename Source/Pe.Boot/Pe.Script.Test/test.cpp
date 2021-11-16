#include "pch.h"
extern "C" {
#   include "../Pe.Script/script.h"
}

//#define SCRIPT_MEMORY_RESOURCE

TestImpl TEST(tstring(_T("ScriptTest")));

#ifdef SCRIPT_MEMORY_RESOURCE
static MEMORY_RESOURCE script_memory_resource;
#endif

TEST_MODULE_INITIALIZE(initialize)
{
#ifdef SCRIPT_MEMORY_RESOURCE
    script_memory_resource = new_memory_resource(0, 0);
    set_script_memory_resource(&script_memory_resource);
#else
    set_script_memory_resource(DEFAULT_MEMORY);
#endif
}

TEST_MODULE_CLEANUP(cleanup)
{
#ifdef SCRIPT_MEMORY_RESOURCE
    release_memory_resource(&script_memory_resource);
#endif
}

