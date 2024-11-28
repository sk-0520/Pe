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
    public abstract class BindModelBase: NotifyPropertyBase, IDisposed
    {
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

        #region IDisposed

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed { get; protected set; }

        /// <summary>
        /// 既に破棄済みの場合は処理を中断する。
        /// </summary>
        /// <param name="_callerMemberName"></param>
        /// <exception cref="ObjectDisposedException">破棄済み。</exception>
        /// <seealso cref="IDisposed"/>
        protected void ThrowIfDisposed([CallerMemberName] string _callerMemberName = "")
        {
            if(IsDisposed) {
                throw new ObjectDisposedException(_callerMemberName);
            }
        }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>の内部処理。
        /// </summary>
        /// <remarks>
        /// <para>継承先クラスでは本メソッドを呼び出す必要がある。</para>
        /// </remarks>
        /// <param name="disposing">CLRの管理下か。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S2953:Methods named \"Dispose\" should implement \"IDisposable.Dispose\"", Justification = "OK")]
        protected virtual void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                IsDisposed = true;
            }
        }

        ~BindModelBase()
        {
            Dispose(disposing: false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3971:\"GC.SuppressFinalize\" should not be called", Justification = "OK")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S2953:Methods named \"Dispose\" should implement \"IDisposable.Dispose\"", Justification = "OK")]
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

}
