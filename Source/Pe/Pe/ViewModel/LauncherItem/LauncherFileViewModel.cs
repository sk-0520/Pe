using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Theme;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using System.IO;
using ContentTypeTextNet.Pe.Main.Model.Data;
using System.Windows.Input;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherItem
{
    public class LauncherFileViewModel : LauncherFileSystemViewModelBase
    {
        public LauncherFileViewModel(LauncherItemElement model, IDispatcherWapper dispatcherWapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, dispatcherWapper, launcherToolbarTheme, loggerFactory)
        { }

        #region property

        LauncherFileDetailData Detail { get; set; }



        #endregion

        #region command


        #endregion

        #region function
        #endregion

        #region LauncherItemViewModelBase

        protected override void RaiseFileSystemInfoChanged() => RaisePropertyChanged(nameof(FileInfo));

        protected override Task InitializeFileSystemAsync()
        {
            return Task.Run(() => {
                Detail = Model.LoadFileDetail();
                FileSystemInfo = Detail.FileSystemInfo;
                Exists = FileSystemInfo.Exists;
            });
        }


        protected override Task ExecuteMainImplAsync()
        {

            if(NowLoading) {
                Logger.Warning($"読み込み中のため抑制: {Model.LauncherItemId}, {Detail.FileSystemInfo}");
                return Task.CompletedTask;
            }

            if(!Detail.FileSystemInfo.Exists) {
                Logger.Warning($"存在しないファイル: {Model.LauncherItemId}, {Detail.FileSystemInfo}");
                return Task.CompletedTask;
            }

            Logger.Trace($"TODO: 起動 {Model.LauncherItemId}, {Detail.FileSystemInfo}");

            return Task.CompletedTask;
        }

        #endregion
    }
}
