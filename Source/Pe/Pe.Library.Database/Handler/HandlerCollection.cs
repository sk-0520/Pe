using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public class HandlerCollection
    {
        #region property

        public List<IStatementHandler> StatementHandlers { get; } = new List<IStatementHandler>();
        public List<IExecuteNonQueryHandler> ExecuteNonQueryHandlers { get; } = new List<IExecuteNonQueryHandler>();
        public List<IExecuteScalarHandler> ExecuteScalarHandlers { get; } = new List<IExecuteScalarHandler>();
        public List<IExecuteDbDataReaderHandler> ExecuteDbDataReaderHandlers { get; } = new List<IExecuteDbDataReaderHandler>();

        #endregion
    }
}
