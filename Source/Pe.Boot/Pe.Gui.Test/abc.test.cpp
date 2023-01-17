#include "pch.h"

extern "C" {
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeGuiTest
{
    TEST_CLASS(abc_test)
    {
    public:
        TEST_METHOD(xyz_test)
        {
            Assert::IsTrue(true);
        }
    };
}
