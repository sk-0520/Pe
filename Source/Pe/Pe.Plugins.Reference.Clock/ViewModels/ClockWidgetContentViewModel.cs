using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels
{
    public abstract class ClockWidgetContentBaseViewModel: ViewModelSkeleton
    {
        #region variable

        private DateTime _currentTime;
        private double _hourAngle;
        private double _minutesAngle;
        private double _secondsAngle;

        private double _hourOppositeAngle;
        private double _minutesOppositeAngle;
        private double _secondsOppositeAngle;

        private bool _isPm;

        #endregion

        protected ClockWidgetContentBaseViewModel(ISkeletonImplements skeletonImplements, IContextDispatcher contextDispatcher,ILoggerFactory loggerFactory)
            : base(skeletonImplements, contextDispatcher, loggerFactory)
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

        public double HourOppositeAngle
        {
            get => this._hourOppositeAngle;
            set => SetProperty(ref this._hourOppositeAngle, value);
        }
        public double MinutesOppositeAngle
        {
            get => this._minutesOppositeAngle;
            set => SetProperty(ref this._minutesOppositeAngle, value);
        }
        public double SecondsOppositeAngle
        {
            get => this._secondsOppositeAngle;
            set => SetProperty(ref this._secondsOppositeAngle, value);
        }

        public bool IsPm
        {
            get => this._isPm;
            set => SetProperty(ref this._isPm, value);
        }

        #endregion

        #region function

        public void SetTime(DateTime dateTime)
        {

            CurrentTime = dateTime;
            SecondsAngle = ClockUtility.GetSecondsAngle(CurrentTime);
            MinutesAngle = ClockUtility.GetMinuteAngle(CurrentTime);
            HourAngle = ClockUtility.GetHourAngle(CurrentTime);

            SecondsOppositeAngle = 180.0 + SecondsAngle;
            MinutesOppositeAngle = 180.0 + MinutesAngle;
            HourOppositeAngle = 180.0 + HourAngle;
            IsPm = 12 <= dateTime.Hour;
        }

        #endregion
    }

    public class ClockWidgetSimpleAnalogClockContentViewModel: ClockWidgetContentBaseViewModel
    {
        public ClockWidgetSimpleAnalogClockContentViewModel(ISkeletonImplements skeletonImplements, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(skeletonImplements, contextDispatcher, loggerFactory)
        {
            HourAngels = Enumerable.Range(0, 12).Select(i => i * 30.0).ToList();
            SecondsAngels = Enumerable.Range(0, 60).Select(i => i * 6.0).Except(HourAngels).ToList();
        }

        #region property

        public IReadOnlyList<double> HourAngels { get; }
        public IReadOnlyList<double> SecondsAngels { get; }

        #endregion
    }

    public class ClockWidgetJaggyAnalogClockContentViewModel: ClockWidgetContentBaseViewModel
    {
        public ClockWidgetJaggyAnalogClockContentViewModel(ISkeletonImplements skeletonImplements, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(skeletonImplements, contextDispatcher, loggerFactory)
        { }
    }
}
