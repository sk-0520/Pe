using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Mvvm.Bindings;
using ContentTypeTextNet.Pe.Mvvm.Commands;

namespace ContentTypeTextNet.Pe.Mvvm.ViewModels
{
    /// <summary>
    /// ビューモデルの基底。
    /// </summary>
    public abstract class ViewModelBase: BindModelBase, INotifyDataErrorInfo
    {
        protected ViewModelBase()
        {
            ErrorsContainer = new ErrorsContainer<string>(OnErrorsChanged);

            var type = GetType();
            var properties = type.GetProperties();

            ObserveProperties = AttributeUtility.GetPropertiesWithAttribute<ObservePropertyAttribute>(properties)
                .ToHashSet()
            ;
            if(ObserveProperties.Any()) {
                PropertyChanged += ViewModelBase_PropertyChanged;
            }
        }
        ~ViewModelBase()
        {
            Dispose(disposing: false);
        }

        #region property

        protected ErrorsContainer<string> ErrorsContainer { get; }

        private IReadOnlyCollection<AttributeProperty<ObservePropertyAttribute>> ObserveProperties { get; }

        #endregion

        #region function

        /// <summary>
        /// オブジェクトのプロパティに値設定。
        /// </summary>
        /// <remarks>
        /// <para>変更があれば変更通知を行う。</para>
        /// </remarks>
        /// <typeparam name="TObject">対象オブジェクトの型。</typeparam>
        /// <typeparam name="TValue">設定値の型。</typeparam>
        /// <param name="obj">対象オブジェクト。</param>
        /// <param name="value">設定値。</param>
        /// <param name="objectProperty"><paramref name="obj"/>のプロパティ。</param>
        /// <param name="notifyPropertyName">変更通知としてのプロパティ名。</param>
        /// <returns>変更されたか。</returns>
        private protected bool ChangePropertyValue<TObject, TValue>(TObject obj, TValue value, PropertyInfo objectProperty, string notifyPropertyName)
        {
#pragma warning disable CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
            var propertyValue = (TValue)objectProperty.GetValue(obj);
#pragma warning restore CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
            if(EqualityComparer<TValue>.Default.Equals(propertyValue, value)) {
                return false;
            }

            objectProperty.SetValue(obj, value);
            RaisePropertyChanged(notifyPropertyName);
            ValidateProperty(obj, value, objectProperty, notifyPropertyName);

            return true;
        }

        /// <summary>
        /// オブジェクトのプロパティに値設定。
        /// </summary>
        /// <remarks>
        /// <para>変更があれば変更通知を行う。</para>
        /// </remarks>
        /// <typeparam name="TObject">対象オブジェクトの型。</typeparam>
        /// <typeparam name="TValue">設定値の型。</typeparam>
        /// <param name="model">対象オブジェクト。</param>
        /// <param name="value">設定値。</param>
        /// <param name="propertyName">プロパティ名。</param>
        /// <param name="notifyPropertyName">変更通知のプロパティ名。基本的に呼び出し側のメンバ名となる想定。</param>
        /// <returns>変更されたか。</returns>
        protected bool SetProperty<TObject, TValue>(TObject model, TValue value, [CallerMemberName] string propertyName = "", [CallerMemberName] string notifyPropertyName = "")
        {
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
            var type = model.GetType();
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。
            var prop = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // 動的な割当ては考慮していないのでデバッグ時にここで死ぬ場合は何か間違ってる。
            Debug.Assert(prop != null);

            return ChangePropertyValue(model, value, prop, notifyPropertyName);
        }

        // 互換用
        protected bool SetPropertyValue<TObject, TValue>(TObject model, TValue value, [CallerMemberName] string propertyName = "", [CallerMemberName] string notifyPropertyName = "")
        {
            return SetProperty(model, value, propertyName, notifyPropertyName);
        }

        protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged(nameof(HasErrors));
        }

        private void ValidateProperty<TObject, TValue>(TObject obj, TValue value, PropertyInfo objectProperty, string notifyPropertyName)
        {
            var context = new ValidationContext(this) {
                MemberName = notifyPropertyName
            };

            //TODO: キャッシュするほどじゃないけど、なんだかなぁ感
            var viewModelProperty = GetType().GetProperty(notifyPropertyName);
            Debug.Assert(viewModelProperty != null);
            var validationValue = viewModelProperty.PropertyType != objectProperty.PropertyType
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

        #region IDisposed

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
                        var command = (ICommand?)props.Property.GetValue(this);
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                        commands.Add(command);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                    } else {
                        properties.Add(nameof(props.Property.Name));
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
