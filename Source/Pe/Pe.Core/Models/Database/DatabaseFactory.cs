using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// データベース関連処理生成器。
    /// </summary>
    public interface IDatabaseFactory
    {
        #region function

        /// <summary>
        /// データベース接続処理の生成。
        /// </summary>
        /// <returns></returns>
        IDbConnection CreateConnection();

        /// <summary>
        /// <see cref="IDbDataAdapter"/>の生成。
        /// </summary>
        /// <returns></returns>
        IDbDataAdapter CreateDataAdapter();

        /// <summary>
        /// データベース実装依存処理の生成。
        /// </summary>
        /// <returns></returns>
        IDatabaseImplementation CreateImplementation();

        #endregion
    }
}
