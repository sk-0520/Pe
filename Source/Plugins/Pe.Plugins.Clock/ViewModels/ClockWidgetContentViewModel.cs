using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Clock.ViewModels
{
    public abstract class ClockWidgetContentBaseViewModel: ViewModelSkeleton
    {
        #region property

        DateTime _currentTime;
        double _hourAngle;
        double _minutesAngle;
        double _secondsAngle;

        #endregion
        protected ClockWidgetContentBaseViewModel(ISkeletonImplements skeletonImplements, ILoggerFactory loggerFactory)
            : base(skeletonImplements, loggerFactory)
        { }

        #region property

        public DateTime CurrentTime
        {
            get => this._currentTime;
            set => SetProperty(ref this._currentTime, value);
        }

        public double HourAngle
        {
            get => this._hourAngle;
            set => SetProperty(ref this._hourAngle, value);
        }
        public double MinutesAngle
        {
            get => this._minutesAngle;
            set => SetProperty(ref this._minutesAngle, value);
        }
        public double SecondsAngle
        {
            get => this._secondsAngle;
            set => SetProperty(ref this._secondsAngle, value);
        }

        #endregion

        #region function

        public void SetTime(DateTime dateTime)
        {
            CurrentTime = dateTime;
            SecondsAngle = dateTime.Second * 6;
            MinutesAngle = dateTime.Minute * 6;
            HourAngle = (dateTime.Hour * 30) + (dateTime.Minute * 0.5);

        }

        #endregion
    }

    public class ClockWidgetSimpleAnalogClockContentViewModel: ClockWidgetContentBaseViewModel
    {
        public ClockWidgetSimpleAnalogClockContentViewModel(ISkeletonImplements skeletonImplements, ILoggerFactory loggerFactory)
            : base(skeletonImplements, loggerFactory)
        { }
    }

    public class ClockWidgetJaggyAnalogClockContentViewModel: ClockWidgetContentBaseViewModel
    {
        public ClockWidgetJaggyAnalogClockContentViewModel(ISkeletonImplements skeletonImplements, ILoggerFactory loggerFactory)
            : base(skeletonImplements, loggerFactory)
        { }
    }
}
