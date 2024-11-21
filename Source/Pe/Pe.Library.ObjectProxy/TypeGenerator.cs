using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ContentTypeTextNet.Pe.Library.ObjectProxy
{
    public class TypeGenerator
    {
        public TypeGenerator(ISourceSetting sourceSetting, INamedTypeSymbol symbol, TypeDeclarationSyntax node)
        {
            SourceSetting = sourceSetting;

            Symbol = symbol;
            Node = node;

            var attribute = Symbol.GetAttributes().First(a => a.AttributeClass?.Name == SourceHelper.AttributeClassName);
            var targetType = (INamedTypeSymbol)attribute.ConstructorArguments[0].Value!;
            TargetType = Type.GetType(targetType.Name);

            TargetAbsoluteName = $"global::{TargetType.FullName}";

            Properties = TargetType.GetProperties(BindingFlags.Public | BindingFlags.Static)
                .ToArray()
            ;
            Methods = TargetType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(a => !(a.Name.StartsWith("get_") || a.Name.StartsWith("set_")))
                .ToArray()
            ;

            SourceNamespace = Symbol.ContainingNamespace.IsGlobalNamespace
                ? string.Empty
                : Symbol.ContainingNamespace.ToString()
            ;
            SourceFilePath = $"{Symbol.Name}.g.cs";

            ProxyInterface = Symbol.Name;
            ProxyImplement = $"I{Symbol.Name}";
        }

        #region property

        private ISourceSetting SourceSetting { get; }

        public string SourceNamespace { get; }
        public string SourceFilePath { get; }

        private INamedTypeSymbol Symbol { get; }
        private TypeDeclarationSyntax Node { get; }
        private Type TargetType { get; }
        private string TargetAbsoluteName { get; }

        public IReadOnlyList<PropertyInfo> Properties { get; }
        public IReadOnlyList<MethodInfo> Methods { get; }

        public string ProxyInterface { get; }
        public string ProxyImplement { get; }


        #endregion

        #region function

        public string CreateUsingStatement()
        {
            return $"using global::{TargetType.Namespace};";
        }

        public string CreateNamespaceStatement()
        {
            return $"namespace {SourceNamespace};";
        }

        public string CreateRefDocument(PropertyInfo property)
        {
            return $"/// <inheritdoc cref=\"{TargetAbsoluteName}.{property.Name}\"/>";
        }

        public string CreateRefDocument(MethodInfo method)
        {
            return $"/// <inheritdoc cref=\"{TargetAbsoluteName}.{method.Name}({string.Join(", ", method.GetParameters().Select(a => SourceHelper.ToSourceType(a.ParameterType)))})\"/>";
        }

        public string CreateMethodParameters(MethodInfo method)
        {
            return string.Join(", ", SourceHelper.ToSignatureParameters(method));
        }

        private void WritePropertyStatement(SourceWriter source, bool isImplement, PropertyInfo property)
        {
            source.AppendLine(CreateRefDocument(property));
            source.AppendLine($"public {SourceHelper.ToSourceType(property.PropertyType)} {property.Name}");
            using(source.Block()) {
                if(property.CanRead) {
                    if(isImplement) {
                        source.AppendLine($"get => {TargetAbsoluteName}.{property.Name};");
                    } else {
                        source.AppendLine("get;");
                    }
                }
                if(property.CanWrite) {
                    if(isImplement) {
                        source.AppendLine($"set => {TargetAbsoluteName}.{property.Name} = value;");
                    } else {
                        source.AppendLine("set;");
                    }
                }
            }
        }

        private void WritePropertiesStatement(SourceWriter source, bool isImplement)
        {
            if(Properties.Any()) {
                using(source.Region(Properties.Count.ToString(CultureInfo.InvariantCulture))) {
                    foreach(var property in Properties) {
                        WritePropertyStatement(source, isImplement, property);
                    }
                }
            }
        }

        private void WriteMethodStatement(SourceWriter source, bool isImplement, MethodInfo method)
        {
            source.AppendLine(CreateRefDocument(method));
            source.Append($"public {SourceHelper.ToSourceType(method.ReturnType)} {method.Name}({CreateMethodParameters(method)})");
            if(isImplement) {
                source.AppendLine($"=> {TargetAbsoluteName}.{method.Name}({SourceHelper.ToCalleParameters(method)})");
            }
            source.AppendLine(";");
        }

        private void WriteMethodsStatement(SourceWriter source, bool isImplement)
        {
            if(Methods.Any()) {
                using(source.Region(Methods.Count.ToString(CultureInfo.InvariantCulture))) {
                    foreach(var method in Methods) {
                        WriteMethodStatement(source, isImplement, method);
                    }
                }
            }
        }

        public string CreateInterfaceStatement()
        {
            var source = new SourceWriter(SourceSetting);

            source.AppendLine($"public interface {ProxyInterface}");
            using(source.Block()) {
                WritePropertiesStatement(source, false);
                WriteMethodsStatement(source, false);
            }

            return source.ToString();
        }

        public string CreateImplementStatement(bool implementsProxy)
        {
            var source = new SourceWriter(SourceSetting);

            if(implementsProxy) {
                source.AppendLine($"partial class {ProxyImplement}: {ProxyInterface}");
            } else {
                source.AppendLine($"partial interface {ProxyInterface}");
            }
            using(source.Block()) {
                WritePropertiesStatement(source, true);
                WriteMethodsStatement(source, true);
            }

            return source.ToString();
        }

        #endregion
    }
}
