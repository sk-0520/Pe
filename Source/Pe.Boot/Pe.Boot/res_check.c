﻿#ifdef RES_CHECK
#   include <stdio.h>
#   include <assert.h>
#
#   include <tchar.h>
#endif

#include <windows.h>

#include "res_check.h"
#include "common.h"

#ifdef RES_CHECK

typedef enum
{
    RES_CHECK_TYPE_HEAP = 0,
    RES_CHECK_TYPE_FILE = 1,
} RES_CHECK_TYPE;

static const struct RES_CHECK_FORAMT
{
    const TCHAR* alloc_msg;
    const TCHAR* alloc_err;
    const TCHAR* free_mgs;
    const TCHAR* free_err;
    const TCHAR* stock_count;
    const TCHAR* stock_list;
    const TCHAR* stock_leak;
} res_check__formats[] = {
    {
        .alloc_msg = _T("[HEAP:+] %p %s(%zu)"),
        .alloc_err = _T("[HEAP:STOCK:ERROR] %p %s(%zu)"),
        .free_mgs = _T("[HEAP:-] %p %s(%zu) - %s(%zu)"),
        .free_err = _T("[HEAP:NOTFOUND:ERROR] %p %s(%zu)"),
        .stock_count = _T("[HEAP:%s:COUNT] %zu"),
        .stock_list = _T("[HEAP:STOCK] %p %s(%zu)"),
        .stock_leak = _T("[HEAP:WARNING:LEAK] %p %s(%zu)"),
    },
    {
        .alloc_msg = _T("[FILE:+] %p %s(%zu)"),
        .alloc_err = _T("[FILE:STOCK:ERROR] %p %s(%zu)"),
        .free_mgs = _T("[FILE:-] %p %s(%zu) - %s(%zu)"),
        .free_err = _T("[FILE:NOTFOUND:ERROR] %p %s(%zu)"),
        .stock_count = _T("[FILE:%s:COUNT] %zu"),
        .stock_list = _T("[FILE:STOCK] %p %s(%zu)"),
        .stock_leak = _T("[FILE:WARNING:LEAK] %p %s(%zu)"),
    },
};

typedef struct tag_RES_CHECK_ITEM
{
    RES_CHECK_STOCK_ITEM* stock_items;
    size_t stock_items_length;
    size_t* stock_item_count;
    const struct RES_CHECK_FORAMT* formats;
} RES_CHECK_ITEM;

static func_rc__output rc__output;
static size_t rc__path_length;
static size_t  rc__buffer_length;

static RES_CHECK_HEAP_STOCK_ITEM* rc_heap__stock_items;
static size_t rc_heap__stock_items_length;
static size_t rc_heap__stock_item_count;

static RES_CHECK_FILE_STOCK_ITEM* rc_file__stock_items;
static size_t rc_file__stock_items_length;
static size_t rc_file__stock_item_count;

#define rc_heap__output(format, ...) do { \
    TCHAR* rc__buffer = HeapAlloc(GetProcessHeap(), 0, rc__buffer_length * sizeof(TCHAR)); \
    swprintf(rc__buffer, rc__buffer_length - 1, format, __VA_ARGS__); \
    rc__output(rc__buffer); \
    HeapFree(GetProcessHeap(), 0, rc__buffer); \
} while (0)

static RES_CHECK_ITEM rc_get_item(RES_CHECK_TYPE type)
{
    switch (type) {
        case RES_CHECK_TYPE_HEAP:
        {
            RES_CHECK_ITEM result = {
                .stock_items = rc_heap__stock_items,
                .stock_items_length = rc_heap__stock_items_length,
                .stock_item_count = &rc_heap__stock_item_count,
                .formats = res_check__formats + type,
            };
            return result;
        }

        case RES_CHECK_TYPE_FILE:
        {
            RES_CHECK_ITEM result = {
                .stock_items = rc_file__stock_items,
                .stock_items_length = rc_file__stock_items_length,
                .stock_item_count = &rc_file__stock_item_count,
                .formats = res_check__formats + type,
            };
            return result;
        }

        default:
            assert(false);
    }

    RES_CHECK_ITEM none = {
        .stock_items = NULL,
        .stock_items_length = 0,
        .stock_item_count = 0,
        .formats = 0,
    };
    return none;
}

static void rc_check_core(void* p, bool allocate, RES_CHECK_TYPE type, RES_CHECK_FUNC_ARGS)
{
    RES_CHECK_ITEM rc_item = rc_get_item(type);

    if (allocate) {
        bool stocked = false;
        for (size_t i = 0; i < rc_item.stock_items_length; i++) {
            if (!rc_item.stock_items[i].p) {
                rc_heap__output(rc_item.formats->alloc_msg, p, caller_file, caller_line);

                RES_CHECK_STOCK_ITEM data = {
                    .p = p,
                    .line = caller_line,
                };
                data.file = (TCHAR*)HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, rc__path_length * sizeof(TCHAR));
#pragma warning(push)
#pragma warning(disable:6387)
                lstrcpy(data.file, caller_file); // tstring.h を取り込みたくないのでAPIを直接呼び出し
#pragma warning(pop)

                rc_item.stock_items[i] = data;
                *rc_item.stock_item_count = *rc_item.stock_item_count + 1;
                stocked = true;
                break;
            }
        }

        if (!stocked) {
            rc_heap__output(rc_item.formats->alloc_err, p, caller_file, caller_line);
        }
    } else {
        bool exists = false;;
        for (size_t i = 0; p && i < rc_item.stock_items_length; i++) {
            if (rc_item.stock_items[i].p == p) {
                rc_heap__output(rc_item.formats->free_mgs, p, caller_file, caller_line, rc_item.stock_items[i].file, rc_item.stock_items[i].line);
                //HeapFree(GetProcessHeap(), 0, rc_item.stock_items->file);

                *rc_item.stock_item_count = *rc_item.stock_item_count - 1;
                memset(&rc_item.stock_items[i], 0, sizeof(RES_CHECK_STOCK_ITEM));
                exists = true;
                break;
            }
        }

        if (!exists) {
            rc_heap__output(rc_item.formats->free_err, p, caller_file, caller_line);
        }
    }
}

void rc_heap__check(void* p, bool allocate, RES_CHECK_FUNC_ARGS)
{
    rc_check_core(p, allocate, RES_CHECK_TYPE_HEAP, RES_CHECK_CALL_ARGS);
}

void rc_file__check(void* p, bool allocate, RES_CHECK_FUNC_ARGS)
{
    rc_check_core(p, allocate, RES_CHECK_TYPE_FILE, RES_CHECK_CALL_ARGS);
}

static void rc__print_core(bool leak, RES_CHECK_TYPE type)
{
    RES_CHECK_ITEM rc_item = rc_get_item(type);

    rc_heap__output(rc_item.formats->stock_count, *rc_item.stock_item_count && leak ? _T("ERROR") : _T("INFORMATION"), *rc_item.stock_item_count);

    for (size_t i = 0; i < rc_item.stock_items_length; i++) {
        RES_CHECK_STOCK_ITEM* item = rc_item.stock_items + i;
        if (item->p) {
            const TCHAR* format = leak
                ? res_check__formats->stock_list
                : res_check__formats->stock_leak
                ;
            rc_heap__output(format, item->p, item->file, item->line);
        }
    }
}

void rc__print(bool leak)
{
    for (size_t i = 0; i < SIZEOF_ARRAY(res_check__formats); i++) {
        rc__print_core(leak, i);
    }
}

void rc__initialize(func_rc__output output, size_t path_length, size_t buffer_length, size_t heap_count, size_t file_count)
{
    rc__output = output;
    rc__path_length = path_length;
    rc__buffer_length = buffer_length;

    rc_heap__stock_items = HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, heap_count * sizeof(RES_CHECK_HEAP_STOCK_ITEM));
    rc_heap__stock_items_length = heap_count;
    rc_heap__stock_item_count = 0;

    rc_file__stock_items = HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, file_count * sizeof(RES_CHECK_FILE_STOCK_ITEM));
    rc_file__stock_items_length = file_count;
    rc_file__stock_item_count = 0;
}

void rc__uninitialize()
{
    HeapFree(GetProcessHeap(), 0, rc_heap__stock_items);
    HeapFree(GetProcessHeap(), 0, rc_file__stock_items);
}


#endif
