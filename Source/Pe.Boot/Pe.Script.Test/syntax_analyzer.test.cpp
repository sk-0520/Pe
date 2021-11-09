#include "pch.h"

extern "C" {
#   include "../Pe.Script/syntax_analyzer.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace ScriptTest
{
    TEST_CLASS(lexical_analyzer_test)
    {
    public:
        TEST_METHOD(test)
        {
            PROJECT_SETTING setting;

            TEXT source = wrap("123");
            TOKEN_RESULT token_result = analyze_lexical(NULL, &source, &setting);
            analyze_syntax(&token_result, &setting);
        }

    };
}
