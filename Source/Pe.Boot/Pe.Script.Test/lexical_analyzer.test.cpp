#include "pch.h"

extern "C" {
#   include "../Pe.Script/lexical_analyzer.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace ScriptTest
{
    TEST_CLASS(lexical_analyzer_test)
    {
    public:

        TEST_METHOD(analyze_empty_test)
        {
            PROJECT_SETTING setting;
            TOKEN_RESULT token_result = analyze(NULL, NULL, &setting);

            Assert::AreEqual((size_t)0, token_result.token.length);
            Assert::AreEqual((size_t)0, token_result.result.length);
            free_token_result(&token_result);
        }

        TEST_METHOD(analyze_plus_inc_test)
        {
            PROJECT_SETTING setting;

            auto tests = {
                DATA(std::vector<int> {TOKEN_KIND_OP_INCREMENT, TOKEN_KIND_OP_PLUS}, wrap("+++")),
                DATA(std::vector<int> {TOKEN_KIND_OP_INCREMENT, TOKEN_KIND_OP_INCREMENT}, wrap("++++")),
                DATA(std::vector<int> {TOKEN_KIND_OP_PLUS}, wrap("+")),
                DATA(std::vector<int> {TOKEN_KIND_OP_PLUS, TOKEN_KIND_OP_PLUS}, wrap("+ +")),
                DATA(std::vector<int> {TOKEN_KIND_OP_PLUS, TOKEN_KIND_OP_INCREMENT, TOKEN_KIND_OP_PLUS}, wrap("+ ++ +")),
            };
            for (auto test : tests) {
                auto arg1 = std::get<0>(test.inputs);
                TOKEN_RESULT actual = analyze(NULL, &arg1, &setting);
                Assert::AreEqual(test.expected.size(), actual.token.length);
                for (size_t i = 0; i < test.expected.size(); i++) {
                    TOKEN* actual_token = (TOKEN*)get_object_list(&actual.token, i).value;
                    Assert::AreEqual<int>(test.expected[i], actual_token->kind);
                }
                free_token_result(&actual);
            }
        }
    };
}
