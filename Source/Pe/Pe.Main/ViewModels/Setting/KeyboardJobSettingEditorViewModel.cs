using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public abstract class KeyboardJobSettingEditorViewModelBase<TJobEditor> : SingleModelViewModelBase<TJobEditor>
        where TJobEditor : KeyboardJobSettingEditorElementBase
    {
        public KeyboardJobSettingEditorViewModelBase(TJobEditor model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
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
    }

    public sealed class KeyboardReplaceJobSettingEditorViewMode : KeyboardJobSettingEditorViewModelBase<KeyboardReplaceJobSettingEditorElement>
    {
        public KeyboardReplaceJobSettingEditorViewMode(KeyboardReplaceJobSettingEditorElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public Key ReplaceKey
        {
            get
            {
                try {
                    var keyReplaceContentConverter = new KeyReplaceContentConverter();
                    return keyReplaceContentConverter.ToReplaceKey(Model.Content);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                    return Key.None;
                }
            }
            set
            {
                var keyReplaceContentConverter = new KeyReplaceContentConverter();
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
}
