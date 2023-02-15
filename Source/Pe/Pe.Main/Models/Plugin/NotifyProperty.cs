using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    internal class NotifyProperty<TObject>: INotifyProperty<TObject>
        where TObject : class, INotifyPropertyChanged
    {
        public NotifyProperty(TObject instance)
        {
            Instance = new WeakReference<TObject>(instance);
        }

        #region property

        private WeakReference<TObject> Instance { get; }

        #endregion

        #region INotifyProperty

        public object? GetProperty([CallerMemberName] string callerMemberName = "")
        {
            throw new NotImplementedException();
        }

        public bool SetProperty<TValue>([AllowNull] TValue value, [CallerMemberName] string callerMemberName = "")
        {
            throw new NotImplementedException();
        }

        #endregion

        private void Instance_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
