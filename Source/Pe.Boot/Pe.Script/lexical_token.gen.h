#pragma once
/* 自動生成: lexical_token.gen.h.tt */
#include "source.h"
#include "script.h"

typedef enum tag_TOKEN_KIND
{
    /// <summary>
    /// なんやろね。
    /// </summary>
    TOKEN_KIND_NONE,
    /// <summary>
    /// 関数名とか変数名とか。
    /// </summary>
    TOKEN_KIND_WORD,
    /// <summary>
    /// =
    /// </summary>
    TOKEN_KIND_OP_ASSIGN,
    /// <summary>
    /// +
    /// </summary>
    TOKEN_KIND_OP_PLUS,
    /// <summary>
    /// -
    /// </summary>
    TOKEN_KIND_OP_MINUS,
    /// <summary>
    /// *
    /// </summary>
    TOKEN_KIND_OP_STAR,
    /// <summary>
    /// /
    /// </summary>
    TOKEN_KIND_OP_SLASH,
    /// <summary>
    /// %
    /// </summary>
    TOKEN_KIND_OP_PERCENT,
    /// <summary>
    /// ,
    /// </summary>
    TOKEN_KIND_OP_COMMA,
    /// <summary>
    /// .
    /// </summary>
    TOKEN_KIND_OP_DOT,
    /// <summary>
    /// ;
    /// </summary>
    TOKEN_KIND_OP_SEMICOLON,
    /// <summary>
    /// :
    /// </summary>
    TOKEN_KIND_OP_COLON,
    /// <summary>
    /// ?
    /// </summary>
    TOKEN_KIND_OP_QUESTION,
    /// <summary>
    /// !
    /// </summary>
    TOKEN_KIND_OP_EXCLAMATION,
    /// <summary>
    /// \
    /// </summary>
    TOKEN_KIND_OP_BACKSLASH,
    /// <summary>
    /// ~
    /// </summary>
    TOKEN_KIND_OP_TILDE,
    /// <summary>
    /// @
    /// </summary>
    TOKEN_KIND_OP_AT,
    /// <summary>
    /// $
    /// </summary>
    TOKEN_KIND_OP_DOLLAR,
    /// <summary>
    /// #
    /// </summary>
    TOKEN_KIND_OP_HASH,
    /// <summary>
    /// &amp;
    /// </summary>
    TOKEN_KIND_OP_AMPERSAND,
    /// <summary>
    /// |
    /// </summary>
    TOKEN_KIND_OP_VERTICALBAR,
    /// <summary>
    /// &amp;&amp;
    /// </summary>
    TOKEN_KIND_OP_AND,
    /// <summary>
    /// ||
    /// </summary>
    TOKEN_KIND_OP_OR,
    /// <summary>
    /// ==
    /// </summary>
    TOKEN_KIND_OP_EQUALS,
    /// <summary>
    /// !=
    /// </summary>
    TOKEN_KIND_OP_NOT_EQUALS,
    /// <summary>
    /// &gt;
    /// </summary>
    TOKEN_KIND_OP_GREATER,
    /// <summary>
    /// &lt;
    /// </summary>
    TOKEN_KIND_OP_LESS,
    /// <summary>
    /// &gt;=
    /// </summary>
    TOKEN_KIND_OP_GREATER_EQUAL,
    /// <summary>
    /// &lt;=
    /// </summary>
    TOKEN_KIND_OP_LESS_EQUAL,
    /// <summary>
    /// =&gt;
    /// </summary>
    TOKEN_KIND_OP_LAMBDA,
    /// <summary>
    /// +=
    /// </summary>
    TOKEN_KIND_OP_ADD_ASSIGN,
    /// <summary>
    /// -=
    /// </summary>
    TOKEN_KIND_OP_SUB_ASSIGN,
    /// <summary>
    /// *=
    /// </summary>
    TOKEN_KIND_OP_MUL_ASSIGN,
    /// <summary>
    /// /=
    /// </summary>
    TOKEN_KIND_OP_DIV_ASSIGN,
    /// <summary>
    /// %=
    /// </summary>
    TOKEN_KIND_OP_REM_ASSIGN,
    /// <summary>
    /// ++
    /// </summary>
    TOKEN_KIND_OP_INCREMENT,
    /// <summary>
    /// --
    /// </summary>
    TOKEN_KIND_OP_DECREMENT,
    /// <summary>
    /// (
    /// </summary>
    TOKEN_KIND_BRACKET_LPAREN,
    /// <summary>
    /// )
    /// </summary>
    TOKEN_KIND_BRACKET_RPAREN,
    /// <summary>
    /// {
    /// </summary>
    TOKEN_KIND_BRACKET_LBRACE,
    /// <summary>
    /// }
    /// </summary>
    TOKEN_KIND_BRACKET_RBRACE,
    /// <summary>
    /// [
    /// </summary>
    TOKEN_KIND_BRACKET_LBRACKET,
    /// <summary>
    /// ]
    /// </summary>
    TOKEN_KIND_BRACKET_RBRACKET,
    /// <summary>
    /// //
    /// </summary>
    TOKEN_KIND_COMMENT_LINE,
    /// <summary>
    /// /*
    /// </summary>
    TOKEN_KIND_COMMENT_BLOCK_BEGIN,
    /// <summary>
    /// */
    /// </summary>
    TOKEN_KIND_COMMENT_BLOCK_END,
    /// <summary>
    /// 整数。
    /// </summary>
    TOKEN_KIND_LITERAL_INTEGER,
    /// <summary>
    /// 少数。つかえない。
    /// </summary>
    TOKEN_KIND_LITERAL_DECIMAL,
    /// <summary>
    /// 文字列&apos;。文字なんてものはない。
    /// </summary>
    TOKEN_KIND_LITERAL_SSTRING,
    /// <summary>
    /// 文字列。
    /// </summary>
    TOKEN_KIND_LITERAL_DSTRING,
    /// <summary>
    /// `文字列`。未実装。
    /// </summary>
    TOKEN_KIND_LITERAL_BSTRING,
    /// <summary>
    /// if
    /// </summary>
    TOKEN_KIND_KEYWORD_IF,
    /// <summary>
    /// else
    /// </summary>
    TOKEN_KIND_KEYWORD_ELSE,
    /// <summary>
    /// for
    /// </summary>
    TOKEN_KIND_KEYWORD_FOR,
    /// <summary>
    /// foreach
    /// </summary>
    TOKEN_KIND_KEYWORD_FOREACH,
    /// <summary>
    /// do
    /// </summary>
    TOKEN_KIND_KEYWORD_DO,
    /// <summary>
    /// while
    /// </summary>
    TOKEN_KIND_KEYWORD_WHILE,
    /// <summary>
    /// break
    /// </summary>
    TOKEN_KIND_KEYWORD_BREAK,
    /// <summary>
    /// continue
    /// </summary>
    TOKEN_KIND_KEYWORD_CONTINUE,
    /// <summary>
    /// goto
    /// </summary>
    TOKEN_KIND_KEYWORD_GOTO,
    /// <summary>
    /// switch
    /// </summary>
    TOKEN_KIND_KEYWORD_SWITCH,
    /// <summary>
    /// default
    /// </summary>
    TOKEN_KIND_KEYWORD_DEFAULT,
    /// <summary>
    /// case
    /// </summary>
    TOKEN_KIND_KEYWORD_CASE,
    /// <summary>
    /// var
    /// </summary>
    TOKEN_KIND_KEYWORD_VAR,
    /// <summary>
    /// val
    /// </summary>
    TOKEN_KIND_KEYWORD_VAL,
    /// <summary>
    /// const
    /// </summary>
    TOKEN_KIND_KEYWORD_CONST,
    /// <summary>
    /// static
    /// </summary>
    TOKEN_KIND_KEYWORD_STATIC,
    /// <summary>
    /// function
    /// </summary>
    TOKEN_KIND_KEYWORD_FUNCTION,
    /// <summary>
    /// return
    /// </summary>
    TOKEN_KIND_KEYWORD_RETURN,
    /// <summary>
    /// scope
    /// </summary>
    TOKEN_KIND_KEYWORD_SCOPE,
    /// <summary>
    /// try
    /// </summary>
    TOKEN_KIND_KEYWORD_TRY,
    /// <summary>
    /// catch
    /// </summary>
    TOKEN_KIND_KEYWORD_CATCH,
    /// <summary>
    /// finally
    /// </summary>
    TOKEN_KIND_KEYWORD_FINALLY,
    /// <summary>
    /// throw
    /// </summary>
    TOKEN_KIND_KEYWORD_THROW,
    /// <summary>
    /// import
    /// </summary>
    TOKEN_KIND_KEYWORD_IMPORT,
    /// <summary>
    /// 真
    /// </summary>
    TOKEN_KIND_KEYWORD_TRUE,
    /// <summary>
    /// 偽
    /// </summary>
    TOKEN_KIND_KEYWORD_FALSE,
    /// <summary>
    /// struct
    /// </summary>
    TOKEN_KIND_KEYWORD_STRUCT,
    /// <summary>
    /// interface
    /// </summary>
    TOKEN_KIND_KEYWORD_INTERFACE,
    /// <summary>
    /// private
    /// </summary>
    TOKEN_KIND_KEYWORD_PRIVATE,
    /// <summary>
    /// in
    /// </summary>
    TOKEN_KIND_KEYWORD_IN,
    /// <summary>
    /// out
    /// </summary>
    TOKEN_KIND_KEYWORD_OUT,
    /// <summary>
    /// property
    /// </summary>
    TOKEN_KIND_KEYWORD_PROPERTY,
    /// <summary>
    /// get
    /// </summary>
    TOKEN_KIND_KEYWORD_GET,
    /// <summary>
    /// set
    /// </summary>
    TOKEN_KIND_KEYWORD_SET,
    /// <summary>
    /// value
    /// </summary>
    TOKEN_KIND_KEYWORD_VALUE,
    /// <summary>
    /// arg
    /// </summary>
    TOKEN_KIND_KEYWORD_ARG,
    /// <summary>
    /// void
    /// </summary>
    TOKEN_KIND_KEYWORD_TYPE_VOID,
    /// <summary>
    /// integer
    /// </summary>
    TOKEN_KIND_KEYWORD_TYPE_INTEGER,
    /// <summary>
    /// decimal
    /// </summary>
    TOKEN_KIND_KEYWORD_TYPE_DECIMAL,
    /// <summary>
    /// string
    /// </summary>
    TOKEN_KIND_KEYWORD_TYPE_STRING,
} TOKEN_KIND;

typedef enum tag_TOKEN_VALUE_TYPE
{
    TOKEN_VALUE_TYPE_NONE,
    TOKEN_VALUE_TYPE_STRING,
    TOKEN_VALUE_TYPE_INTEGER,
    TOKEN_VALUE_TYPE_DECIMAL,
} TOKEN_VALUE_TYPE;

typedef union tag_TOKEN_VALUE
{
    void* none;
    /// <summary>
    /// 解放が必要。
    /// </summary>
    TEXT word;
    ssize_t integer;
    double decimal;
} TOKEN_VALUE;

typedef struct tag_TOKEN
{
    SOURCE_POSITION position;
    TOKEN_KIND kind;
    TOKEN_VALUE_TYPE type;
    TOKEN_VALUE value;
} TOKEN;

/// <summary>
/// <see cref="TOKEN_KIND" />から<c>enum</c>メンバ名を取得。
/// </summary>
/// <param name="kind"></param>
/// <returns></returns>
const TEXT* get_member_name_by_token_kind(TOKEN_KIND kind);

void add_token_kind(OBJECT_LIST* tokens, TOKEN_KIND kind, const SOURCE_POSITION* source_position);
void add_token_word(OBJECT_LIST* tokens, TOKEN_KIND kind, const TEXT* word, const SOURCE_POSITION* source_position);
void add_token_integer(OBJECT_LIST* tokens, TOKEN_KIND kind, ssize_t value, const SOURCE_POSITION* source_position);
void add_token_decimal(OBJECT_LIST* tokens, TOKEN_KIND kind, double value, const SOURCE_POSITION* source_position);


