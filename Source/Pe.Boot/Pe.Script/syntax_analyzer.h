#pragma once
#include <stddef.h>

#include "lexical_analyzer.h"
#include "lexical_analyzer.z.token.h"
//#include "lexical_analyzer.z.string.h"
//#include "lexical_analyzer.z.number.h"
//#include "lexical_analyzer.z.word.h"

/// <summary>
/// 構文定義種別。
/// </summary>
typedef enum tag_SYNTAX_DEFINE_KIND
{
    /// <summary>
    /// なし。
    /// <para>番兵にでも使うかなぁ</para>
    /// </summary>
    SYNTAX_DEFINE_KIND_NONE,
    SYNTAX_DEFINE_KIND_MORE_ARGUMENTS,
    SYNTAX_DEFINE_KIND_ARGUMENTS,
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


/// <summary>
/// 構文要素タイプ。
/// </summary>
typedef enum tag_SYNTAX_ELEMENT_TYPE
{
    /// <summary>
    /// トークン。
    /// </summary>
    SYNTAX_ELEMENT_TYPE_TOKEN,
    /// <summary>
    /// 構文定義。
    /// </summary>
    SYNTAX_ELEMENT_TYPE_DEFINE,
    /// <summary>
    /// 要素(再帰的に使用),
    /// </summary>
    SYNTAX_ELEMENT_TYPE_ELEMENTS,
} SYNTAX_ELEMENT_TYPE;

typedef enum tag_SYNTAX_RULE
{
    /// <summary>
    /// 通常。
    /// </summary>
    SYNTAX_RULE_NORMAL,
    /// <summary>
    /// 0個以上。*
    /// </summary>
    SYNTAX_RULE_MORE_0,
    /// <summary>
    /// 1個以上。*
    /// </summary>
    SYNTAX_RULE_MORE_1,
    /// <summary>
    /// 省略可能。?
    /// </summary>
    SYNTAX_RULE_OPTION,
} SYNTAX_RULE;

struct tag_SYNTAX_ELEMENTS;

/// <summary>
/// 構文要素。
/// </summary>
typedef struct tag_SYNTAX_ELEMENT
{
    /// <summary>
    /// 構文要素タイプ。
    /// </summary>
    SYNTAX_ELEMENT_TYPE type;
    /// <summary>
    /// 構文ルール。
    /// </summary>
    SYNTAX_RULE rule;
    union
    {
        SYNTAX_DEFINE_KIND define;
        TOKEN_KIND token;
        /// <summary>
        /// 再帰的要素
        /// </summary>
        struct tag_SYNTAX_ELEMENTS* elements;
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

void analyze_syntax(const TOKEN_RESULT* token_result, const PROJECT_SETTING* setting);
