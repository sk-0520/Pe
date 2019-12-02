using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class KeyboardSettingEditorViewModel : SettingEditorViewModelBase<KeyboardSettingEditorElement>
    {
        #region variable

        bool _isPopupCreateJobMenu;

        #endregion

        public KeyboardSettingEditorViewModel(KeyboardSettingEditorElement model, ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> allLauncherItemCollection, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            ReplaceJobEditorCollection = new ActionModelViewModelObservableCollectionManager<KeyboardReplaceJobSettingEditorElement, KeyboardReplaceJobSettingEditorViewMode>(model.ReplaceJobEditors) {
                ToViewModel = m => new KeyboardReplaceJobSettingEditorViewMode(m, LoggerFactory),
            };
            ReplaceJobEditors = ReplaceJobEditorCollection.GetDefaultView();

            DisableJobEditorCollection = new ActionModelViewModelObservableCollectionManager<KeyboardDisableJobSettingEditorElement, KeyboardDisableJobSettingEditorViewModel>(Model.DisableJobEditors) {
                ToViewModel = m => new KeyboardDisableJobSettingEditorViewModel(m, LoggerFactory),
            };
            DisableJobEditors = DisableJobEditorCollection.GetDefaultView();

            AllLauncherItemCollection = allLauncherItemCollection;
            AllLauncherItems = AllLauncherItemCollection.CreateView();

            var replaceKeyItems = EnumUtility.GetMembers<Key>()
                .OrderBy(i => i)
            ;
            ReplaceKeyItems = new ObservableCollection<Key>(replaceKeyItems);

            var disableKeyItems = EnumUtility.GetMembers<Key>()
                .OrderBy(i => i)
            ;
            DisableKeyItems = new ObservableCollection<Key>(disableKeyItems);

            var pressedIgnoreKeys = new[] {
                Key.LeftShift,
                Key.RightShift,
                Key.LeftCtrl,
                Key.RightCtrl,
                Key.LeftAlt,
                Key.RightAlt,
                Key.RightAlt,
                Key.LWin,
                Key.RWin,
            };
            var pressedKeyItems = EnumUtility.GetMembers<Key>()
                .Where(i => !pressedIgnoreKeys.Any(ii => ii == i))
                .OrderBy(i => i)
            ;
            PressedKeyItems = new ObservableCollection<Key>(pressedKeyItems);

        }

        #region property

        ModelViewModelObservableCollectionManagerBase<KeyboardReplaceJobSettingEditorElement, KeyboardReplaceJobSettingEditorViewMode> ReplaceJobEditorCollection { get; }
        public ICollectionView ReplaceJobEditors { get; }

        ModelViewModelObservableCollectionManagerBase<KeyboardDisableJobSettingEditorElement, KeyboardDisableJobSettingEditorViewModel> DisableJobEditorCollection { get; }
        public ICollectionView DisableJobEditors { get; }


        [IgnoreValidation]
        ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }
        [IgnoreValidation]
        public ICollectionView AllLauncherItems { get; }
        public bool IsPopupCreateJobMenu
        {
            get => this._isPopupCreateJobMenu;
            set => SetProperty(ref this._isPopupCreateJobMenu, value);
        }

        [IgnoreValidation]
        public ObservableCollection<Key> ReplaceKeyItems { get; }
        public ObservableCollection<Key> DisableKeyItems { get; }
        public ObservableCollection<Key> PressedKeyItems { get; }

        #endregion

        #region command

        public ICommand AddReplaceJobCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 Model.AddReplaceJob();
             }
         ));

        public ICommand AddDisableJobCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 Model.AddDisableJob();
             }
         ));

        public ICommand CreateNewLauncherItemJobCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
            }
        ));

        #endregion

        #region function
        #endregion

        #region SettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Header_Keyboard;

        public override void Flush()
        {
        }

        #endregion
    }
}
