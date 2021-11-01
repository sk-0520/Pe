#include "../Pe.Library/common.h"

#include "lexical_analyzer.h"
#include "lexical_analyzer.z.token.h"

static SINGLE_SYMBOL_TOKEN library__single_symbol_tokens[] = {
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
        .kind = TOKEN_KIND_OP_LPAREN,
    },
    {
        .symbol = ')',
        .kind = TOKEN_KIND_OP_RPAREN,
    },
    {
        .symbol = '{',
        .kind = TOKEN_KIND_OP_LBRACE,
    },
    {
        .symbol = '}',
        .kind = TOKEN_KIND_OP_RBRACE,
    },
    {
        .symbol = '[',
        .kind = TOKEN_KIND_OP_LBRACKET,
    },
    {
        .symbol = ']',
        .kind = TOKEN_KIND_OP_RBRACKET,
    },
};

#if MULTI_CHAR_SYMBOL_COUNT == 2
#   define NONE_SYMBOL_SKIP_2
#   define NONE_KIND_SKIP_2
#   define COMMENT_ALL_SKIP true, true
#elif MULTI_CHAR_SYMBOL_COUNT == 3
#   define NONE_SYMBOL_SKIP_2 '\0'
#   define NONE_KIND_SKIP_2 TOKEN_KIND_NONE
#   define COMMENT_ALL_SKIP true, true, true
#elif MULTI_CHAR_SYMBOL_COUNT == 4
#   define NONE_SYMBOL_SKIP_2 '\0', '\0'
#   define NONE_KIND_SKIP_2 TOKEN_KIND_NONE, TOKEN_KIND_NONE
#   define COMMENT_ALL_SKIP true, true, true, true
#else
#   error !MULTI_CHAR_SYMBOL_COUNT!
#endif

static MULTI_SYMBOL_TOKEN library__multi_symbol_tokens[] = {
    // +
    {
        .symbols = { '+', '=', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_ADD_ASSIGN, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    {
        .symbols = { '+', '+', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_OP_PLUS, TOKEN_KIND_OP_INCREMENT, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    // -
    {
        .symbols = { '-', '=', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_SUB_ASSIGN, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    {
        .symbols = { '-', '-', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_OP_MINUS, TOKEN_KIND_OP_DECREMENT, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    // *
    {
        .symbols = { '*', '=', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_MUL_ASSIGN, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    {
        .symbols = { '*', '/', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_OP_STAR, TOKEN_KIND_COMMENT_BLOCK_END, NONE_KIND_SKIP_2 },
        .skip_comments = { true, false, /*true, true,*/ },
    },
    // /
    {
        .symbols = { '/', '/', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_COMMENT_LINE, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    {
        .symbols = { '/', '*', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_COMMENT_BLOCK_BEGIN, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    {
        .symbols = { '/', '=', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_OP_SLASH, TOKEN_KIND_OP_DIV_ASSIGN, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    // %
    {
        .symbols = { '%', '=', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_OP_PERCENT, TOKEN_KIND_OP_REM_ASSIGN, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    // =
    {
        .symbols = { '=', '>', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_LAMBDA, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    {
        .symbols = { '=', '=', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_OP_ASSIGN, TOKEN_KIND_OP_EQUALS, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    // <
    {
        .symbols = { '<', '=', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_OP_LESS, TOKEN_KIND_OP_LESS_EQUAL, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    // >
    {
        .symbols = { '>', '=', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_OP_GREATER, TOKEN_KIND_OP_GREATER_EQUAL, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    // !
    {
        .symbols = { '!', '=', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_OP_EXCLAMATION, TOKEN_KIND_OP_NOT_EQUALS, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    // &
    {
        .symbols = { '&', '&', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_OP_AMPERSAND, TOKEN_KIND_OP_AND, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
    // |
    {
        .symbols = { '|', '|', NONE_SYMBOL_SKIP_2 },
        .kinds = { TOKEN_KIND_OP_VERTICALBAR, TOKEN_KIND_OP_OR, NONE_KIND_SKIP_2 },
        .skip_comments = { COMMENT_ALL_SKIP },
    },
};

bool is_comment_token_kind(TOKEN_KIND kind)
{
    return
        kind == TOKEN_KIND_COMMENT_LINE
        ||
        kind == TOKEN_KIND_COMMENT_BLOCK_BEGIN
        ;
}

const SINGLE_SYMBOL_TOKEN* find_single_symbol_token(TCHAR c)
{
    for (size_t i = 0; i < sizeof(library__single_symbol_tokens) / sizeof(library__single_symbol_tokens[0]); i++) {
        if (library__single_symbol_tokens[i].symbol == c) {
            return library__single_symbol_tokens + i;
        }
    }

    return NULL;
}

size_t read_multi_symbol_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, TOKEN_KIND last_token_kind, const SOURCE_POSITION* source_position, const PROJECT_SETTING* project_setting)
{
    TCHAR symbols[MULTI_CHAR_SYMBOL_COUNT];
    const size_t symbols_length = sizeof(symbols) / sizeof(symbols[0]);
    for (size_t i = 0; i < symbols_length; i++) {
        symbols[i] = get_relative_character(source, start_index, i);
    }

    for (size_t i = 0; i < sizeof(library__multi_symbol_tokens) / sizeof(library__multi_symbol_tokens[0]); i++) {
        const MULTI_SYMBOL_TOKEN* multi_symbol_token = library__multi_symbol_tokens + i;

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
