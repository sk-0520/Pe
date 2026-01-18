using ContentTypeTextNet.Pe.Library.Common;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public abstract class BindModelWithLoggerBase: ContentTypeTextNet.Pe.Mvvm.Bindings.BindModelBase
    {
        protected BindModelWithLoggerBase(ILoggerFactory loggerFactory, EventReference propertyChangedEventReference, EventReference disposingEventReference)
            : base(propertyChangedEventReference, disposingEventReference)
        {

            Logger = loggerFactory.CreateLogger(GetType());
            LoggerFactory = loggerFactory;
        }

        protected BindModelWithLoggerBase(ILoggerFactory loggerFactory)
            : this(loggerFactory, EventReference.Weak, EventReference.Weak)
        { }

        #region property

        protected ILogger Logger { get; private set; }
        protected ILoggerFactory LoggerFactory { get; private set; }


        #endregion
    }
}
