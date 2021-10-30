#pragma once
#include "script.h"

typedef enum tag_TOKEN_KIND
{
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
    /// ++
    /// </summary>
    TOKEN_KIND_OP_INCREMENT,
    /// <summary>
    /// --
    /// </summary>
    TOKEN_KIND_OP_DECREMENT,
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
    /// \
    /// </summary>
    TOKEN_KIND_OP_BACKSLASH,

    /// <summary>
    /// (
    /// </summary>
    TOKEN_KIND_OP_LPAREN,
    /// <summary>
    /// )
    /// </summary>
    TOKEN_KIND_OP_RPAREN,
    /// <summary>
    /// {
    /// </summary>
    TOKEN_KIND_OP_LBRACE,
    /// <summary>
    /// }
    /// </summary>
    TOKEN_KIND_OP_RBRACE,
    /// <summary>
    /// [
    /// </summary>
    TOKEN_KIND_OP_LBRACKET,
    /// <summary>
    /// ]
    /// </summary>
    TOKEN_KIND_OP_RBRACKET,

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
    /// let
    /// </summary>
    TOKEN_KIND_KEYWORD_LET,
    /// <summary>
    /// const
    /// </summary>
    TOKEN_KIND_KEYWORD_CONST,
    /// <summary>
    /// function
    /// </summary>
    TOKEN_KIND_KEYWORD_FUNCTION,
    /// <summary>
    /// return
    /// </summary>
    TOKEN_KIND_KEYWORD_RETURN,
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
    /// true
    /// </summary>
    TOKEN_KIND_KEYWORD_TRUE,
    /// <summary>
    /// false
    /// </summary>
    TOKEN_KIND_KEYWORD_FALSE,
    /// <summary>
    /// new
    /// </summary>
    TOKEN_KIND_KEYWORD_NEW,
    /// <summary>
    /// delete
    /// </summary>
    TOKEN_KIND_KEYWORD_DELETE,
    /// <summary>
    /// class
    /// </summary>
    TOKEN_KIND_KEYWORD_CLASS,
    /// <summary>
    /// private
    /// </summary>
    TOKEN_KIND_KEYWORD_PRIVATE,
    /// <summary>
    /// public
    /// </summary>
    TOKEN_KIND_KEYWORD_PUBLIC,
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
    //TOKEN_KIND_KEYWORD_,

    /// <summary>
    /// void
    /// </summary>
    TOKEN_KIND_KEYWORD_TYPE_VOID,
    /// <summary>
    /// int
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

    /// <summary>
    /// 整数。
    /// </summary>
    TOKEN_KIND_LITERAL_INTEGER,
    /// <summary>
    /// 少数。つかえない。
    /// </summary>
    TOKEN_KIND_LITERAL_DECIMAL,
    /// <summary>
    /// "文字列"
    /// </summary>
    TOKEN_KIND_LITERAL_DSTRING,
    /// <summary>
    /// '文字列'。文字なんてものはない。
    /// </summary>
    TOKEN_KIND_LITERAL_SSTRING,
    /// <summary>
    /// `文字列`
    /// </summary>
    TOKEN_KIND_LITERAL_BSTRING,
} TOKEN_KIND;


typedef struct tag_TOKEN_PAIR
{
    TOKEN_KIND kind;
    TEXT word;
} TOKEN_PAIR;


typedef struct tag_TOKEN
{
    SOURCE_POSITION position;
    TOKEN_KIND kind;
    /// <summary>
    /// 解放が必要。
    /// </summary>
    TEXT word;
} TOKEN;


