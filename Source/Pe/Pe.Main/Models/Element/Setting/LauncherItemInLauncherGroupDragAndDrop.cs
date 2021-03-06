using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.ViewModels;
using ContentTypeTextNet.Pe.Main.ViewModels.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherItemInLauncherGroupDragAndDrop: DragAndDropGuidelineBase
    {
        public LauncherItemInLauncherGroupDragAndDrop(IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(dispatcherWrapper, loggerFactory)
        { }

        #region function

        public IResultSuccessValue<DragParameter> GetDragParameter(bool fromAllItems, UIElement sender, MouseEventArgs e, Action<LauncherItemSettingEditorViewModel> selectedItemChanger)
        {
            if(e.Source is ListBox listbox) {
                var scollbar = UIUtility.GetVisualClosest<ScrollBar>((DependencyObject)e.OriginalSource);
                if(scollbar == null && listbox.SelectedItem != null) {
                    var item = (LauncherItemSettingEditorViewModel)listbox.SelectedItem;
                    selectedItemChanger(item);
                    var data = new DataObject(typeof(LauncherItemDragData), new LauncherItemDragData(item, fromAllItems));
                    return ResultSuccessValue.Success(new DragParameter(sender, fromAllItems ? DragDropEffects.Copy : DragDropEffects.Move, data));
                }
            }

            return ResultSuccessValue.Failure<DragParameter>();
        }

        #endregion
    }
}
