using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class SettingViewModel : SingleModelViewModelBase<SettingElement>, IViewLifecycleReceiver
    {
        public SettingViewModel(SettingElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            LauncherItemSetting = new LauncherItemSettingViewModel(Model.LauncherItemSetting, LoggerFactory);
        }

        #region property

        public LauncherItemSettingViewModel LauncherItemSetting { get; }
        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }

        public void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed()
        {
            Model.ReceiveViewClosed();
        }

        #endregion
    }
}
