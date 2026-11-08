#include "pch.h"

extern "C" {
#   include "../Pe.Library/fsio.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(fsio_textfile_test)
    {
    public:
        TEST_METHOD(writer_native_test)
        {
            TEST_INIT_DIR;
            TEST_GET_PATH(path, _T("path.txt"));

            FILE_WRITER file_writer = new_file_writer(&path, FILE_ENCODING_NATIVE, FILE_OPEN_MODE_NEW, FILE_WRITER_OPTIONS_NONE, DEFAULT_MEMORY_ARENA);
            write_string_file_writer(&file_writer, _T("𩸽🍶"), true);

            Assert::IsTrue(release_file_writer(&file_writer));
        }

#ifdef _UNICODE
        TEST_METHOD(writer_bom_existing_file_test)
        {
            TEST_INIT_DIR;
            TEST_GET_PATH(path, _T("existing.txt"));

            const std::string originals[] = { "abcdef", std::string("\xef\xbb\xbf", 3) + "abcdef" };
            for (FILE_OPEN_MODE mode : { FILE_OPEN_MODE_OPEN, FILE_OPEN_MODE_OPEN_OR_CREATE }) {
                for (const std::string& original : originals) {
                    {
                        std::ofstream file(std::filesystem::path(test_path), std::ios::binary | std::ios::trunc);
                        Assert::IsTrue(file.is_open());
                        file.write(original.data(), (std::streamsize)original.size());
                        Assert::IsTrue(file.good());
                    }

                    FILE_WRITER file_writer = new_file_writer(&path, FILE_ENCODING_UTF8, mode, FILE_WRITER_OPTIONS_BOM, DEFAULT_MEMORY_ARENA);
                    Assert::IsTrue(is_enabled_file_writer(&file_writer));
                    Assert::IsTrue(seek_end_file_resource(&file_writer.resource));
                    write_string_file_writer(&file_writer, _T("xyz"), false);
                    Assert::IsTrue(release_file_writer(&file_writer));

                    std::ifstream file(std::filesystem::path(test_path), std::ios::binary);
                    Assert::IsTrue(file.is_open());
                    const std::string actual((std::istreambuf_iterator<char>(file)), std::istreambuf_iterator<char>());
                    Assert::IsTrue(original + "xyz" == actual);
                }
            }
        }

        TEST_METHOD(writer_bom_empty_file_test)
        {
            TEST_INIT_DIR;
            TEST_GET_PATH(path, _T("empty.txt"));
            const std::string expected("\xef\xbb\xbf", 3);

            for (FILE_OPEN_MODE mode : { FILE_OPEN_MODE_NEW, FILE_OPEN_MODE_OPEN, FILE_OPEN_MODE_OPEN_OR_CREATE, FILE_OPEN_MODE_TRUNCATE }) {
                if (std::filesystem::exists(test_path)) {
                    std::filesystem::remove(test_path);
                }
                if (mode == FILE_OPEN_MODE_OPEN || mode == FILE_OPEN_MODE_TRUNCATE) {
                    std::ofstream file(std::filesystem::path(test_path), std::ios::binary);
                    Assert::IsTrue(file.is_open());
                    if (mode == FILE_OPEN_MODE_TRUNCATE) {
                        file << "old content";
                        Assert::IsTrue(file.good());
                    }
                }

                FILE_WRITER file_writer = new_file_writer(&path, FILE_ENCODING_UTF8, mode, FILE_WRITER_OPTIONS_BOM, DEFAULT_MEMORY_ARENA);
                Assert::IsTrue(is_enabled_file_writer(&file_writer));
                Assert::IsTrue(release_file_writer(&file_writer));

                std::ifstream file(std::filesystem::path(test_path), std::ios::binary);
                Assert::IsTrue(file.is_open());
                const std::string actual((std::istreambuf_iterator<char>(file)), std::istreambuf_iterator<char>());
                Assert::IsTrue(expected == actual);
            }
        }

        TEST_METHOD(writer_no_bom_empty_file_test)
        {
            TEST_INIT_DIR;
            TEST_GET_PATH(path, _T("empty.txt"));

            FILE_WRITER file_writer = new_file_writer(&path, FILE_ENCODING_UTF8, FILE_OPEN_MODE_OPEN_OR_CREATE, FILE_WRITER_OPTIONS_NONE, DEFAULT_MEMORY_ARENA);
            Assert::IsTrue(is_enabled_file_writer(&file_writer));
            write_string_file_writer(&file_writer, _T("abc"), false);
            Assert::IsTrue(release_file_writer(&file_writer));

            std::ifstream file(std::filesystem::path(test_path), std::ios::binary);
            Assert::IsTrue(file.is_open());
            const std::string actual((std::istreambuf_iterator<char>(file)), std::istreambuf_iterator<char>());
            Assert::IsTrue(std::string("abc") == actual);
        }

        TEST_METHOD(writer_utf8n_test)
        {
            TEST_INIT_DIR;
            TEST_GET_PATH(path, _T("path.txt"));

            FILE_WRITER file_writer = new_file_writer(&path, FILE_ENCODING_UTF8, FILE_OPEN_MODE_NEW, FILE_WRITER_OPTIONS_NONE, DEFAULT_MEMORY_ARENA);
            write_string_file_writer(&file_writer, _T("𩸽🍶"), true);

            Assert::IsTrue(release_file_writer(&file_writer));
        }

        TEST_METHOD(writer_utf8bom_test)
        {
            TEST_INIT_DIR;
            TEST_GET_PATH(path, _T("path.txt"));

            FILE_WRITER file_writer = new_file_writer(&path, FILE_ENCODING_UTF8, FILE_OPEN_MODE_NEW, FILE_WRITER_OPTIONS_BOM, DEFAULT_MEMORY_ARENA);
            write_string_file_writer(&file_writer, _T("𩸽🍶"), true);

            Assert::IsTrue(release_file_writer(&file_writer));
        }
#endif
    };
}
