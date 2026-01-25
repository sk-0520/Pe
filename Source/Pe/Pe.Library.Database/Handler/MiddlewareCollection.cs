using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    /// <summary>
    /// データベース処理に対するミドルウェア群。
    /// </summary>
    /// <remarks>
    /// <para>後ろに登録されているものから頭に向かって処理される。</para>
    /// </remarks>
    public class MiddlewareCollection
    {
        #region property

        public List<IStatementMiddleware> Statements { get; init; } = [];
        public List<IExecuteNonQueryMiddleware> ExecuteNonQuerys { get; init; } = [];
        public List<IExecuteScalarHandler> ExecuteScalars { get; init; } = [];
        public List<IExecuteDataReaderHandler> ExecuteDataReaders { get; init; } = [];

        #endregion
    }

}
