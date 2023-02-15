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
    /// ViewModel とかで使うやつのヘルパー。
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public interface INotifyPropertyHelper<TObject>: INotifyPropertyChanged
    {
        #region function

        public object? GetProperty(TObject instance, [CallerMemberName] string callerMemberName = "");
        public bool SetProperty<TValue>(TObject instance, [AllowNull] TValue value, [CallerMemberName] string callerMemberName = "");

        #endregion
    }
}
