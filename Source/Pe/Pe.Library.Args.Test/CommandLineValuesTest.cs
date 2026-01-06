using System;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Args.Test
{
    public class CommandLineValuesTest
    {
        #region function

        [Fact]
        public void First_Throw_Test()
        {
            var test = new CommandLineValues();
            Assert.Throws<ArgumentOutOfRangeException>(() => test.First);
            Assert.Throws<ArgumentOutOfRangeException>(() => test.Data[0]);
        }

        [Fact]
        public void First_1_Test()
        {
            var test = new CommandLineValues();

            test.Add("value");

            Assert.Equal("value", test.First);
            Assert.Equal("value", test.Data[0]);
        }


        [Fact]
        public void First_2_Test()
        {
            var test = new CommandLineValues();

            test.Add("value1");
            test.Add("value2");

            Assert.Equal("value1", test.First);
            Assert.Equal("value1", test.Data[0]);
            Assert.Equal("value2", test.Data[1]);
        }

        #endregion
    }
}
