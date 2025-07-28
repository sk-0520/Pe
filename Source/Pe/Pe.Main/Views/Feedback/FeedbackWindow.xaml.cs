using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Mvvm.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Feedback
{
    /// <summary>
    /// FeedbackWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class FeedbackWindow: Window
    {
        public FeedbackWindow()
        {
            InitializeComponent();
        }

        #region property

        private ICommandFactory CommandFactory { get; } = new CommandFactory();

        #endregion

        #region command

        private ICommand? _CloseCommand;
        public ICommand CloseCommand => this._CloseCommand ??= CommandFactory.Create(
            () => Close()
        );

        #endregion

    }
}
