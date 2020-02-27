using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels
{
    public abstract class DragAndDropGuidelineBase
    {
        public DragAndDropGuidelineBase(IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IDispatcherWrapper DispatcherWrapper { get; }
        protected ILogger Logger { get; }

        #endregion;
    }
}