using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element._Debug_;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels._Debug_
{
    public abstract class DebugViewModelBase<TModel> : SingleModelViewModelBase<TModel>, IViewLifecycleReceiver
        where TModel : DebugElementBase
    {
        public DebugViewModelBase(TModel model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region IViewLifecycleReceiver

        public virtual void ReceiveViewInitialized(Window window)
        {
        }

        public virtual void ReceiveViewLoaded(Window window)
        {
        }

        public void ReceiveViewUserClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }

        public void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed(Window window)
        {
            Model.ReceiveViewClosed();
        }

        #endregion

    }
}
