using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

}
