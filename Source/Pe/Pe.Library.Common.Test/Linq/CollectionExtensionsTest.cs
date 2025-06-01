using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ContentTypeTextNet.Pe.Library.Common.Linq;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test.Linq
{
    public class CollectionExtensionsTest
    {
        #region function

        [Fact]
        public void SetRange_List_Test()
        {
            var test = new List<int>() {
                1, 2, 3
            };
            test.SetRange(Enumerable.Range(10, 3));
            Assert.Equal(new[] { 10, 11, 12 }, test);
        }

        [Fact]
        public void SetRange_NotList_Test()
        {
            var test = new Collection<int>() {
                1, 2, 3
            };
            test.SetRange(Enumerable.Range(10, 3));
            Assert.Equal(new[] { 10, 11, 12 }, test);
        }

        [Fact]
        public void AddRange_NotList_Test()
        {
            var test = new Collection<int>() {
                1, 2, 3
            };
            test.AddRange(Enumerable.Range(10, 3));
            Assert.Equal(new[] { 1, 2, 3, 10, 11, 12 }, test);
        }

        [Theory]
        [InlineData(0, 1, 0, 0)]
        [InlineData(0, 1, 0, 123)]
        [InlineData(0, 1, 0, -456)]
        [InlineData(4, 5, 1, 3)]
        [InlineData(2, 5, 1, 6)]
        [InlineData(4, 5, 1, -2)]
        [InlineData(4, 5, 0, -1)]
        [InlineData(0, 5, 4, 1)]
        [InlineData(1, 3, 2, -4)]
        public void GetNextIndexTest(int expected, int size, int startIndex, int distance)
        {
            var array = new int[size];
            var actual = array.GetNextIndex(startIndex, distance);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNextIndex_SourceIsNull_ThrowsArgumentNullException()
        {
            int[] source = null!;
            Assert.Throws<ArgumentNullException>(() => source.GetNextIndex(0, 0));
        }

        [Theory]
        // 空コレクションは startIndex のチェックで落ちる
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 5)]
        [InlineData(0, 0, -5)]
        public void GetNextIndex_EmptyCollection_AlwaysThrowsArgumentOutOfRange(int size, int startIndex, int distance)
        {
            var xs = new int[size];
            Assert.Throws<ArgumentOutOfRangeException>(() => xs.GetNextIndex(startIndex, distance));
        }

        [Theory]
        // startIndex < 0
        [InlineData(5, -1, 0)]
        [InlineData(5, -1, 10)]
        [InlineData(5, -1, -5)]
        // startIndex > Count-1
        [InlineData(5, 5, 0)]
        [InlineData(5, 5, 3)]
        [InlineData(5, 5, -3)]
        public void GetNextIndex_StartIndexOutOfRange_ThrowsArgumentOutOfRange(int size, int startIndex, int distance)
        {
            var xs = new int[size];
            Assert.Throws<ArgumentOutOfRangeException>(() => xs.GetNextIndex(startIndex, distance));
        }

        #endregion
    }
}
