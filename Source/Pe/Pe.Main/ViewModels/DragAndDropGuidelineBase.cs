using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels
{
    /// <summary>
    /// たぶんめちゃくちゃになっていくD&amp;D処理のメソッドを整理したかったのではないかと。。。
    /// </summary>
    public abstract class DragAndDropGuidelineBase
    {
        protected DragAndDropGuidelineBase(IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
        {
            ContextDispatcher = contextDispatcher;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IContextDispatcher ContextDispatcher { get; }
        protected ILogger Logger { get; }

        #endregion
    }
}
