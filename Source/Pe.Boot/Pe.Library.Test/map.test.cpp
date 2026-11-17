#include "pch.h"

extern "C" {
#   include "../Pe.Library/map.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(map_test)
    {
    public:
        TEST_METHOD(new_release_test)
        {
            MAP map = new_map(sizeof(int), MAP_DEFAULT_CAPACITY, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);
            release_map(&map);
        }

        TEST_METHOD(add_map_test)
        {
            MAP map = new_map(sizeof(int), MAP_DEFAULT_CAPACITY, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

            TEXT input_key_1 = wrap("key-1");
            int input_value_1 = 10;
            bool actual_1 = add_map(&map, &input_key_1, &input_value_1);
            Assert::IsTrue(actual_1);

            TEXT input_key_2 = wrap("key-2");
            int input_value_2 = 20;
            bool actual_2 = add_map(&map, &input_key_2, &input_value_2);
            Assert::IsTrue(actual_2);

            TEXT input_key_3 = wrap("key-1");
            int input_value_3 = 30;
            bool actual_3 = add_map(&map, &input_key_3, &input_value_3);
            Assert::IsFalse(actual_3);

            release_map(&map);
        }

        TEST_METHOD(get_map_test)
        {
            MAP map = new_map(sizeof(int), MAP_DEFAULT_CAPACITY, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

            TEXT input_key_0 = wrap("key-0");
            MAP_RESULT_VALUE actual_0 = get_map(&map, &input_key_0);
            Assert::IsFalse(actual_0.exists);

            TEXT input_key_1 = wrap("key-1");
            int input_value_1 = 10;
            add_map(&map, &input_key_1, &input_value_1);

            TEXT input_key_2 = wrap("key-2");
            int input_value_2 = 20;
            add_map(&map, &input_key_2, &input_value_2);

            TEXT input_key_3 = wrap("key-3");
            int input_value_3 = 30;
            add_map(&map, &input_key_3, &input_value_3);

            TEXT input_key_4 = wrap("key-4");

            MAP_RESULT_VALUE actual_1 = get_map(&map, &input_key_1);
            Assert::IsTrue(actual_1.exists);
            Assert::AreEqual(input_value_1, *(int*)actual_1.value);

            MAP_RESULT_VALUE actual_2 = get_map(&map, &input_key_2);
            Assert::IsTrue(actual_2.exists);
            Assert::AreEqual(input_value_2, *(int*)actual_2.value);

            MAP_RESULT_VALUE actual_3 = get_map(&map, &input_key_3);
            Assert::IsTrue(actual_3.exists);
            Assert::AreEqual(input_value_3, *(int*)actual_3.value);

            MAP_RESULT_VALUE actual_4 = get_map(&map, &input_key_4);
            Assert::IsFalse(actual_4.exists);

            release_map(&map);
        }

        TEST_METHOD(set_map_test)
        {
            MAP map = new_map(sizeof(int), MAP_DEFAULT_CAPACITY, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

            TEXT input_key = wrap("key-1");

            int input_value_1 = 10;

            set_map(&map, &input_key, &input_value_1);
            MAP_RESULT_VALUE actual_1 = get_map(&map, &input_key);
            Assert::IsTrue(actual_1.exists);
            Assert::AreEqual(input_value_1, *(int*)actual_1.value);

            int input_value_2 = 20;

            set_map(&map, &input_key, &input_value_2);
            MAP_RESULT_VALUE actual_2 = get_map(&map, &input_key);
            Assert::IsTrue(actual_2.exists);
            Assert::AreEqual(input_value_2, *(int*)actual_2.value);

            release_map(&map);
        }

        TEST_METHOD(remove_map_test)
        {
            MAP map = new_map(sizeof(int), MAP_DEFAULT_CAPACITY, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

            TEXT key = wrap("key");
            int value = 10;

            Assert::IsFalse(remove_map(&map, &key));
            Assert::AreEqual((size_t)0, map.length);

            set_map(&map, &key, &value);
            Assert::AreEqual((size_t)1, map.length);
            Assert::IsTrue(remove_map(&map, &key));

            Assert::AreEqual((size_t)0, map.length);

            release_map(&map);
        }

        TEST_METHOD(extend_test)
        {
            MAP map = new_map(sizeof(int), 2, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);
            //size_t length = 64;
            size_t length = 8;
            TEXT format = wrap("key-%d");
            for (size_t i = 0; i < length; i++) {
                TEXT key = format_text(DEFAULT_MEMORY_ARENA, &format, i);
                add_map(&map, &key, &i);
                release_text(&key);
            }
            Assert::AreEqual(length, map.length);

            release_map(&map);
        }

        struct foreach_state
        {
            bool seen[26] = {};
            size_t count = 0;
            size_t expected_length = 0;
            size_t stop_after = 0;
        };

        static size_t foreach_test_hash(const TEXT* key)
        {
            return static_cast<size_t>(key->value[0] - _T('a'));
        }

        static bool foreach_map_test(const KEY_VALUE_PAIR* pair, size_t index, size_t length, const void* arg)
        {
            foreach_state* state = (foreach_state*)arg;
            Assert::AreEqual(state->count, index);
            Assert::AreEqual(state->expected_length, length);
            Assert::AreEqual(static_cast<size_t>(1), static_cast<size_t>(pair->key.length));
            int key_index = pair->key.value[0] - _T('a');
            Assert::IsTrue(key_index >= 0 && key_index < 26);
            Assert::IsFalse(state->seen[key_index]);
            Assert::AreEqual(key_index * 10, *(const int*)pair->value);
            state->seen[key_index] = true;
            state->count += 1;
            return !state->stop_after || state->count < state->stop_after;
        }

        static void add_foreach_item(MAP* map, TCHAR character)
        {
            TCHAR text[] = { character, 0 };
            TEXT key = wrap_text(text);
            int value = (character - _T('a')) * 10;
            Assert::IsTrue(add_map(map, &key, &value));
        }

        TEST_METHOD(foreach_map_test)
        {
            MAP map = new_map(sizeof(int), 8, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, foreach_test_hash, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);
            foreach_state empty;
            foreach_map(&map, foreach_map_test, &empty);
            Assert::AreEqual(static_cast<size_t>(0), empty.count);

            add_foreach_item(&map, _T('h'));
            foreach_state sparse;
            sparse.expected_length = 1;
            foreach_map(&map, foreach_map_test, &sparse);
            Assert::AreEqual(static_cast<size_t>(1), sparse.count);
            Assert::IsTrue(sparse.seen[7]);
            Assert::IsTrue(release_map(&map));
        }

        TEST_METHOD(foreach_map_collision_test)
        {
            MAP map = new_map(sizeof(int), 8, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, foreach_test_hash, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);
            for (TCHAR key : { _T('a'), _T('i'), _T('q'), _T('b'), _T('h') }) {
                add_foreach_item(&map, key);
            }

            foreach_state all;
            all.expected_length = 5;
            foreach_map(&map, foreach_map_test, &all);
            Assert::AreEqual(static_cast<size_t>(5), all.count);
            for (int index : { 0, 8, 16, 1, 7 }) {
                Assert::IsTrue(all.seen[index]);
            }
            Assert::IsTrue(release_map(&map));
        }

        TEST_METHOD(foreach_map_stop_test)
        {
            MAP map = new_map(sizeof(int), 8, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, foreach_test_hash, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);
            for (TCHAR key : { _T('a'), _T('i'), _T('q'), _T('b'), _T('h') }) {
                add_foreach_item(&map, key);
            }
            for (size_t stop : { (size_t)1, (size_t)2, (size_t)3, (size_t)4 }) {
                foreach_state stopped;
                stopped.expected_length = 5;
                stopped.stop_after = stop;
                foreach_map(&map, foreach_map_test, &stopped);
                Assert::AreEqual(stop, stopped.count);
            }
            Assert::IsTrue(release_map(&map));
        }

        TEST_METHOD(foreach_map_rehash_and_remove_test)
        {
            MAP map = new_map(sizeof(int), 2, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, foreach_test_hash, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);
            for (TCHAR key = _T('a'); key <= _T('t'); key++) {
                add_foreach_item(&map, key);
            }
            for (TCHAR character : { _T('a'), _T('i'), _T('t') }) {
                TCHAR text[] = { character, 0 };
                TEXT key = wrap_text(text);
                Assert::IsTrue(remove_map(&map, &key));
            }

            foreach_state state;
            state.expected_length = 17;
            foreach_map(&map, foreach_map_test, &state);
            Assert::AreEqual(state.expected_length, state.count);
            for (int index = 0; index < 20; index++) {
                Assert::AreEqual(index != 0 && index != 8 && index != 19, state.seen[index]);
            }
            Assert::IsTrue(release_map(&map));
        }
    };
}
