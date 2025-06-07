#include "app_console_prompt.h"

EXIT_CODE console_execute_prompt(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option, const CONSOLE_RESOURCE* console_resource)
{
    size_t max = 100;
    size_t i = 0;
    bool running_prompt = true;
    while (running_prompt) {
        TEXT t = wrap_text(_T("abc"));
        output_console_text(console_resource, &t, true);
        if (max < i++) {
            break;
        }
    }

    return EXIT_CODE_SUCCESS;
}
