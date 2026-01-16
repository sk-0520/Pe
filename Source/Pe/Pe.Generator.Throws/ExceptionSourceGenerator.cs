using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Generator.Throws
{
    [Generator(LanguageNames.CSharp)]
    public class ExceptionSourceGenerator: IIncrementalGenerator
    {
        #region function

        private static string ToAttributeCode(SourceBuilder sourceBuilder, AttributeData attr)
        {
            var className = attr.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) ?? "";
            var args = new List<string>();

            foreach(var c in attr.ConstructorArguments) {
                args.Add(sourceBuilder.ToCode(c));
            }
            foreach(var na in attr.NamedArguments) {
                args.Add(na.Key + " = " + sourceBuilder.ToCode(na.Value));
            }

            return "[" + className + "(" + string.Join(", ", args) + ")]";
        }

        private static string ToConstructorParameterCode(IParameterSymbol parameter)
        {
            var t = parameter.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var name = parameter.Name;
            return t + " " + name;
        }

        private static string GenerateSourceCore(SourceBuilder sourceBuilder, INamedTypeSymbol baseType, INamedTypeSymbol targetSymbol)
        {
            var targetName = targetSymbol.Name;

            // クラス属性を親から持ってくる＋謎の SerializableAttribute を追加
            var classAttributeCodes = sourceBuilder
                .FilterAttribute(baseType.GetAttributes())
                .Select(a => ToAttributeCode(sourceBuilder, a))
                .Prepend("[global::System.SerializableAttribute]")
                .Prepend("[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]")
                .Distinct()
                .ToArray()
            ;

            // 親の非 private コンストラクタを列挙
            var constructors = baseType.InstanceConstructors
                .Where(c => c.DeclaredAccessibility != Accessibility.Private)
                .ToArray()
            ;

            var constructorCodes = new List<string>();

            foreach(var ctor in constructors) {
                // コンストラクタ属性をコピー
                var constructorAttributeCodes = sourceBuilder
                    .FilterAttribute(ctor.GetAttributes())
                    .Select(a => ToAttributeCode(sourceBuilder, a))
                    .ToArray()
                ;

                var accessibility = sourceBuilder.ToCode(ctor.DeclaredAccessibility);

                var parameters = ctor.Parameters.Select(a => ToConstructorParameterCode(a));
                var args = ctor.Parameters.Select(p => p.Name);

                var signature = $"{accessibility} {targetName}({string.Join(", ", parameters)})";

                var constructorCode = $$"""

                    {{string.Join("", constructorAttributeCodes)}}
                    {{signature}}
                        : base({{string.Join(", ", args)}})
                    { }

                """;
                constructorCodes.Add(constructorCode);
            }

            var classAccessibilityCode = sourceBuilder.ToCode(targetSymbol.DeclaredAccessibility);

            var source = $$"""
{{sourceBuilder.Header}}

{{(sourceBuilder.ToNamespaceCode(targetSymbol))}}

{{string.Join("", classAttributeCodes)}}
{{classAccessibilityCode}} partial class {{targetName}}
{
{{string.Join("", constructorCodes)}}
}

""";
            return source;
        }

        private static void GenerateSource(SourceProductionContext context, ImmutableArray<GeneratorAttributeSyntaxContext> array)
        {
            var sourceBuilder = new SourceBuilder();

            // マークされた型を集める（ジェネレータ対象の型集合）
            var marked = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
            foreach(var attrCtx in array) {
                if(attrCtx.TargetSymbol is INamedTypeSymbol nts) {
                    marked.Add(nts);
                }
            }

            foreach(var attribute in array) {
                var targetSymbol = (INamedTypeSymbol)attribute.TargetSymbol;

                // ネストは無理
                if(targetSymbol.ContainingType != null) {
                    var location = targetSymbol.Locations.Length > 0 ? targetSymbol.Locations[0] : Location.None;
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.NestedTypeNotSupported, location, targetSymbol.ToDisplayString()));
                    continue;
                }

                var baseType = targetSymbol.BaseType;
                if(baseType is null) {
                    continue;
                }

                // ベースがジェネレータで生成される型の場合、その型の親へ遡る
                var effectiveBase = baseType;
                while(effectiveBase != null) {
                    // 同一コンパイル内でマーク済みかを判定
                    if(marked.Contains(effectiveBase)) {
                        effectiveBase = effectiveBase.BaseType;
                        continue;
                    }
                    break;
                }

                if(effectiveBase is null) {
                    effectiveBase = baseType;
                }

                var source = GenerateSourceCore(sourceBuilder, effectiveBase, targetSymbol);
                var sourceFileName = sourceBuilder.ToSourceFilePath(targetSymbol);
                context.AddSource(sourceFileName, source);
            }
        }

        #endregion


        #region IIncrementalGenerator

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var sourceBuilder = new SourceBuilder();

            var attributeNamespace = "ContentTypeTextNet.Pe.Generator.Throws";
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

        #endregion
    }
}
