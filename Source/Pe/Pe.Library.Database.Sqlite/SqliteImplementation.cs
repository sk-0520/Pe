#define USE_DB_SQLITE
#if USE_DB_SQLITE

using System;
using System.Data;
using System.Globalization;
using ContentTypeTextNet.Pe.Library.Database.Implementations;
using Dapper;

namespace ContentTypeTextNet.Pe.Library.Database.Sqlite
{
    /// <summary>
    /// booleanを制御
    /// </summary>
    /// <remarks>
    /// <para>0: 偽, 0以外: 真</para>
    /// </remarks>
    internal class SqliteBooleanHandler: SqlMapper.TypeHandler<bool>
    {
        public override void SetValue(IDbDataParameter parameter, bool value)
        {
            parameter.Value = value ? 1 : 0;
        }

        public override bool Parse(object value)
        {
            if(value == null) {
                return false;
            }

            if(value is bool result) {
                return result;
            }

            return (long)value != 0;
        }
    }

    internal class SqliteVersionHandler: SqlMapper.TypeHandler<Version>
    {
        public override void SetValue(IDbDataParameter parameter, Version? value)
        {
            parameter.DbType = DbType.String;
            parameter.Value = value?.ToString(3);
        }

        public override Version Parse(object value)
        {
            if(value is string s) {
                if(Version.TryParse(s, out var ret)) {
                    return ret;
                }
            }

            return null!;
        }
    }

    internal class SqliteGuidHandler: SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            // 00000000-0000-0000-0000-000000000000
            parameter.Value = value.ToString("D");
        }

        public override Guid Parse(object value)
        {
            var s = (string)value;
            if(s != null) {
                if(Guid.TryParse(s, out var ret)) {
                    return ret;
                }
            }

            return Guid.Empty;
        }
    }

    internal class SqliteTimeSpanHandler: SqlMapper.TypeHandler<TimeSpan>
    {
        public override void SetValue(IDbDataParameter parameter, TimeSpan value)
        {
            // [-][d.]hh:mm:ss[.fffffff]
            parameter.Value = value.ToString("c");
        }

        public override TimeSpan Parse(object value)
        {
            var s = (string)value;
            if(s != null) {
                if(TimeSpan.TryParse(s, CultureInfo.InvariantCulture, out var ret)) {
                    return ret;
                }
            }

            return TimeSpan.Zero;
        }

    }

    public class SqliteImplementation: DatabaseImplementation
    {
        static SqliteImplementation()
        {
            SqlMapper.AddTypeMap(typeof(TimeSpan), DbType.String);
            SqlMapper.AddTypeHandler(new SqliteBooleanHandler());
            SqlMapper.AddTypeHandler(new SqliteVersionHandler());
            SqlMapper.AddTypeHandler(new SqliteGuidHandler());
            SqlMapper.AddTypeHandler(new SqliteTimeSpanHandler());
        }

        #region DatabaseImplementation

        public override bool SupportedTransactionDDL { get; } = true;

        public override string ToStatementTableName(string tableName) => "[" + tableName + "]";
        public override string ToStatementColumnName(string columnName) => "[" + columnName + "]";
        public override string ToStatementParameterName(string parameterName, int index) => "@" + parameterName;

        public override IDatabaseManagement CreateManagement(IDatabaseContext context)
        {
            return new SqliteManagement(context, this);
        }

        #endregion
    }
}

#endif
