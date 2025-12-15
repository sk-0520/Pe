using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class SizePerTimeTest
    {
        #region function

        [Fact]
        public void Constructor_Throw_Test()
        {
            var exception = Assert.Throws<ArgumentException>(() => new SizePerTime(Timeout.InfiniteTimeSpan, TimeProvider.System));
            Assert.Contains("Timeout.InfiniteTimeSpan", exception.Message);
            Assert.Equal("baseTime", exception.ParamName);
        }

        [Fact]
        public void Pattern_Simple_Test()
        {
            var timeProvider = new FakeTimeProvider();
            var sizePerTime = new SizePerTime(TimeSpan.FromSeconds(1), timeProvider);

            sizePerTime.Start();
            Assert.Equal(0, sizePerTime.Size);

            // 時間が経過していないのでサイズは増えない
            sizePerTime.Add(100);
            Assert.Equal(0, sizePerTime.Size);

            // 合計 1000 byte だが時間が経過していないのでサイズは増えない
            sizePerTime.Add(900);
            Assert.Equal(0, sizePerTime.Size);

            // 経過した！
            timeProvider.Advance(TimeSpan.FromSeconds(1));
            sizePerTime.Add(1);
            Assert.Equal(1001, sizePerTime.Size);

            // 時間が基準より経過していないのでサイズは増えない
            timeProvider.Advance(TimeSpan.FromSeconds(0.5));
            sizePerTime.Add(499);
            Assert.Equal(1001, sizePerTime.Size);
        }

        #endregion
    }
}
