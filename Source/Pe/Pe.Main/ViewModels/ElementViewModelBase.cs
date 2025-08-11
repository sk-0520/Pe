using System.Diagnostics;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels
{
    public abstract class ElementViewModelBase<TElement>: SingleModelViewModelBase<TElement>
        where TElement : ElementBase
    {
        protected ElementViewModelBase(TElement model, IUserTracker userTracker, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            // 初期化完了済みかはデバッグ時に検知すべし
            Debug.Assert(Model.IsInitialized);

            UserTracker = userTracker;
            ContextDispatcher = contextDispatcher;
        }

        #region property

        protected IUserTracker UserTracker { get; }
        protected IContextDispatcher ContextDispatcher { get; }

        #endregion
    }
}
