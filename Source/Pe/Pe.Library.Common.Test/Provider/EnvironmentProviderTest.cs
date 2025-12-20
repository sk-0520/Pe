using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Library.Common.Provider;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test.Provider
{
    public class DefaultEnvironmentProviderTest
    {
        #region function

        [Fact]
        public void NewLineTest()
        {
            var provider = EnvironmentProvider.Default;
            Assert.Equal(Environment.NewLine, provider.NewLine);
        }

        [Fact]
        public void TryGetVariableValueTest()
        {
            var provider = EnvironmentProvider.Default;

            Assert.True(provider.TryGetVariableValue("PATH", out var actual1));
            Assert.Equal(Environment.GetEnvironmentVariable("PATH"), actual1);

            Assert.False(provider.TryGetVariableValue("\0", out var actual2));
            Assert.Equal(Environment.GetEnvironmentVariable("\0"), actual2);
        }

        #endregion
    }
}
