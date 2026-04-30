using System.Windows;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Microsoft.Extensions.Logging;

#pragma warning disable CA1707 // 識別子はアンダースコアを含むことはできません
namespace ContentTypeTextNet.Pe.Main.Views._Debug_
#pragma warning restore CA1707 // 識別子はアンダースコアを含むことはできません
{
    /// <summary>
    /// DebugColorPickerWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DebugColorPickerWindow: Window
    {
        public DebugColorPickerWindow()
        {
            InitializeComponent();
        }

        [DiInjection]
        ILogger? Logger { get; set; }
    }
}
