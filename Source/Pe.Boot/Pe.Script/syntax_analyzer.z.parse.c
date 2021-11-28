#include "syntax_analyzer.z.parse.h"

void release_syntaxes(SYNTAXES* syntaxes)
{
    const MEMORY_RESOURCE* memory_resource = syntaxes->script.memory_resource;

    for (size_t i = 0; i < syntaxes->length; i++) {
        for (size_t j = 0; j < syntaxes->defines[i].length; j++) {
            release_memory(syntaxes->defines[i].elements[j].data, memory_resource);
        }
        release_memory(syntaxes->defines[i].elements, memory_resource);
    }
    release_memory(syntaxes->defines, memory_resource);
    set_memory(syntaxes, sizeof(SYNTAXES), 0);
}
