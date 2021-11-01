#include "lexical_analyzer.h"
#include "lexical_analyzer.z.token.h"

static SINGLE_CHAR_TOKEN library__single_character_tokens[] = {
    {
        .character = ',',
        .kind = TOKEN_KIND_OP_COMMA,
    },
    {
        .character = '.',
        .kind = TOKEN_KIND_OP_DOT,
    },
    {
        .character = ';',
        .kind = TOKEN_KIND_OP_SEMICOLON,
    },
    {
        .character = ':',
        .kind = TOKEN_KIND_OP_COLON,
    },
    {
        .character = '?',
        .kind = TOKEN_KIND_OP_QUESTION,
    },
    {
        .character = '\\',
        .kind = TOKEN_KIND_OP_BACKSLASH,
    },
    {
        .character = '~',
        .kind = TOKEN_KIND_OP_TILDE,
    },
    {
        .character = '@',
        .kind = TOKEN_KIND_OP_AT,
    },
    {
        .character = '$',
        .kind = TOKEN_KIND_OP_DOLLAR,
    },
    {
        .character = '#',
        .kind = TOKEN_KIND_OP_HASH,
    },
    {
        .character = '(',
        .kind = TOKEN_KIND_OP_LPAREN,
    },
    {
        .character = ')',
        .kind = TOKEN_KIND_OP_RPAREN,
    },
    {
        .character = '{',
        .kind = TOKEN_KIND_OP_LBRACE,
    },
    {
        .character = '}',
        .kind = TOKEN_KIND_OP_RBRACE,
    },
    {
        .character = '[',
        .kind = TOKEN_KIND_OP_LBRACKET,
    },
    {
        .character = ']',
        .kind = TOKEN_KIND_OP_RBRACKET,
    },
};

static MULTI_CHAR_TOKEN library__multi_tokens[] = {
    // +
    {
        .characters = { '+', '=' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_ADD_ASSIGN },
        .skip_comments = { true, true },
    },
    {
        .characters = { '+', '+' },
        .kinds = { TOKEN_KIND_OP_PLUS, TOKEN_KIND_OP_INCREMENT },
        .skip_comments = { true, true },
    },
    // -
    {
        .characters = { '-', '=' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_SUB_ASSIGN },
        .skip_comments = { true, true },
    },
    {
        .characters = { '-', '-' },
        .kinds = { TOKEN_KIND_OP_MINUS, TOKEN_KIND_OP_DECREMENT },
        .skip_comments = { true, true },
    },
    // *
    {
        .characters = { '*', '=' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_MUL_ASSIGN },
        .skip_comments = { true, true },
    },
    {
        .characters = { '*', '/' },
        .kinds = { TOKEN_KIND_OP_STAR, TOKEN_KIND_COMMENT_BLOCK_END },
        .skip_comments = { true, false },
    },
    // /
    {
        .characters = { '/', '/' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_COMMENT_LINE },
        .skip_comments = { false, false },
    },
    {
        .characters = { '/', '*' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_COMMENT_BLOCK_BEGIN },
        .skip_comments = { false, false },
    },
    {
        .characters = { '/', '=' },
        .kinds = { TOKEN_KIND_OP_SLASH, TOKEN_KIND_OP_DIV_ASSIGN },
        .skip_comments = { false, false },
    },
    // %
    {
        .characters = { '%', '=' },
        .kinds = { TOKEN_KIND_OP_PERCENT, TOKEN_KIND_OP_REM_ASSIGN },
        .skip_comments = { false, false },
    },
    // =
    {
        .characters = { '=', '>' },
        .kinds = { TOKEN_KIND_NONE, TOKEN_KIND_OP_LAMBDA },
        .skip_comments = { false, false },
    },
    {
        .characters = { '=', '=' },
        .kinds = { TOKEN_KIND_OP_ASSIGN, TOKEN_KIND_OP_EQUALS },
        .skip_comments = { false, false },
    },
    // <
    {
        .characters = { '<', '=' },
        .kinds = { TOKEN_KIND_OP_LESS, TOKEN_KIND_OP_LESS_EQUAL },
        .skip_comments = { false, false },
    },
    // >
    {
        .characters = { '>', '=' },
        .kinds = { TOKEN_KIND_OP_GREATER, TOKEN_KIND_OP_GREATER_EQUAL },
        .skip_comments = { false, false },
    },
    // !
    {
        .characters = { '!', '=' },
        .kinds = { TOKEN_KIND_OP_EXCLAMATION, TOKEN_KIND_OP_NOT_EQUALS },
        .skip_comments = { false, false },
    },
    // &
    {
        .characters = { '&', '&' },
        .kinds = { TOKEN_KIND_OP_AMPERSAND, TOKEN_KIND_OP_AND },
        .skip_comments = { false, false },
    },
    // |
    {
        .characters = { '|', '|' },
        .kinds = { TOKEN_KIND_OP_VERTICALBAR, TOKEN_KIND_OP_OR },
        .skip_comments = { false, false },
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

const SINGLE_CHAR_TOKEN* find_single_character_token(TCHAR c)
{
    for (size_t i = 0; i < sizeof(library__single_character_tokens) / sizeof(library__single_character_tokens[0]); i++) {
        if (library__single_character_tokens[i].character == c) {
            return library__single_character_tokens + i;
        }
    }

    return NULL;
}

size_t read_multi_character_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, TOKEN_KIND last_token_kind, const SOURCE_POSITION* source_position, const PROJECT_SETTING* project_setting)
{
    TCHAR current_character = source->value[start_index];
    TCHAR next_character = get_next_character(source, start_index);

    for (size_t i = 0; i < sizeof(library__multi_tokens) / sizeof(library__multi_tokens[0]); i++) {
        const MULTI_CHAR_TOKEN* multi_token = library__multi_tokens + i;
        if (current_character == multi_token->characters[MULTI_CHAR_TOKEN_FIRST]) {
            if (next_character && next_character == multi_token->characters[MULTI_CHAR_TOKEN_SECOND] && multi_token->kinds[MULTI_CHAR_TOKEN_SECOND] != TOKEN_KIND_NONE) {
                if (!is_comment_token_kind(last_token_kind) || !multi_token->skip_comments[MULTI_CHAR_TOKEN_SECOND]) {
                    add_token_kind(&token_result->token, multi_token->kinds[MULTI_CHAR_TOKEN_SECOND], source_position);
                }
                return 2;
            } else if (multi_token->kinds[MULTI_CHAR_TOKEN_FIRST] != TOKEN_KIND_NONE) {
                if (!is_comment_token_kind(last_token_kind) || !multi_token->skip_comments[MULTI_CHAR_TOKEN_FIRST]) {
                    add_token_kind(&token_result->token, multi_token->kinds[MULTI_CHAR_TOKEN_FIRST], source_position);
                }
                return 1;
            }
        }
    }

    return 0;
}
