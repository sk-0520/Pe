using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Bridge.ViewModels
{
    public class SkeletonViewModel: INotifyPropertyChanged
    {
        protected SkeletonViewModel(INotifyPropertyFactory notifyPropertyFactory, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            NotifyPropertyFactory = notifyPropertyFactory;

            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        protected ILogger Logger { get; }
        protected ILoggerFactory LoggerFactory { get; }

        protected INotifyPropertyFactory NotifyPropertyFactory { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;


        #endregion
    }
}
