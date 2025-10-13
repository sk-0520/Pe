using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Generator.Id
{
    [Generator(LanguageNames.CSharp)]
    public class GuidIdSourceGenerator: IIncrementalGenerator
    {
        #region define

        private static readonly DiagnosticDescriptor NestedTypeNotSupported = new DiagnosticDescriptor(
            id: "PEIG001",
            title: "Nested type not supported",
            messageFormat: "The type '{0}' is nested inside another type. Apply the attribute only to top-level types.",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        #endregion

        #region IIncrementalGenerator

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var sourceBuilder = new SourceBuilder();

            var attributeNamespace = "ContentTypeTextNet.Pe.Generator.Id";
            var attributeTarget = AttributeTargets.Struct;
            var attributeName = "GenerateGuidIdAttribute";

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
                    context.ReportDiagnostic(Diagnostic.Create(NestedTypeNotSupported, location, targetSymbol.ToDisplayString()));
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

readonly partial record struct {{targetName}}
{
    /// <summary>
    /// 生成。
    /// </summary>
    [JsonConstructor]
    public {{targetName}}({{sourceBuilder.ToCode<Guid>()}} id)
    {
        Id = id;
    }
        
    #region property

    /// <summary>
    /// 生ID。
    /// </summary>
    public {{sourceBuilder.ToCode<Guid>()}} Id { get; }

    /// <summary>
    /// 空ID。
    /// </summary>
    public static {{targetName}} Empty
    {
        get
        {
            return new {{targetName}}(default);
        }
    }

    #endregion


    #region function

    /// <summary>
    /// <see cref="{{targetName}}"/>に変換。
    /// </summary>
    /// <param name="s">入力文字列。</param>
    /// <param name="result">変更成功。</param>
    /// <returns></returns>
    public static bool TryParse(string s, out {{targetName}} result)
    {
        if({{sourceBuilder.ToCode<Guid>()}}.TryParse(s, out var val)) {
            result = new {{targetName}}(val);
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// <see cref="{{targetName}}"/>に変換。
    /// </summary>
    /// <param name="s">入力文字列。</param>
    /// <returns></returns>
    public static {{targetName}} Parse(string s)
    {
        var id = {{sourceBuilder.ToCode<Guid>()}}.Parse(s);
        return new {{targetName}}(id);
    }

    /// <summary>
    /// 新規IDの生成。
    /// </summary>
    /// <returns></returns>
    public static {{targetName}} NewId()
    {
        return new {{targetName}}({{sourceBuilder.ToCode<Guid>()}}.NewGuid());
    }

    #endregion

    #region object

    /// <summary>
    /// IDを文字列表現に変換。
    /// </summary>
    /// <remarks>
    /// <para>ファイル名として使用可能な文字列表現にもなる。</para>
    /// </remarks>
    /// <returns></returns>
    public override string ToString()
    {
        return Id.ToString("D");
    }

    #endregion
}

""";
                var sourceFileName =  sourceBuilder.ToSourceFilePath(targetSymbol);
                context.AddSource(sourceFileName, source);
            }
        }

        #endregion
    }
}
