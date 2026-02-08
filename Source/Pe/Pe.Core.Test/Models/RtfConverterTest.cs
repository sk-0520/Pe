using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Core.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class RtfConverterTest
    {
        #region function

        [WinFormsTheory]
        [InlineData("", "")]
        [InlineData("", """{\rtf1}""")]
        [InlineData("2", """{\rtf 2}""")]
        // https://ja.wikipedia.org/wiki/Rich_Text_Format#%E3%82%B3%E3%83%BC%E3%83%89%E4%BE%8B
        [InlineData(
            """
            Hello!
            This is some bold text.
            """,
            """
            {\rtf
            Hello!\par
            This is some {\b bold} text.\par
            }
            """
        )]
        public void ToStringTest(string expected, string rtf)
        {
            var rtfConverter = new RtfConverter();

            var actual = rtfConverter.ToString(rtf);

            AssertEx.EqualMultiLineTextIgnoreNewline(expected, actual);
        }

        #endregion
    }
}
