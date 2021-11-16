﻿#include "pch.h"

extern "C" {
#   include "../Pe.Library/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(text_convert_test)
    {
    public:

        TEST_METHOD(parse_i32_from_text_success_test)
        {
            auto tests = {
                DATA(false, wrap(""), false),
                DATA(true, wrap("1"), false),
                DATA(true, wrap("0xf"), false),

                DATA(false, wrap(""), true),
                DATA(true, wrap("1"), true),
                DATA(true, wrap("0xf"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool arg2 = std::get<1>(test.inputs);
                auto actual = parse_i32_from_text(&arg1, arg2, DEFAULT_MEMORY);
                if (test.expected) {
                    Assert::IsTrue(actual.success);
                } else {
                    Assert::IsFalse(actual.success);
                }
            }
        }

        TEST_METHOD(parse_i32_from_text_value_test)
        {
            auto tests = {
                DATA(1, wrap("1"), false),
                DATA(0, wrap("0xf"), false),

                DATA(1, wrap("1"), true),
                DATA(15, wrap("0xf"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool arg2 = std::get<1>(test.inputs);
                auto actual = parse_i32_from_text(&arg1, arg2, DEFAULT_MEMORY);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }

#ifdef _WIN64
        TEST_METHOD(parse_i64_from_text_success_test)
        {
            auto tests = {
                DATA(false, wrap(""), false),
                DATA(true, wrap("1"), false),
                DATA(true, wrap("0xf"), false),

                DATA(false, wrap(""), true),
                DATA(true, wrap("1"), true),
                DATA(true, wrap("0xf"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool arg2 = std::get<1>(test.inputs);
                auto actual = parse_i64_from_text(&arg1, arg2, DEFAULT_MEMORY);
                if (test.expected) {
                    Assert::IsTrue(actual.success);
                } else {
                    Assert::IsFalse(actual.success);
                }
            }
        }
#endif

#ifdef _WIN64
        TEST_METHOD(parse_i64_from_text_value_test)
        {
            auto tests = {
                DATA((__int64)1, wrap("1"), false),
                DATA((__int64)0, wrap("0xf"), false),

                DATA((__int64)1, wrap("1"), true),
                DATA((__int64)15, wrap("0xf"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool arg2 = std::get<1>(test.inputs);
                auto actual = parse_i64_from_text(&arg1, arg2, DEFAULT_MEMORY);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }
#endif

        TEST_METHOD(multibyte_success_test)
        {
            auto input = wrap("あいうえお");
            auto tests = {
                MULTI_BYTE_CHARACTER_TYPE_UTF8,
                MULTI_BYTE_CHARACTER_TYPE_SJIS,
            };
            for (auto test : tests) {
                auto res = convert_to_multibyte_character(&input, test, DEFAULT_MEMORY);
                Assert::IsTrue(is_enabled_multibyte_character_result(&res));
                auto text = make_text_from_multibyte(res.buffer, res.length, test, DEFAULT_MEMORY);
                Assert::AreEqual(text.value, input.value);
                Assert::IsTrue(release_multibyte_character_result(&res, DEFAULT_MEMORY));
                Assert::IsTrue(release_text(&text));
                Assert::IsFalse(release_multibyte_character_result(&res, DEFAULT_MEMORY));
            }
        }

        TEST_METHOD(parse_i32_from_bin_text_test)
        {
            auto tests = {
                DATA((int32_t)0, wrap("0")),
                DATA((int32_t)1, wrap("1")),

                DATA((int32_t)0, wrap("0000")),
                DATA((int32_t)1, wrap("0001")),
                DATA((int32_t)2, wrap("0010")),
                DATA((int32_t)3, wrap("0011")),
                DATA((int32_t)4, wrap("0100")),
                DATA((int32_t)5, wrap("0101")),
                DATA((int32_t)6, wrap("0110")),
                DATA((int32_t)7, wrap("0111")),
                DATA((int32_t)8, wrap("1000")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT_PARSED_I32_RESULT actual = parse_i32_from_bin_text(&arg1);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }

#ifdef _WIN64
        TEST_METHOD(parse_i64_from_bin_text_test)
        {
            auto tests = {
                DATA((int64_t)0, wrap("0")),
                DATA((int64_t)1, wrap("1")),

                DATA((int64_t)0, wrap("0000")),
                DATA((int64_t)1, wrap("0001")),
                DATA((int64_t)2, wrap("0010")),
                DATA((int64_t)3, wrap("0011")),
                DATA((int64_t)4, wrap("0100")),
                DATA((int64_t)5, wrap("0101")),
                DATA((int64_t)6, wrap("0110")),
                DATA((int64_t)7, wrap("0111")),
                DATA((int64_t)8, wrap("1000")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT_PARSED_I64_RESULT actual = parse_i64_from_bin_text(&arg1);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }
#endif
    };
}
