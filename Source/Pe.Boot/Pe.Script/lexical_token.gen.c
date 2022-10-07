/* 自動生成: lexical_token.c.tt */
#include "../Pe.Library/debug.h"
#include "../Pe.Library/object_list.h"
#include "lexical_token.gen.h"

static const TEXT script__token_kind_member_names[] = {
    static_text("TOKEN_KIND_OP_ASSIGN"),
    static_text("TOKEN_KIND_OP_PLUS"),
    static_text("TOKEN_KIND_OP_MINUS"),
    static_text("TOKEN_KIND_OP_STAR"),
    static_text("TOKEN_KIND_OP_SLASH"),
    static_text("TOKEN_KIND_OP_PERCENT"),
    static_text("TOKEN_KIND_OP_COMMA"),
    static_text("TOKEN_KIND_OP_DOT"),
    static_text("TOKEN_KIND_OP_SEMICOLON"),
    static_text("TOKEN_KIND_OP_COLON"),
    static_text("TOKEN_KIND_OP_QUESTION"),
    static_text("TOKEN_KIND_OP_EXCLAMATION"),
    static_text("TOKEN_KIND_OP_BACKSLASH"),
    static_text("TOKEN_KIND_OP_TILDE"),
    static_text("TOKEN_KIND_OP_AT"),
    static_text("TOKEN_KIND_OP_DOLLAR"),
    static_text("TOKEN_KIND_OP_HASH"),
    static_text("TOKEN_KIND_OP_AMPERSAND"),
    static_text("TOKEN_KIND_OP_VERTICALBAR"),
    static_text("TOKEN_KIND_OP_AND"),
    static_text("TOKEN_KIND_OP_OR"),
    static_text("TOKEN_KIND_OP_EQUALS"),
    static_text("TOKEN_KIND_OP_NOT_EQUALS"),
    static_text("TOKEN_KIND_OP_GREATER"),
    static_text("TOKEN_KIND_OP_LESS"),
    static_text("TOKEN_KIND_OP_GREATER_EQUAL"),
    static_text("TOKEN_KIND_OP_LESS_EQUAL"),
    static_text("TOKEN_KIND_OP_LAMBDA"),
    static_text("TOKEN_KIND_OP_ADD_ASSIGN"),
    static_text("TOKEN_KIND_OP_SUB_ASSIGN"),
    static_text("TOKEN_KIND_OP_MUL_ASSIGN"),
    static_text("TOKEN_KIND_OP_DIV_ASSIGN"),
    static_text("TOKEN_KIND_OP_REM_ASSIGN"),
    static_text("TOKEN_KIND_OP_INCREMENT"),
    static_text("TOKEN_KIND_OP_DECREMENT"),
    static_text("TOKEN_KIND_BRACKET_LPAREN"),
    static_text("TOKEN_KIND_BRACKET_RPAREN"),
    static_text("TOKEN_KIND_BRACKET_LBRACE"),
    static_text("TOKEN_KIND_BRACKET_RBRACE"),
    static_text("TOKEN_KIND_BRACKET_LBRACKET"),
    static_text("TOKEN_KIND_BRACKET_RBRACKET"),
    static_text("TOKEN_KIND_COMMENT_LINE"),
    static_text("TOKEN_KIND_COMMENT_BLOCK_BEGIN"),
    static_text("TOKEN_KIND_COMMENT_BLOCK_END"),
    static_text("TOKEN_KIND_LITERAL_INTEGER"),
    static_text("TOKEN_KIND_LITERAL_DECIMAL"),
    static_text("TOKEN_KIND_LITERAL_SSTRING"),
    static_text("TOKEN_KIND_LITERAL_DSTRING"),
    static_text("TOKEN_KIND_LITERAL_BSTRING"),
    static_text("TOKEN_KIND_KEYWORD_IF"),
    static_text("TOKEN_KIND_KEYWORD_ELSE"),
    static_text("TOKEN_KIND_KEYWORD_FOR"),
    static_text("TOKEN_KIND_KEYWORD_FOREACH"),
    static_text("TOKEN_KIND_KEYWORD_DO"),
    static_text("TOKEN_KIND_KEYWORD_WHILE"),
    static_text("TOKEN_KIND_KEYWORD_BREAK"),
    static_text("TOKEN_KIND_KEYWORD_CONTINUE"),
    static_text("TOKEN_KIND_KEYWORD_GOTO"),
    static_text("TOKEN_KIND_KEYWORD_SWITCH"),
    static_text("TOKEN_KIND_KEYWORD_DEFAULT"),
    static_text("TOKEN_KIND_KEYWORD_CASE"),
    static_text("TOKEN_KIND_KEYWORD_VAR"),
    static_text("TOKEN_KIND_KEYWORD_VAL"),
    static_text("TOKEN_KIND_KEYWORD_CONST"),
    static_text("TOKEN_KIND_KEYWORD_STATIC"),
    static_text("TOKEN_KIND_KEYWORD_FUNCTION"),
    static_text("TOKEN_KIND_KEYWORD_RETURN"),
    static_text("TOKEN_KIND_KEYWORD_SCOPE"),
    static_text("TOKEN_KIND_KEYWORD_TRY"),
    static_text("TOKEN_KIND_KEYWORD_CATCH"),
    static_text("TOKEN_KIND_KEYWORD_FINALLY"),
    static_text("TOKEN_KIND_KEYWORD_THROW"),
    static_text("TOKEN_KIND_KEYWORD_IMPORT"),
    static_text("TOKEN_KIND_KEYWORD_TRUE"),
    static_text("TOKEN_KIND_KEYWORD_FALSE"),
    static_text("TOKEN_KIND_KEYWORD_STRUCT"),
    static_text("TOKEN_KIND_KEYWORD_INTERFACE"),
    static_text("TOKEN_KIND_KEYWORD_PRIVATE"),
    static_text("TOKEN_KIND_KEYWORD_IN"),
    static_text("TOKEN_KIND_KEYWORD_OUT"),
    static_text("TOKEN_KIND_KEYWORD_PROPERTY"),
    static_text("TOKEN_KIND_KEYWORD_GET"),
    static_text("TOKEN_KIND_KEYWORD_SET"),
    static_text("TOKEN_KIND_KEYWORD_VALUE"),
    static_text("TOKEN_KIND_KEYWORD_ARG"),
    static_text("TOKEN_KIND_KEYWORD_TYPE_VOID"),
    static_text("TOKEN_KIND_KEYWORD_TYPE_INTEGER"),
    static_text("TOKEN_KIND_KEYWORD_TYPE_DECIMAL"),
    static_text("TOKEN_KIND_KEYWORD_TYPE_STRING"),
};

const TEXT* get_member_name_by_token_kind(TOKEN_KIND kind)
{
    assert(kind < SIZEOF_ARRAY(script__token_kind_member_names));

    return script__token_kind_member_names + kind;
}


void add_token_kind(OBJECT_LIST* tokens, TOKEN_KIND kind, const SOURCE_POSITION* source_position)
{
    TOKEN token = {
        .kind = kind,
        .position = *source_position,
        .type = TOKEN_VALUE_TYPE_NONE,
        .value = {
            .none = NULL,
        },
    };

    push_object_list(tokens, &token);
}

static void add_token_value_core(OBJECT_LIST* tokens, TOKEN_KIND kind, TOKEN_VALUE_TYPE type, TOKEN_VALUE value, const SOURCE_POSITION* source_position)
{
    TOKEN token = {
        .kind = kind,
        .position = *source_position,
        .type = type,
        .value = value,
    };

    push_object_list(tokens, &token);
}

void add_token_word(OBJECT_LIST* tokens, TOKEN_KIND kind, const TEXT* word, const SOURCE_POSITION* source_position)
{
    TOKEN_VALUE token_value = {
        .word = clone_text(word, SCRIPT_MEMORY),
    };
    add_token_value_core(tokens, kind, TOKEN_VALUE_TYPE_STRING, token_value, source_position);
}

void add_token_integer(OBJECT_LIST* tokens, TOKEN_KIND kind, ssize_t value, const SOURCE_POSITION* source_position)
{
    TOKEN_VALUE token_value = {
        .integer = value,
    };
    add_token_value_core(tokens, kind, TOKEN_VALUE_TYPE_INTEGER, token_value, source_position);
}

void add_token_decimal(OBJECT_LIST* tokens, TOKEN_KIND kind, double value, const SOURCE_POSITION* source_position)
{
    TOKEN_VALUE token_value = {
        .decimal = value,
    };
    add_token_value_core(tokens, kind, TOKEN_VALUE_TYPE_DECIMAL, token_value, source_position);
}


