#define PROPERTY_CACHE

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Input;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;

namespace ContentTypeTextNet.Pe.Core.ViewModels
{
    /// <summary>
    /// 検証無視。
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class IgnoreValidationAttribute: Attribute
    {
        // This is a positional argument
        public IgnoreValidationAttribute()
        { }
    }

    /// <summary>
    /// ViewModel の基底。
    /// </summary>
    public abstract class ViewModelBase: BindableBase, INotifyDataErrorInfo, IDisposer
    {
        protected ViewModelBase(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
            ErrorsContainer = new ErrorsContainer<string>(OnErrorsChanged);
        }

        ~ViewModelBase()
        {
            Dispose(false);
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <summary>
        /// ロガー。
        /// </summary>
        protected ILogger Logger { get; }
        //IDictionary<string, ICommand> CommandCache { get; } = new Dictionary<string, ICommand>();
        /// <summary>
        /// コマンド一覧。
        /// </summary>
        protected IEnumerable<ICommand> Commands => CommandStore.Commands;
        CommandStore CommandStore { get; } = new CommandStore();

#if PROPERTY_CACHE
        /// <summary>
        /// プロパティアクセス処理キャッシュ。
        /// </summary>
        ConcurrentDictionary<object, PropertyCacher> PropertyCacher { get; } = new ConcurrentDictionary<object, PropertyCacher>();
#endif
        /// <summary>
        /// プロパティ変更時のイベントキャッシュ。
        /// </summary>
        ConcurrentDictionary<string, PropertyChangedEventArgs> PropertyChangedEventArgsCache { get; } = new ConcurrentDictionary<string, PropertyChangedEventArgs>();

        /// <summary>
        /// このVMは検証対象か。
        /// </summary>
        protected virtual bool SkipValidation { get; } = false;

        #endregion

        #region function

        /// <summary>
        /// オブジェクトのプロパティに値設定。
        /// </summary>
        /// <typeparam name="TValue">設定値のデータ。</typeparam>
        /// <param name="obj">対象オブジェクト。</param>
        /// <param name="value">設定値。</param>
        /// <param name="targetMemberName">設定対象となる<paramref name="obj"/>のメンバ名。未設定で呼び出しメンバ名。</param>
        /// <param name="notifyPropertyName">値設定後に通知するプロパティ名。未設定で呼び出しメンバ名。</param>
        /// <returns>設定されたか。同一値の場合は設定しない。</returns>
        protected virtual bool SetPropertyValue<TValue>(object obj, TValue value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "")
        {
#if DEBUG
            using var _a_ = ActionDisposerHelper.Create((d, sw) => Logger.LogTrace("PROP TIME: {0}", sw.Elapsed), Stopwatch.StartNew());
#endif
            ThrowIfDisposed();

#if PROPERTY_CACHE
            var propertyCacher = PropertyCacher.GetOrAdd(obj, o => new PropertyCacher(o));
            var nowValue = propertyCacher.Get(targetMemberName);
#else
            var type = obj.GetType();
            var propertyInfo = type.GetProperty(targetMemberName);
            Debug.Assert(propertyInfo != null);

            var nowValue = (TValue)propertyInfo.GetValue(obj);
#endif

            if(!IComparable<TValue>.Equals(nowValue, value)) {
#if PROPERTY_CACHE
                propertyCacher.Set(targetMemberName, value);
#else
                propertyInfo.SetValue(obj, value);
#endif
                var e = PropertyChangedEventArgsCache.GetOrAdd(notifyPropertyName, s => new PropertyChangedEventArgs(s));
                OnPropertyChanged(e);

                return true;
            }

            return false;
        }

        /// <summary>
        /// コマンド生成。
        /// <para>メモリ状態は知らんけどコマンドプロパティ記述位置でインスタンスを触りながら処理書きたいのよ。</para>
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="creator"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <returns></returns>
        protected TCommand GetOrCreateCommand<TCommand>(Func<TCommand> creator, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            where TCommand : ICommand
        {
            if(IsDisposed) {
                // なーんかワケわからんことになるので破棄後は null を返すようにしておく
                return default!;
            }

            //var sb = new StringBuilder();
            //sb.Append(GetType().FullName);
            //sb.Append(':');
            //sb.Append(callerMemberName);
            //sb.Append(':');
            //sb.Append(callerFilePath.GetHashCode());
            //sb.Append(':');
            //sb.Append(callerLineNumber);

            //var key = sb.ToString();

            //if(CommandCache.TryGetValue(key, out var cahceCommand)) {
            //    return (TCommand)cahceCommand;
            //}

            //var command = creator();
            //CommandCache.Add(key, command);

            //return command;
            return CommandStore.GetOrCreate(creator, callerMemberName, callerFilePath, callerLineNumber);
        }

        /// <summary>
        /// プロパティ検証。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected void ValidateProperty(object? value, [CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            var context = new ValidationContext(this) { MemberName = propertyName };
            var validationErrors = new List<ValidationResult>();
            if(!Validator.TryValidateProperty(value, context, validationErrors)) {
                var errors = validationErrors.Select(error => error.ErrorMessage ?? string.Empty);
                ErrorsContainer.SetErrors(propertyName, errors);
            } else {
                ErrorsContainer.ClearErrors(propertyName);
            }
        }

        private (IReadOnlyCollection<PropertyInfo> properties, IReadOnlyCollection<ViewModelBase> childViewModels) GetValidationItems()
        {
            ThrowIfDisposed();

            if(SkipValidation) {
                return (Array.Empty<PropertyInfo>(), Array.Empty<ViewModelBase>());
            }

            var type = GetType();
            //var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var properties = type.GetProperties()
                .Select(i => new { Property = i, Attribute = i.GetCustomAttribute<IgnoreValidationAttribute>() })
                .Where(i => i.Attribute == null)
                .Select(i => i.Property)
                .ToList()
            ;
            var targetProperties = properties
                .Select(i => new { Property = i, Attributes = i.GetCustomAttributes<ValidationAttribute>() })
                .Where(i => i.Attributes.Any())
                .Select(i => i.Property)
                .ToList()
            ;

            var childProperties = properties.Except(targetProperties);
            var childViewModels = new List<ViewModelBase>();
            foreach(var property in childProperties) {
                var rawValue = property.GetValue(this);
                switch(rawValue) {
                    case ViewModelBase viewModel:
                        childViewModels.Add(viewModel);
                        break;

                    case IEnumerable enumerable:
                        foreach(var element in enumerable.OfType<ViewModelBase>()) {
                            childViewModels.Add(element);
                        }
                        break;

                    default:
                        break;
                }
            }

            return (targetProperties, childViewModels);
        }

        /// <summary>
        /// 全プロパティ検証。
        /// </summary>
        private void ValidateAllProperty()
        {
            ThrowIfDisposed();

            var v = GetValidationItems();
            //var type = GetType();
            //var properties = type.GetProperties();
            //var targetProperties = properties
            //    .Select(i => new { Property = i, Attributes = i.GetCustomAttributes<ValidationAttribute>() })
            //    .Where(i => i.Attributes != null)
            //    .Select(i => i.Property)
            //    .ToList()
            //;
            foreach(var property in v.properties) {
                var rawValue = property.GetValue(this);
                ValidateProperty(rawValue!, property.Name);
            }

            //var childProperties = properties.Except(targetProperties);
            //foreach(var property in childProperties) {
            //    var rawValue = property.GetValue(this);
            //    switch(rawValue) {
            //        case ViewModelBase viewModel:
            //            viewModel.ValidateAllProperty();
            //            break;

            //        case IEnumerable enumerable:
            //            foreach(var element in enumerable.OfType<ViewModelBase>()) {
            //                element.ValidateAllProperty();
            //            }
            //            break;

            //        default:
            //            break;
            //    }
            //}
            foreach(var childViewModel in v.childViewModels) {
                childViewModel.ValidateAllProperty();
            }
        }

        /// <summary>
        /// ビジネスロジックの検証。
        /// </summary>
        protected virtual void ValidateDomain()
        {
            ThrowIfDisposed();
        }

        private void ValidateAllDomain()
        {
            ThrowIfDisposed();

            var v = GetValidationItems();
            ValidateDomain();
            foreach(var childViewModel in v.childViewModels) {
                childViewModel.ValidateAllDomain();
            }
        }

        bool HasChildrenErros()
        {
            ThrowIfDisposed();

            var v = GetValidationItems();
            var result = v.childViewModels.Any(i => i.HasErrors || i.HasChildrenErros());
            return result;
        }

        private void ClearAllErrors()
        {
            ThrowIfDisposed();

            ErrorsContainer.ClearErrors();
            var v = GetValidationItems();
            foreach(var property in v.properties) {
                ClearError(property.Name);
            }
            foreach(var viewModels in v.childViewModels) {
                viewModels.ClearAllErrors();
            }
        }

        public bool Validate()
        {
            ThrowIfDisposed();

            if(HasErrors || HasChildrenErros()) {
                return false;
            }

            ValidateAllProperty();

            if(HasErrors || HasChildrenErros()) {
                return false;
            }

            ClearAllErrors();
            ValidateAllDomain();

            return !HasErrors && !HasChildrenErros();
        }

        protected void ClearError([CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            ErrorsContainer.ClearErrors(propertyName);
        }

        protected void SetError(string errorMessage, [CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            ErrorsContainer.SetErrors(propertyName, new[] { errorMessage });
        }
        protected void SetErrors(IEnumerable<string> errorMessage, [CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            ErrorsContainer.SetErrors(propertyName, errorMessage);
        }
        protected void AddError(string message, [CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            var errors = ErrorsContainer.GetErrors(propertyName).ToList();
            if(!errors.Contains(message)) {
                errors.Add(message);
                ErrorsContainer.SetErrors(propertyName, errors);
            }
        }
        protected void AddErrors(IEnumerable<string> messages, [CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            var errors = ErrorsContainer.GetErrors(propertyName).ToList();
            foreach(var message in messages) {
                if(!errors.Contains(message)) {
                    errors.Add(message);
                    ErrorsContainer.SetErrors(propertyName, errors);
                }
            }
        }

        #endregion

        #region INotifyDataErrorInfo

        private ErrorsContainer<string> ErrorsContainer { get; }

        protected void OnErrorsChanged([CallerMemberName] string propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string? propertyName)
        {
            return ErrorsContainer.GetErrors(propertyName);
        }

        public bool HasErrors => ErrorsContainer.HasErrors;

        #endregion

        #region IDisposable

        protected void ThrowIfDisposed([CallerMemberName] string _callerMemberName = "")
        {
            if(IsDisposed) {
                throw new ObjectDisposedException(_callerMemberName);
            }
        }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// <para>呼び出し時点では<see cref="IsDisposed"/>は偽のまま。</para>
        /// </summary>
        [field: NonSerialized]
        public event EventHandler? Disposing;

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

#if PROPERTY_CACHE
            PropertyCacher.Clear();
#endif
            ErrorsContainer.ClearErrors();
            PropertyChangedEventArgsCache.Clear();

            if(disposing) {
                CommandStore.Dispose();
            }

            if(disposing) {
#pragma warning disable S3971 // "GC.SuppressFinalize" should not be called
                GC.SuppressFinalize(this);
#pragma warning restore S3971 // "GC.SuppressFinalize" should not be called
            }

            IsDisposed = true;
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }

    /// <summary>
    /// model と対になる ViewModel の基底クラス。
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class SingleModelViewModelBase<TModel>: ViewModelBase
        where TModel : INotifyPropertyChanged
    {
        protected SingleModelViewModelBase(TModel model, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Model = model;

            AttachModelEvents();
        }


        #region property

        /// <summary>
        /// 取り込んだモデル。
        /// <para><see cref="Dispose(bool)"/>後は null が入るので注意ね。</para>
        /// </summary>
        protected TModel Model { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// <see cref="Model"/>のプロパティに対して値設定。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="targetMemberName"></param>
        /// <param name="notifyPropertyName"></param>
        /// <returns></returns>
        protected bool SetModelValue<T>(T value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "")
        {
            return SetPropertyValue(Model, value, targetMemberName, notifyPropertyName);
        }

        /// <summary>
        /// モデルを取り込んだ際に一度だけ呼び出される処理。
        /// <para>継承クラスでは一番最初に呼び出すこと。</para>
        /// </summary>
        protected virtual void AttachModelEventsImpl()
        {
            ThrowIfDisposed();

        }

        protected void AttachModelEvents()
        {
            if(Model != null) {
                ThrowIfDisposed();

                AttachModelEventsImpl();
            }
        }

        /// <summary>
        /// モデルとサヨナラするとき(<see cref="Dispose(bool)"/>とか)するときに一度だけ呼び出される。
        /// <para>継承クラスでは一番最初に呼び出すこと。</para>
        /// </summary>
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
            Model = default(TModel)!;
        }

        #endregion
    }

    public class SimpleDataViewModel<TData>: ViewModelBase
    {
        #region variable

        TData _data;

        #endregion

        public SimpleDataViewModel(TData data, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this._data = data;
        }

        #region function

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

        public static SimpleDataViewModel<TData> Create<TData>(TData data, ILoggerFactory loggerFactory) => new SimpleDataViewModel<TData>(data, loggerFactory);

        #endregion
    }
}
