using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeCommonViewModel: LauncherItemCustomizeDetailViewModelBase
    {
        #region variable

        #endregion

        public LauncherItemCustomizeCommonViewModel(LauncherItemCustomizeEditorElement model, IRequestSender iconSelectRequest, IRequestSender imageSelectRequest, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(model, contextDispatcher, loggerFactory)
        {
            IconSelectRequest = iconSelectRequest;
            ImageSelectRequest = imageSelectRequest;
        }

        #region property


        public IRequestSender IconSelectRequest { get; }
        public IRequestSender ImageSelectRequest { get; }


        [Required]
        public string Name
        {
            get => Model.Name;
            set
            {
                SetModelValue(value);
                ValidateProperty(value);
            }
        }

        public string? IconPath
        {
            get => IconData!.ToString();
        }

        public IconData? IconData
        {
            get => Model.IconData;
            private set
            {
                SetModelValue(value);
                RaisePropertyChanged(nameof(IconPath));
            }
        }

        public bool IsEnabledIconSetting => Model.Kind != LauncherItemKind.Separator;

        public bool IsEnabledCommandLauncher
        {
            get => Model.IsEnabledCommandLauncher;
            set => SetModelValue(value);
        }
        public bool IsEnabledBadgeSetting => Model.Kind != LauncherItemKind.Separator;

        public bool BadgeIsVisible
        {
            get => Model.Badge.IsVisible;
            set => SetPropertyValue(Model.Badge, value, nameof(Model.Badge.IsVisible));
        }
        public Color BadgeBackground
        {
            get => Model.Badge.Background;
            set => SetPropertyValue(Model.Badge, value, nameof(Model.Badge.Background));
        }

        public ObservableCollection<BadgeShape> BadgeShapeCollection { get; } = new ObservableCollection<BadgeShape>(Enum.GetValues<BadgeShape>());

        public BadgeShape BadgeShape
        {
            get => Model.Badge.BadgeShape;
            set => SetPropertyValue(Model.Badge, value, nameof(Model.Badge.BadgeShape));
        }
        public string BadgeDisplay
        {
            get => Model.Badge.Display;
            set => SetPropertyValue(Model.Badge, value, nameof(Model.Badge.Display));
        }


        public bool IsEnabledOtherSetting => Model.Kind != LauncherItemKind.Separator;

        #endregion

        #region command

        private ICommand? _IconFileSelectCommand;
        public ICommand IconFileSelectCommand => this._IconFileSelectCommand ??= new DelegateCommand(
            () => {
                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectIcon(
                    IconSelectRequest,
                    dialogRequester.ExpandPath(IconData?.Path),
                    IconData?.Index ?? 0,
                    r => {
                        IconData = new IconData() {
                            Path = r.FileName,
                            Index = r.IconIndex,
                        };
                    }
                );

            }
        );

        private ICommand? _IconImageSelectCommand;
        public ICommand IconImageSelectCommand => this._IconImageSelectCommand ??= new DelegateCommand(
            () => {
                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectFile(
                    ImageSelectRequest,
                    dialogRequester.ExpandPath(IconData?.Path),
                    true,
                    new[] {
                        new DialogFilterItem("image", string.Empty, IconImageLoaderBase.ImageFileExtensions.Select(i => "*." + i)),
                    },
                    r => {
                        IconData = new IconData() {
                            Path = r.ResponseFilePaths[0],
                            Index = 0,
                        };
                    }
                );
            }
        );

        private ICommand? _IconClearSelectCommand;
        public ICommand IconClearSelectCommand => this._IconClearSelectCommand ??= new DelegateCommand(
            () => {
                IconData = new IconData();
            }
        );

        #endregion

        #region function

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            //Name = Model.Name;
            //Code = Model.Code;
            //IconData = new IconData(Model.IconData);
            //IsEnabledCommandLauncher = Model.IsEnabledCommandLauncher;
        }

        #endregion
    }
}
