#include "pch.h"

extern "C" {
#   include "../Pe.Library/tstring.h"
#   include "../Pe.Library/string_builder.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(string_builder_test)
    {
    public:
        TEST_METHOD(new_string_builder_test)
        {
            auto tests = {
                DATA((size_t)4, 4),
                DATA((size_t)1, 1),
                DATA((size_t)2, 2),
            };

            for (auto test : tests) {
                auto [arg1] = test.inputs;
                auto actual = new_string_builder(arg1, DEFAULT_MEMORY_ARENA);
                //Assert::AreEqual(test.expected, actual.library.capacity);

                Assert::IsTrue(release_string_builder(&actual));
            }
        }

        TEST_METHOD(release_string_builder_test)
        {
            STRING_BUILDER* input1 = NULL;
            Assert::IsFalse(release_string_builder(input1));

            STRING_BUILDER input2 = { 0 };
            Assert::IsFalse(release_string_builder(&input2));

            STRING_BUILDER input3 = new_string_builder(1, DEFAULT_MEMORY_ARENA);
            Assert::IsTrue(release_string_builder(&input3));
        }

        TEST_METHOD(append_string_builder_test)
        {
            auto expected = _T("ABCDEFGtrue-1false1");
            auto sb = new_string_builder(3, DEFAULT_MEMORY_ARENA);

            append_builder_string(&sb, _T("ABC"), false);

            TEXT text = wrap("DEF");
            append_builder_text(&sb, &text, false);

            append_builder_character(&sb, _T('G'), false);

            append_builder_bool(&sb, true, false);
            append_builder_int(&sb, -1, false);
            append_builder_bool(&sb, false, false);
            append_builder_uint(&sb, 1, false);

            TEXT actual = build_text_string_builder(&sb);

            Assert::AreEqual(expected, actual.value);

            TEXT free1 = reference_text_string_builder(&sb);
            TEXT free2 = reference_text_string_builder(&sb);
            TEXT free3 = reference_text_string_builder(&sb);


            Assert::IsTrue(release_text(&actual));
            Assert::IsTrue(release_string_builder(&sb));
        }

        TEST_METHOD(reference_text_string_builder_capacity_boundaries_test)
        {
            for (size_t capacity : { (size_t)1, (size_t)2, (size_t)3, (size_t)16, (size_t)64 }) {
                for (size_t length = 0; length <= capacity + 1; ++length) {
                    auto sb = new_string_builder(capacity, DEFAULT_MEMORY_ARENA);
                    for (size_t i = 0; i < length; ++i) {
                        append_builder_character(&sb, _T('A') + static_cast<TCHAR>(i % 26), false);
                    }

                    TEXT actual = reference_text_string_builder(&sb);
                    Assert::IsTrue(is_enabled_text(&actual));
                    Assert::AreEqual(length, static_cast<size_t>(actual.length));
                    Assert::IsFalse(actual.library.need_release);
                    Assert::IsTrue(sb.library.list.library.capacity_bytes / sizeof(TCHAR) > length);
                    for (size_t i = 0; i < length; ++i) {
                        Assert::AreEqual(static_cast<TCHAR>(_T('A') + i % 26), actual.value[i]);
                    }
                    Assert::AreEqual(static_cast<TCHAR>(0), actual.value[length]);

                    TEXT repeated = reference_text_string_builder(&sb);
                    Assert::AreEqual(length, static_cast<size_t>(repeated.length));
                    Assert::IsTrue(actual.value == repeated.value);
                    Assert::AreEqual(static_cast<TCHAR>(0), repeated.value[length]);

                    TEXT built = build_text_string_builder(&sb);
                    Assert::AreEqual(length, static_cast<size_t>(built.length));
                    for (size_t i = 0; i < length; ++i) {
                        Assert::AreEqual(static_cast<TCHAR>(_T('A') + i % 26), built.value[i]);
                    }
                    Assert::IsTrue(release_text(&built));
                    Assert::IsTrue(release_string_builder(&sb));
                }
            }
        }

        TEST_METHOD(reference_text_string_builder_append_and_clear_test)
        {
            auto sb = new_string_builder(1, DEFAULT_MEMORY_ARENA);
            for (size_t length = 0; length < 130; ++length) {
                TEXT actual = reference_text_string_builder(&sb);
                Assert::AreEqual(length, static_cast<size_t>(actual.length));
                Assert::IsTrue(sb.library.list.library.capacity_bytes / sizeof(TCHAR) > length);
                for (size_t i = 0; i < length; ++i) {
                    Assert::AreEqual(static_cast<TCHAR>(_T('a') + i % 26), actual.value[i]);
                }
                Assert::AreEqual(static_cast<TCHAR>(0), actual.value[length]);
                append_builder_character(&sb, _T('a') + static_cast<TCHAR>(length % 26), false);
            }

            clear_builder(&sb);
            TEXT empty = reference_text_string_builder(&sb);
            Assert::AreEqual(static_cast<size_t>(0), static_cast<size_t>(empty.length));
            Assert::AreEqual(static_cast<TCHAR>(0), empty.value[0]);
            append_builder_string(&sb, _T("reuse"), false);
            TEXT reused = reference_text_string_builder(&sb);
            Assert::AreEqual(static_cast<size_t>(5), static_cast<size_t>(reused.length));
            Assert::AreEqual(_T("reuse"), reused.value);
            Assert::IsTrue(release_string_builder(&sb));
        }

        TEST_METHOD(reference_text_string_builder_embedded_zero_test)
        {
            auto sb = new_string_builder(3, DEFAULT_MEMORY_ARENA);
            append_builder_character(&sb, _T('A'), false);
            append_builder_character(&sb, _T('\0'), false);
            append_builder_character(&sb, _T('B'), false);

            TEXT actual = reference_text_string_builder(&sb);
            Assert::AreEqual(static_cast<size_t>(3), static_cast<size_t>(actual.length));
            Assert::IsTrue(sb.library.list.library.capacity_bytes / sizeof(TCHAR) > actual.length);
            Assert::AreEqual(static_cast<TCHAR>(_T('A')), actual.value[0]);
            Assert::AreEqual(static_cast<TCHAR>(0), actual.value[1]);
            Assert::AreEqual(static_cast<TCHAR>(_T('B')), actual.value[2]);
            Assert::AreEqual(static_cast<TCHAR>(0), actual.value[3]);
            Assert::IsTrue(release_string_builder(&sb));
        }

        TEST_METHOD(append_builder_format_test)
        {
            auto sb = new_string_builder(3, DEFAULT_MEMORY_ARENA);

            auto expected = _T("1 -1 +100 -100 -200 A abc def");
            TEXT format = wrap("%d %d %+d %d %+d %c %s %t");
            auto t = wrap("def");
            append_builder_format(&sb, &format, 1, -1, 100, -100, -200, _T('A'), _T("abc"), &t);
            TEXT actual = build_text_string_builder(&sb);

            Assert::AreEqual(expected, actual.value);

            Assert::IsTrue(release_text(&actual));
            Assert::IsTrue(release_string_builder(&sb));
        }
    };
}
