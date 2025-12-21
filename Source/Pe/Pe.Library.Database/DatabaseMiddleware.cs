using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Database
{
    public readonly record struct DatabaseStatementData(string Statement, object? parameter);

    public interface IDatabaseStatementMiddleware
    {
        #region function

        DatabaseStatementData Next(in DatabaseStatementData data);

        #endregion
    }

    public interface IDatabaseReaderMiddleware
    {
        #region function


        #endregion
    }

    public interface IDatabaseExecutorMiddleware
    {
        #region function


        #endregion
    }

}
