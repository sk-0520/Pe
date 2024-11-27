using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Mvvm.Binding
{
    public abstract class BindModelWithLoggerBase: BindModelBase
    {
        protected BindModelWithLoggerBase(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILogger Logger { get; }
        protected ILoggerFactory LoggerFactory { get; }

        #endregion
    }
}
