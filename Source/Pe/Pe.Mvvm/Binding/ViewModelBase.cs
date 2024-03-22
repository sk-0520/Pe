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
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Mvvm.Command;
using Microsoft.Extensions.Logging;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Mvvm.Binding
{
    /// <summary>
    /// ビューモデルの基底。
    /// </summary>
    public abstract class ViewModelBase: BindModelBase, INotifyDataErrorInfo
    {
        protected ViewModelBase(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        protected Dictionary<string, IList<ValidateMessage>> Errors { get; } = new Dictionary<string, IList<ValidateMessage>>();

        private IDictionary<ICommand, ISet<string>> CommandHooks { get; } = new Dictionary<ICommand, ISet<string>>();

        #endregion

        #region function

        /// <summary>
        /// オブジェクトのプロパティに値設定。
        /// <para>変更があれば変更通知を行う。</para>
        /// </summary>
        /// <typeparam name="TObject">対象オブジェクトの型。</typeparam>
        /// <typeparam name="TValue">設定値の型。</typeparam>
        /// <param name="obj">対象オブジェクト。</param>
        /// <param name="value">設定値。</param>
        /// <param name="objectProperty"><paramref name="obj"/>のプロパティ。</param>
        /// <param name="notifyPropertyName">変更通知としてのプロパティ名。</param>
        /// <returns>変更されたか。</returns>
        private protected bool ChangePropertyValue<TObject, TValue>(TObject obj, TValue value, PropertyInfo objectProperty, string notifyPropertyName)
        {
            var propertyValue = (TValue)objectProperty.GetValue(obj);
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
        /// <para>変更があれば変更通知を行う。</para>
        /// </summary>
        /// <typeparam name="TObject">対象オブジェクトの型。</typeparam>
        /// <typeparam name="TValue">設定値の型。</typeparam>
        /// <param name="model">対象オブジェクト。</param>
        /// <param name="value">設定値。</param>
        /// <param name="propertyName">プロパティ名。</param>
        /// <param name="notifyPropertyName">変更通知のプロパティ名。基本的に呼び出し側のメンバ名となる想定。</param>
        /// <returns>変更されたか。</returns>
        protected bool SetProperty<TObject, TValue>(TObject model, TValue value, [CallerMemberName] string propertyName = "", [CallerMemberName] string notifyPropertyName = "")
        {
            var type = model.GetType();
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
            var validationValue = viewModelProperty.PropertyType != objectProperty.PropertyType
                ? viewModelProperty.GetValue(this)
                : value
            ;

            var validationErrors = new List<ValidationResult>();
            if(!Validator.TryValidateProperty(validationValue, context, validationErrors)) {
                foreach(var validationError in validationErrors) {
                    AddError(notifyPropertyName, new ValidateMessage(validationError.ErrorMessage));
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

        protected void AddCommandHook(ICommand command, IEnumerable<string> propertyNames)
        {
            ThrowIfDisposed();

            if(!propertyNames.Any()) {
                throw new ArgumentException(nameof(propertyNames));
            }
            if(CommandHooks.Count == 0) {
                PropertyChanged += ViewModelBase_PropertyChanged;
            }

            if(!CommandHooks.TryGetValue(command, out var props)) {
                props = new HashSet<string>();
                CommandHooks.Add(command, props);
            }
            foreach(var propertyName in propertyNames) {
                props.Add(propertyName);
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

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => Errors.Any();

        public IEnumerable GetErrors(string propertyName)
        {
            if(Errors.TryGetValue(propertyName, out var errors)) {
                return errors;
            }

            return Array.Empty<DataErrorsChangedEventArgs>();
        }

        #endregion

        private void ViewModelBase_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            var commands = new HashSet<ICommand>();

            foreach(var pair in CommandHooks) {
                if(pair.Value.Contains(e.PropertyName)) {
                    commands.Add(pair.Key);
                }
            }

            var needsCallInvalidateRequerySuggested = false;
            foreach(var command in commands) {
                if(command is CommandBase commandBase) {
                    commandBase.RaiseCanExecuteChanged();
                } else {
                    needsCallInvalidateRequerySuggested = true;
                }
            }
            if(needsCallInvalidateRequerySuggested) {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}
