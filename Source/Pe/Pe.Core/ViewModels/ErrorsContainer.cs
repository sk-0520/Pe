using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Core.ViewModels
{
    public class ErrorsContainer
    {
        public ErrorsContainer(Action<string> onErrorsChanged)
        {
            OnErrorsChanged = onErrorsChanged;
        }

        #region property

        protected Action<string> OnErrorsChanged { get; private set; }
        protected IDictionary<string, List<string>> Errors { get; } = new Dictionary<string, List<string>>();

        public bool HasErrors
        {
            get
            {
                return 0 < Errors.Count;
            }
        }
        #endregion

        #region function



        public void ClearErrors()
        {
            Errors.Clear();
        }

        public void ClearErrors(string propertyName)
        {
            Errors.Remove(propertyName);
        }

        public void SetErrors(string propertyName, IEnumerable<string> errors)
        {
            Errors[propertyName] = errors.ToList();
        }

        public IReadOnlyList<string> GetErrors(string propertyName)
        {
            if(Errors.TryGetValue(propertyName, out var result)) {
                return result;
            }

            return Array.Empty<string>();
        }

        #endregion
    }
}
