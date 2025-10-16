using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Generator.Id
{
    [Generator(LanguageNames.CSharp)]
    public class GuidIdTypeHandlerSourceGenerator: IIncrementalGenerator
    {
        #region IIncrementalGenerator

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var sourceBuilder = new SourceBuilder();

            var attributeNamespace = "ContentTypeTextNet.Pe.Generator.Id";
            var attributeTarget = AttributeTargets.Class;
            var attributeName = "GenerateGuidIdTypeHandlerAttribute";

            context.RegisterPostInitializationOutput(initContext => {
                var attributeSource = $$"""
{{sourceBuilder.Header}}

namespace {{attributeNamespace}}
{
    [{{sourceBuilder.ToCode<System.AttributeUsageAttribute>()}}({{sourceBuilder.ToCode(attributeTarget)}}, AllowMultiple = false)]
    internal sealed class {{attributeName}}: {{sourceBuilder.ToCode<System.Attribute>()}}
    {
        public {{attributeName}}({{sourceBuilder.ToCode<System.Type>()}} type)
        {
            //NOP
        }

        #region property

        public {{sourceBuilder.ToCode<System.Type>()}} Type { get; }

        #endregion
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
            var sourceBuilder = new SourceBuilder();

            foreach(var attribute in array) {
                var targetSymbol = (INamedTypeSymbol)attribute.TargetSymbol;

                // ネストはむり
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


                var constructor0 = targetSymbol.GetAttributes()[0];
                var idType = "global::" + constructor0.ConstructorArguments[0].Value;

                var source = $$"""
{{sourceBuilder.Header}}
using System;
using System.Data;

{{(sourceBuilder.ToNamespaceCode(targetSymbol))}}

partial class {{targetName}}: global::Dapper.SqlMapper.TypeHandler<{{idType}}>
{
    #region TypeHandler

    public override void SetValue(IDbDataParameter parameter, {{idType}} value)
    {
        parameter.Value = value.ToString();
    }

    public override {{idType}} Parse(object value)
    {
        var s = (string)value;
        if(s != null) {
            if({{idType}}.TryParse(s, out var ret)) {
                return ret;
            }
        }

        return {{idType}}.Empty;
    }

    #endregion
}

""";
                var sourceFileName = sourceBuilder.ToSourceFilePath(targetSymbol);
                context.AddSource(sourceFileName, source);
            }
        }

        #endregion
    }
}
