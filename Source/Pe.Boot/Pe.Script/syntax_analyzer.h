#pragma once
#include <stddef.h>

#include "lexical_analyzer.h"
#include "lexical_analyzer.z.token.h"
//#include "lexical_analyzer.z.string.h"
//#include "lexical_analyzer.z.number.h"
//#include "lexical_analyzer.z.word.h"

typedef enum tag_SYNTAX_DEFINE_KIND
{
    SYNTAX_DEFINE_KIND_NONE,
    /// <summary>
    /// [{, expr}*]
    /// </summary>
    SYNTAX_DEFINE_KIND_EXPRESSIONS_IF_EXISTS,
    /// <summary>
    /// SYNTAX_DEFINE_KIND_PRIMARY_EXPRESSION
    /// expr TOKEN_KIND_OP_PLUS expr
    /// expr TOKEN_KIND_OP_MINUS expr
    /// expr TOKEN_KIND_OP_STAR expr
    /// expr TOKEN_KIND_OP_SLASH expr
    /// expr TOKEN_KIND_OP_PERCENT expr
    /// </summary>
    SYNTAX_DEFINE_KIND_EXPRESSION,
    /// <summary>
    /// TOKEN_KIND_WORD
    /// TOKEN_KIND_LITERAL_INTEGER
    /// TOKEN_KIND_LITERAL_DECIMAL
    /// TOKEN_KIND_LITERAL_SSTRING
    /// TOKEN_KIND_LITERAL_DSTRING
    /// TOKEN_KIND_LITERAL_BSTRING
    /// </summary>
    SYNTAX_DEFINE_KIND_PRIMARY_EXPRESSION,
} SYNTAX_DEFINE_KIND;

typedef enum tag_SYNTAX_TYPE
{
    SYNTAX_TYPE_TOKEN,
    SYNTAX_TYPE_DEFINE,
    //SYNTAX_TYPE_ELEMENTS_IF_EXISTS,
} SYNTAX_TYPE;

typedef struct tag_SYNTAX_ELEMENT
{
    SYNTAX_TYPE type;
    union
    {
        SYNTAX_DEFINE_KIND define;
        TOKEN_KIND token;
    } item;
} SYNTAX_ELEMENT;

typedef struct tag_SYNTAX_ELEMENTS
{
    SYNTAX_ELEMENT data[16]; // 自動生成作って動的にする想定
    size_t length;
} SYNTAX_ELEMENTS;

typedef struct tag_SYNTAX_DEFINE
{
    SYNTAX_DEFINE_KIND kind;
    SYNTAX_ELEMENTS elements[128];
    size_t length;
} SYNTAX_DEFINE;

typedef struct tag_SYNTAX_DEFINES
{
    SYNTAX_DEFINE defines[128];
    size_t length;
} SYNTAX_DEFINES;


