using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Logic
{
    public class FontConverter
    {
        public FontConverter(ILogger logger)
        {
            Logger = logger;
        }
        public FontConverter(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region function

        /// <summary>
        /// 指定フォントファミリ名からFontFamily作成。
        /// </summary>
        /// <param name="fontFamily">フォントファミリ名</param>
        /// <param name="defaultFontFamily">指定名から生成できなかった場合に代替として使用されるFontFamily。</param>
        /// <returns></returns>
        public FontFamily MakeFontFamily(string fontFamily, FontFamily defaultFontFamily)
        {
            if(!string.IsNullOrWhiteSpace(fontFamily)) {
                if(Fonts.SystemFontFamilies.Any(f => f.FamilyNames.Any(n => n.Value == fontFamily))) {
                    var result = new FontFamily(fontFamily);
                    return result;
                }
            }

            return defaultFontFamily;
        }

        /// <summary>
        /// <para>TODO: この環境で再現できないのでスタブのみ作成</para>
        /// </summary>
        /// <param name="fontFamily"></param>
        /// <returns></returns>
        public string GetOriginalFontFamilyName(FontFamily fontFamily)
        {
            if(fontFamily == null) {
                throw new ArgumentNullException(nameof(fontFamily));
            }

            if(fontFamily.FamilyNames.Any()) {
                return fontFamily.FamilyNames.First().Value;
            }

            return fontFamily.Source;
        }

        public bool IsBold(FontWeight fontWeight)
        {
            return FontWeights.Normal.ToOpenTypeWeight() < fontWeight.ToOpenTypeWeight();
        }

        public bool IsItalic(FontStyle fontStyle)
        {
            return FontStyles.Normal != fontStyle;
        }

        public FontWeight ToWeight(bool bold)
        {
            return bold
                ? FontWeights.Bold
                : FontWeights.Normal
            ;
        }

        public FontStyle ToStyle(bool italic)
        {
            return italic
                ? FontStyles.Italic
                : FontStyles.Normal
            ;
        }


        #endregion
    }
}
