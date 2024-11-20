using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.CliProxy
{
    public class TypeGenerator
    {
        public TypeGenerator(ISourceSetting sourceSetting, Type type, string baseNamespace, string filePath)
        {
            SourceSetting = sourceSetting;
            TargetType = type;

            Properties = TargetType.GetProperties(BindingFlags.Public | BindingFlags.Static)
                .ToArray()
            ;
            Methods = TargetType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(a => !(a.Name.StartsWith("get_") || a.Name.StartsWith("set_")))
                .ToArray()
            ;

            BaseNamespace = baseNamespace;
            FilePath = filePath;

            OriginalAbsoluteName = "global::" + TargetType.FullName;
            ProxyInterface = $"IDirect{TargetType.Name}Proxy";
            ProxyImplement = $"Direct{TargetType.Name}Proxy";
        }

        #region property

        private ISourceSetting SourceSetting { get; }

        public string BaseNamespace { get; }
        public string FilePath { get; }

        public Type TargetType { get; }
        public IReadOnlyList<PropertyInfo> Properties { get; }
        public IReadOnlyList<MethodInfo> Methods { get; }

        public string OriginalAbsoluteName { get; }
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
            return $"namespace {BaseNamespace}.{TargetType.Namespace};";
        }

        public string CreateRefDocument(PropertyInfo property)
        {
            return $"/// <inheritdoc cref=\"{OriginalAbsoluteName}.{property.Name}\"/>";
        }

        public string CreateRefDocument(MethodInfo method)
        {
            return $"/// <inheritdoc cref=\"{OriginalAbsoluteName}.{method.Name}({string.Join(", ", method.GetParameters().Select(a => SourceHelper.ToSourceType(a.ParameterType)))})\"/>";
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
                        source.AppendLine($"get => {OriginalAbsoluteName}.{property.Name};");
                    } else {
                        source.AppendLine("get;");
                    }
                }
                if(property.CanWrite) {
                    if(isImplement) {
                        source.AppendLine($"set => {OriginalAbsoluteName}.{property.Name} = value;");
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
                source.AppendLine($"=> {OriginalAbsoluteName}.{method.Name}({SourceHelper.ToCalleParameters(method)})");
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
                source.AppendLine($"public class {ProxyImplement}: {ProxyInterface}");
            } else {
                source.AppendLine($"public class {ProxyImplement}");
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
