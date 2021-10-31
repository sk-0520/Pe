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

        TEST_METHOD(analyze_newline_test)
        {
            PROJECT_SETTING setting;

            TEXT input = wrap("*\r *\n  *\r\n   *");
            TOKEN_RESULT actual = analyze(NULL, &input, &setting);

            TOKEN* actual1 = (TOKEN*)get_object_list(&actual.token, 0).value;
            TOKEN* actual2 = (TOKEN*)get_object_list(&actual.token, 1).value;
            TOKEN* actual3 = (TOKEN*)get_object_list(&actual.token, 2).value;
            TOKEN* actual4 = (TOKEN*)get_object_list(&actual.token, 3).value;

            Assert::AreEqual((size_t)1, actual1->position.line_number);
            Assert::AreEqual((size_t)2, actual2->position.line_number);
            Assert::AreEqual((size_t)3, actual3->position.line_number);
            Assert::AreEqual((size_t)4, actual4->position.line_number);

            Assert::AreEqual((size_t)0, actual1->position.column_position);
            Assert::AreEqual((size_t)1, actual2->position.column_position);
            Assert::AreEqual((size_t)2, actual3->position.column_position);
            Assert::AreEqual((size_t)3, actual4->position.column_position);

            free_token_result(&actual);
        }

        TEST_METHOD(analyze_block_comment_test)
        {
            PROJECT_SETTING setting;

            TEXT input = wrap("/*");
            TOKEN_RESULT actual = analyze(NULL, &input, &setting);

            TOKEN* actual1 = (TOKEN*)get_object_list(&actual.token, 0).value;
            Assert::AreEqual<int>(TOKEN_KIND_COMMENT_BLOCK_BEGIN, actual1->kind);

            COMPILE_RESULT* actual2 = (COMPILE_RESULT*)get_object_list(&actual.result, 0).value;
            Assert::AreEqual<int>(COMPILE_CODE_NOT_CLOSE_COMMENT, actual2->code);

            free_token_result(&actual);
        }

        TEST_METHOD(analyze_multi_token_test)
        {
            PROJECT_SETTING setting;

            auto tests = {
                DATA(std::vector<int> {TOKEN_KIND_OP_INCREMENT, TOKEN_KIND_OP_PLUS}, wrap("+++")),
                DATA(std::vector<int> {TOKEN_KIND_OP_INCREMENT, TOKEN_KIND_OP_INCREMENT}, wrap("++++")),
                DATA(std::vector<int> {TOKEN_KIND_OP_PLUS}, wrap("+")),
                DATA(std::vector<int> {TOKEN_KIND_OP_PLUS, TOKEN_KIND_OP_PLUS}, wrap("+ +")),
                DATA(std::vector<int> {TOKEN_KIND_OP_PLUS, TOKEN_KIND_OP_INCREMENT, TOKEN_KIND_OP_PLUS}, wrap("+ ++ +")),

                DATA(std::vector<int> {TOKEN_KIND_OP_DECREMENT, TOKEN_KIND_OP_MINUS}, wrap("---")),
                DATA(std::vector<int> {TOKEN_KIND_OP_EQUALS, TOKEN_KIND_OP_ASSIGN}, wrap("===")),
                DATA(std::vector<int> {TOKEN_KIND_OP_LESS, TOKEN_KIND_OP_GREATER, TOKEN_KIND_OP_LESS_EQUAL, TOKEN_KIND_OP_GREATER_EQUAL, TOKEN_KIND_OP_LAMBDA }, wrap("<><=>==>")),

                DATA(std::vector<int> {TOKEN_KIND_OP_INCREMENT, TOKEN_KIND_OP_PLUS, TOKEN_KIND_OP_ADD_ASSIGN, TOKEN_KIND_OP_ASSIGN}, wrap("++ + += =")),
                DATA(std::vector<int> {TOKEN_KIND_OP_STAR, TOKEN_KIND_OP_STAR, TOKEN_KIND_OP_STAR, TOKEN_KIND_OP_MUL_ASSIGN, TOKEN_KIND_COMMENT_BLOCK_END}, wrap("** * *= */")),
                DATA(std::vector<int> {TOKEN_KIND_OP_SLASH, TOKEN_KIND_OP_DIV_ASSIGN, TOKEN_KIND_COMMENT_LINE}, wrap("/ /= //")),

                DATA(std::vector<int> {TOKEN_KIND_COMMENT_LINE}, wrap("// *")),
                DATA(std::vector<int> {TOKEN_KIND_COMMENT_LINE}, wrap("//*")),
                DATA(std::vector<int> {TOKEN_KIND_COMMENT_BLOCK_BEGIN}, wrap("/*")),
                DATA(std::vector<int> {TOKEN_KIND_COMMENT_BLOCK_END}, wrap("*/")),
                DATA(std::vector<int> {TOKEN_KIND_COMMENT_BLOCK_BEGIN, TOKEN_KIND_COMMENT_BLOCK_END}, wrap("/**/")),
                DATA(std::vector<int> {TOKEN_KIND_COMMENT_BLOCK_BEGIN, TOKEN_KIND_COMMENT_BLOCK_END, TOKEN_KIND_OP_STAR}, wrap("/***/*")),
                DATA(std::vector<int> {TOKEN_KIND_COMMENT_LINE, TOKEN_KIND_OP_STAR}, wrap("//\n*")),

                DATA(std::vector<int> {TOKEN_KIND_OP_PERCENT, TOKEN_KIND_OP_REM_ASSIGN}, wrap("%%=")),
                DATA(std::vector<int> {TOKEN_KIND_OP_EXCLAMATION, TOKEN_KIND_OP_NOT_EQUALS}, wrap("!!=")),
                DATA(std::vector<int> {TOKEN_KIND_OP_AND, TOKEN_KIND_OP_AMPERSAND}, wrap("&&&")),
                DATA(std::vector<int> {TOKEN_KIND_OP_OR, TOKEN_KIND_OP_VERTICALBAR}, wrap("|||")),
            };
            for (auto test : tests) {
                auto arg1 = std::get<0>(test.inputs);
                TOKEN_RESULT actual = analyze(NULL, &arg1, &setting);
                Assert::AreEqual(test.expected.size(), actual.token.length, arg1.value);
                for (size_t i = 0; i < test.expected.size(); i++) {
                    TOKEN* actual_token = (TOKEN*)get_object_list(&actual.token, i).value;
                    Assert::AreEqual<int>(test.expected[i], actual_token->kind);
                }
                free_token_result(&actual);
            }
        }

        TEST_METHOD(analyze_single_character_token_test)
        {
            PROJECT_SETTING setting;

            auto tests = {
                DATA(std::vector<int> {TOKEN_KIND_OP_COMMA}, wrap(",")),
                DATA(std::vector<int> {TOKEN_KIND_OP_DOT}, wrap(".")),
                DATA(std::vector<int> {TOKEN_KIND_OP_SEMICOLON}, wrap(";")),
                DATA(std::vector<int> {TOKEN_KIND_OP_COLON}, wrap(":")),
                DATA(std::vector<int> {TOKEN_KIND_OP_QUESTION}, wrap("?")),
                DATA(std::vector<int> {TOKEN_KIND_OP_BACKSLASH}, wrap("\\")),
                DATA(std::vector<int> {TOKEN_KIND_OP_TILDE}, wrap("~")),
                DATA(std::vector<int> {TOKEN_KIND_OP_AT}, wrap("@")),
                DATA(std::vector<int> {TOKEN_KIND_OP_DOLLAR}, wrap("$")),
                DATA(std::vector<int> {TOKEN_KIND_OP_HASH}, wrap("#")),
                DATA(std::vector<int> {TOKEN_KIND_OP_LPAREN, TOKEN_KIND_OP_RPAREN}, wrap("()")),
                DATA(std::vector<int> {TOKEN_KIND_OP_LBRACE, TOKEN_KIND_OP_RBRACE}, wrap("{}")),
                DATA(std::vector<int> {TOKEN_KIND_OP_LBRACKET, TOKEN_KIND_OP_RBRACKET}, wrap("[]")),
            };
            for (auto test : tests) {
                auto arg1 = std::get<0>(test.inputs);
                TOKEN_RESULT actual = analyze(NULL, &arg1, &setting);
                Assert::AreEqual(test.expected.size(), actual.token.length, arg1.value);
                for (size_t i = 0; i < test.expected.size(); i++) {
                    TOKEN* actual_token = (TOKEN*)get_object_list(&actual.token, i).value;
                    Assert::AreEqual<int>(test.expected[i], actual_token->kind);
                }
                free_token_result(&actual);
            }
        }

        TEST_METHOD(analyze_string_not_close_1_test)
        {
            PROJECT_SETTING setting;

            auto tests = {
                DATA((size_t)0, wrap("\'")),
                DATA((size_t)0, wrap("\'\r")),
                DATA((size_t)0, wrap("\'\n")),
                DATA((size_t)1, wrap("\'\'\'")),
                DATA((size_t)1, wrap("\'\'    \'        Z")),
                DATA((size_t)1, wrap("\'\'    \'        Z\r")),

                DATA((size_t)0, wrap("\"")),
                DATA((size_t)0, wrap("\"\r")),
                DATA((size_t)0, wrap("\"\n")),
                DATA((size_t)1, wrap("\"\"\"")),
                DATA((size_t)1, wrap("\"\"    \"        Z")),
                DATA((size_t)1, wrap("\"\"    \"        Z\r")),
            };
            for (auto test : tests) {
                auto arg1 = std::get<0>(test.inputs);
                TOKEN_RESULT actual = analyze(NULL, &arg1, &setting);
                Assert::AreEqual(test.expected, actual.token.length, arg1.value);
                size_t error_count = 0;
                for (size_t i = 0; i < actual.result.length; i++) {
                    COMPILE_RESULT* cr = (COMPILE_RESULT*)get_object_list(&actual.result, i).value;
                    if (cr->kind == COMPILE_RESULT_KIND_ERROR && cr->code == COMPILE_CODE_NOT_CLOSE_STRING) {
                        error_count += 1;
                    }
                }
                Assert::IsTrue(actual.result.length, arg1.value);
                free_token_result(&actual);
            }
        }

        struct STRING_TEST
        {
            TOKEN_KIND kind;
            TEXT word;
        };

        TEST_METHOD(analyze_string_sq_test)
        {
            PROJECT_SETTING setting;

            auto tests = {
                DATA(std::vector<STRING_TEST> { { TOKEN_KIND_LITERAL_SSTRING, wrap("") }  }, wrap("\'\'")),
                DATA(std::vector<STRING_TEST> { { TOKEN_KIND_LITERAL_SSTRING, wrap(" ") }  }, wrap("\' \'")),
                DATA(std::vector<STRING_TEST> { { TOKEN_KIND_LITERAL_SSTRING, wrap(" ") }, { TOKEN_KIND_OP_STAR, wrap("") }  }, wrap("\' \'*")),
                DATA(std::vector<STRING_TEST> { { TOKEN_KIND_LITERAL_SSTRING, wrap("A") }, { TOKEN_KIND_LITERAL_SSTRING, wrap("B") }  }, wrap("\'A\' \'B\'")),
                DATA(std::vector<STRING_TEST> { { TOKEN_KIND_LITERAL_SSTRING, wrap("\\r\\n\\''\\t") }  }, wrap("\'\\r\\n\\\\'\\'\\t'")),
            };
            for (auto test : tests) {
                auto arg1 = std::get<0>(test.inputs);
                TOKEN_RESULT actual = analyze(NULL, &arg1, &setting);
                Assert::AreEqual(test.expected.size(), actual.token.length, arg1.value);
                for (size_t i = 0; i < test.expected.size(); i++) {
                    TOKEN* actual_token = (TOKEN*)get_object_list(&actual.token, i).value;
                    Assert::AreEqual<int>(test.expected[i].kind, actual_token->kind);
                    if (test.expected[i].kind == TOKEN_KIND_LITERAL_SSTRING || test.expected[i].kind == TOKEN_KIND_LITERAL_DSTRING || test.expected[i].kind == TOKEN_KIND_LITERAL_BSTRING) {
                        Assert::AreEqual(test.expected[i].word.value, actual_token->word.value);
                    }
                }
                free_token_result(&actual);
            }
        }

        TEST_METHOD(analyze_string_dq_test)
        {
            PROJECT_SETTING setting;

            auto tests = {
                DATA(std::vector<STRING_TEST> { { TOKEN_KIND_LITERAL_DSTRING, wrap("") }  }, wrap("\"\"")),
                DATA(std::vector<STRING_TEST> { { TOKEN_KIND_LITERAL_DSTRING, wrap(" ") }  }, wrap("\" \"")),
                DATA(std::vector<STRING_TEST> { { TOKEN_KIND_LITERAL_DSTRING, wrap(" ") }, { TOKEN_KIND_OP_STAR, wrap("") }  }, wrap("\" \"*")),
                DATA(std::vector<STRING_TEST> { { TOKEN_KIND_LITERAL_DSTRING, wrap("A") }, { TOKEN_KIND_LITERAL_DSTRING, wrap("B") }  }, wrap("\"A\" \"B\"")),
                DATA(std::vector<STRING_TEST> { { TOKEN_KIND_LITERAL_DSTRING, wrap("\\\r\n\"'\t\a\b\f\v") }  }, wrap("\"\\\\\\r\\n\\\"\\'\\t\\a\\b\\f\\v\"")),
            };
            for (auto test : tests) {
                auto arg1 = std::get<0>(test.inputs);
                TOKEN_RESULT actual = analyze(NULL, &arg1, &setting);
                Assert::AreEqual(test.expected.size(), actual.token.length, arg1.value);
                for (size_t i = 0; i < test.expected.size(); i++) {
                    TOKEN* actual_token = (TOKEN*)get_object_list(&actual.token, i).value;
                    Assert::AreEqual<int>(test.expected[i].kind, actual_token->kind);
                    if (test.expected[i].kind == TOKEN_KIND_LITERAL_SSTRING || test.expected[i].kind == TOKEN_KIND_LITERAL_DSTRING || test.expected[i].kind == TOKEN_KIND_LITERAL_BSTRING) {
                        Assert::AreEqual(test.expected[i].word.value, actual_token->word.value);
                    }
                }
                free_token_result(&actual);
            }
        }

        TEST_METHOD(analyze_string_dq_esc_test)
        {
            PROJECT_SETTING setting;
            TEXT input = wrap("\"\\Q\"");
            TOKEN_RESULT actual = analyze(NULL, &input, &setting);
            Assert::AreEqual((size_t)0, actual.token.length);
            Assert::AreEqual((size_t)1, actual.result.length);
            COMPILE_RESULT* cr = (COMPILE_RESULT*)get_object_list(&actual.result, 0).value;
            Assert::AreEqual<int>(COMPILE_CODE_UNKNOWN_ESCAPE_SEQUENCE, cr->code);

            free_token_result(&actual);
        }

        TEST_METHOD(analyze_string_bq_notimpl_test)
        {
            PROJECT_SETTING setting;
            TEXT input = wrap("`a`");
            TOKEN_RESULT actual = analyze(NULL, &input, &setting);
            Assert::AreEqual((size_t)0, actual.token.length);
            Assert::AreEqual((size_t)1, actual.result.length);
            COMPILE_RESULT* cr = (COMPILE_RESULT*)get_object_list(&actual.result, 0).value;
            Assert::AreEqual<int>(COMPILE_CODE_NOT_IMPLEMENT, cr->code);

            free_token_result(&actual);
        }
    };
}
