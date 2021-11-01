#pragma once
#include "lexical_analyzer.h"

/// <summary>
/// 文字列を読み込み。
/// 呼ばれる時点で最初の文字は文字列トークン判定済みなので初回の次文字読み込みは絶対に成功する。
/// </summary>
/// <param name="token_result">結果格納</param>
/// <param name="source">ソース全体</param>
/// <param name="start_index">文字列トークンとしての開始点</param>
/// <returns>読み込み成功後に飛ばす長さ(start_indexからの相対位置)。0の場合は失敗しているので後続不要。</returns>
size_t read_string_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, size_t column_position, size_t line_number);
