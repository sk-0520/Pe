using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Mvvm.ViewModels
{
    public class ValidateMessage
    {
        public ValidateMessage(string message)
        {
            Message = message;
        }

        #region property

        public string Message { get; }

        #endregion

        #region object

        public override string ToString() => Message;

        #endregion
    }
}
