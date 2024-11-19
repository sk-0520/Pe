using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Library.CliProxy
{
    [Generator(LanguageNames.CSharp)]
    public partial class CliProxyGenerator: IIncrementalGenerator
    {
        #region define

        #endregion

        #region function

        private static string ToCSharpType(Type type)
        {
            switch(type.FullName) {
                case "System.Void": return "void";
                case "System.Int32": return "int";
                case "System.Int64": return "long";
                case "System.Boolean": return "bool";
                case "System.String": return "string";
            }

            return string.Empty;
        }

        private static string ToSourceType(Type type)
        {
            var cstype = ToCSharpType(type);
            if(cstype.Length != 0) {
                return cstype;
            }

            if(type.IsArray) {
                var elementType = type.GetElementType();
                var csArrayType = ToCSharpType(elementType);
                if(csArrayType.Length != 0) {
                    return csArrayType + "[]";
                }
            }

            if(type.IsGenericType) {
                var t = type.FullName;
                t = "global::" + t.Substring(0, t.IndexOf('`'));

                return t
                    + "<"
                    + string.Join(", ", type.GenericTypeArguments.Select(a => ToSourceType(a)))
                    + ">"
                ;
            }

            return "global::" + type.FullName.Replace('+', '.');
        }

        private static string ToDocumentParameter(ParameterInfo parameter)
        {
            return ToSourceType(parameter.ParameterType);
        }

        private static string ToSignatureParameter(ParameterInfo parameter)
        {
            return $"{ToSourceType(parameter.ParameterType)} {parameter.Name}";
        }

        private static string ToMethodParameter(ParameterInfo parameter)
        {
            return $"{parameter.Name}";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1035:アナライザーに対して禁止された API を使用しない", Justification = "<保留中>")]
        private void GenerateSource(IncrementalGeneratorPostInitializationContext context)
        {
            var baseNamespace = "ContentTypeTextNet.Pe.Library.CliProxy";
            var targetDefines = new[] {
                typeof(System.Environment),
                //typeof(System.IO.File),
                //typeof(System.IO.Directory),
            };

            foreach(var targetDefine in targetDefines) {
                context.CancellationToken.ThrowIfCancellationRequested();

                var targetType = Type.GetType(targetDefine.FullName, true);
                var properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Static);
                var methods = targetType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(a => !(a.Name.StartsWith("get_") || a.Name.StartsWith("set_")))
                    .ToArray()
                ;

                var proxyInterface = $"IDirect{targetDefine.Name}Proxy";
                var proxyImplement = $"Direct{targetDefine.Name}Proxy";

                var source = new StringBuilder();
                source.AppendLine($"using global::{targetDefine.FullName};");
                source.AppendLine();

                source.AppendLine($"namespace {baseNamespace}.{targetDefine.Namespace};");
                source.AppendLine();
                // インターフェイス
                source.AppendLine($"public interface {proxyInterface}");
                source.AppendLine("{");
                if(properties.Any()) {
                    source.AppendLine($"#region property (<#= {properties.Length} #>)");
                    foreach(var property in properties) {
                        source.AppendLine($"/// <inheritdoc cref=\"<#= originalClassFulleName #>.<#= proeprty.Name #>\"/>")
                    }
                    source.AppendLine("#endregion");
                }
                if(methods.Any()) {
                    source.AppendLine($"#region function (<#= {methods.Length} #>)");
                    source.AppendLine("#endregion");
                }
                source.AppendLine("}");
                source.AppendLine();
                // インターフェイスに対する直接クラス
                source.AppendLine($"public class {proxyImplement}");
                source.AppendLine("{");
                if(properties.Any()) {
                    source.AppendLine($"#region property (<#= {properties.Length} #>)");
                    source.AppendLine("#endregion");
                }
                if(methods.Any()) {
                    source.AppendLine($"#region function (<#= {methods.Length} #>)");
                    source.AppendLine("#endregion");
                }
                source.AppendLine("}");
                source.AppendLine();

                context.AddSource(
                    hintName: $"{baseNamespace}.{targetDefine.FullName}.cs",
                    source: source.ToString()
                );
            }
        }

        #endregion

        #region IIncrementalGenerator

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(GenerateSource);
        }

        #endregion
    }
}
