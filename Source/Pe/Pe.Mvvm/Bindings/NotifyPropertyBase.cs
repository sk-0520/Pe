using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            if(PropertyChangedWeakEvent is null) {
                StrongPropertyChanged?.Invoke(this, eventArgs);
            } else {
                PropertyChangedWeakEvent.Raise(this, eventArgs);
            }
        }

        protected void RaisePropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            OnPropertyChanged(eventArgs);
        }

        protected void RaisePropertyChanged(string notifyPropertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(notifyPropertyName));
        }

        /// <summary>
        /// プロパティ値変更処理。
        /// </summary>
        /// <remarks>
        /// <para>プロパティ対象となるフィールドを指定し、変更があれば変更通知を行う。</para>
        /// </remarks>
        /// <typeparam name="T">プロパティの型。</typeparam>
        /// <param name="variable">プロパティの実体となるフィールド。</param>
        /// <param name="value">設定する値。</param>
        /// <param name="notifyPropertyName">プロパティ名。</param>
        /// <returns>変更されたか。</returns>
        protected bool SetVariable<T>(ref T variable, T value, [CallerMemberName] string notifyPropertyName = "")
        {
            if(EqualityComparer<T>.Default.Equals(variable, value)) {
                return false;
            }

            variable = value;

            RaisePropertyChanged(notifyPropertyName);

            return true;
        }

        // 互換用処理
        // TODO: 廃止予定
        protected bool SetProperty<T>(ref T variable, T value, [CallerMemberName] string notifyPropertyName = "")
        {
            return SetVariable(ref variable, value, notifyPropertyName);
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
}
