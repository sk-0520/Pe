﻿#include "pch.h"

extern "C" {
#   include "../Pe.Boot/tstring.h"
#   include "../Pe.Boot/string_builder.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(string_builder_test)
    {
    public:

        TEST_METHOD(initialize_string_builder_test)
        {
            auto tests = {
                DATA((size_t)4, _T("abcd"), 0),
                DATA((size_t)1, _T(""), 1),
                DATA((size_t)2, _T("ab"), 1),
            };

            for (auto test : tests) {
                auto [arg1, arg2] = test.inputs;
                auto actual = initialize_string_builder(arg1, arg2);
                Assert::AreEqual(get_string_length(arg1), actual.length);
                Assert::AreEqual(test.expected, actual.library.capacity);

                Assert::IsTrue(free_string_builder(&actual));
            }
        }

        TEST_METHOD(create_string_builder_test)
        {
            auto tests = {
                DATA((size_t)4, 4),
                DATA((size_t)1, 1),
                DATA((size_t)2, 2),
            };

            for (auto test : tests) {
                auto [arg1] = test.inputs;
                auto actual = create_string_builder(arg1);
                Assert::AreEqual(test.expected, actual.library.capacity);

                Assert::IsTrue(free_string_builder(&actual));
            }
        }

        TEST_METHOD(free_string_builder_test)
        {
            STRING_BUILDER* input1 = NULL;
            Assert::IsFalse(free_string_builder(input1));

            STRING_BUILDER input2 = { 0 };
            Assert::IsFalse(free_string_builder(&input2));

            STRING_BUILDER input3 = create_string_builder(1);
            Assert::IsTrue(free_string_builder(&input3));
        }

        TEST_METHOD(append_string_builder_test)
        {
            auto sb = create_string_builder(3);

            append_builder_string(&sb, _T("ABC"), false);

            TEXT text = wrap("DEF");
            append_builder_text(&sb, &text, false);

            append_builder_character(&sb, _T('G'), false);

            TEXT actual = build_text_string_builder(&sb);

            Assert::AreEqual(_T("ABCDEFG"), actual.value);

            TEXT free1 = reference_text_string_builder(&sb);
            TEXT free2 = reference_text_string_builder(&sb);
            TEXT free3 = reference_text_string_builder(&sb);


            Assert::IsTrue(free_text(&actual));
            Assert::IsTrue(free_string_builder(&sb));
        }
    };
}
