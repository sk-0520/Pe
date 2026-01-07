using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Library.Common.Linq;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test.Linq
{
    file sealed class NormalComparer<T>: IComparer<T>
        where T : IComparable<T>
    {
        public int Compare(T? x, T? y)
        {
            return x?.CompareTo(y) ?? 0;
        }
    }

    file sealed class ReverseComparer<T>: IComparer<T>
        where T : IComparable<T>
    {
        public int Compare(T? x, T? y)
        {
            return y?.CompareTo(x) ?? 0;
        }
    }

    public class IEnumerableExtensionsTest
    {
        #region define

        private class TestObj
        {
            public int Value { get; set; }
        }


        #endregion

        [Fact]
        public void FrozenSequenceTest()
        {
            var src = new[] { 1, 2, 3 };
            var actual = src.FrozenSequence();
            Assert.Equal(src, actual);
            Assert.IsAssignableFrom<IReadOnlyList<int>>(actual);
        }

        [Fact]
        public void FrozenSequence_Throw_Immutability_Test()
        {
            var src = new List<string> { "a", "b", "c" };
            var actual = src.FrozenSequence();
            Assert.Equal(src, actual);
            Assert.Throws<NotSupportedException>(() => ((ICollection<string>)actual).Add("d"));
            Assert.Throws<NotSupportedException>(() => ((ICollection<string>)actual).Remove("a"));
        }

        [Fact]
        public void FrozenSequence_SourceChangeDoesNotAffectFrozen()
        {
            var src = new List<int> { 1, 2, 3 };
            var actual = src.FrozenSequence();
            src[0] = 100;

            // コピーされてる
            Assert.Equal([1, 2, 3], actual);
        }

        [Fact]
        public void FrozenSequence_ElementReferenceChange()
        {
            var obj1 = new TestObj { Value = 1 };
            var obj2 = new TestObj { Value = 2 };
            var src = new[] { obj1, obj2 };
            var actual = src.FrozenSequence();

            obj1.Value = 100;
            // 要素自体は参照型なので変更可能
            Assert.Equal(100, actual[0].Value);
        }

        [Fact]
        public void CountingTest()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            var counting = array.Counting().ToArray();

            for(var i = 0; i < array.Length; i++) {
                Assert.Equal(i, counting[i].Number);
            }
        }

        [Fact]
        public void Counting_Base_Test()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            var counting = array.Counting(99).ToArray();

            for(var i = 0; i < array.Length; i++) {
                Assert.Equal(i + 99, counting[i].Number);
            }
        }

        [Theory]
        [InlineData("a,b,c", new[] { "a", "b", "c" }, ",")]
        [InlineData("abc", new[] { "a", "b", "c" }, "")]
        [InlineData("abc", new[] { "a", "b", "c" }, null)]
        public void JoinStringTest(string expected, IEnumerable<string> source, string? separator)
        {
            var actual = source.JoinString(separator);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new[] { "a", "b", "c" }, new[] { "a", "b", "c" }, Order.Ascending)]
        [InlineData(new[] { "a", "b", "c" }, new[] { "c", "b", "a" }, Order.Ascending)]
        [InlineData(new[] { "c", "b", "a" }, new[] { "a", "b", "c" }, Order.Descending)]
        [InlineData(new[] { "c", "b", "a" }, new[] { "c", "b", "a" }, Order.Descending)]
        public void OrderByTest(IEnumerable<string> expected, IEnumerable<string> source, Order order)
        {
            var actual = source.OrderBy(order, a => a);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OrderBy_Throw_Test()
        {
            Order order = (Order)(-1);
            Assert.Throws<NotImplementedException>(() => Array.Empty<int>().OrderBy(order, a => a));
            Assert.Throws<NotImplementedException>(() => Array.Empty<int>().OrderBy(order, a => a, new NormalComparer<int>()));
        }

        public static TheoryData<IEnumerable<string>, IEnumerable<string>, Order, IComparer<string>> OrderBy_Comparer_Data => new() {
            {
                new[] { "a", "b", "c" },
                new[] { "a", "b", "c" },
                Order.Ascending,
                new NormalComparer<string>()
            },
            {
                new[] { "c", "b", "a" },
                new[] { "a", "b", "c" },
                Order.Ascending,
                new ReverseComparer<string>()
            },
            {
                new[] { "c", "b", "a" },
                new[] { "c", "b", "a" },
                Order.Descending,
                new NormalComparer<string>()
            },
            {
                new[] { "a", "b", "c" },
                new[] { "c", "b", "a" },
                Order.Descending,
                new ReverseComparer<string>()
            },
        };

        [Theory]
        [MemberData(nameof(OrderBy_Comparer_Data))]
        public void OrderBy_Comparer_Test(IEnumerable<string> expected, IEnumerable<string> source, Order order, IComparer<string>? comparer)
        {
            var actual = source.OrderBy(order, a => a, comparer);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new[] { "a0", "a1", "a2" }, new[] { "a0", "a1", "a2" }, Order.Ascending)]
        [InlineData(new[] { "a0", "b0", "b1" }, new[] { "b1", "a0", "b0" }, Order.Ascending)]
        [InlineData(new[] { "a2", "a1", "a0" }, new[] { "a0", "a1", "a2" }, Order.Descending)]
        [InlineData(new[] { "a0" , "b1", "b0", }, new[] { "b1", "a0", "b0" }, Order.Descending)]
        public void ThenByTest(IEnumerable<string> expected, IEnumerable<string> source, Order order)
        {
            var actual = source.OrderBy(a => a.First()).ThenBy(order, a => a.Last());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThenBy_Throw_Test()
        {
            Order order = (Order)(-1);
            Assert.Throws<NotImplementedException>(() => Array.Empty<int>().OrderBy(a => a).ThenBy(order, a => a));
            Assert.Throws<NotImplementedException>(() => Array.Empty<int>().OrderBy(a => a).ThenBy(order, a => a, new NormalComparer<int>()));
        }

        public static TheoryData<IEnumerable<string>, IEnumerable<string>, Order, IComparer<string>> ThenBy_Comparer_Data => new() {
            {
                new[] { "a0", "a1", "a1" },
                new[] { "a0", "a1", "a1" },
                Order.Ascending,
                new NormalComparer<string>()
            },
            {
                new[] { "a2", "a1", "a0" },
                new[] { "a0", "a1", "a2" },
                Order.Ascending,
                new ReverseComparer<string>()
            },
            {
                new[] { "a2", "a1", "a0" },
                new[] { "a2", "a1", "a0" },
                Order.Descending,
                new NormalComparer<string>()
            },
            {
                new[] { "a0", "a1", "a2" },
                new[] { "a2", "a1", "a0" },
                Order.Descending,
                new ReverseComparer<string>()
            },
        };

        [Theory]
        [MemberData(nameof(ThenBy_Comparer_Data))]
        public void ThenBy_Comparer_Test(IEnumerable<string> expected, IEnumerable<string> source, Order order, IComparer<string>? comparer)
        {
            var actual = source.OrderBy(Order.Ascending, a => a.First()).ThenBy(order, a => a.Last().ToString(), comparer);
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(false, new object[] { 1, 2, 3 })]
        [InlineData(false, new object[] { "a", "b", "c" })]
        [InlineData(true, new object[] { })]
        [InlineData(true, new object[] { "a" })]
        [InlineData(true, new object[] { "a", "a" })]
        [InlineData(false, new object?[] { "a", null })]
        [InlineData(true, new object[] { 1 })]
        [InlineData(true, new object[] { 1, 1 })]
        [InlineData(false, new object?[] { 1, null })]
        [InlineData(true, new object?[] { null })]
        [InlineData(true, new object?[] { null, null })]
        [InlineData(false, new object?[] { null, "str" })]
        [InlineData(false, new object?[] { null, 1 })]
        public void AllEqualsTest(bool expected, IEnumerable<object?> source)
        {
            var actual = source.AllEquals();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AllEquals_throw_Test()
        {
            IEnumerable<object> source = null!;
            Assert.Throws<ArgumentNullException>(() => source.AllEquals());
        }
    }
}
