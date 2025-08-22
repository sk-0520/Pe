using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize
{
    internal class EnvRemoveCompletionData: ICompletionData
    {
        public EnvRemoveCompletionData(string environmentVariableName, string environmentVariableValue)
        {
            EnvironmentVariableName = environmentVariableName;
            EnvironmentVariableValue = environmentVariableValue;
        }

        #region property

        private string EnvironmentVariableName { get; }
        private string EnvironmentVariableValue { get; }

        #endregion

        #region ICompletionData

        public ImageSource? Image { get; } = null;

        public string Text => EnvironmentVariableName;

        public object Content => EnvironmentVariableName;

        public object Description => EnvironmentVariableValue;

        public double Priority => 0;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, EnvironmentVariableName);
        }

        #endregion
    }
}
