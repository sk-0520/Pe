using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Generator.Throws
{
    internal static class DiagnosticDescriptors
    {
        #region define

        public static readonly DiagnosticDescriptor NestedTypeNotSupported = new DiagnosticDescriptor(
            id: "PEEG001",
            title: "Nested type not supported",
            messageFormat: "The type '{0}' is nested inside another type. Apply the attribute only to top-level types.",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        #endregion
    }
}
