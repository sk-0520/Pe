﻿#pragma once
#include <stdbool.h>
#include <stddef.h>

#include <tchar.h>


#ifdef RES_CHECK

#define RES_CHECK_INIT_PATH_LENGTH (260)
#define RES_CHECK_INIT_BUFFER_LENGTH (1024)
#define RES_CHECK_INIT_HEAP_COUNT (1024 * 4)
#define RES_CHECK_INIT_FILE_COUNT (1024 * 1)

#define RES_CHECK_ARG_FLIE caller_file
#define RES_CHECK_ARG_LINE caller_line
#define RES_CHECK_WRAP_ARGS _T(__FILE__), (__LINE__)
#define RES_CHECK_FUNC_ARGS const TCHAR* RES_CHECK_ARG_FLIE, size_t RES_CHECK_ARG_LINE
#define RES_CHECK_CALL_ARGS RES_CHECK_ARG_FLIE, RES_CHECK_ARG_LINE

typedef struct
{
    void* p; // ヒープだったりハンドルだったり！
    TCHAR* file;
    size_t line;
} RES_CHECK_STOCK_ITEM;

typedef RES_CHECK_STOCK_ITEM RES_CHECK_HEAP_STOCK_ITEM;
typedef RES_CHECK_STOCK_ITEM RES_CHECK_FILE_STOCK_ITEM;

typedef void (*func_rc__output)(const TCHAR* s);

void rc_heap__check(void* p, bool allocate, RES_CHECK_FUNC_ARGS);
void rc_file__check(void* p, bool allocate, RES_CHECK_FUNC_ARGS);

void rc__print(bool leak);

void rc__initialize(func_rc__output output, size_t path_length, size_t buffer_length, size_t heap_count, size_t file_count);
void rc__uninitialize();

#endif

/// リソースチェック処理呼び出し切り替え処理
#ifdef RES_CHECK
#   define RC_HEAP_CALL(function_name, ...) rc_heap__##function_name(__VA_ARGS__, RES_CHECK_CALL_ARGS)
#   define RC_FILE_CALL(function_name, ...) rc_file__##function_name(__VA_ARGS__, RES_CHECK_CALL_ARGS)
#else
#   define RC_HEAP_CALL(function_name, ...) function_name(__VA_ARGS__)
#   define RC_FILE_CALL(function_name, ...) function_name(__VA_ARGS__)
#endif

#ifdef RES_CHECK
#   define RC_HEAP_FUNC(function_name, ...) rc_heap__##function_name(__VA_ARGS__, RES_CHECK_FUNC_ARGS)
#   define RC_FILE_FUNC(function_name, ...) rc_file__##function_name(__VA_ARGS__, RES_CHECK_FUNC_ARGS)
#   define RC_FILE_WRAP(function_name, ...) rc_file__##function_name(__VA_ARGS__, RES_CHECK_WRAP_ARGS)
#else
#   define RC_HEAP_FUNC(function_name, ...) function_name(__VA_ARGS__)
#   define RC_FILE_FUNC(function_name, ...) function_name(__VA_ARGS__)
#endif
