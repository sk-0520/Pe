#pragma once
#include <stdbool.h>

#include "token.h"

typedef struct tag_SINGLE_CHAR_TOKEN
{
    TCHAR character;
    TOKEN_KIND kind;
} SINGLE_CHAR_TOKEN;

#define MULTI_CHAR_TOKEN_FIRST (0)
#define MULTI_CHAR_TOKEN_SECOND (1)
#define MULTI_CHAR_TOKEN_COUNT (4)
typedef struct tag_MULTI_CHAR_TOKEN
{
    TCHAR characters[MULTI_CHAR_TOKEN_COUNT];
    /// <summary>
    /// MULTI_CHAR_TOKEN_FIRST: characters[MULTI_CHAR_TOKEN_FIRST] のみのトークン
    /// MULTI_CHAR_TOKEN_SECOND: characters[MULTI_CHAR_TOKEN_FIRST][MULTI_CHAR_TOKEN_SECOND]のトークン
    /// </summary>
    TOKEN_KIND kinds[MULTI_CHAR_TOKEN_COUNT];
    /// <summary>
    /// 真の場合にコメント中は無視する
    /// </summary>
    bool skip_comments[MULTI_CHAR_TOKEN_COUNT];
} MULTI_CHAR_TOKEN;

/// <summary>
/// 指定トークンはコメントか。
/// </summary>
/// <param name="kind"></param>
/// <returns></returns>
bool is_comment_token_kind(TOKEN_KIND kind);

/// <summary>
/// 単一文字で構成されるトークンを取得。
/// </summary>
/// <param name="c"></param>
/// <returns>取得できたトークン設定。取得できない場合は<c>NULL</c></returns>
const SINGLE_CHAR_TOKEN* find_single_character_token(TCHAR c);

/// <summary>
/// 複数文字で構成されるトークンを読み込み。
/// </summary>
/// <param name="token_result">結果格納</param>
/// <param name="source">ソース全体</param>
/// <param name="start_index">複数文字トークンとしての開始点</param>
/// <param name="last_token_kind">前回トークン種別</param>
/// <param name="source_position"></param>
/// <param name="project_setting"></param>
/// <returns>読み込み成功後に飛ばす長さ(start_indexからの相対位置)。0の場合は失敗しているので後続不要。</returns>
size_t read_multi_character_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, TOKEN_KIND last_token_kind, const SOURCE_POSITION* source_position, const PROJECT_SETTING* project_setting);
