using System;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.About
{
    public class AboutComponentItemViewModel: ViewModelBase
    {
        public AboutComponentItemViewModel(AboutComponentItem item, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Item = item;
        }

        #region property

        private AboutComponentItem Item { get; }

        public AboutComponentKind Kind => Item.Kind;

        public string Name => Item.Data.Name;
        public bool HasUri => !string.IsNullOrWhiteSpace(Item.Data.Uri);
        public Uri? Uri => HasUri ? new Uri(Item.Data.Uri) : null;

        //public bool HasLicenseName => !string.IsNullOrWhiteSpace(License);
        public bool HasLicenseUri => !string.IsNullOrWhiteSpace(Item.Data.License.Uri);
        public string License => Item.Data.License.Name;
        public Uri? LicenseUri => HasLicenseUri ? new Uri(Item.Data.License.Uri) : null;

        public bool HasComment => !string.IsNullOrWhiteSpace(Comment);
        public string Comment => Item.Data.Comment;

        public int Sort => Item.Sort;

        #endregion

        #region command

        #endregion

        #region function

        #endregion
    }
}
