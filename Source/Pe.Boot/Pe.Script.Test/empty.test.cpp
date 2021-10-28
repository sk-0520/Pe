#include "pch.h"

extern "C" {
#   include "../Pe.Library/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace ScriptTest
{
    TEST_CLASS(empty_test)
    {
    public:

        TEST_METHOD(test)
        {
            TEXT expected = wrap("C:\\abc\\bin\\Pe.Main.exe");
            TEXT actual = wrap("C:\\abc\\bin\\Pe.Main.exe");

            Assert::AreEqual(expected.value, actual.value);
        }
    };
}
