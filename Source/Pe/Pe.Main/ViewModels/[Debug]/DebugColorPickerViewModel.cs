using System.Windows.Media;
using ContentTypeTextNet.Pe.Main.Models.Element._Debug_;
using Microsoft.Extensions.Logging;

#pragma warning disable CA1707 // 識別子はアンダースコアを含むことはできません
namespace ContentTypeTextNet.Pe.Main.ViewModels._Debug_
#pragma warning restore CA1707 // 識別子はアンダースコアを含むことはできません
{
    public class DebugColorPickerViewModel: DebugViewModelBase<DebugColorPickerElement>
    {
        #region variable

        Color _color = Colors.Red;

        #endregion

        public DebugColorPickerViewModel(DebugColorPickerElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public Color Color
        {
            get => this._color;
            set => SetProperty(ref this._color, value);
        }


        #endregion

        #region command
        #endregion

        #region function
        #endregion
    }
}
