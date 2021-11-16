﻿#include "app_boot.h"
#include "app_path.h"
#include "../Pe.Library/path.h"
#include "../Pe.Library/logging.h"
#include "../Pe.Library/debug.h"
#include "../Pe.Library/tstring.h"
#include "../Pe.Library/platform.h"
#include "../Pe.Library/command_line.h"
#include "app_command_line.h"
#include "execute.h"

static EXIT_CODE boot_core(HINSTANCE hInstance, const TEXT* command_line)
{
    APP_PATH_ITEMS app_path_items;
    initialize_app_path_items(&app_path_items, hInstance);

    add_visual_cpp_runtime_redist_env_path(&app_path_items.root_directory);

    ShellExecute(NULL, _T("open"), app_path_items.main_module.value, is_enabled_text(command_line) ? command_line->value : NULL, NULL, SW_SHOWNORMAL);

    uninitialize_app_path_items(&app_path_items);

    return 0;
}

EXIT_CODE boot_normal(HINSTANCE hInstance)
{
    logger_put_information(_T("通常起動処理"));

    return boot_core(hInstance, NULL);
}

EXIT_CODE boot_with_option(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option)
{
    logger_put_information(_T("オプションあり処理"));

    const WAIT_TIME_ARG wait_time_arg = get_wait_time(command_line_option);

    if (wait_time_arg.enabled) {
        logger_format_information(_T("起動前停止 %d ms"), wait_time_arg.time);
        Sleep(wait_time_arg.time);
        logger_put_information(_T("待機終了"));
    }

    TEXT_LIST args = allocate_memory(command_line_option->count, sizeof(TEXT), DEFAULT_MEMORY);
    size_t arg_count = filter_enable_command_line_items(args, command_line_option);

    TEXT argument = to_command_line_argument(args, arg_count, DEFAULT_MEMORY);
    logger_put_information(argument.value);

    EXIT_CODE result = boot_core(hInstance, &argument);
    free_text(&argument);
    free_memory(args, DEFAULT_MEMORY);

    return result;
}
