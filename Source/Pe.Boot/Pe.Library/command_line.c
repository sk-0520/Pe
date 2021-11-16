﻿#include "debug.h"
#include "command_line.h"
#include "memory.h"
#include "tstring.h"

static bool equals_command_line_item_key(const TEXT* a, const TEXT* b)
{
    return equals_map_key_default(a, b);
}

static void release_command_line_item_core(COMMAND_LINE_ITEM* item, const MEMORY_RESOURCE* memory_resource)
{
    free_text(&item->value);
    release_memory(item, memory_resource);
}

static void release_command_line_item_value(MAP_PAIR* pair, const MEMORY_RESOURCE* memory_resource)
{
    release_command_line_item_core((COMMAND_LINE_ITEM*)pair->value, memory_resource);
}

static void set_command_line_map_setting(MAP* map, size_t capacity, const MEMORY_RESOURCE* memory_resource)
{
    *map = new_map(capacity, equals_command_line_item_key, release_command_line_item_value, memory_resource, memory_resource);
}

/// <summary>
/// <c>convert_map_from_arguments.mark_texts</c>に依存するインデックス番号の取得。
/// </summary>
/// <param name="argument"></param>
/// <param name="mark_texts"></param>
/// <param name="count"></param>
/// <returns></returns>
static ssize_t get_key_mark_index(const TEXT* argument, TEXT_LIST mark_texts, size_t count)
{
    for (size_t i = 0; i < count; i++) {
        const TEXT* mark_text = mark_texts + i;
        if (starts_with_text(argument, mark_text)) {
            return i;
        }
    }

    return -1;
}

static void convert_map_from_arguments(MAP* result, const TEXT arguments[], size_t count)
{
    const MEMORY_RESOURCE* memory_resource = result->library.map_memory_resource;

    TEXT mark_texts[] = {
        wrap_text(_T("--")),
        wrap_text(_T("-")),
        wrap_text(_T("/")),
    };

    for (size_t i = 0; i < count; i++) {
        const TEXT* current = &arguments[i];

        ssize_t mark_index = get_key_mark_index(current, mark_texts, SIZEOF_ARRAY(mark_texts));
        if (mark_index == -1) {
            // キー不明の値は無視する
            continue;
        }

        const TEXT* mark_text = mark_texts + mark_index;
        // 先頭のマークを外した引数取得
        TEXT arg = wrap_text_with_length(current->value + mark_text->length, (size_t)current->length - mark_text->length, false, NULL);

        COMMAND_LINE_ITEM* item = allocate_memory(1, sizeof(COMMAND_LINE_ITEM), memory_resource);
        item->key_index = i;

        TEXT key;
        TEXT value_with_separator = find_character(&arg, _T('='));
        if (is_enabled_text(&value_with_separator)) {
            // 引数が値とキーを持つ
            key = new_text_with_length(arg.value, (value_with_separator.value - arg.value), memory_resource);
            if (1 < value_with_separator.length) {
                // 値は存在する
                TEXT raw_value = wrap_text_with_length(value_with_separator.value + 1, (size_t)value_with_separator.length - 1, false, NULL);
                if (raw_value.length && ((raw_value.value[0] == '"' || raw_value.value[0] == '\'') && raw_value.value[raw_value.length - 1] == raw_value.value[0]) ) {
                    // 囲まれている
                    item->value = new_text_with_length(raw_value.value + 1, (size_t)raw_value.length - 2, memory_resource);
                } else {
                    item->value = raw_value;
                }
            } else {
                // 値は存在しない、が=指定されてるなら空文字列
                item->value = new_empty_text(memory_resource);
            }
            item->value_index = i;
        } else if(i + 1 < count) {
            key = arg;
            // 値は次要素を用いる
            ssize_t next_mark_index = get_key_mark_index(current + 1, mark_texts, SIZEOF_ARRAY(mark_texts));
            if (next_mark_index != -1) {
                // キーっぽいので値なし引数扱いにする
                item->value_index = 0;
                item->value = create_invalid_text();
            } else {
                // 次要素を値として取り込む
                item->value_index = i + 1;
                item->value = clone_text(current + 1, memory_resource);
                // 次要素をスキップ
                i += 1;
            }
        } else {
            key = arg;
            // 次要素はないのでキーのみを用いる
            item->value_index = 0;
            item->value = create_invalid_text();
        }

        MAP_PAIR* pair = add_map(result, &key, item, true);
        if (!pair) {
            release_command_line_item_core(item, memory_resource);
        }
        free_text(&key);
    }
}

COMMAND_LINE_OPTION RC_HEAP_FUNC(parse_command_line, const TEXT* command_line, bool with_command, const MEMORY_RESOURCE* memory_resource)
{
    if (!command_line || !command_line->length) {
        COMMAND_LINE_OPTION empty;
        set_memory(&empty, 0, sizeof(empty));
        set_command_line_map_setting(&empty.library.map, 2, memory_resource);
        return empty;
    }

    int temp_argc;
    TCHAR** argv = CommandLineToArgvW(command_line->value, &temp_argc);
    size_t argc = (size_t)temp_argc;

    TEXT* arguments = allocate_raw_memory(argc * sizeof(TEXT), false, memory_resource);
    for (size_t i = 0; i < argc; i++) {
        TCHAR* arg = argv[i];
        arguments[i] = wrap_text(arg);
    }

    size_t count;
    TEXT* arguments_without_command;
    TEXT* command;
    if (with_command) {
        if (1 < argc) {
            arguments_without_command = arguments + 1;
        } else {
            arguments_without_command = NULL;
        }
        count = argc - 1;
        command = arguments;
    } else {
        arguments_without_command = arguments;
        count = argc;
        command = NULL;
    }

    COMMAND_LINE_OPTION result = {
        .arguments = arguments_without_command,
        .count = count,
        .library = {
            .argv = argv,
            .raw_arguments = arguments,
            .raw_count = argc,
            .command = command,
        },
    };
    set_command_line_map_setting(&result.library.map, result.count, memory_resource);
    convert_map_from_arguments(&result.library.map, result.arguments, result.count);

    return result;
}

bool RC_HEAP_FUNC(release_command_line, COMMAND_LINE_OPTION* command_line_option)
{
    if (!command_line_option) {
        return false;
    }

    const MEMORY_RESOURCE* memory_resource = command_line_option->library.map.library.map_memory_resource;

    release_map(&command_line_option->library.map);

    RC_HEAP_CALL(release_memory, command_line_option->library.raw_arguments, memory_resource);
    command_line_option->library.raw_arguments = NULL;

    LocalFree((HLOCAL)command_line_option->library.argv);
    command_line_option->library.argv = NULL;

    return true;
}

const COMMAND_LINE_ITEM* get_command_line_item(const COMMAND_LINE_OPTION* command_line_option, const TEXT* key)
{
    MAP_RESULT_VALUE result_value = get_map(&command_line_option->library.map, key);
    if (result_value.exists) {
        return (COMMAND_LINE_ITEM*)result_value.value;
    }

    return NULL;
}

bool has_value_command_line_item(const COMMAND_LINE_ITEM* item)
{
    if (!item) {
        return false;
    }

    return is_enabled_text(&item->value);
}

bool is_inputed_command_line_item(const COMMAND_LINE_ITEM* item)
{
    if (!has_value_command_line_item(item)) {
        return false;
    }

    return !is_whitespace_text(&item->value);
}

TEXT to_command_line_argument(const TEXT_LIST arguments, size_t count, const MEMORY_RESOURCE* memory_resource)
{
    if (!arguments || !count) {
        return new_empty_text(memory_resource);
    }

    size_t total_length = count - 1; // スペース分

    bool* hasSpaceList = allocate_memory(count, sizeof(bool), memory_resource);

    for (size_t i = 0; i < count; i++) {
        const TEXT* argument = &arguments[i];
        ssize_t space_index = index_of_character(argument, ' ');
        if (space_index == -1) {
            total_length += argument->length;
        } else {
            ssize_t separator_index = index_of_character(argument, '=');
            if (separator_index == -1) {
                total_length += (size_t)argument->length + 2/* "" */;
                hasSpaceList[i] = true;
            } else if(separator_index < index_of_character(argument, '"')) {
                // 最初から key="" として囲まれてる場合はあえて括る必要なし
                total_length += argument->length;
            } else {
                total_length += (size_t)argument->length + 2/* "" */;
                hasSpaceList[i] = true;
            }
        }
    }

    TCHAR* buffer = allocate_string(total_length, memory_resource);
    size_t position = 0;
    for (size_t i = 0; i < count; i++) {
        const TEXT* argument = &arguments[i];
        bool hasSpace = hasSpaceList[i];

        if (i) {
            buffer[position++] = ' ';
        }

        if (hasSpace) {
            buffer[position++] = '"';
            copy_memory(buffer + position, argument->value, argument->length * sizeof(TCHAR));
            position += argument->length;
            buffer[position++] = '"';
        } else {
            copy_memory(buffer + position, argument->value, argument->length * sizeof(TCHAR));
            position += argument->length;
        }
    }
    buffer[position] = 0;

    release_memory(hasSpaceList, memory_resource);

    return wrap_text_with_length(buffer, position, true, memory_resource);
}

