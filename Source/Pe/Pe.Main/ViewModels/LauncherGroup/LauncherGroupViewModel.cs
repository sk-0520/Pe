using System;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Mvvm.Binding;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherGroup
{
    public class LauncherGroupViewModel: SingleModelViewModelBase<LauncherGroupElement>
    {
        public LauncherGroupViewModel(LauncherGroupElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        public LauncherGroupId LauncherGroupId => Model.LauncherGroupId;

        public int RowIndex { get; set; }
        private IDispatcherWrapper DispatcherWrapper { get; }
        public string? Name => Model.Name;
        public LauncherGroupImageName ImageName => Model.ImageName;
        public Color ImageColor => Model.ImageColor;

        private LauncherGroupIconMaker IconMaker { get; } = new LauncherGroupIconMaker();

        public DependencyObject NormalGroupIcon => IconMaker.GetGroupImage(ImageName, ImageColor, IconBox.Small, IconSize.DefaultScale, false);
        public DependencyObject StrongGroupIcon => IconMaker.GetGroupImage(ImageName, ImageColor, IconBox.Small, IconSize.DefaultScale, true);

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region SingleModelViewModelBase


        #endregion
    }
}
