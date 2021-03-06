using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NoteIconMaker: ViewElementMakerBase
    {
        #region function

        public DependencyObject GetIconImage(IconBox iconBox, bool isCompact, bool isLocked, ColorPair<Color> baseColor)
        {
            var size = new Size((int)iconBox, isCompact ? (int)iconBox / 2 : (int)iconBox);
            var box = CreateBox(baseColor.Foreground, baseColor.Background, size);
            return box;
        }

        #endregion
    }
}
