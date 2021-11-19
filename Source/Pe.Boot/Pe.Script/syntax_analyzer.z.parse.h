#pragma once
/* 自動生成: syntax_analyzer.z.parse.h.tt */
#include "lexical_analyzer.h"
#include "lexical_analyzer.z.token.h"

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
    /// <summary>
    /// <list type="number">
    /// <item>term TOKEN_KIND_OP_PLUS term</item>
    /// <item>term TOKEN_KIND_OP_MINUS term</item>
    /// <item>primary_expression</item>
    /// </list>
    /// </summary>
    SYNTAX_DEFINE_KIND_EXPRESSION,
    /// <summary>
    /// <list type="number">
    /// <item>TOKEN_KIND_WORD</item>
    /// <item>TOKEN_KIND_LITERAL_INTEGER</item>
    /// <item>TOKEN_KIND_LITERAL_DECIMAL</item>
    /// <item>string</item>
    /// </list>
    /// </summary>
    SYNTAX_DEFINE_KIND_PRIMARY_EXPRESSION,
    /// <summary>
    /// <list type="number">
    /// <item>TOKEN_KIND_LITERAL_SSTRING</item>
    /// <item>TOKEN_KIND_LITERAL_DSTRING</item>
    /// <item>TOKEN_KIND_LITERAL_BSTRING</item>
    /// </list>
    /// </summary>
    SYNTAX_DEFINE_KIND_STRING,
    /// <summary>
    /// <list type="number">
    /// <item>expression TOKEN_KIND_OP_STAR expression</item>
    /// <item>expression TOKEN_KIND_OP_SLASH expression</item>
    /// <item>factor</item>
    /// </list>
    /// </summary>
    SYNTAX_DEFINE_KIND_TERM,
    /// <summary>
    /// <list type="number">
    /// <item>TOKEN_KIND_BRACKET_LPAREN expression TOKEN_KIND_BRACKET_RPAREN</item>
    /// </list>
    /// </summary>
    SYNTAX_DEFINE_KIND_FACTOR,
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
    SYNTAX_ELEMENT_TYPE_ELEMENTS, // これいる？
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
    /// 1個以上。+
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
        struct tag_SYNTAX_ELEMENTS* elements; // これいる？
    } item;
} SYNTAX_ELEMENT;

typedef struct tag_SYNTAX_ELEMENTS
{
    SYNTAX_ELEMENT* data; // 自動生成作って動的にする想定
    size_t length;
} SYNTAX_ELEMENTS;

typedef struct tag_SYNTAX_DEFINE
{
    SYNTAX_DEFINE_KIND kind;
    SYNTAX_ELEMENTS* elements;
    size_t length;
} SYNTAX_DEFINE;

#define SYNTAX_DEFINE_LENGTH (5)
extern SYNTAX_DEFINE script__syntax_defines[SYNTAX_DEFINE_LENGTH];

const TEXT* get_member_name_by_syntax_define_kind(SYNTAX_DEFINE_KIND syntax_define_kind);

void script__initialize_syntax_defines(void);

void script__uninitialize_syntax_defines(void);


