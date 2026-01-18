using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Library.Common.Linq;

namespace ContentTypeTextNet.Pe.Mvvm.ViewModels
{
    public class ErrorsContainer<T>
    {
        public ErrorsContainer(Action<string> raiseErrorsChanged)
        {
            RaiseErrorsChanged = raiseErrorsChanged;
        }

        #region property

        private Action<string> RaiseErrorsChanged { get; }
        private Dictionary<string, List<T>> ValidationErrors { get; } = new();

        public bool HasErrors => ValidationErrors.Count != 0;

        #endregion

        #region function

        public IReadOnlyDictionary<string, IReadOnlyList<T>> GetErrors()
        {
            return ValidationErrors.ToFrozenDictionary(vk => vk.Key, vk => vk.Value.FrozenSequence());
        }

        public IEnumerable<T> GetError(string propertyName)
        {
            if(ValidationErrors.TryGetValue(propertyName, out var value)) {
                return value;
            }

            return [];
        }

        public void ClearErrors()
        {
            var propertyNames = ValidationErrors.Keys.ToArray();
            foreach(string propertyName in propertyNames) {
                ClearError(propertyName);
            }
        }

        public void ClearError(string propertyName)
        {
            SetErrorCore(propertyName, []);
        }

        protected void SetErrorCore(string propertyName, IEnumerable<T> validationErrors)
        {
            if(ValidationErrors.TryGetValue(propertyName, out var currentValidationErrors)) {
                currentValidationErrors.SetRange(validationErrors);
            } else {
                ValidationErrors.Add(propertyName, validationErrors.ToList());
            }
            if(ValidationErrors[propertyName].Count == 0) {
                ValidationErrors.Remove(propertyName);
            }

            RaiseErrorsChanged(propertyName);
        }

        public void SetError(string propertyName, IEnumerable<T> validationErrors)
        {
            if(!validationErrors.Any()) {
                throw new ArgumentException("empty", nameof(validationErrors));
            }

            SetErrorCore(propertyName, validationErrors);
        }

        public void SetError(string propertyName, T validationError)
        {
            SetErrorCore(propertyName, [validationError]);
        }

        protected void AddErrorCore(string propertyName, IEnumerable<T> validationErrors)
        {
            if(ValidationErrors.TryGetValue(propertyName, out var currentValidationErrors)) {
                currentValidationErrors.AddRange(validationErrors);
            } else {
                ValidationErrors.Add(propertyName, validationErrors.ToList());
            }

            RaiseErrorsChanged(propertyName);
        }

        public void AddError(string propertyName, IEnumerable<T> validationErrors)
        {
            if(!validationErrors.Any()) {
                throw new ArgumentException("empty", nameof(validationErrors));
            }

            AddErrorCore(propertyName, validationErrors);
        }

        public void AddError(string propertyName, T validationError)
        {
            AddErrorCore(propertyName, [validationError]);
        }

        #endregion
    }
}
