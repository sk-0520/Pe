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
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public abstract class KeyboardJobSettingEditorViewModelBase<TJobEditor> : SettingItemViewModelBase<TJobEditor>, IKeyActionId
        where TJobEditor : KeyboardJobSettingEditorElementBase
    {
        public KeyboardJobSettingEditorViewModelBase(TJobEditor model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
        }

        #region property

        public string Comment
        {
            get => Model.Comment;
            set => SetModelValue(value);
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region IKeyActionId

        public Guid KeyActionId => Model.KeyActionId;

        #endregion
    }

    public sealed class KeyboardReplaceJobSettingEditorViewMode : KeyboardJobSettingEditorViewModelBase<KeyboardReplaceJobSettingEditorElement>
    {
        public KeyboardReplaceJobSettingEditorViewMode(KeyboardReplaceJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public Key ReplaceKey
        {
            get
            {
                try {
                    var replaceContentConverter = new ReplaceContentConverter();
                    return replaceContentConverter.ToReplaceKey(Model.Content);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                    return Key.None;
                }
            }
            set
            {
                var keyReplaceContentConverter = new ReplaceContentConverter();
                var content = keyReplaceContentConverter.ToContent(value);
                SetModelValue(content, nameof(Model.Content));
            }
        }

        public Key SourceKey
        {
            get
            {
                return Model.Mappings[0].Data.Key;
            }
            set
            {
                Model.Mappings[0].Data.Key = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

    }

    public sealed class KeyboardDisableJobSettingEditorViewModel : KeyboardJobSettingEditorViewModelBase<KeyboardDisableJobSettingEditorElement>
    {
        public KeyboardDisableJobSettingEditorViewModel(KeyboardDisableJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
        }

        #region property

        public Key Key
        {
            get
            {
                return Model.Mappings[0].Data.Key;
            }
            set
            {
                Model.Mappings[0].Data.Key = value;
                RaisePropertyChanged();
            }
        }

        public bool Forever
        {
            get
            {
                var doc = new DisableOptionConverter();
                if(doc.TryGetForever(Model.Options, out var result)) {
                    return result;
                }
                return false;
            }
            set
            {
                var doc = new DisableOptionConverter();
                doc.SetForever(Model.Options, value);
                RaisePropertyChanged();
            }
        }


        #endregion

        #region command
        #endregion

        #region function
        #endregion
    }

    public sealed class KeyMappingEditorViewModel : ViewModelBase
    {
        public KeyMappingEditorViewModel(KeyMappingData mapping, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Mapping = mapping;
        }

        #region property

        KeyMappingData Mapping { get; }

        public Key Key
        {
            get => Mapping.Key;
            set => SetPropertyValue(Mapping, value);
        }

        public ModifierKey Shift
        {
            get => Mapping.Shift;
            set => SetPropertyValue(Mapping, value);
        }

        public ModifierKey Control
        {
            get => Mapping.Control;
            set => SetPropertyValue(Mapping, value);
        }

        public ModifierKey Alt
        {
            get => Mapping.Alt;
            set => SetPropertyValue(Mapping, value);
        }

        public ModifierKey Super
        {
            get => Mapping.Super;
            set => SetPropertyValue(Mapping, value);
        }



        #endregion
    }

    public abstract class KeyboardPressedJobSettingEditorViewModelBase : KeyboardJobSettingEditorViewModelBase<KeyboardPressedJobSettingEditorElement>
    {
        public KeyboardPressedJobSettingEditorViewModelBase(KeyboardPressedJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            MappingCollection = new ActionModelViewModelObservableCollectionManager<WrapModel<KeyMappingData>, KeyMappingEditorViewModel>(Model.Mappings) {
                ToViewModel = m => new KeyMappingEditorViewModel(m.Data, LoggerFactory),
            };
            MappingItems = MappingCollection.GetDefaultView();
        }


        #region property

        public bool ThroughSystem
        {
            get
            {
                var poc = new PressedOptionConverter();
                if(poc.TryGetThroughSystem(Model.Options, out var result)) {
                    return result;
                }

                return false;
            }
            set
            {
                var poc = new PressedOptionConverter();
                poc.SetThroughSystem(Model.Options, value);
                RaisePropertyChanged();
            }
        }

        ModelViewModelObservableCollectionManagerBase<WrapModel<KeyMappingData>, KeyMappingEditorViewModel> MappingCollection { get; }
        public ICollectionView MappingItems { get; }

        #endregion

        #region command

        public ICommand AddMappingCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.AddMapping();
            }
        ));

        public ICommand RemoveMappingCommand => GetOrCreateCommand(() => new DelegateCommand<KeyMappingEditorViewModel>(
            o => {
                var index = MappingCollection.ViewModels.IndexOf(o);
                Model.RemoveMappingAt(index);
            },
            o => 1 < MappingCollection.ViewModels.Count
        ).ObservesProperty(() => MappingCollection.ViewModels.Count));

        public ICommand UpMappingCommand => GetOrCreateCommand(() => new DelegateCommand<KeyMappingEditorViewModel>(
             o => {
                 var index = MappingCollection.ViewModels.IndexOf(o);
                 if(index == 0) {
                     return;
                 }
                 var next = index - 1;
                 Model.MoveMapping(index, next);
             }
        ));
        public ICommand DownMappingCommand => GetOrCreateCommand(() => new DelegateCommand<KeyMappingEditorViewModel>(
             o => {
                 var index = MappingCollection.ViewModels.IndexOf(o);
                 if(index == MappingCollection.ViewModels.Count - 1) {
                     return;
                 }
                 var next = index + 1;
                 Model.MoveMapping(index, next);
             }
        ));

        #endregion
    }

    public abstract class KeyboardPressedJobSettingEditorViewModelBase<TContent> : KeyboardPressedJobSettingEditorViewModelBase
    {
        public KeyboardPressedJobSettingEditorViewModelBase(KeyboardPressedJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public abstract TContent Content { get; set; }

        #endregion
    }

    public sealed class KeyboardCommandJobSettingEditorViewModel : KeyboardPressedJobSettingEditorViewModelBase
    {
        public KeyboardCommandJobSettingEditorViewModel(KeyboardPressedJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }
    }

    public sealed class KeyboardLauncherItemJobSettingEditorViewModel : KeyboardPressedJobSettingEditorViewModelBase<KeyActionContentLauncherItem>
    {
        public KeyboardLauncherItemJobSettingEditorViewModel(KeyboardPressedJobSettingEditorElement model, ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> allLauncherItemCollection, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            AllLauncherItemCollection = allLauncherItemCollection;
            AllLauncherItems = AllLauncherItemCollection.CreateView();
        }

        #region property

        [IgnoreValidation]
        ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }
        [IgnoreValidation]
        public ICollectionView AllLauncherItems { get; }

        public LauncherItemSettingEditorViewModel? LauncherItem
        {
            get
            {
                var lioc = new LauncherItemOptionConverter();
                if(lioc.TryGetLauncherItemId(Model.Options, out var launcherItemId)) {
                    return AllLauncherItemCollection.ViewModels.FirstOrDefault(i => i.LauncherItemId == launcherItemId);
                }
                return null;
            }
            set
            {
                var lioc = new LauncherItemOptionConverter();
                if(value != null) {
                    lioc.WriteLauncherItemId(Model.Options, value.LauncherItemId);
                } else {
                    lioc.WriteLauncherItemId(Model.Options, Guid.Empty);
                }
            }
        }

        #endregion

        #region KeyboardPressedJobSettingEditorViewModelBase

        public override KeyActionContentLauncherItem Content
        {
            get
            {
                var launcherItemContentConverter = new LauncherItemContentConverter();
                try {
                    return launcherItemContentConverter.ToKeyActionContentLauncherItem(Model.Content);
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                    // 泥臭い
                    Model.Content = launcherItemContentConverter.ToContent(KeyActionContentLauncherItem.Execute);
                }
                return KeyActionContentLauncherItem.Execute;
            }
            set
            {
                var launcherItemContentConverter = new LauncherItemContentConverter();
                var content = launcherItemContentConverter.ToContent(value);
                SetModelValue(content);
            }
        }

        #endregion

    }

    public sealed class KeyboardLauncherToolbarJobSettingEditorViewModel : KeyboardPressedJobSettingEditorViewModelBase<KeyActionContentLauncherToolbar>
    {
        public KeyboardLauncherToolbarJobSettingEditorViewModel(KeyboardPressedJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            // 将来的に何か増えてもOKにしたい思いとこのタイミング以外で何もできないのでしゃあなし
            Content = KeyActionContentLauncherToolbar.AutoHiddenToHide;
        }

        #region KeyboardPressedJobSettingEditorViewModelBase

        public override KeyActionContentLauncherToolbar Content
        {
            get
            {
                var launcherToolbarContentConverter = new LauncherToolbarContentConverter();
                try {
                    return launcherToolbarContentConverter.ToKeyActionContentLauncherToolbar(Model.Content);
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                }
                return KeyActionContentLauncherToolbar.AutoHiddenToHide;
            }
            set
            {
                var launcherToolbarContentConverter = new LauncherToolbarContentConverter();
                var content = launcherToolbarContentConverter.ToContent(value);
                SetModelValue(content);
            }
        }

        #endregion

    }

    public sealed class KeyboardNoteJobSettingEditorViewModel : KeyboardPressedJobSettingEditorViewModelBase<KeyActionContentNote>
    {
        public KeyboardNoteJobSettingEditorViewModel(KeyboardPressedJobSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            var items = EnumUtility.GetMembers<KeyActionContentNote>();
            ContentItems = new List<KeyActionContentNote>(items);
        }

        #region property

        public IReadOnlyList<KeyActionContentNote> ContentItems { get; }

        #endregion

        #region KeyboardPressedJobSettingEditorViewModelBase

        public override KeyActionContentNote Content
        {
            get
            {
                var noteContentConverter = new NoteContentConverter();
                try {
                    return noteContentConverter.ToKeyActionContentNote(Model.Content);
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                    // 泥臭い
                    Model.Content = noteContentConverter.ToContent(KeyActionContentNote.Create);
                }
                return KeyActionContentNote.Create;
            }
            set
            {
                var noteContentConverter = new NoteContentConverter();
                var content = noteContentConverter.ToContent(value);
                SetModelValue(content);
            }
        }

        #endregion

    }

}
