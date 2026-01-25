using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    /// <summary>
    /// データベース処理に対するミドルウェア群。
    /// </summary>
    /// <remarks>
    /// <para>先頭がパイプライン上の外側にあたる。</para>
    /// </remarks>
    public class MiddlewareCollection
    {
        #region property

        public List<IStatementMiddleware> Statements { get; init; } = [];
        public List<IExecuteNonQueryMiddleware> ExecuteNonQueries { get; init; } = [];
        public List<IExecuteScalarMiddleware> ExecuteScalars { get; init; } = [];
        public List<IExecuteDataReaderMiddleware> ExecuteDataReaders { get; init; } = [];

        #endregion

        #region function

        public MiddlewareCollection Clone()
        {
            return new MiddlewareCollection() {
                Statements = [.. Statements],
                ExecuteNonQueries = [.. ExecuteNonQueries],
                ExecuteScalars = [.. ExecuteScalars],
                ExecuteDataReaders = [.. ExecuteDataReaders],
            };
        }

        #endregion
    }

}
