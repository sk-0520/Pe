using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public class HandlerCollection
    {
        #region property

        public List<IStatementHandler> StatementHandlers { get; init; } = [];
        public List<IExecuteNonQueryHandler> ExecuteNonQueryHandlers { get; init; } = [];
        public List<IExecuteScalarHandler> ExecuteScalarHandlers { get; init; } = [];
        public List<IExecuteDataReaderHandler> ExecuteDataReaderHandlers { get; init; } = [];

        #endregion
    }
}
