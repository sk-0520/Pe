using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Unmanaged
{
    public class GlobalAllocTest
    {
        #region define

        private struct TestStruct
        {
#pragma warning disable CS0649
            public int Int;
            public byte Byte;
            public double Double;
#pragma warning restore CS0649
        }

        #endregion

        #region function

        [Fact]
        public void Test()
        {
            var test = new GlobalAlloc(16);
            Assert.Equal(16, test.Size);
            Assert.False(test.IsInvalid);

            var input = Enumerable.Range(0, 16).Select(a => (byte)a).ToArray();
            Marshal.Copy(input, 0, test.Heap, input.Length);
            var output = new byte[input.Length];
            Marshal.Copy(test.Heap, output, 0, input.Length);
            Assert.Equal(input, output);

            test.Dispose();
            Assert.Equal(16, test.Size);
            Assert.True(test.IsInvalid);
        }

        [Fact]
        public void Test_empty_Create()
        {
            using var test = GlobalAlloc.Create<TestStruct>();
            Assert.Equal(test.Size, Marshal.SizeOf<TestStruct>());
        }

        [Fact]
        public void Test_struct_Create()
        {
            var input = new TestStruct() {
                Byte = 64,
                Int = 123456,
                Double = 10.5
            };
            using var test = GlobalAlloc.Create(input);
            Assert.Equal(test.Size, Marshal.SizeOf(input));

            var actual = Marshal.PtrToStructure<TestStruct>(test.Heap);
            Assert.Equal(input.Byte, actual.Byte);
            Assert.Equal(input.Int, actual.Int);
            Assert.Equal(input.Double, actual.Double);
        }

        [Fact]
        public void ToArrayTest()
        {
            using var test1 = new GlobalAlloc(4);
            var actual1 = test1.ToArray();
            Assert.Equal(4, actual1.Length);
        }

        [Fact]
        public void Index_Test()
        {
            using var test = GlobalAlloc.Create(4);
            test[0] = (byte)1;
            test[1] = (byte)2;
            test[2] = (byte)3;
            test[3] = (byte)4;

            Assert.Equal(1, test[0]);
            Assert.Equal(2, test[1]);
            Assert.Equal(3, test[2]);
            Assert.Equal(4, test[3]);
        }

        [Fact]
        public void Index_OutOfRange_Test()
        {
            using var test = GlobalAlloc.Create(4);

            Assert.Throws<ArgumentOutOfRangeException>(() => test[-1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => test[4]);

            Assert.Throws<ArgumentOutOfRangeException>(() => test[-1] = 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => test[4] = 0);
        }

        #endregion
    }
}
