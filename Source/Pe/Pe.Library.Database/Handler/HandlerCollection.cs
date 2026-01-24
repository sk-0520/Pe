using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public class HandlerCollection
    {
        #region property

        public List<StatementHandlerBase> StatementHandlers { get; } = new List<StatementHandlerBase>();
        public List<IExecuteNonQueryHandler> ExecuteNonQueryHandlers { get; } = new List<IExecuteNonQueryHandler>();

        #endregion
    }
}
