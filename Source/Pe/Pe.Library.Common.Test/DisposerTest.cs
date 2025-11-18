using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class DisposerBaseTest
    {
        #region define

        private sealed class TestDisposer: DisposerBase
        {
            #region property

            public int Value { get; set; } = 0;

            #endregion

            #region function

            public void PublicThrowIfDisposed()
            {
                ThrowIfDisposed();
            }

            #endregion
        }

        #endregion

        #region function

        void Test_Disposing1_1(object? sender, EventArgs e)
        {
            var test = (TestDisposer)sender!;
            Assert.False(test.IsDisposed);
            Assert.Equal(1, test.Value);
            test.Value += 1;
        }

        [Fact]
        public void Disposing_1_Test()
        {
            var test = new TestDisposer();
            test.Disposing += Test_Disposing1_1;
            using(test) {
                var exception = Record.Exception(() => test.PublicThrowIfDisposed());
                Assert.Null(exception);
                test.Value = 1;
            }
            Assert.Equal(2, test.Value);
            Assert.True(test.IsDisposed);
            Assert.Throws<ObjectDisposedException>(() => test.PublicThrowIfDisposed());
        }


        void Test_Disposing2_1(object? sender, EventArgs e)
        {
            var test = (TestDisposer)sender!;
            Assert.False(test.IsDisposed);
            Assert.Equal(1, test.Value);
            test.Value += 10;
        }

        void Test_Disposing2_2(object? sender, EventArgs e)
        {
            Assert.Fail();
        }

        [Fact]
        public void Disposing_2_Test()
        {
            var test = new TestDisposer();
            test.Disposing += Test_Disposing2_1;
            test.Disposing += Test_Disposing2_2;
            using(test) {
                var exception = Record.Exception(() => test.PublicThrowIfDisposed());
                Assert.Null(exception);
                test.Value = 1;
                test.Disposing -= Test_Disposing2_2;
            }
            Assert.Equal(11, test.Value);
            Assert.True(test.IsDisposed);
            Assert.Throws<ObjectDisposedException>(() => test.PublicThrowIfDisposed());
        }

        #endregion
    }

    public class ActionDisposerTest
    {
        [Fact]
        public void Constructor_throw_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionDisposer(null!));
        }

        [Fact]
        public void UsingTest()
        {
            using(var disposer = new ActionDisposer(disposing => {
                Assert.True(disposing);
            })) {
                //NOP
            }
        }

        [Fact]
        public void FinalizeTest()
        {
            var disposer = new ActionDisposer(disposing => {
                Assert.False(disposing);
            });
        }
    }

    public class ActionDisposerHelperTest
    {
        #region function

        [Fact]
        public void CreateTest()
        {
            using(ActionDisposerHelper.Create(a => Assert.True(a))) {
                //NOP
            }
        }

        [Fact]
        public void Create_T_Test()
        {
            using(ActionDisposerHelper.Create((a, num) => {
                Assert.True(a);
                Assert.Equal(100, num);
            }, 100)) {
                //NOP
            }
        }

        [Fact]
        public void CreateEmptyTest()
        {
            var empty = ActionDisposerHelper.CreateEmpty();
            Assert.False(empty.IsDisposed);
            empty.Dispose();
            Assert.True(empty.IsDisposed);
        }

        #endregion
    }

    public class DisposableCollectionTest
    {
        #region function

        [Fact]
        public void Test()
        {
            var stocks = new List<int>();

            var test = new DisposableCollection();
            test.Add(ActionDisposerHelper.Create(a => stocks.Add(10)));
            test.AddRange(new[] {
                ActionDisposerHelper.Create(a => stocks.Add(20)),
                ActionDisposerHelper.Create(a => stocks.Add(30)),
            });

            Assert.Empty(stocks);

            test.Dispose();

            Assert.Equal(3, stocks.Count);
            Assert.Equal(30, stocks[0]);
            Assert.Equal(20, stocks[1]);
            Assert.Equal(10, stocks[2]);

            test.Dispose();
        }

        [Fact]
        public void Add_throw_Test()
        {
            var test = new DisposableCollection();
            Assert.Throws<ArgumentNullException>(() => test.Add(default(IDisposable)!));
        }

        [Fact]
        public void AddRange_throw_Test()
        {
            var test = new DisposableCollection();
            Assert.Throws<ArgumentNullException>(() => test.AddRange(default(IDisposable[])!));
        }

        #endregion
    }

    public class ArrayPoolValueTest
    {
        #region function

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(16)]
        [InlineData(32)]
        [InlineData(64)]
        [InlineData(128)]
        [InlineData(256)]
        [InlineData(512)]
        [InlineData(1024)]
        [InlineData(2048)]
        [InlineData(4096)]
        [InlineData(8192)]
        [InlineData(16384)]
        [InlineData(32768)]
        [InlineData(65536)]
        [InlineData(131072)]
        [InlineData(262144)]
        [InlineData(524288)]
        [InlineData(1048576)]
        [InlineData(2097152)]
        [InlineData(4194304)]
        [InlineData(8388608)]
        [InlineData(16777216)]
        [InlineData(33554432)]
        public void ConstructorTest(int size)
        {
            using var array = new ValueArrayPool<byte>(size);
            Assert.True(size <= array.Items.Length);
            Assert.Equal(size, array.Length);
        }

        [Fact]
        public void ConstructorPoolTest()
        {
            var ap = ArrayPool<byte>.Create();
            using var array = new ValueArrayPool<byte>(128, ap);
            Assert.True(128 <= array.Items.Length);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(127, 255)]
        public void Index(int index, byte input)
        {
            using var array = new ValueArrayPool<byte>(128);
            array.Items[index] = input;
            Assert.Equal(input, array.Items[index]);
            Assert.Equal(input, array[index]);
        }

        #endregion
    }

    public class ArrayPoolObjectTest
    {
        #region function

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(16)]
        [InlineData(32)]
        [InlineData(64)]
        [InlineData(128)]
        [InlineData(256)]
        [InlineData(512)]
        [InlineData(1024)]
        [InlineData(2048)]
        [InlineData(4096)]
        [InlineData(8192)]
        [InlineData(16384)]
        [InlineData(32768)]
        [InlineData(65536)]
        [InlineData(131072)]
        [InlineData(262144)]
        [InlineData(524288)]
        [InlineData(1048576)]
        [InlineData(2097152)]
        [InlineData(4194304)]
        [InlineData(8388608)]
        [InlineData(16777216)]
        [InlineData(33554432)]
        public void ConstructorTest(int size)
        {
            using var array = new DisposableArrayPool<byte>(size);
            Assert.True(size <= array.Items.Length);
            Assert.Equal(size, array.Length);
        }

        [Fact]
        public void ConstructorPoolTest()
        {
            var ap = ArrayPool<byte>.Create();
            using var array = new DisposableArrayPool<byte>(128, ap);
            Assert.True(128 <= array.Items.Length);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(127, 255)]
        public void Index(int index, byte input)
        {
            using var array = new DisposableArrayPool<byte>(128);
            array.Items[index] = input;
            Assert.Equal(input, array.Items[index]);
            Assert.Equal(input, array[index]);
        }

        #endregion
    }
}
