using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Prism.Mvvm;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel
{
    public abstract class ViewModelBase : BindableBase, INotifyDataErrorInfo, IDisposable, IDisposer
    {
        ViewModelBase()
        {
            ErrorsContainer = new ErrorsContainer<string>(OnErrorsChanged);
        }

        public ViewModelBase(ILogger logger)
            : this()
        {
            Logger = logger;
        }
        public ViewModelBase(ILoggerFactory loggerFactory)
            : this()
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        ~ViewModelBase()
        {
            Dispose(false);
        }

        #region property

        protected ILogger Logger { get; }
        IDictionary<string, ICommand> CommandCache { get; } = new Dictionary<string, ICommand>();
        protected IEnumerable<ICommand> Commands => CommandCache.Values;

        #endregion

        #region function

        protected virtual bool SetPropertyValue<TValue>(object obj, TValue value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "")
        {
            var type = obj.GetType();
            var propertyInfo = type.GetProperty(targetMemberName);

            var nowValue = (TValue)propertyInfo.GetValue(obj);

            if(!IComparable<TValue>.Equals(nowValue, value)) {
                propertyInfo.SetValue(obj, value);
                OnPropertyChanged(new PropertyChangedEventArgs(notifyPropertyName));

                return true;
            }

            return false;
        }

        protected TCommand GetOrCreateCommand<TCommand>(Func<TCommand> creator, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
            where TCommand : ICommand
        {
            var sb = new StringBuilder();
            sb.Append(GetType().FullName);
            sb.Append(':');
            sb.Append(callerMemberName);
            sb.Append(':');
            sb.Append(callerLineNumber);

            var key = sb.ToString();

            if(CommandCache.TryGetValue(key, out var cahceCommand)) {
                return (TCommand)cahceCommand;
            }

            var command = creator();
            CommandCache.Add(key, command);

            return command;
        }

        protected void ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            var context = new ValidationContext(this) { MemberName = propertyName };
            var validationErrors = new List<ValidationResult>();
            if(!Validator.TryValidateProperty(value, context, validationErrors)) {
                var errors = validationErrors.Select(error => error.ErrorMessage);
                ErrorsContainer.SetErrors(propertyName, errors);
            } else {
                ErrorsContainer.ClearErrors(propertyName);
            }
        }

        #endregion

        #region INotifyDataErrorInfo

        private ErrorsContainer<string> ErrorsContainer { get; }

        protected void OnErrorsChanged([CallerMemberName] string propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return ErrorsContainer.GetErrors(propertyName);
        }

        public bool HasErrors => ErrorsContainer.HasErrors;

        #endregion

        #region IDisposable

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// <para>呼び出し時点では<see cref="IsDisposed"/>は偽のまま。</para>
        /// </summary>
        [field: NonSerialized]
        public event EventHandler Disposing;

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>の内部処理。
        /// <para>継承先クラスでは本メソッドを呼び出す必要がある。</para>
        /// </summary>
        /// <param name="disposing">CLRの管理下か。</param>
        protected virtual void Dispose(bool disposing)
        {
            if(IsDisposed) {
                return;
            }

            if(Disposing != null) {
                Disposing(this, EventArgs.Empty);
            }

            if(disposing) {
                GC.SuppressFinalize(this);
            }

            IsDisposed = true;
        }

        /// <summary>
        /// 解放。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }

    public abstract class SingleModelViewModelBase<TModel> : ViewModelBase
        where TModel : INotifyPropertyChanged
    {
        public SingleModelViewModelBase(TModel model, ILogger logger)
            : base(logger)
        {
            Model = model;

            AttachModelEvents();
        }

        public SingleModelViewModelBase(TModel model, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Model = model;

            AttachModelEvents();
        }


        #region property

        protected TModel Model { get; private set; }

        #endregion

        #region function

        protected bool SetModelValue<T>(T value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "")
        {
            return SetPropertyValue(Model, value, targetMemberName, notifyPropertyName);
        }

        protected virtual void AttachModelEventsImpl()
        { }

        protected void AttachModelEvents()
        {
            if(Model != null) {
                AttachModelEventsImpl();
            }
        }

        protected virtual void DetachModelEventsImpl()
        { }

        protected void DetachModelEvents()
        {
            if(Model != null) {
                DetachModelEventsImpl();
            }
        }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                DetachModelEvents();
            }
            base.Dispose(disposing);
            Model = default(TModel);
        }

        #endregion
    }

    public class SimpleDataViewModel<TData> : ViewModelBase
    {
        #region variable

        TData _data;

        #endregion

        public SimpleDataViewModel(ILogger logger)
            : this(default(TData), logger)
        { }

        public SimpleDataViewModel(ILoggerFactory loggerFactory)
            : this(default(TData), loggerFactory)
        { }

        public SimpleDataViewModel(TData data, ILogger logger)
            : base(logger)
        {
            Data = data;
        }

        public SimpleDataViewModel(TData data, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Data = data;
        }

        #region property

        public TData Data
        {
            get => this._data;
            set => SetProperty(ref this._data, value);
        }

        #endregion
    }

    public static class SimpleDataViewModel
    {
        #region function

        public static SimpleDataViewModel<TData> Create<TData>(TData data, ILogger logger) => new SimpleDataViewModel<TData>(data, logger);
        public static SimpleDataViewModel<TData> Create<TData>(TData data, ILoggerFactory loggerFactory) => new SimpleDataViewModel<TData>(data, loggerFactory);

        #endregion
    }
}
