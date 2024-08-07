using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Standard.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar
{
    public class LauncherFileItemDragAndDrop: DragAndDropGuidelineBase
    {
        public LauncherFileItemDragAndDrop(IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(dispatcherWrapper, loggerFactory)
        { }

        #region function

        public IResultSuccess<DragParameter> GetDragParameter(UIElement sender, MouseEventArgs e) => Result.CreateFailure<DragParameter>();

        public bool CanDragStart(UIElement sender, MouseEventArgs e) => false;

        public void DragOverOrEnter(UIElement sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                // ファイル登録準備
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if(filePaths.Length == 1) {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                } else {
                    e.Effects = DragDropEffects.None;
                    e.Handled = true;
                }
            } else {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        public async Task DropAsync(UIElement sender, DragEventArgs e, Action<string> action, CancellationToken cancellationToken)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if(filePaths.Length == 1) {
                    await DispatcherWrapper.BeginAsync(() => action(filePaths[0]));
                    e.Handled = true;
                }
            }
        }

        public void DragLeave(UIElement sender, DragEventArgs e)
        { }

        public void RegisterDropFile(IRequestSender requestSender, string path, Action<string, bool> register)
        {
            if(PathUtility.IsShortcut(path)) {
                var request = new CommonMessageDialogRequestParameter() {
                    Message = Properties.Resources.String_LauncherFileItemDragAndDrop_Shortcut_Message,
                    Caption = Properties.Resources.String_LauncherFileItemDragAndDrop_Shortcut_Caption,
                    Button = MessageBoxButton.YesNoCancel,
                    DefaultResult = MessageBoxResult.Yes,
                    Icon = MessageBoxImage.Question,
                };
                requestSender.Send<YesNoResponse>(request, r => {
                    if(r.ResponseIsCancel) {
                        Logger.LogTrace("ショートカット登録取り消し");
                        return;
                    }
                    register(path, r.ResponseIsYes);
                });
            } else {
                register(path, false);
            }
        }

        #endregion
    }
}
