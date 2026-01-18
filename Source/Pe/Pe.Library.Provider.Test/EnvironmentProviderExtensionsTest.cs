using System;
using System.Collections.Generic;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Provider.Test
{
    public class EnvironmentProviderExtensionsTest
    {
        #region function

        [Fact]
        public void Default_GetVariableValue_NotFound_Test()
        {
            var provider = EnvironmentProvider.Default;
            Assert.Throws<KeyNotFoundException>(() => provider.GetVariableValue("\0"));
        }

        [Fact]
        public void Default_GetVariableValueTest()
        {
            var provider = EnvironmentProvider.Default;
            Assert.Equal(Environment.GetEnvironmentVariable("PATH"), provider.GetVariableValue("PATH"));
        }

        #endregion
    }
}
