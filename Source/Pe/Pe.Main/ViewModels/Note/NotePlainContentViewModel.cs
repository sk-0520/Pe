using System;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NotePlainContentViewModel: NoteContentViewModelBase
    {
        #region variable

        private string _content = string.Empty;

        #endregion

        public NotePlainContentViewModel(NoteContentElement model, NoteConfiguration noteConfiguration, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, noteConfiguration, clipboardManager, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public string Content
        {
            get => this._content;
            set
            {
                if(SetProperty(ref this._content, value)) {
                    if(CanVisible && EnabledUpdate) {
                        Model.ChangePlainContent(Content);
                    }
                }
            }
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region NoteContentViewModelBase

        protected override Task LoadContentAsync(FrameworkElement baseElement)
        {
            return Task.Run(() => {
                try {
                    var content = Model.LoadPlainContent();
                    Content = content;
                } catch(Exception ex) {
                    Content = ex.Message;
                }
            });
        }

        protected override void UnloadContent()
        { }

        protected override IDataObject GetClipbordContentData()
        {
            var data = new DataObject();
            if(CanVisible) {
                data.SetText(Content, TextDataFormat.UnicodeText);
            } else {
                var value = Model.LoadPlainContent();
                data.SetText(value, TextDataFormat.UnicodeText);
            }

            return data;
        }

        #endregion
    }
}
