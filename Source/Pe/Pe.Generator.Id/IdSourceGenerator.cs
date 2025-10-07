using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            var sourceBuilder = new SourceBuilder();

            var attributeNamespace = "ContentTypeTextNet.Pe.Generator.Id";
            var attributeTarget = AttributeTargets.Struct;
            var attributeName = "GenerateIdAttribute";

            context.RegisterPostInitializationOutput(initContext => {
                var attributeSource = $$"""
{{sourceBuilder.Header}}

namespace {{attributeNamespace}}
{
    [{{sourceBuilder.ToCode<System.AttributeUsageAttribute>()}}({{sourceBuilder.ToCode(attributeTarget)}}, AllowMultiple = false)]
    internal sealed record class {{attributeName}}<TId>: {{sourceBuilder.ToCode<System.Attribute>()}}
    {
        public {{attributeName}}(
            public TId DefaultValue = default,
            public bool JsonConstructor = true,
            public global::System.Func<string, TId> CustomParse = null,
            public global::System.Func<TId> CustomNewId = null,
            public global::System.Func<string> CustomToString = null
        )
        {
            //NOP
        }
    }
}
""";

                initContext.AddSource($"{attributeName}.g.cs", attributeSource);
            });

            var provider = context.SyntaxProvider.ForAttributeWithMetadataName(
                $"{attributeNamespace}.{attributeName}",
                (node, cancellationToken) => true,
                (context, cancellationToken) => context
            ).Collect();

            context.RegisterSourceOutput(provider, Emit);
        }

        private void Emit(SourceProductionContext context, ImmutableArray<GeneratorAttributeSyntaxContext> array)
        {
        }

        #endregion
    }
}
