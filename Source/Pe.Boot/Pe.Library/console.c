﻿#include "console.h"
#include "logging.h"

CONSOLE_RESOURCE begin_console(const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    bool attached = false;
    if (AttachConsole(ATTACH_PARENT_PROCESS)) {
        attached = true;
    }
    if (!attached) {
        AllocConsole();
    }

    TEXT stdio_input_test = wrap_text(_T("CONIN$"));
    TEXT stdio_output_test = wrap_text(_T("CONOUT$"));
    TEXT stdio_error_test = wrap_text(_T("CONOUT$"));

    CONSOLE_RESOURCE console_resource = {
        .handle = {
            .input = GetStdHandle(STD_INPUT_HANDLE),
            .output = GetStdHandle(STD_OUTPUT_HANDLE),
            .error = GetStdHandle(STD_ERROR_HANDLE),
    },
    .stdio = {
        .input = open_file_resource(&stdio_input_test, memory_arena_resource),
        .output = open_file_resource(&stdio_output_test, memory_arena_resource),
        .error = open_file_resource(&stdio_error_test, memory_arena_resource),
    },
    .library = {
        .attached = attached,
    }
    };

    DWORD consoleMode;
    GetConsoleMode(console_resource.handle.output, &consoleMode);

    return console_resource;
}

void end_console(CONSOLE_RESOURCE* console_resource)
{
    if (!console_resource->library.attached) {
        FreeConsole();
    }

    release_file_resource(&console_resource->stdio.input);
    release_file_resource(&console_resource->stdio.output);
    release_file_resource(&console_resource->stdio.error);
}

size_t output_console_text(const CONSOLE_RESOURCE* console_resource, const TEXT* text, bool newline)
{
    DWORD write_length;
    WriteConsole(console_resource->handle.output, text->value, (DWORD)text->length, &write_length, NULL);

    if (newline) {
        DWORD newline_length;
        TEXT new_line = wrap_text(NEWLINET);
        WriteConsole(console_resource->handle.output, new_line.value, (DWORD)new_line.length, &newline_length, NULL);
        write_length += newline_length;
    }

    return (size_t)write_length;
}

size_t write_console_text(const CONSOLE_RESOURCE* console_resource, const TEXT* text, bool newline)
{
    DWORD write_length = (DWORD)write_file_resource(&console_resource->stdio.output, text->value, (text->length * sizeof(TCHAR)));

    if (newline) {
        TEXT new_line = wrap_text(NEWLINET);
        write_length += (DWORD)write_file_resource(&console_resource->stdio.output, new_line.value, (new_line.length * sizeof(TCHAR)));
    }

    return (size_t)write_length;
}
