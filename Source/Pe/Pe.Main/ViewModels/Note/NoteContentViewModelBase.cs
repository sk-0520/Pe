using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public abstract class NoteContentViewModelBase: SingleModelViewModelBase<NoteContentElement>
    {
        #region variable

        bool _canVisible;

        #endregion

        protected NoteContentViewModelBase(NoteContentElement model, NoteConfiguration noteConfiguration, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            ClipboardManager = clipboardManager;
            DispatcherWrapper = dispatcherWrapper;
            NoteConfiguration = noteConfiguration;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(IsLink), nameof(IsLink));
        }

        #region property

        public NoteContentKind Kind => Model.ContentKind;
        protected NoteConfiguration NoteConfiguration { get; }
        protected IClipboardManager ClipboardManager { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }
        public bool CanVisible
        {
            get => this._canVisible;
            private set => SetProperty(ref this._canVisible, value);
        }

        private FrameworkElement? BaseElement { get; set; }

        public bool IsLink => Model.IsLink;

        PropertyChangedHooker PropertyChangedHooker { get; }

        protected bool EnabledUpdate { get; private set; } = true;

        #endregion

        #region command

        public ICommand LoadedCommand => GetOrCreateCommand(() => new DelegateCommand<FrameworkElement>(
            async o => {
                if(CanVisible) {
                    return;
                }

                AttachControlCore(o);

                try {
                    Logger.LogDebug("??????????????????");
                    await LoadContentAsync(o);
                    Logger.LogDebug("??????????????????");
                    CanVisible = true;
                } catch(Exception ex) {
                    Logger.LogError(ex, "??????????????????");
                    throw; // ??????????????????????????????
                }
            }
        ));

        public ICommand CopyCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var data = GetClipbordContentData();
                ClipboardManager.CopyData(data, ClipboardNotify.None);
            }
        ));

        #endregion

        #region function

        private void AttachControlCore(FrameworkElement o)
        {
            BaseElement = o;
            BaseElement.Unloaded += Control_Unloaded;
        }

        private void DetachControlCore()
        {
            if(BaseElement != null) {
                BaseElement.Unloaded -= Control_Unloaded;
            }
        }

        /// <summary>
        /// ???????????????????????????????????????????????????????????????
        /// <para>UI????????????????????????????????????????????????</para>
        /// </summary>
        /// <param name="baseElement"></param>
        /// <returns></returns>
        protected abstract Task LoadContentAsync(FrameworkElement baseElement);
        /// <summary>
        /// ???????????????????????????????????????????????????????????????
        /// <para>UI????????????????????????????????????????????????</para>
        /// </summary>
        protected abstract void UnloadContent();

        /// <summary>
        /// ?????????????????????????????????????????????
        /// </summary>
        /// <returns></returns>
        protected abstract IDataObject GetClipbordContentData();

        #endregion

        #region SingleModelViewModelBase

        protected override void AttachModelEventsImpl()
        {
            Model.LinkContentChanged += Model_LinkContentChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            Model.LinkContentChanged -= Model_LinkContentChanged;
        }

        #endregion

        private void Control_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            UnloadContent();
            DetachControlCore();
            CanVisible = false;
        }

        private void Model_LinkContentChanged(object? sender, EventArgs e)
        {
            if(BaseElement == null) {
                Logger.LogTrace("????????????????????????????????????????????????");
                return;
            }

            Logger.LogDebug("??????????????????????????????");
            EnabledUpdate = false;
            UnloadContent();
            LoadContentAsync(BaseElement).ContinueWith(t => {
                EnabledUpdate = true;
            }).ConfigureAwait(false);
        }


    }

    public static class NoteContentViewModelFactory
    {
        #region function

        public static NoteContentViewModelBase Create(NoteContentElement model, NoteConfiguration noteConfiguration, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            switch(model.ContentKind) {
                case NoteContentKind.Plain:
                    return new NotePlainContentViewModel(model, noteConfiguration, clipboardManager, dispatcherWrapper, loggerFactory);

                case NoteContentKind.RichText:
                    return new NoteRichTextContentViewModel(model, noteConfiguration, clipboardManager, dispatcherWrapper, loggerFactory);

                //case NoteContentKind.Link:
                //    return new NoteLinkContentViewModel(model, clipboardManager, dispatcherWapper, loggerFactory);

                default:
                    throw new NotImplementedException();
            }

        }


        #endregion
    }

}
