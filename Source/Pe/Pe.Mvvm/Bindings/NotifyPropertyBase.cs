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
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region property

        #endregion

        #region function

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged(string notifyPropertyName)
        {
            OnPropertyChanged(notifyPropertyName);
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
