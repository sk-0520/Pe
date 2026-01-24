using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public class DbCommandWrapper: DbCommand
    {
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_" + nameof(DbParameterCollection))]
        private static extern DbParameterCollection Get_DbParameterCollection(DbCommand dbCommand);

        public DbCommandWrapper(DbCommand command, HandlerCollection handlerCollection)
        {
            Raw = command;
            HandlerCollection = handlerCollection;
        }

        #region property

        public DbCommand Raw { get; private set; }
        public HandlerCollection HandlerCollection { get; }

        #endregion

        #region DbCommand

        [AllowNull]
        public override string CommandText
        {
            get => Raw.CommandText;
            set => Raw.CommandText = value;
        }

        public override int CommandTimeout
        {
            get => Raw.CommandTimeout;
            set => Raw.CommandTimeout = value;
        }

        public override CommandType CommandType
        {
            get => Raw.CommandType;
            set => Raw.CommandType = value;
        }

        public override bool DesignTimeVisible
        {
            get => Raw.DesignTimeVisible;
            set => Raw.DesignTimeVisible = value;
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get => Raw.UpdatedRowSource;
            set => Raw.UpdatedRowSource = value;
        }

        protected override DbConnection? DbConnection
        {
            get => Raw.Connection;
            set => Raw.Connection = value;
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                return Get_DbParameterCollection(Raw);
            }
        }

        protected override DbTransaction? DbTransaction
        {
            get => Raw.Transaction;
            set => Raw.Transaction = value;
        }

        public override void Cancel()
        {
            Raw.Cancel();
        }

        public override int ExecuteNonQuery()
        {
            var handlers = HandlerCollection.ExecuteNonQueryHandlers.Append(new ExecuteNonQueryAction());
            int result = 0;
            foreach(var handler in handlers) {
                result = handler.Next(this.Raw);
            }
            return result;
            // return Raw.ExecuteNonQuery();
        }

        public override object? ExecuteScalar()
        {
            var handlers = HandlerCollection.ExecuteScalarHandlers.Append(new ExecuteScalarAction());
            object? result = null;
            foreach(var handler in handlers) {
                result = handler.Next(this.Raw);
            }
            return result;
            // return Raw.ExecuteScalar();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return Raw.ExecuteReader(behavior);
        }

        public override void Prepare()
        {
            Raw.Prepare();
        }

        protected override DbParameter CreateDbParameter()
        {
            return Raw.CreateParameter();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing) {
                Raw?.Dispose();
                Raw = null!;
            }

            base.Dispose(disposing);
        }


        #endregion

    }
}
