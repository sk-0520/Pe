using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Main.AppMode.CrashReport.Models.Data;
using ContentTypeTextNet.Pe.Main.AppMode.Database.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.AppMode.Database.Models.Element
{
    internal class DatabaseElement: ElementBase, IViewCloseReceiver
    {
        public DatabaseElement(DatabaseOptions options, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Options = options;
        }

        #region property
        private DatabaseOptions Options { get; }

        public bool ViewCreated { get; private set; }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            //nop
        }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            return false;
        }
        public bool ReceiveViewClosing()
        {
            return true;
        }

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosed(bool)"/>
        public void ReceiveViewClosed(bool isUserOperation)
        {
            ViewCreated = false;
        }

        #endregion
    }
}
