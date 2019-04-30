using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.Model.Note
{
    public class NoteContentConverter
    {
        public NoteContentConverter(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        ILogger Logger { get; }
        public Encoding Encoding { get; set; } = EncodingUtility.UTF8n;
        public string RichTextFormat { get; set; } = DataFormats.Rtf;

        #endregion

        public string ToRichText(string plainText, FontData fontData, Color foregroundColor)
        {
            var document = new FlowDocument();
            using(Initializer.BeginInitialize(document)) {
                var fontConverter = new FontConverter(Logger.Factory);
                document.FontFamily = fontConverter.MakeFontFamily(fontData.FamilyName, SystemFonts.MessageFontFamily);
                document.FontSize = fontData.Size;
                document.FontWeight = fontConverter.ToWeight(fontData.IsBold);
                document.FontStyle = fontConverter.ToStyle(fontData.IsItalic);
                document.Foreground = FreezableUtility.GetSafeFreeze(new SolidColorBrush(foregroundColor));

                document.Blocks.Add(new Paragraph(new Run(plainText)));
            }

            return ToRtfString(document);
        }

        public string ToRtfString(FlowDocument document)
        {
            var range = new TextRange(document.ContentStart, document.ContentEnd);
            using(var stream = new MemoryStream()) {
                range.Save(stream, RichTextFormat);
                var contentValue = Encoding.GetString(stream.ToArray());
                return contentValue;
            }
        }

        public FlowDocument ToFlowDocument(string content)
        {
            var document = new FlowDocument();
            using(Initializer.BeginInitialize(document)) {
                var range = new TextRange(document.ContentStart, document.ContentEnd);
                using(var stream = new MemoryStream(Encoding.GetBytes(content))) {
                    stream.Seek(0, SeekOrigin.Begin);
                    range.Load(stream, RichTextFormat);
                }
            }

            return document;
        }

        public string ToPlain(string rtfText)
        {
            using(var formRichTextBox = new System.Windows.Forms.RichTextBox() {
                Rtf = rtfText
            }) {
                return formRichTextBox.Text;
            }
        }

        public string ToLinkSettingString(NoteLinkContentData linkData)
        {
            var serializer = new XmlDataContractSerializer();
            using(var stream = new MemoryStream()) {
                serializer.Save(linkData, stream);
                return Encoding.GetString(stream.ToArray());
            }
        }
        public NoteLinkContentData ToLinkSetting(string rawSetting)
        {
            var serializer = new XmlDataContractSerializer();
            using(var stream = new MemoryStream()) {
                using(var writer = new StreamWriter(stream, Encoding)) {
                    writer.Write(rawSetting);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    return serializer.Load<NoteLinkContentData>(stream);
                }
            }
        }
    }
}
