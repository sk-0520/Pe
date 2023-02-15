using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// プロパティ変更通知(と取得)ヘルパー。
    /// <para>使用側で通知するには自分で <see cref="INotifyPropertyChanged.PropertyChanged"/> の <c>add, remove</c> に割り当てること。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface INotifyProperty<TObject>: INotifyPropertyChanged
    {
        #region function

        public object? GetProperty([CallerMemberName] string callerMemberName = "");

        public bool SetProperty<TValue>([AllowNull] TValue value, [CallerMemberName] string callerMemberName = "");

        #endregion
    }

    public interface INotifyPropertyFactory
    {
        #region function

        public INotifyProperty<TObject> Create<TObject>(TObject instance);

        #endregion
    }
}
