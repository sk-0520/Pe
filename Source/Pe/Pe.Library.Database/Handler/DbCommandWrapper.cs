using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    public class DbCommandWrapper: DbCommand
    {
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_" + nameof(DbParameterCollection))]
        private static extern DbParameterCollection Get_DbParameterCollection(DbCommand dbCommand);

        public DbCommandWrapper(DbCommand command, MiddlewareCollection middlewareCollection)
        {
            Raw = command;
            MiddlewareCollection = middlewareCollection;
        }

        #region property

        public DbCommand Raw { get; private set; }
        public MiddlewareCollection MiddlewareCollection { get; }

        #endregion

        #region function

        protected ExecuteNonQueryPipeline CreateExecuteNonQueryPipeline()
        {
            var pipeline = new ExecuteNonQueryPipeline();
            pipeline.UseRange(MiddlewareCollection.ExecuteNonQuerys);
            return pipeline;
        }

        protected virtual IExecuteNonQueryHandler CreateExecuteNonQueryProcess()
        {
            return new DefaultExecuteNonQueryProcess();
        }

        protected ExecuteScalarPipeline CreateExecuteScalarPipeline()
        {
            var pipeline = new ExecuteScalarPipeline();
            pipeline.UseRange(MiddlewareCollection.ExecuteScalars);
            return pipeline;
        }

        protected virtual IExecuteScalarHandler CreateExecuteScalarProcess()
        {
            return new DefaultExecuteScalarProcess();
        }

        protected ExecuteDataReaderPipeline CreateExecuteDataReaderPipeline()
        {
            var pipeline = new ExecuteDataReaderPipeline();
            pipeline.UseRange(MiddlewareCollection.ExecuteDataReaders);
            return pipeline;
        }

        protected virtual IExecuteDataReaderHandler CreateExecuteDataReaderProcess()
        {
            return new DefaultExecuteDataReaderProcess();
        }

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
            var pipeline = CreateExecuteNonQueryPipeline();
            var process = CreateExecuteNonQueryProcess();
            var handler = pipeline.Build(process);
            var result = handler.Handle(Raw, default);

            return result;
        }

        public override object? ExecuteScalar()
        {
            var pipeline = CreateExecuteScalarPipeline();
            var process = CreateExecuteScalarProcess();
            var handler = pipeline.Build(process);
            var result = handler.Handle(Raw, default);

            return result;
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            var pipeline = CreateExecuteDataReaderPipeline();
            var process = CreateExecuteDataReaderProcess();
            var handler = pipeline.Build(process);
            var result = handler.Handle(Raw, behavior, default!);

            return result;
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
