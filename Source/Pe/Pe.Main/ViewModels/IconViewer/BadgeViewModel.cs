using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.IconViewer
{
    public class BadgeViewModel: ViewModelBase
    {
        public BadgeViewModel(IReadOnlyBadgeData badge, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Badge = badge;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        private IReadOnlyBadgeData Badge { get; }

        private IDispatcherWrapper DispatcherWrapper { get; }

        #region IReadOnlyBadgeData

        public bool IsEnabled => Badge.IsEnabled;
        public string Display => Badge.Display;
        public BadgeShape BadgeShape => Badge.BadgeShape;
        public Color Background => Badge.Background;

        #endregion

        #endregion

    }
}
