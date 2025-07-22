using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Library.Common;

namespace ContentTypeTextNet.Pe.Mvvm.Bindings
{
    /// <summary>
    /// MVVM で使用するモデル基底。
    /// </summary>
    public abstract class BindModelBase: NotifyPropertyBase
    {
        protected BindModelBase(EventReference propertyChangedEventType)
            : base(propertyChangedEventType)
        { }

        protected BindModelBase()
            : this(EventReference.Weak)
        { }

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
            OnPropertyChanged(notifyPropertyName);

            return true;
        }
    }
}
