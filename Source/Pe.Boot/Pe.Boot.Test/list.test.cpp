﻿#include "pch.h"

extern "C" {
#   include "../Pe.Boot/list.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(list_test)
    {
    public:
        TEST_METHOD(life_test)
        {
            PRIMITIVE_LIST_UINT32 list = new_primitive_list(PRIMITIVE_LIST_TYPE_UINT32, 2);
            Assert::AreEqual((size_t)0, list.length);
            Assert::AreEqual((size_t)2 * sizeof(uint32_t), list.library.capacity_bytes);

            Assert::IsFalse(pop_list_uint32(NULL, &list));

            push_list_uint32(&list, 100);
            Assert::AreEqual((size_t)1, list.length);
            Assert::AreEqual((size_t)2 * sizeof(uint32_t), list.library.capacity_bytes);

            push_list_uint32(&list, 200);
            Assert::AreEqual((size_t)2, list.length);
            Assert::AreEqual((size_t)2 * sizeof(uint32_t), list.library.capacity_bytes);

            push_list_uint32(&list, 300);
            Assert::AreEqual((size_t)3, list.length);
            Assert::AreEqual((size_t)4 * sizeof(uint32_t), list.library.capacity_bytes);

            uint32_t result1;
            Assert::IsFalse(get_list_uint32(&result1, &list, 3));
            Assert::IsTrue(get_list_uint32(&result1, &list, 1));
            Assert::AreEqual((uint32_t)200, result1);

            Assert::IsTrue(pop_list_uint32(NULL, &list));
            Assert::AreEqual((size_t)2, list.length);

            uint32_t result2;
            pop_list_uint32(&result2, &list);
            Assert::AreEqual((uint32_t)200, result2);
            Assert::AreEqual((size_t)1, list.length);

            Assert::IsTrue(free_primitive_list(&list));
        }

        TEST_METHOD(type_error_test)
        {
            PRIMITIVE_LIST_UINT32 list = new_primitive_list(PRIMITIVE_LIST_TYPE_UINT32, 2);

            Assert::IsFalse(push_list_uint8(&list, 1));
            Assert::IsFalse(push_list_uint16(&list, 1));
            Assert::IsTrue(push_list_uint32(&list, 1));

            uint8_t u8;
            uint16_t u16;
            uint32_t u32;
            Assert::IsFalse(get_list_uint8(&u8, &list, 0));
            Assert::IsFalse(get_list_uint16(&u16, &list, 0));
            Assert::IsTrue(get_list_uint32(&u32, &list, 0));

            free_primitive_list(&list);
        }
    };
}
