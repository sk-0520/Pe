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
    public abstract class BadgeViewModelBase: ViewModelBase
    {
        protected BadgeViewModelBase(IReadOnlyBadgeData badge, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Badge = badge;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        private IReadOnlyBadgeData Badge { get; }

        private IDispatcherWrapper DispatcherWrapper { get; }

        #region IReadOnlyBadgeData

        public bool IsVisible => Badge.IsVisible;
        public string Display => Badge.Display;
        public BadgeShape BadgeShape => Badge.BadgeShape;
        public Color Background => Badge.Background;

        #endregion

        #endregion

        #region function

        internal static BadgeViewModelBase Create(IReadOnlyBadgeData badge, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            return badge.BadgeShape switch {
                BadgeShape.RoundedSquare => new BadgeRoundedSquareViewModel(badge, dispatcherWrapper, loggerFactory),
                BadgeShape.SolidSquare => new BadgeSolidSquareViewModel(badge, dispatcherWrapper, loggerFactory),
                BadgeShape.Circle => new BadgeCircleViewModel(badge, dispatcherWrapper, loggerFactory),
                _ => throw new NotImplementedException(),
            };
        }

        #endregion
    }

    public sealed class BadgeRoundedSquareViewModel: BadgeViewModelBase
    {
        public BadgeRoundedSquareViewModel(IReadOnlyBadgeData badge, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(badge, dispatcherWrapper, loggerFactory)
        { }
    }

    public sealed class BadgeSolidSquareViewModel: BadgeViewModelBase
    {
        public BadgeSolidSquareViewModel(IReadOnlyBadgeData badge, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(badge, dispatcherWrapper, loggerFactory)
        { }
    }

    public sealed class BadgeCircleViewModel: BadgeViewModelBase
    {
        public BadgeCircleViewModel(IReadOnlyBadgeData badge, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(badge, dispatcherWrapper, loggerFactory)
        { }
    }

}
