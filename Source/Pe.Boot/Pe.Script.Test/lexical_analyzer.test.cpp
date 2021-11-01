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

        TEST_METHOD(get_relative_character_test)
        {
            auto tests = {
                DATA(_T('a'), wrap("abc"), 0, 0),
                DATA(_T('b'), wrap("abc"), 0, 1),
                DATA(_T('b'), wrap("abc"), 1, 0),
                DATA(_T('c'), wrap("abc"), 1, 1),
                DATA(_T('\0'), wrap("abc"), 1, 2),

                DATA(_T('a'), wrap("abc"), 1, -1),
                DATA(_T('\0'), wrap("abc"), 1, -2),
            };
            for (auto test : tests) {
                auto arg1 = std::get<0>(test.inputs);
                auto arg2 = std::get<1>(test.inputs);
                auto arg3 = std::get<2>(test.inputs);
                TCHAR actual = get_relative_character(&arg1, arg2, arg3);
                Assert::AreEqual(test.expected, actual, arg1.value);
            }
        }

        TEST_METHOD(get_next_character_test)
        {
            auto tests = {
                DATA(_T('b'), wrap("abc"), 0),
                DATA(_T('c'), wrap("abc"), 1),
                DATA(_T('\0'), wrap("abc"), 2),
            };
            for (auto test : tests) {
                auto arg1 = std::get<0>(test.inputs);
                auto arg2 = std::get<1>(test.inputs);
                TCHAR actual = get_next_character(&arg1, arg2);
                Assert::AreEqual(test.expected, actual, arg1.value);
            }
        }

        TEST_METHOD(lexical_analyze_empty_test)
        {
            PROJECT_SETTING setting;
            TOKEN_RESULT token_result = lexical_analyze(NULL, NULL, &setting);

            Assert::AreEqual((size_t)0, token_result.token.length);
            Assert::AreEqual((size_t)0, token_result.result.length);
            free_token_result(&token_result);
        }

        TEST_METHOD(lexical_analyze_newline_test)
        {
            PROJECT_SETTING setting;

            TEXT input = wrap("*\r *\n  *\r\n   *");
            TOKEN_RESULT actual = lexical_analyze(NULL, &input, &setting);

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

        TEST_METHOD(lexical_analyze_block_comment_test)
        {
            PROJECT_SETTING setting;

            TEXT input = wrap("/*");
            TOKEN_RESULT actual = lexical_analyze(NULL, &input, &setting);

            TOKEN* actual1 = (TOKEN*)get_object_list(&actual.token, 0).value;
            Assert::AreEqual<int>(TOKEN_KIND_COMMENT_BLOCK_BEGIN, actual1->kind);

            COMPILE_RESULT* actual2 = (COMPILE_RESULT*)get_object_list(&actual.result, 0).value;
            Assert::AreEqual<int>(COMPILE_CODE_NOT_CLOSE_COMMENT, actual2->code);

            free_token_result(&actual);
        }

        TEST_METHOD(lexical_analyze_multi_token_test)
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
                DATA(std::vector<int> {TOKEN_KIND_COMMENT_BLOCK_BEGIN, TOKEN_KIND_COMMENT_BLOCK_END, TOKEN_KIND_COMMENT_BLOCK_BEGIN}, wrap("/*/*//*/*")),
                DATA(std::vector<int> {TOKEN_KIND_COMMENT_BLOCK_BEGIN, TOKEN_KIND_COMMENT_BLOCK_END, TOKEN_KIND_OP_STAR}, wrap("/*/* // */*")),
                DATA(std::vector<int> {TOKEN_KIND_COMMENT_LINE, TOKEN_KIND_OP_STAR}, wrap("//\n*")),

                DATA(std::vector<int> {TOKEN_KIND_OP_PERCENT, TOKEN_KIND_OP_REM_ASSIGN}, wrap("%%=")),
                DATA(std::vector<int> {TOKEN_KIND_OP_EXCLAMATION, TOKEN_KIND_OP_NOT_EQUALS}, wrap("!!=")),
                DATA(std::vector<int> {TOKEN_KIND_OP_AND, TOKEN_KIND_OP_AMPERSAND}, wrap("&&&")),
                DATA(std::vector<int> {TOKEN_KIND_OP_OR, TOKEN_KIND_OP_VERTICALBAR}, wrap("|||")),
            };
            for (auto test : tests) {
                auto arg1 = std::get<0>(test.inputs);
                TOKEN_RESULT actual = lexical_analyze(NULL, &arg1, &setting);
                Assert::AreEqual(test.expected.size(), actual.token.length, arg1.value);
                for (size_t i = 0; i < test.expected.size(); i++) {
                    TOKEN* actual_token = (TOKEN*)get_object_list(&actual.token, i).value;
                    Assert::AreEqual<int>(test.expected[i], actual_token->kind);
                }
                free_token_result(&actual);
            }
        }

        TEST_METHOD(lexical_analyze_single_character_token_test)
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
                TOKEN_RESULT actual = lexical_analyze(NULL, &arg1, &setting);
                Assert::AreEqual(test.expected.size(), actual.token.length, arg1.value);
                for (size_t i = 0; i < test.expected.size(); i++) {
                    TOKEN* actual_token = (TOKEN*)get_object_list(&actual.token, i).value;
                    Assert::AreEqual<int>(test.expected[i], actual_token->kind);
                }
                free_token_result(&actual);
            }
        }

        TEST_METHOD(lexical_analyze_string_not_close_1_test)
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
                TOKEN_RESULT actual = lexical_analyze(NULL, &arg1, &setting);
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

        struct VALUE_TEST
        {
        public:
            VALUE_TEST(TOKEN_KIND kind, TEXT value)
            {
                this->kind = kind;
                this->value.word = value;
            }
            VALUE_TEST(TOKEN_KIND kind, int value)
            {
                this->kind = kind;
                this->value.integer = value;
            }
            VALUE_TEST(TOKEN_KIND kind, double value)
            {
                this->kind = kind;
                this->value.decimal = value;
            }

            TOKEN_KIND kind;
            TOKEN_VALUE value;
        };

        TEST_METHOD(lexical_analyze_string_sq_test)
        {
            PROJECT_SETTING setting;

            auto tests = {
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_SSTRING, wrap(""))  }, wrap("\'\'")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_SSTRING, wrap(" "))  }, wrap("\' \'")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_SSTRING, wrap(" ")), { TOKEN_KIND_OP_STAR, wrap("") }  }, wrap("\' \'*")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_SSTRING, wrap("A")), { TOKEN_KIND_LITERAL_SSTRING, wrap("B") }  }, wrap("\'A\' \'B\'")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_SSTRING, wrap("\\r\\n\\''\\t")) }, wrap("\'\\r\\n\\\\'\\'\\t'")),
            };
            for (auto test : tests) {
                auto arg1 = std::get<0>(test.inputs);
                TOKEN_RESULT actual = lexical_analyze(NULL, &arg1, &setting);
                Assert::AreEqual(test.expected.size(), actual.token.length, arg1.value);
                for (size_t i = 0; i < test.expected.size(); i++) {
                    TOKEN* actual_token = (TOKEN*)get_object_list(&actual.token, i).value;
                    Assert::AreEqual<int>(test.expected[i].kind, actual_token->kind);
                    if (test.expected[i].kind == TOKEN_KIND_LITERAL_SSTRING || test.expected[i].kind == TOKEN_KIND_LITERAL_DSTRING || test.expected[i].kind == TOKEN_KIND_LITERAL_BSTRING) {
                        Assert::AreEqual(test.expected[i].value.word.value, actual_token->value.word.value);
                    }
                }
                free_token_result(&actual);
            }
        }

        TEST_METHOD(lexical_analyze_string_dq_test)
        {
            PROJECT_SETTING setting;

            auto tests = {
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_DSTRING, wrap("")) }, wrap("\"\"")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_DSTRING, wrap(" "))  }, wrap("\" \"")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_DSTRING, wrap(" ")), VALUE_TEST(TOKEN_KIND_OP_STAR, wrap("")) }, wrap("\" \"*")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_DSTRING, wrap("A")), VALUE_TEST(TOKEN_KIND_LITERAL_DSTRING, wrap("B"))  }, wrap("\"A\" \"B\"")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_DSTRING, wrap("\\\r\n\"'\t\a\b\f\v")) }, wrap("\"\\\\\\r\\n\\\"\\'\\t\\a\\b\\f\\v\"")),
            };
            for (auto test : tests) {
                auto arg1 = std::get<0>(test.inputs);
                TOKEN_RESULT actual = lexical_analyze(NULL, &arg1, &setting);
                Assert::AreEqual(test.expected.size(), actual.token.length, arg1.value);
                for (size_t i = 0; i < test.expected.size(); i++) {
                    TOKEN* actual_token = (TOKEN*)get_object_list(&actual.token, i).value;
                    Assert::AreEqual<int>(test.expected[i].kind, actual_token->kind);
                    if (test.expected[i].kind == TOKEN_KIND_LITERAL_SSTRING || test.expected[i].kind == TOKEN_KIND_LITERAL_DSTRING || test.expected[i].kind == TOKEN_KIND_LITERAL_BSTRING) {
                        Assert::AreEqual(test.expected[i].value.word.value, actual_token->value.word.value);
                    }
                }
                free_token_result(&actual);
            }
        }

        TEST_METHOD(lexical_analyze_string_dq_esc_test)
        {
            PROJECT_SETTING setting;
            TEXT input = wrap("\"\\Q\"");
            TOKEN_RESULT actual = lexical_analyze(NULL, &input, &setting);
            Assert::AreEqual((size_t)0, actual.token.length);
            Assert::AreEqual((size_t)1, actual.result.length);
            COMPILE_RESULT* cr = (COMPILE_RESULT*)get_object_list(&actual.result, 0).value;
            Assert::AreEqual<int>(COMPILE_CODE_UNKNOWN_ESCAPE_SEQUENCE, cr->code);

            free_token_result(&actual);
        }

        TEST_METHOD(lexical_analyze_string_bq_notimpl_test)
        {
            PROJECT_SETTING setting;
            TEXT input = wrap("`a`");
            TOKEN_RESULT actual = lexical_analyze(NULL, &input, &setting);
            Assert::AreEqual((size_t)0, actual.token.length);
            Assert::AreEqual((size_t)1, actual.result.length);
            COMPILE_RESULT* cr = (COMPILE_RESULT*)get_object_list(&actual.result, 0).value;
            Assert::AreEqual<int>(COMPILE_CODE_NOT_IMPLEMENT, cr->code);

            free_token_result(&actual);
        }

        TEST_METHOD(lexical_analyze_number_test)
        {
            PROJECT_SETTING setting;

            auto tests = {
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 0) }, wrap("0")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 1) }, wrap("1")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 9) }, wrap("9")),

                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 123), VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 4)  }, wrap("123 4")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 1234), VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 5678) }, wrap("123_4 5__6___7____8__")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 1), VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 2), VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 3), { TOKEN_KIND_LITERAL_INTEGER, 4 } }, wrap("1\r2\n3\r\n4")),

                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 0) }, wrap("0x00")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 1) }, wrap("0x01")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 15) }, wrap("0x0F")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 15) }, wrap("0x0f")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 16) }, wrap("0x1_0")),

                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 1) }, wrap("0b0001")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 2) }, wrap("0b0010")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 4) }, wrap("0b0100")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 8) }, wrap("0b1000")),
                DATA(std::vector<VALUE_TEST> { VALUE_TEST(TOKEN_KIND_LITERAL_INTEGER, 8) }, wrap("0b10_00")),
            };
            for (auto test : tests) {
                auto arg1 = std::get<0>(test.inputs);
                TOKEN_RESULT actual = lexical_analyze(NULL, &arg1, &setting);
                Assert::AreEqual(test.expected.size(), actual.token.length, arg1.value);
                for (size_t i = 0; i < test.expected.size(); i++) {
                    TOKEN* actual_token = (TOKEN*)get_object_list(&actual.token, i).value;
                    Assert::AreEqual<int>(test.expected[i].kind, actual_token->kind);
                    if (test.expected[i].kind == TOKEN_KIND_LITERAL_INTEGER || test.expected[i].kind == TOKEN_KIND_LITERAL_DECIMAL) {
                        Assert::AreEqual(test.expected[i].value.integer, actual_token->value.integer);
                    }
                }
                free_token_result(&actual);
            }
        }
    };
}
