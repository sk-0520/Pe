#include "compile.h"
#include "script.h"

static bool is_error(COMPILE_RESULT_KIND compile_result_kind, const PROJECT_SETTING* project_setting)
{
    assert(project_setting);

    if (compile_result_kind == COMPILE_RESULT_KIND_ERROR) {
        return true;
    }

    if (project_setting->warning_is_error) {
        if (compile_result_kind == COMPILE_RESULT_KIND_WARNING) {
            return true;
        }
    }

    return false;
}

static size_t get_error_count(OBJECT_LIST* compile_results, const PROJECT_SETTING* project_setting)
{
    size_t error_count = 0;

    for (size_t i = 0; i < compile_results->length; i++) {
        const COMPILE_RESULT* compile_result = (COMPILE_RESULT*)get_object_list(compile_results, i).value;
        if (is_error(compile_result->kind, project_setting)) {
            error_count += 1;
        }
    }

    return error_count;
}

void add_compile_result(OBJECT_LIST* compile_results, COMPILE_RESULT_KIND kind, COMPILE_CODE code, const TEXT* remark, const SOURCE_POSITION* source_position)
{
    TEXT auto_remark;
    if (!remark) {
        switch (code) {
            case COMPILE_CODE_NOT_IMPLEMENT:
                auto_remark = wrap_text(_T("未実装"));
                break;

            case COMPILE_CODE_NOT_IMPLEMENT_DECIMAL:
                auto_remark = wrap_text(_T("非整数は未実装"));
                break;

            case COMPILE_CODE_NOT_IMPLEMENT_KEYWORD:
                auto_remark = wrap_text(_T("使用できない予約語"));
                break;

            case COMPILE_CODE_NOT_CLOSE_COMMENT:
                auto_remark = wrap_text(_T("コメントが閉じられていない"));
                break;

            case COMPILE_CODE_NOT_CLOSE_STRING:
                auto_remark = wrap_text(_T("文字列が閉じられていない"));
                break;

            case COMPILE_CODE_UNKNOWN_ESCAPE_SEQUENCE:
                auto_remark = wrap_text(_T("不明なエスケープシーケンス"));
                break;

            case COMPILE_CODE_INVALID_NUMBER:
                auto_remark = wrap_text(_T("不正な数値"));
                break;

            case COMPILE_CODE_PAESE_ERROR_NUMBER:
                auto_remark = wrap_text(_T("数値変換失敗"));
                break;

            case COMPILE_CODE_INVALID_WORD:
                auto_remark = wrap_text(_T("単語として使用できない文字を含んでいる"));
                break;

            default:
                assert(false);
                break;
        }
        remark = &auto_remark;
    }

    COMPILE_RESULT compile_result = {
        .stage = COMPILE_STAGE_LEX,
        .kind = kind,
        .code = code,
        .remark = clone_text(remark),
        .position = *source_position,
    };

    push_object_list(compile_results, &compile_result);
}

bool has_error(OBJECT_LIST* compile_results, const PROJECT_SETTING* project_setting)
{
    return 0 < get_error_count(compile_results, project_setting);
}
