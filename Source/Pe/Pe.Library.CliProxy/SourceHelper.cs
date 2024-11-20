using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.CliProxy
{
    public static class SourceHelper
    {
        #region function

        public static string ToCSharpType(Type type)
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

        public static string ToSourceType(Type type)
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

        public static string ToDocumentParameter(ParameterInfo parameter)
        {
            return ToSourceType(parameter.ParameterType);
        }

        public static string ToSignatureParameter(ParameterInfo parameter)
        {
            return $"{ToSourceType(parameter.ParameterType)} {parameter.Name}";
        }

        public static string ToSignatureParameters(MethodInfo method)
        {
            return string.Join(", ", method.GetParameters().Select(a => ToSignatureParameter(a)));
        }

        public static string ToCalleParameter(ParameterInfo parameter)
        {
            return $"{parameter.Name}";
        }

        public static string ToCalleParameters(MethodInfo method)
        {
            return string.Join(", ", method.GetParameters().Select(a => ToCalleParameter(a)));
        }

        #endregion
    }
}
