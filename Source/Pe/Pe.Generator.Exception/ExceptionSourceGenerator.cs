using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Generator.Exception
{
    [Generator(LanguageNames.CSharp)]
    public class ExceptionSourceGenerator: IIncrementalGenerator
    {
        #region IIncrementalGenerator

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var sourceBuilder = new SourceBuilder();

            var attributeNamespace = "ContentTypeTextNet.Pe.Generator.Exception";
            var attributeTarget = AttributeTargets.Class;
            var attributeName = "GenerateExceptionAttribute";

            context.RegisterPostInitializationOutput(initContext => {
                var attributeSource = $$"""
{{sourceBuilder.Header}}

namespace {{attributeNamespace}}
{
    [{{sourceBuilder.ToCode<System.AttributeUsageAttribute>()}}({{sourceBuilder.ToCode(attributeTarget)}}, AllowMultiple = false)]
    internal sealed class {{attributeName}}: {{sourceBuilder.ToCode<System.Attribute>()}}
    {
        public {{attributeName}}()
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

            context.RegisterSourceOutput(provider, GenerateSource);
        }

        private void GenerateSource(SourceProductionContext context, ImmutableArray<GeneratorAttributeSyntaxContext> array)
        {
            var sourceBuilder = new SourceBuilder();

            foreach(var attribute in array) {
                var targetSymbol = (INamedTypeSymbol)attribute.TargetSymbol;

                // ネストは無理
                if(targetSymbol.ContainingType != null) {
                    var location = targetSymbol.Locations.Length > 0 ? targetSymbol.Locations[0] : Location.None;
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.NestedTypeNotSupported, location, targetSymbol.ToDisplayString()));
                    continue;
                }

                var namespaceName = targetSymbol.ContainingNamespace.IsGlobalNamespace
                    ? ""
                    : targetSymbol.ContainingNamespace.ToString();

                var targetName = targetSymbol.Name;
                var fullName = (targetSymbol.ContainingNamespace.IsGlobalNamespace ? "" : namespaceName + ".") + targetName;

                var source = $$"""
{{sourceBuilder.Header}}
using System;
using System.Text.Json.Serialization;

{{(sourceBuilder.ToNamespaceCode(targetSymbol))}}

partial class {{targetName}}
{
}

""";
                var sourceFileName = sourceBuilder.ToSourceFilePath(targetSymbol);
                context.AddSource(sourceFileName, source);
            }
        }

        #endregion
    }
}
