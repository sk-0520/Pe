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
            return "using {originalFullName};";
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
            return string.Join(", ", method.GetParameters().Select(a => $"{SourceHelper.ToCSharpType(a.ParameterType)} {a.Name}"));
        }

        public string CreateInterfaceStatement()
        {
            var source = new SourceWriter(SourceSetting);

            source.AppendLine($"public interface {ProxyInterface}");
            using(source.Block()) {
                if(Properties.Any()) {
                    using(source.Region(Properties.Count.ToString(CultureInfo.InvariantCulture))) {
                        foreach(var property in Properties) {
                            source.AppendLine(CreateRefDocument(property));
                            source.AppendLine($"public {SourceHelper.ToSourceType(property.PropertyType)} {property.Name}");
                            using(source.Block()) {
                                if(property.CanRead) {
                                    source.AppendLine("get;");
                                }
                                if(property.CanWrite) {
                                    source.AppendLine("set;");
                                }
                            }
                        }
                    }
                }

                if(Methods.Any()) {
                    using(source.Region(Methods.Count.ToString(CultureInfo.InvariantCulture))) {
                        foreach(var method in Methods) {
                            source.AppendLine(CreateRefDocument(method));
                            source.AppendLine($"public {SourceHelper.ToSourceType(method.ReturnType)} {method.Name}({CreateMethodParameters(method)})");
                        }
                    }
                }
            }

            return source.ToString();
        }

        public string CreateImplementStatement()
        {
            var source = new StringBuilder();

            source.AppendLine($"public class {ProxyImplement}");
            source.AppendLine("{");

            if(Properties.Any()) {
                source.AppendLine($"#region property (<#= {Properties.Count} #>)");
                source.AppendLine("#endregion");
            }
            if(Methods.Any()) {
                source.AppendLine($"#region function (<#= {Methods.Count} #>)");
                source.AppendLine("#endregion");
            }

            source.AppendLine("}");

            return source.ToString();
        }

        #endregion
    }
}
