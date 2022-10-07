#pragma once
#include <stdbool.h>

#include "lexical_token.gen.h"

typedef struct tag_SINGLE_SYMBOL_TOKEN
{
    TCHAR symbol;
    TOKEN_KIND kind;
} SINGLE_SYMBOL_TOKEN;

/// <summary>
/// 指定トークンはコメントか。
/// </summary>
/// <param name="kind"></param>
/// <returns></returns>
bool is_comment_token_kind(TOKEN_KIND kind);

/// <summary>
/// 指定文字は記号トークンとして使用されるか。
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
bool is_synbol_token(TCHAR c);

/// <summary>
/// 単一記号で構成されるトークンを取得。
/// </summary>
/// <param name="c"></param>
/// <returns>取得できたトークン設定。取得できない場合は<c>NULL</c></returns>
const SINGLE_SYMBOL_TOKEN* find_single_symbol_token(TCHAR c);

/// <summary>
/// 複数記号で構成されるトークンを読み込み。
/// </summary>
/// <param name="token_result">結果格納</param>
/// <param name="source">ソース全体</param>
/// <param name="start_index">複数記号トークンとしての開始点</param>
/// <param name="last_token_kind">前回トークン種別</param>
/// <param name="source_position"></param>
/// <param name="project_setting"></param>
/// <returns>読み込み成功後に飛ばす長さ(start_indexからの相対位置)。0の場合は失敗しているので後続不要。</returns>
size_t read_multi_symbol_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, TOKEN_KIND last_token_kind, const SOURCE_POSITION* source_position, const PROJECT_SETTING* project_setting);
