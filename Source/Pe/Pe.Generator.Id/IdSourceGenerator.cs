using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Generator.Id
{
    [Generator(LanguageNames.CSharp)]
    public class IdSourceGenerator: IIncrementalGenerator
    {
        #region IIncrementalGenerator

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(static initContext =>
            {
                var sourceBuilder = new SourceBuilder();

                var attributeNamespace = "ContentTypeTextNet.Pe.Generator.Id";
                var attributeTarget = AttributeTargets.Struct;
                var attributeName = "GenerateIdAttribute";

                var source = $$"""
{{sourceBuilder.ToHeader()}}

namespace {{attributeNamespace}}
{
    [{{sourceBuilder.ToCode<System.AttributeUsageAttribute>()}}({{sourceBuilder.ToCode(attributeTarget)}}, AllowMultiple = false)]
    public sealed class {{attributeName}}: {{sourceBuilder.ToCode<System.Attribute>()}}
    {

    }
}
""";

                initContext.AddSource($"{attributeName}.g.cs", source);
            });
        }

        #endregion
    }
}
