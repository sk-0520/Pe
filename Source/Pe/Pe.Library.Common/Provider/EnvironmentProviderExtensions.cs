using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Common.Provider
{
    public static class EnvironmentProviderExtensions
    {
        //extension(EnvironmentProvider provider)
        //{
            #region function

            public static string GetVariableValue(this EnvironmentProvider provider, string variable)
            {
                if(provider.TryGetVariableValue(variable, out var result)) {
                    return result;
                }

                throw new KeyNotFoundException($"not found: {variable}");
            }

            #endregion
        //}
    }
}
