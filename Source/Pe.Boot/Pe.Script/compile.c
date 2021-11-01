#include "compile.h"
#include "script.h"

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

