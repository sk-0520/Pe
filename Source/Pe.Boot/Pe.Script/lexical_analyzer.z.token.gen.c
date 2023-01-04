/* 自動生成: lexical_analyzer.z.token.gen.c.tt */
#include "../Pe.Library/common.h"

#include "lexical_analyzer.h"
#include "lexical_analyzer.z.token.h"

#define MULTI_CHAR_SYMBOL_COUNT (2)
typedef struct tag_MULTI_SYMBOL_TOKEN
{
    TCHAR symbols[MULTI_CHAR_SYMBOL_COUNT];
    /// <summary>
    /// 順々のトークン種別。
    /// </summary>
    /// <list type="number">
    ///     <listheader>
    ///         <term>【定義見出し・見出し】</term>
    ///         <description>【説明見出し】</description>
    ///     </listheader>
    ///     <item>
    ///         <description>symbols[0] のみのトークン</description>
    ///     </item>
    ///     <item>
    ///         <description>symbols[0][1]のトークン</description>
    ///     </item>
    ///     <item>
    ///         <description>symbols[0][1][N..]のトークン</description>
    ///     </item>
    /// </list>
    TOKEN_KIND kinds[MULTI_CHAR_SYMBOL_COUNT];
    /// <summary>
    /// 真の場合にコメント中は無視する
    /// </summary>
    bool skip_comments[MULTI_CHAR_SYMBOL_COUNT];
} MULTI_SYMBOL_TOKEN;

static const SINGLE_SYMBOL_TOKEN script_single_symbol_tokens[] = {
    {
        .symbol = ',',
        .kind = TOKEN_KIND_OP_COMMA,
    },
    {
        .symbol = '.',
        .kind = TOKEN_KIND_OP_DOT,
    },
    {
        .symbol = ';',
        .kind = TOKEN_KIND_OP_SEMICOLON,
    },
    {
        .symbol = ':',
        .kind = TOKEN_KIND_OP_COLON,
    },
    {
        .symbol = '?',
        .kind = TOKEN_KIND_OP_QUESTION,
    },
    {
        .symbol = '\\',
        .kind = TOKEN_KIND_OP_BACKSLASH,
    },
    {
        .symbol = '~',
        .kind = TOKEN_KIND_OP_TILDE,
    },
    {
        .symbol = '@',
        .kind = TOKEN_KIND_OP_AT,
    },
    {
        .symbol = '$',
        .kind = TOKEN_KIND_OP_DOLLAR,
    },
    {
        .symbol = '#',
        .kind = TOKEN_KIND_OP_HASH,
    },
    {
        .symbol = '(',
        .kind = TOKEN_KIND_BRACKET_LPAREN,
    },
    {
        .symbol = ')',
        .kind = TOKEN_KIND_BRACKET_RPAREN,
    },
    {
        .symbol = '{',
        .kind = TOKEN_KIND_BRACKET_LBRACE,
    },
    {
        .symbol = '}',
        .kind = TOKEN_KIND_BRACKET_RBRACE,
    },
    {
        .symbol = '[',
        .kind = TOKEN_KIND_BRACKET_LBRACKET,
    },
    {
        .symbol = ']',
        .kind = TOKEN_KIND_BRACKET_RBRACKET,
    },
};

static const MULTI_SYMBOL_TOKEN script_multi_symbol_tokens[] = {
    // &
    {
        .symbols = { '&', '&' },
        .kinds = { TOKEN_KIND_OP_AMPERSAND, TOKEN_KIND_OP_AND },
        .skip_comments = { true, true },
    },
    // |
    {
        .symbols = { '|', '|' },
        .kinds = { TOKEN_KIND_OP_VERTICALBAR, TOKEN_KIND_OP_OR },
        .skip_comments = { true, true },
    },
    // =
    {
        .symbols = { '=', '=' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_EQUALS },
        .skip_comments = { true, true },
    },
    {
        .symbols = { '=', '>' },
        .kinds = { TOKEN_KIND_OP_ASSIGN, TOKEN_KIND_OP_LAMBDA },
        .skip_comments = { true, true },
    },
    // !
    {
        .symbols = { '!', '=' },
        .kinds = { TOKEN_KIND_OP_EXCLAMATION, TOKEN_KIND_OP_NOT_EQUALS },
        .skip_comments = { true, true },
    },
    // >
    {
        .symbols = { '>', '=' },
        .kinds = { TOKEN_KIND_OP_GREATER, TOKEN_KIND_OP_GREATER_EQUAL },
        .skip_comments = { true, true },
    },
    // <
    {
        .symbols = { '<', '=' },
        .kinds = { TOKEN_KIND_OP_LESS, TOKEN_KIND_OP_LESS_EQUAL },
        .skip_comments = { true, true },
    },
    // +
    {
        .symbols = { '+', '=' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_ADD_ASSIGN },
        .skip_comments = { true, true },
    },
    {
        .symbols = { '+', '+' },
        .kinds = { TOKEN_KIND_OP_PLUS, TOKEN_KIND_OP_INCREMENT },
        .skip_comments = { true, true },
    },
    // -
    {
        .symbols = { '-', '=' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_SUB_ASSIGN },
        .skip_comments = { true, true },
    },
    {
        .symbols = { '-', '-' },
        .kinds = { TOKEN_KIND_OP_MINUS, TOKEN_KIND_OP_DECREMENT },
        .skip_comments = { true, true },
    },
    // *
    {
        .symbols = { '*', '=' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_MUL_ASSIGN },
        .skip_comments = { true, true },
    },
    {
        .symbols = { '*', '/' },
        .kinds = { TOKEN_KIND_OP_STAR, TOKEN_KIND_COMMENT_BLOCK_END },
        .skip_comments = { true, false },
    },
    // /
    {
        .symbols = { '/', '=' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_DIV_ASSIGN },
        .skip_comments = { true, true },
    },
    {
        .symbols = { '/', '/' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_COMMENT_LINE },
        .skip_comments = { true, true },
    },
    {
        .symbols = { '/', '*' },
        .kinds = { TOKEN_KIND_OP_SLASH, TOKEN_KIND_COMMENT_BLOCK_BEGIN },
        .skip_comments = { true, true },
    },
    // %
    {
        .symbols = { '%', '=' },
        .kinds = { TOKEN_KIND_OP_PERCENT, TOKEN_KIND_OP_REM_ASSIGN },
        .skip_comments = { true, true },
    },
};

static const TCHAR script_symbol_tokens[] = {
    '=',
    '+',
    '-',
    '*',
    '/',
    '%',
    ',',
    '.',
    ';',
    ':',
    '?',
    '!',
    '\\',
    '~',
    '@',
    '$',
    '#',
    '&',
    '|',
    '>',
    '<',
    '(',
    ')',
    '{',
    '}',
    '[',
    ']',
};

bool is_comment_token_kind(TOKEN_KIND kind)
{
    return
        kind == TOKEN_KIND_COMMENT_LINE
        ||
        kind == TOKEN_KIND_COMMENT_BLOCK_BEGIN
        ;
}

bool is_synbol_token(TCHAR c)
{
    for (size_t i = 0; i < SIZEOF_ARRAY(script_symbol_tokens); i++) {
        if (c == script_symbol_tokens[i]) {
            return true;
        }
    }

    return false;
}

const SINGLE_SYMBOL_TOKEN* find_single_symbol_token(TCHAR c)
{
    for (size_t i = 0; i < SIZEOF_ARRAY(script_single_symbol_tokens); i++) {
        if (script_single_symbol_tokens[i].symbol == c) {
            return script_single_symbol_tokens + i;
        }
    }

    return NULL;
}

size_t read_multi_symbol_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, TOKEN_KIND last_token_kind, const SOURCE_POSITION* source_position, const PROJECT_SETTING* project_setting)
{
    TCHAR symbols[MULTI_CHAR_SYMBOL_COUNT];
    const size_t symbols_length = SIZEOF_ARRAY(symbols);
    for (size_t i = 0; i < symbols_length; i++) {
        symbols[i] = get_relative_character(source, start_index, i);
    }

    for (size_t i = 0; i < SIZEOF_ARRAY(script_multi_symbol_tokens); i++) {
        const MULTI_SYMBOL_TOKEN* multi_symbol_token = script_multi_symbol_tokens + i;

        // 一文字目が合致しないのであればどう頑張っても比較する意味がない
        if (multi_symbol_token->symbols[0] != symbols[0]) {
            continue;
        }

        // 終端から最初に向かって戻していく
        for (size_t j = symbols_length; j; j--) {
            size_t index = j - 1;
            if (!symbols[index] || multi_symbol_token->kinds[index] == TOKEN_KIND_NONE) {
                continue;
            }

            bool all_symbols_match = true;
            for (size_t k = 0; all_symbols_match && k < j; k++) {
                all_symbols_match &= symbols[k] == multi_symbol_token->symbols[k];
            }
            if (!all_symbols_match) {
                continue;
            }

            if (!is_comment_token_kind(last_token_kind) || !multi_symbol_token->skip_comments[index]) {
                add_token_kind(&token_result->token, multi_symbol_token->kinds[index], source_position);
                return j;
            }
        }
    }

    return 0;
}


