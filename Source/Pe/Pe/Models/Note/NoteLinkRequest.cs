using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Note
{
    public class NoteLinkChangeRequestParameter : FileDialogRequestParameter
    {
        #region peoperty

        public bool IsOpen { get; set; }
        public Encoding? Encoding { get; set; }

        #endregion
    }
    public class NoteLinkChangeRequestResponse : FileDialogRequestResponse
    {
        #region peoperty

        public Encoding? Encoding { get; set; }

        #endregion
    }
}
