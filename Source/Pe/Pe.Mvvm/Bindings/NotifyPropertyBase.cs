using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common;

namespace ContentTypeTextNet.Pe.Mvvm.Bindings
{
    /// <summary>
    /// 通知モデル基底。
    /// </summary>
    public abstract class NotifyPropertyBase: INotifyPropertyChanged
    {
        #region event

        private event PropertyChangedEventHandler? StrongPropertyChanged;

        #endregion

        protected NotifyPropertyBase(EventReference propertyChangedEventReference)
        {
            PropertyChangedEventReference = propertyChangedEventReference;
            if(PropertyChangedEventReference == EventReference.Weak) {
                PropertyChangedWeakEvent = new PropertyChangedWeakEvent(nameof(PropertyChanged));
            }
        }

        #region property

        public EventReference PropertyChangedEventReference { get; }

        private PropertyChangedWeakEvent? PropertyChangedWeakEvent { get; }

        #endregion

        #region function

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if(PropertyChangedWeakEvent is null) {
                StrongPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            } else {
                PropertyChangedWeakEvent.Raise(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void RaisePropertyChanged(string notifyPropertyName)
        {
            OnPropertyChanged(notifyPropertyName);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged
        {
            add
            {
                if(PropertyChangedWeakEvent is null) {
                    StrongPropertyChanged += value;
                } else {
                    PropertyChangedWeakEvent.Add(value);
                }
            }
            remove
            {
                if(PropertyChangedWeakEvent is null) {
                    StrongPropertyChanged -= value;
                } else {
                    PropertyChangedWeakEvent.Remove(value);
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 通知モデル基底。
    /// </summary>
    public abstract class WeakNotifyPropertyBase: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged
        {
            add => PropertyChangedWeakEvent.Add(value);
            remove => PropertyChangedWeakEvent.Remove(value);
        }

        #endregion

        #region property

        private PropertyChangedWeakEvent PropertyChangedWeakEvent { get; } = new PropertyChangedWeakEvent(nameof(PropertyChanged));

        #endregion

        #region function

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedWeakEvent.Raise(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged(string notifyPropertyName)
        {
            OnPropertyChanged(notifyPropertyName);
        }

        #endregion
    }
}
