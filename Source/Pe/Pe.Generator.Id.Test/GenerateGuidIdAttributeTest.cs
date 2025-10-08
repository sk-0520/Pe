using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Generator.Id;
using Xunit;

namespace ContentTypeTextNet.Pe.Generator.Id.Test
{
    /// <summary>
    /// テストデータ。
    /// </summary>
    [GenerateGuidId()]
    public partial record struct TestData;

    public class GenerateGuidIdAttributeTest
    {
        #region function

        [Fact]
        public void Constructor_Empty_Test()
        {
            var actual = new TestData();
            Assert.Equal(TestData.Empty, actual);
        }

        #endregion
    }
}
