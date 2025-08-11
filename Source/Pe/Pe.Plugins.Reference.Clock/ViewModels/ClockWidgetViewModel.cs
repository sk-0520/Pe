using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels
{
    public class ClockWidgetViewModel: ViewModelSkeleton
    {
        #region variable


        #endregion

        public ClockWidgetViewModel(ClockWidgetSetting setting, ISkeletonImplements skeletonImplements, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(skeletonImplements, contextDispatcher, loggerFactory)
        {
            Setting = setting;

            Content = Setting.ClockWidgetKind switch
            {
                ClockWidgetKind.SimpleAnalog => new ClockWidgetSimpleAnalogClockContentViewModel(skeletonImplements.Clone(), ContextDispatcher, loggerFactory),
                ClockWidgetKind.JaggyAnalog => new ClockWidgetJaggyAnalogClockContentViewModel(skeletonImplements.Clone(), ContextDispatcher, loggerFactory),
                _ => throw new NotImplementedException(),
            };
        }

        #region property

        private ClockWidgetSetting Setting { get; }

        private DispatcherTimer ClockTimer { get; } = new DispatcherTimer(DispatcherPriority.Normal);

        public ClockWidgetContentBaseViewModel Content { get; private set; }

        #endregion

        #region function

        public void StartClock()
        {
            ClockTimer.Tick += ClockTimer_Tick;
            ClockTimer.Interval = TimeSpan.FromMilliseconds(500);
            ClockTimer.Start();
        }

        public void StopClock()
        {
            ClockTimer.Stop();
            ClockTimer.Tick -= ClockTimer_Tick;
        }

        private void ClockTimer_Tick(object? sender, EventArgs e)
        {
            ClockTimer.Stop();

            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Setting.TimeZone);

            Content.SetTime(currentTime);

            ClockTimer.Interval = TimeSpan.FromMilliseconds(TimeSpan.FromSeconds(1).TotalMilliseconds - currentTime.Millisecond);
            ClockTimer.Start();
        }

        #endregion

        #region ViewModelSkeleton

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Content?.Dispose();
                    Content = null!;
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
