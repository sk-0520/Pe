﻿#include "pch.h"

extern "C" {
#   include "../Pe.Library/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(text_manipulate_test)
    {
    public:

        TEST_METHOD(add_text_normal_test)
        {
            auto tests = {
                DATA(_T("ab"), wrap("a"), wrap("b")),
                DATA(_T("a"), wrap("a"), wrap("")),
                DATA(_T("b"), wrap(""), wrap("b")),
                DATA(_T("🐭🐮🐯🐰🐉🐍🐴🐏🐵🐔🐶🐷"), wrap("🐭🐮🐯🐰🐉🐍"), wrap("🐴🐏🐵🐔🐶🐷")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
                TEXT actual = add_text(&arg1, &arg2);
                Assert::AreEqual(test.expected, actual.value);
                free_text(&actual);
            }
        }

        TEST_METHOD(add_text_fail_test)
        {
            TEXT a = wrap("");
            TEXT b = create_invalid_text();

            TEXT actual_ab = add_text(&a, &b);
            Assert::IsTrue(is_enabled_text(&actual_ab));

            TEXT actual_ba = add_text(&b, &a);
            Assert::IsTrue(is_enabled_text(&actual_ba));

            TEXT actual_bb = add_text(&b, &b);
            Assert::IsFalse(is_enabled_text(&actual_bb));

            free_text(&actual_ab);
            free_text(&actual_ba);
            free_text(&actual_bb);
        }

        TEST_METHOD(join_text_test)
        {
            TCHAR* expected1 = _T("1,2,3");
            TEXT input1[] = {
                wrap("1"),
                wrap("2"),
                wrap("3"),
            };
            TEXT sep1 = wrap(",");
            TEXT actual1 = join_text(&sep1, input1, SIZEOF_ARRAY(input1), IGNORE_EMPTY_NONE);
            Assert::AreEqual(expected1, actual1.value);
            free_text(&actual1);

            TCHAR* expected2 = _T("123");
            TEXT input2[] = {
                wrap(""),
                wrap("1"),
                wrap(""),
                wrap("2"),
                wrap(""),
                wrap("3"),
                wrap(""),
            };
            TEXT sep2 = wrap("");
            TEXT actual2 = join_text(&sep2, input2, SIZEOF_ARRAY(input2), IGNORE_EMPTY_NONE);
            Assert::AreEqual(expected2, actual2.value);
            free_text(&actual2);

            TCHAR* expected3_1 = _T(",1,,2, ,,3,");
            TCHAR* expected3_2 = _T("1,2, ,3");
            TCHAR* expected3_3 = _T("1,2,3");
            TEXT input3[] = {
                wrap(""),
                wrap("1"),
                wrap(""),
                wrap("2"),
                wrap(" "),
                wrap(""),
                wrap("3"),
                wrap(""),
            };
            TEXT sep3 = wrap(",");
            TEXT actual3_1 = join_text(&sep3, input3, SIZEOF_ARRAY(input3), IGNORE_EMPTY_NONE);
            Assert::AreEqual(expected3_1, actual3_1.value);
            free_text(&actual3_1);

            TEXT actual3_2 = join_text(&sep3, input3, SIZEOF_ARRAY(input3), IGNORE_EMPTY_ONLY);
            Assert::AreEqual(expected3_2, actual3_2.value);
            free_text(&actual3_2);

            TEXT actual3_3 = join_text(&sep3, input3, SIZEOF_ARRAY(input3), IGNORE_EMPTY_WHITESPACE);
            Assert::AreEqual(expected3_3, actual3_3.value);
            free_text(&actual3_3);
        }

        TEST_METHOD(is_empty_text_test)
        {
            auto tests = {
                DATA(false, wrap("a")),
                DATA(false, wrap(" ")),
                DATA(false, wrap("\t")),
                DATA(true, wrap("")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool actual = is_empty_text(&arg1);
                if (test.expected) {
                    Assert::IsTrue(actual);
                } else {
                    Assert::IsFalse(actual);
                }
            }
        }

        TEST_METHOD(is_white_space_text_test)
        {
            auto tests = {
                DATA(false, wrap("a")),
                DATA(true, wrap(" ")),
                DATA(true, wrap("\t")),
                DATA(true, wrap("")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool actual = is_whitespace_text(&arg1);
                if (test.expected) {
                    Assert::IsTrue(actual);
                } else {
                    Assert::IsFalse(actual);
                }
            }
        }

        TEST_METHOD(trim_text_test)
        {
            auto tests = {
                DATA(_T("a"), wrap(" a "), true, true, std::vector<TCHAR>({ ' ', })),
                DATA(_T("a "), wrap(" a "), true, false, std::vector<TCHAR>({ ' ', })),
                DATA(_T(" a"), wrap(" a "), false, true, std::vector<TCHAR>({ ' ', })),
                DATA(_T("a b"), wrap("      a b    "), true, true, std::vector<TCHAR>({ ' ', })),
                DATA(_T("a"), wrap(" \t a\t \t"), true, true, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T("a\tb c"), wrap(" \t a\tb c\t \t"), true, true, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T(" \t a\tb c"), wrap(" \t a\tb c\t \t"), false, true, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T("a\tb c\t \t"), wrap(" \t a\tb c\t \t"), true, false, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T(" \t a\tb c\t \t"), wrap(" \t a\tb c\t \t"), false, false, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T(" \t \t \t"), wrap(" \t \t \t"), false, false, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T(""), wrap(" \t \t \t"), true, false, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T(""), wrap(" \t \t \t"), false, true, std::vector<TCHAR>({ ' ', '\t' })),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool arg2 = std::get<1>(test.inputs);
                bool arg3 = std::get<2>(test.inputs);
                auto arg4 = std::get<3>(test.inputs);
                TEXT actual = trim_text(&arg1, arg2, arg3, arg4.data(), arg4.size());
                Assert::AreEqual(test.expected, actual.value);
                free_text(&actual);
            }
        }

        TEST_METHOD(trim_white_space_text_test)
        {
            auto tests = {
                DATA(_T("a"), wrap(" a ")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT actual = trim_whitespace_text(&arg1);
                Assert::AreEqual(test.expected, actual.value);
                free_text(&actual);
            }
        }

        static TEXT split_text_EASY_CSV(const TEXT* source, size_t* next_index)
        {
            ssize_t index = index_of_character(source, _T(','));
            if (index == -1) {
                *next_index = source->length;
                return wrap_text_with_length(source->value, source->length, false);
            }

            *next_index = index + 1;
            return wrap_text_with_length(source->value, index, false);
        }

        TEST_METHOD(split_text_EASY_CSV_test)
        {
            TCHAR* expected[] = {
                _T("a"),
                _T("b"),
                _T("c"),
                _T("d"),
                _T(""),
                _T(" "),
                _T("e"),
                _T(""),
            };
            TEXT input = wrap("a,b,c,d,, ,e,");
            OBJECT_LIST actual = split_text(&input, split_text_EASY_CSV);
            Assert::AreEqual(sizeof(expected) / sizeof(expected[0]), actual.length);
            for (size_t i = 0; i < actual.length; i++) {
                OBJECT_RESULT_VALUE result = get_object_list(&actual, i);
                TEXT* t = (TEXT*)result.value;
                TCHAR* s = expected[i];
                Assert::AreEqual(s, t->value);
            }
            free_object_list(&actual);
        }

        TEST_METHOD(split_newline_text_test)
        {
            TCHAR* expected[] = {
                _T("abc"),
                _T("def"),
                _T("ghi"),
                _T("jkl"),
                _T(""),
                _T(""),
                _T(""),
                _T("xyz"),
                _T(""),
            };
            TEXT input = wrap_text(
                _T("abc\r")
                _T("def\n")
                _T("ghi\r\n")
                _T("jkl\r\n")
                _T("\r") //CRLF
                _T("\n") //----
                _T("\n")
                _T("\r")
                _T("xyz\r\n")
            );
            OBJECT_LIST actual = split_newline_text(&input);
            Assert::AreEqual(sizeof(expected) / sizeof(expected[0]), actual.length);
            for (size_t i = 0; i < actual.length; i++) {
                OBJECT_RESULT_VALUE result = get_object_list(&actual, i);
                TEXT* t = (TEXT*)result.value;
                TCHAR* s = expected[i];
                Assert::AreEqual(s, t->value);
            }
            free_object_list(&actual);
        }
    };
}
