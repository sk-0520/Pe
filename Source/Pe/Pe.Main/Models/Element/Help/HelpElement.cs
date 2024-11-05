using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Help
{
    public class HelpElement: ElementBase
    {
        public HelpElement(EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            EnvironmentParameters = environmentParameters;
        }

        #region property

        private EnvironmentParameters EnvironmentParameters { get; }

        public FileInfo HtmlFile => EnvironmentParameters.HelpFile;

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
