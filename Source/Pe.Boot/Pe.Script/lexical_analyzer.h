#pragma once
#include <stddef.h>

#include "../Pe.Library/text.h"
#include "../Pe.Library/res_check.h"

#include "compile.h"
#include "source.h"
#include "token.h"

typedef struct tag_TOKEN_RESULT
{
    TEXT* file_path;
    OBJECT_LIST/*TOKEN*/ token;
    OBJECT_LIST/*COMPILE_RESULT*/ result;
} TOKEN_RESULT;

typedef struct tag_ANALYZE_DATA
{
    TEXT* file_path;
    TOKEN_RESULT* result;
    PROJECT_SETTING* setting;
} ANALYZE_DATA;


TOKEN_RESULT RC_HEAP_FUNC(analyze, const TEXT* file_path, const TEXT* source, const PROJECT_SETTING* setting);
#ifdef RES_CHECK
#   define analyze(file_path, source, setting) RC_HEAP_WRAP(analyze, file_path, source, setting)
#endif

void RC_HEAP_FUNC(free_token_result, TOKEN_RESULT* token_result);
#ifdef RES_CHECK
#   define free_token_result(token_result) RC_HEAP_WRAP(free_token_result, token_result)
#endif
