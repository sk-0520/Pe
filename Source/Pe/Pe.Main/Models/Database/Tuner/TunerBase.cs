using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Tuner
{
    public abstract class TunerBase
    {
        public TunerBase(IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            StatementLoader = statementLoader;
            Logger = loggerFactory.CreateLogger(GetType());

            var commonStatus = DatabaseCommonStatus.CreateCurrentAccount();
            TuneCommonDtoSource = commonStatus.CreateCommonDtoMapping();
        }

        #region property

        protected IDatabaseStatementLoader StatementLoader { get; }
        protected ILogger Logger { get; }

        IDictionary<string, object> TuneCommonDtoSource { get; }
        #endregion

        #region function

        protected IDictionary<string, object> GetCommonDto()
        {
            return new Dictionary<string, object>(TuneCommonDtoSource);
        }

        protected abstract void TuneImpl(IDatabaseCommander commander);

        public void Tune(IDatabaseCommander commander)
        {
            TuneImpl(commander);
        }

        #endregion
    }
}