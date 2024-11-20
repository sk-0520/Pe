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

                var source = new SourceWriter();
                var typeGenerator = new TypeGenerator(source, targetDefine, baseNamespace, targetDefine.FullName + ".cs");

                source.Append(typeGenerator.CreateUsingStatement());
                source.AppendEmptyLine();

                source.AppendLine(typeGenerator.CreateNamespaceStatement());
                source.AppendEmptyLine();

                // インターフェイス
                source.AppendLine(typeGenerator.CreateInterfaceStatement());
                source.AppendEmptyLine();

                // インターフェイスに対する直接クラス
                source.AppendLine(typeGenerator.CreateImplementStatement());
                source.AppendEmptyLine();

                context.AddSource(
                    hintName: typeGenerator.FilePath,
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
