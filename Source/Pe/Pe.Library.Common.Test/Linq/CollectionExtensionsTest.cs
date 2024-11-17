﻿using System.Collections.Generic;
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

        #endregion
    }
}
