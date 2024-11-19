using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Library.CliProxy
{
    [Generator(LanguageNames.CSharp)]
    public partial class CliProxyGenerator: IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(
                static context => {
                    
                    context.AddSource(
                        "a.cs",
                        """
namespace ContentTypeTextNet.Pe.Library.CliProxy;
public class XXX { public int Value { get;set;} }
""");
                }
            );

        }
    }
}
