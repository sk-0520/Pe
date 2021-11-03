#pragma once
#include "lexical_analyzer.h"

/// <summary>
/// 以降を数値として解釈してよいか。
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
bool is_number_start(TCHAR c);

/// <summary>
/// 数値を読み込み。
/// 呼ばれた時点で最初の文字は数値判定されている。
/// </summary>
/// <param name="token_result">結果格納</param>
/// <param name="source">ソース全体</param>
/// <param name="start_index">数値トークンとしての開始点</param>
/// <param name="column_position"></param>
/// <param name="line_number"></param>
/// <returns>読み込み成功後に飛ばす長さ(start_indexからの相対位置)。0の場合は失敗しているので後続不要だが呼び出し時点で数値なのでまぁ0はない。</returns>
size_t read_number_token(TOKEN_RESULT* token_result, const TEXT* source, size_t start_index, const SOURCE_POSITION* source_position, const PROJECT_SETTING* project_setting);
