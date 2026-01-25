using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ContentTypeTextNet.Pe.Library.Database.Handler
{
    /// <summary>
    /// <see cref="DbCommand"/> ラッパー。
    /// </summary>
    /// <remarks><see cref="IDbCommand"/> は知らん。</remarks>
    public class DbCommandWrapper: DbCommand
    {
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_" + nameof(DbParameterCollection))]
        private static extern DbParameterCollection Get_DbParameterCollection(DbCommand dbCommand);

        public DbCommandWrapper(DbCommand command, MiddlewareCollection middlewareCollection)
        {
            BaseCommand = command;
            MiddlewareCollection = middlewareCollection;
        }

        #region property

        /// <summary>
        /// 元の <see cref="DbCommand"/>。
        /// </summary>
        public DbCommand BaseCommand { get; private set; }
        /// <inheritdoc cref="ContentTypeTextNet.Pe.Library.Database.Handler.MiddlewareCollection"/>
        public MiddlewareCollection MiddlewareCollection { get; }

        #endregion

        #region function

        protected ExecuteNonQueryPipeline CreateExecuteNonQueryPipeline()
        {
            var pipeline = new ExecuteNonQueryPipeline();
            pipeline.UseRange(MiddlewareCollection.ExecuteNonQueries);
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
            get => BaseCommand.CommandText;
            set => BaseCommand.CommandText = value;
        }

        public override int CommandTimeout
        {
            get => BaseCommand.CommandTimeout;
            set => BaseCommand.CommandTimeout = value;
        }

        public override CommandType CommandType
        {
            get => BaseCommand.CommandType;
            set => BaseCommand.CommandType = value;
        }

        public override bool DesignTimeVisible
        {
            get => BaseCommand.DesignTimeVisible;
            set => BaseCommand.DesignTimeVisible = value;
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get => BaseCommand.UpdatedRowSource;
            set => BaseCommand.UpdatedRowSource = value;
        }

        protected override DbConnection? DbConnection
        {
            get => BaseCommand.Connection;
            set => BaseCommand.Connection = value;
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                return Get_DbParameterCollection(BaseCommand);
            }
        }

        protected override DbTransaction? DbTransaction
        {
            get => BaseCommand.Transaction;
            set => BaseCommand.Transaction = value;
        }

        public override void Cancel()
        {
            BaseCommand.Cancel();
        }

        public override int ExecuteNonQuery()
        {
            var pipeline = CreateExecuteNonQueryPipeline();
            var process = CreateExecuteNonQueryProcess();
            var handler = pipeline.Build(process);
            var result = handler.Invoke(BaseCommand, default);

            return result;
        }

        public override object? ExecuteScalar()
        {
            var pipeline = CreateExecuteScalarPipeline();
            var process = CreateExecuteScalarProcess();
            var handler = pipeline.Build(process);
            var result = handler.Invoke(BaseCommand, default);

            return result;
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            var pipeline = CreateExecuteDataReaderPipeline();
            var process = CreateExecuteDataReaderProcess();
            var handler = pipeline.Build(process);
            var result = handler.Invoke(BaseCommand, behavior, default!);

            return result;
        }

        public override void Prepare()
        {
            BaseCommand.Prepare();
        }

        protected override DbParameter CreateDbParameter()
        {
            return BaseCommand.CreateParameter();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing) {
                BaseCommand?.Dispose();
                BaseCommand = null!;
            }

            base.Dispose(disposing);
        }


        #endregion

    }
}
