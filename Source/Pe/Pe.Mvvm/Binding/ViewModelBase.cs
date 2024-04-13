using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Mvvm.Command;

namespace ContentTypeTextNet.Pe.Mvvm.Binding
{
    /// <summary>
    /// ビューモデルの基底。
    /// </summary>
    public abstract class ViewModelBase: BindModelBase, INotifyDataErrorInfo
    {
        protected ViewModelBase()
        {
            var type = GetType();
            var properties = type.GetProperties();

            ObserveProperties = properties
                .Select(a => (property: a, attributes: a.GetCustomAttributes<ObservePropertyAttribute>().ToArray()))
                .Where(a => a.attributes.Any())
                .Select(a => new ObserveProperties(a.property, a.attributes))
                .ToHashSet()
            ;
            if(ObserveProperties.Any()) {
                PropertyChanged += ViewModelBase_PropertyChanged;
            }
        }

        #region property

        protected Dictionary<string, IList<ValidateMessage>> Errors { get; } = new Dictionary<string, IList<ValidateMessage>>();

        private IReadOnlyCollection<ObserveProperties> ObserveProperties { get; }

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
            OnPropertyChanged(notifyPropertyName);
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

        protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
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
                    AddError(notifyPropertyName, new ValidateMessage(validationError.ErrorMessage ?? string.Empty));
                }
            } else {
                RemoveError(notifyPropertyName);
            }
        }

        protected void AddError(string propertyName, ValidateMessage validateMessage)
        {
            if(!Errors.TryGetValue(propertyName, out var errorMessages)) {
                errorMessages = new List<ValidateMessage>();
                Errors.Add(propertyName, errorMessages);
            }
            errorMessages.Add(validateMessage);

            OnErrorsChanged(propertyName);
        }

        protected void RemoveError(string propertyName)
        {
            Errors.Remove(propertyName);
            OnErrorsChanged(propertyName);
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
            if(!IsDisposed) {
                PropertyChanged -= ViewModelBase_PropertyChanged;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => Errors.Any();

        public IEnumerable GetErrors(string? propertyName)
        {
            if(propertyName is not null && Errors.TryGetValue(propertyName, out var errors)) {
                return errors;
            }

            return Array.Empty<DataErrorsChangedEventArgs>();
        }

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
                OnPropertyChanged(property);
            }
            foreach(var command in commands) {
                RaiseCommandChanged(command);
            }
        }
    }
}
