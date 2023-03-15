using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Database.Query
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseEntityTableAttribute: Attribute
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="tableName">テーブル名。</param>
        public DatabaseEntityTableAttribute(string tableName)
        {
            TableName = tableName;
        }

        #region property

        /// <summary>
        /// テーブル名。
        /// </summary>
        public string TableName { get; }

        #endregion
    }
}
