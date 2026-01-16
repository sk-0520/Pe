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
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Library.Property;
using ContentTypeTextNet.Pe.Mvvm.Bindings;
using ContentTypeTextNet.Pe.Mvvm.Commands;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Mvvm.ViewModels
{
    public enum PropertyMode
    {
        Reflection,
        Cache,
    }

    /// <summary>
    /// ビューモデルの基底。
    /// </summary>
    public abstract class ViewModelBase: BindModelBase, INotifyDataErrorInfo
    {
        #region define

        public const PropertyMode DefaultPropertyMode = PropertyMode.Cache;

        #endregion

        protected ViewModelBase(PropertyMode propertyMode, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());

            ErrorsContainer = new ErrorsContainer<string>(OnErrorsChanged);

            if(propertyMode == PropertyMode.Cache) {
                CachedProperty = new ConcurrentDictionary<object, CachedProperty>();
            }

            var type = GetType();
            var properties = type.GetProperties();

            ObserveProperties = AttributeUtility.GetPropertiesWithAttribute<ObservePropertyAttribute>(properties)
                .ToHashSet()
            ;
            if(ObserveProperties.Any()) {
                PropertyChanged += ViewModelBase_PropertyChanged;
            }
        }

        protected ViewModelBase(ILoggerFactory loggerFactory)
            : this(DefaultPropertyMode, loggerFactory)
        { }

        #region property

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        protected ErrorsContainer<string> ErrorsContainer { get; }

        private IReadOnlyCollection<AttributeProperty<ObservePropertyAttribute>> ObserveProperties { get; }

        /// <summary>
        /// プロパティアクセス処理キャッシュ。
        /// </summary>
        private ConcurrentDictionary<object, CachedProperty>? CachedProperty { get; }
        /// <summary>
        /// プロパティ変更時のイベントキャッシュ。
        /// </summary>
        private ConcurrentDictionary<string, PropertyChangedEventArgs> PropertyChangedEventArgsCache { get; } = new ConcurrentDictionary<string, PropertyChangedEventArgs>();

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
        protected virtual bool SetProperty<TValue>(object obj, TValue value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "")
        {
#if DEBUG
            using var _ = ActionDisposerHelper.Create((d, sw) => Debug.WriteLine($"PROP TIME: {sw.Elapsed}, {targetMemberName} {notifyPropertyName}"), Stopwatch.StartNew());
#endif
            ThrowIfDisposed();

            PropertyInfo? propertyInfo = null;
            CachedProperty? cachedProperty = null;
            TValue? nowValue;
            Type targetType;

            if(CachedProperty is null) {
                var type = obj.GetType();
                propertyInfo = type.GetProperty(targetMemberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                Debug.Assert(propertyInfo != null);

                nowValue = (TValue?)propertyInfo.GetValue(obj);
                targetType = propertyInfo.PropertyType;
            } else {
                cachedProperty = CachedProperty.GetOrAdd(obj, o => new CachedProperty(o));

                nowValue = (TValue?)cachedProperty.Get(targetMemberName);
                targetType = nowValue is not null ? nowValue.GetType() : typeof(TValue);
            }

            if(!EqualityComparer<TValue>.Default.Equals(nowValue, value)) {
                if(CachedProperty is null) {
                    propertyInfo!.SetValue(obj, value);
                } else {
                    cachedProperty!.Set(targetMemberName, value);
                }

                ValidateProperty(obj, value, targetType, notifyPropertyName);

                var e = PropertyChangedEventArgsCache.GetOrAdd(notifyPropertyName, s => new PropertyChangedEventArgs(s));
                RaisePropertyChanged(e);

                return true;
            }

            return false;
        }

        protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged(nameof(HasErrors));
        }

        private void ValidateProperty<TObject, TValue>(TObject obj, TValue value, Type targetMemberType, string notifyPropertyName)
        {
            var context = new ValidationContext(this) {
                MemberName = notifyPropertyName
            };

            //TODO: キャッシュするほどじゃないけど、なんだかなぁ感
            var viewModelProperty = GetType().GetProperty(notifyPropertyName);
            Debug.Assert(viewModelProperty != null);
            var validationValue = viewModelProperty.PropertyType != targetMemberType
                ? viewModelProperty.GetValue(this)
                : value
            ;

            var validationErrors = new List<ValidationResult>();
            if(!Validator.TryValidateProperty(validationValue, context, validationErrors)) {
                foreach(var validationError in validationErrors) {
                    AddError(notifyPropertyName, validationError.ErrorMessage ?? string.Empty);
                }
            } else {
                RemoveError(notifyPropertyName);
            }
        }

        protected void AddError(string propertyName, string validateError)
        {
            ErrorsContainer.AddError(propertyName, validateError);
        }

        protected void RemoveError(string propertyName)
        {
            ErrorsContainer.ClearError(propertyName);
        }

        protected void RaiseCommandChanged(ICommand command)
        {
            if(command is CommandBase commandBase) {
                commandBase.RaiseCanExecuteChanged();
            } else {
                CommandManager.InvalidateRequerySuggested();
            }
        }

        #endregion

        #region BindModelBase

        protected override void Dispose(bool disposing)
        {
            PropertyChanged -= ViewModelBase_PropertyChanged;

            base.Dispose(disposing);
        }


        #endregion

        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => ErrorsContainer.HasErrors;

        public IEnumerable<string> GetErrors(string? propertyName)
        {
            return ErrorsContainer.GetError(propertyName ?? string.Empty);
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
        {
            return GetErrors(propertyName);
        }

        /// <summary>
        /// プロパティ検証。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected void ValidateProperty(object? value, [CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            var context = new ValidationContext(this) {
                MemberName = propertyName
            };
            var validationErrors = new List<ValidationResult>();
            if(!Validator.TryValidateProperty(value, context, validationErrors)) {
                var errors = validationErrors
                    .Select(error => error.ErrorMessage ?? string.Empty)
                ;
                ErrorsContainer.SetError(propertyName, errors);
            } else {
                ErrorsContainer.ClearError(propertyName);
            }
        }

        ///// <summary>
        ///// 子を含む全ての検証要素を取得。
        ///// </summary>
        ///// <returns></returns>
        //private (IReadOnlyCollection<PropertyInfo> properties, IReadOnlyCollection<ViewModelBase> childViewModels) GetValidationItems()
        //{
        //    ThrowIfDisposed();

        //    if(SkipValidation) {
        //        return (Array.Empty<PropertyInfo>(), Array.Empty<ViewModelBase>());
        //    }

        //    var type = GetType();
        //    var properties = type.GetProperties()
        //        .Select(i => (property: i, attribute: i.GetCustomAttribute<IgnoreValidationAttribute>()))
        //        .Where(i => i.attribute == null)
        //        .Select(i => i.property)
        //        .ToList()
        //    ;
        //    var targetProperties = properties
        //        .Select(i => (property: i, attributes: i.GetCustomAttributes<ValidationAttribute>()))
        //        .Where(i => i.attributes.Any())
        //        .Select(i => i.property)
        //        .ToList()
        //    ;

        //    var childProperties = properties.Except(targetProperties);
        //    var childViewModels = new List<ViewModelBase>();
        //    foreach(var property in childProperties) {
        //        var rawValue = property.GetValue(this);
        //        switch(rawValue) {
        //            case ViewModelBase viewModel:
        //                childViewModels.Add(viewModel);
        //                break;

        //            case IEnumerable enumerable:
        //                foreach(var element in enumerable.OfType<ViewModelBase>()) {
        //                    childViewModels.Add(element);
        //                }
        //                break;

        //            default:
        //                break;
        //        }
        //    }

        //    return (targetProperties, childViewModels);
        //}

        ///// <summary>
        ///// 子を含む全てのプロパティ検証。
        ///// </summary>
        //private void ValidateAllProperty()
        //{
        //    ThrowIfDisposed();

        //    var validationItems = GetValidationItems();

        //    foreach(var property in validationItems.properties) {
        //        var rawValue = property.GetValue(this);
        //        ValidateProperty(rawValue!, property.Name);
        //    }

        //    foreach(var childViewModel in validationItems.childViewModels) {
        //        childViewModel.ValidateAllProperty();
        //    }
        //}

        ///// <summary>
        ///// ビジネスロジックの検証。
        ///// <para>継承先でこいつを最初に呼び出すこと。</para>
        ///// </summary>
        //protected virtual void ValidateDomain()
        //{
        //    ThrowIfDisposed();
        //}

        ///// <summary>
        ///// 子を含む全てのビジネスロジックの検証。
        ///// </summary>
        //private void ValidateAllDomain()
        //{
        //    ThrowIfDisposed();

        //    var v = GetValidationItems();
        //    ValidateDomain();
        //    foreach(var childViewModel in v.childViewModels) {
        //        childViewModel.ValidateAllDomain();
        //    }
        //}

        //private bool HasChildrenErrors()
        //{
        //    ThrowIfDisposed();

        //    var v = GetValidationItems();
        //    var result = v.childViewModels.Any(i => i.HasErrors || i.HasChildrenErrors());
        //    return result;
        //}

        //private void ClearAllErrors()
        //{
        //    ThrowIfDisposed();

        //    ErrorsContainer.ClearErrors();
        //    var v = GetValidationItems();
        //    foreach(var property in v.properties) {
        //        ClearError(property.Name);
        //    }
        //    foreach(var viewModels in v.childViewModels) {
        //        viewModels.ClearAllErrors();
        //    }
        //}

        //public bool Validate()
        //{
        //    ThrowIfDisposed();

        //    if(HasErrors || HasChildrenErrors()) {
        //        return false;
        //    }

        //    ValidateAllProperty();

        //    if(HasErrors || HasChildrenErrors()) {
        //        return false;
        //    }

        //    ClearAllErrors();
        //    ValidateAllDomain();

        //    return !HasErrors && !HasChildrenErrors();
        //}

        //protected void ClearError([CallerMemberName] string propertyName = "")
        //{
        //    ThrowIfDisposed();

        //    ErrorsContainer.ClearErrors(propertyName);
        //}

        //protected void SetError(string errorMessage, [CallerMemberName] string propertyName = "")
        //{
        //    ThrowIfDisposed();

        //    ErrorsContainer.SetErrors(propertyName, new[] { errorMessage });
        //}
        //protected void SetErrors(IEnumerable<string> errorMessage, [CallerMemberName] string propertyName = "")
        //{
        //    ThrowIfDisposed();

        //    ErrorsContainer.SetErrors(propertyName, errorMessage);
        //}
        //protected void AddError(string message, [CallerMemberName] string propertyName = "")
        //{
        //    ThrowIfDisposed();

        //    var errors = ErrorsContainer.GetErrors(propertyName).ToList();
        //    if(!errors.Contains(message)) {
        //        errors.Add(message);
        //        ErrorsContainer.SetErrors(propertyName, errors);
        //    }
        //}
        //protected void AddErrors(IEnumerable<string> messages, [CallerMemberName] string propertyName = "")
        //{
        //    ThrowIfDisposed();

        //    var errors = ErrorsContainer.GetErrors(propertyName).ToList();
        //    foreach(var message in messages) {
        //        if(!errors.Contains(message)) {
        //            errors.Add(message);
        //            ErrorsContainer.SetErrors(propertyName, errors);
        //        }
        //    }
        //}

        #endregion

        private void ViewModelBase_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            var properties = new HashSet<string>();
            var commands = new HashSet<ICommand>();

            foreach(var props in ObserveProperties) {
                if(props.Attributes.Any(a => a.PropertyName == e.PropertyName)) {
                    if(typeof(ICommand).IsAssignableFrom(props.Property.PropertyType)) {
                        var command = props.Property.GetValue(this) as ICommand;
                        Debug.Assert(command is not null, "ICommand は判定済み");
                        commands.Add(command);
                    } else {
                        properties.Add(props.Property.Name);
                    }
                }
            }

            foreach(var property in properties) {
                RaisePropertyChanged(property);
            }
            foreach(var command in commands) {
                RaiseCommandChanged(command);
            }
        }
    }
}
