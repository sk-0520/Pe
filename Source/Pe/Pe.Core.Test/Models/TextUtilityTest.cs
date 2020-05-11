using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class TextUtilityTest
    {
        [TestMethod]
        [DataRow("", "", "<", ">")]
        [DataRow("a", "a", "<", ">")]
        [DataRow("<a", "<a", "<", ">")]
        [DataRow("a>", "a>", "<", ">")]
        [DataRow("[a]", "<a>", "<", ">")]
        [DataRow("[a][b]", "<a><b>", "<", ">")]
        public void ReplacePlaceholderTest(string result, string src, string head, string tail)
        {
            var actual = TextUtility.ReplacePlaceholder(src, head, tail, s => "[" + s + "]");
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow("a", "a", "<", ">")]
        [DataRow("A", "<a>", "<", ">")]
        [DataRow("<aa>", "<aa>", "<", ">")]
        [DataRow("AB", "<a><b>", "<", ">")]
        [DataRow("<a<a>>B", "<a<a>><b>", "<", ">")]
        [DataRow("a", "a", "@[", "]")]
        [DataRow("A", "@[a]", "@[", "]")]
        [DataRow("@[aa]", "@[aa]", "@[", "]")]
        [DataRow("AB", "@[a]@[b]", "@[", "]")]
        [DataRow("@[a@[a]]B", "@[a@[a]]@[b]", "@[", "]")]
        public void ReplaceRangeFromDictionaryTest(string result, string src, string head, string tail)
        {
            var map = new Dictionary<string, string>() {
                ["A"] = "a",
                ["B"] = "b",
                ["C"] = "c",
                ["D"] = "d",
                ["E"] = "e",
                ["a"] = "A",
                ["b"] = "B",
                ["c"] = "C",
                ["d"] = "D",
                ["e"] = "E",
            };
            var actual = TextUtility.ReplacePlaceholderFromDictionary(src, head, tail, map);
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("a", "${A}")]
        public void ReplaceFromDictionaryTest(string result, string src)
        {
            var map = new Dictionary<string, string>() {
                ["A"] = "a",
                ["B"] = "b",
                ["C"] = "c",
                ["D"] = "d",
                ["E"] = "e",
                ["a"] = "A",
                ["b"] = "B",
                ["c"] = "C",
                ["d"] = "D",
                ["e"] = "E",
            };
            var actual = TextUtility.ReplaceFromDictionary(src, map);
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow(0, "")]
        [DataRow(1, "a")]
        [DataRow(1, "a\r\n")]
        [DataRow(2, "a\r\nb")]
        [DataRow(2, "a\rb")]
        [DataRow(2, "a\nb")]
        [DataRow(2, " a \r b ")]
        [DataRow(2, " a \n b ")]
        [DataRow(2, " a \r\n b ")]
        public void ReadLinesTest(int result, string s)
        {
            var actual = TextUtility.ReadLines(s).Count();
            Assert.AreEqual(result, actual, TextUtility.ReadLines(s).Count().ToString());
        }

#if false
        public void ReadLinesTest_Null()
        {
            Assert.ThrowsException<ArgumentException>(() => TextUtility.ReadLines(default(string)));
        }
#endif

        [TestMethod]
        [DataRow("a", "a", new[] { "" })]
        [DataRow("a", "a", new[] { "b" })]
        [DataRow("a(2)", "a", new[] { "a" })]
        [DataRow("A", "A", new[] { "A(2)" })]
        [DataRow("a(3)", "a", new[] { "a(5)", "a(2)", "a(4)", "a" })]
        public void ToUniqueDefaultTest(string result, string src, string[] list)
        {
            var actual = TextUtility.ToUniqueDefault(src, list, StringComparison.Ordinal);
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow(0, "")]
        [DataRow(0, default(string))]
        [DataRow(1, "1")]
        [DataRow(2, "22")]
        [DataRow(1, "あ")]
        [DataRow(1, "🐙")]
        public void TextWidthTest(int result, string text)
        {
            var actual = TextUtility.TextWidth(text);
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow(0, "")]
        [DataRow(1, "a")]
        [DataRow(1, "あ")]
        [DataRow(1, "亜")]
        [DataRow(2, "ab")]
        [DataRow(2, "あい")]
        [DataRow(2, "亜伊")]
        [DataRow(5, "ァｲu工ぉ")]
        [DataRow(1, "〇")]
        [DataRow(1, "⛄")]
        [DataRow(1, "🐎")]
        [DataRow(2, "🐎🐎")]
        public void GetCharactersText(int result, string text)
        {
            var actual = TextUtility.GetCharacters(text);
            Assert.AreEqual(result, actual.Count());
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("", null)]
        [DataRow("a", " a")]
        [DataRow("a", "a ")]
        [DataRow("a", " a ")]
        [DataRow("", "   ")]
        public void SafeTrimTest(string result, string text)
        {
            var actual = TextUtility.SafeTrim(text);
            Assert.AreEqual(result, actual, $"`{result}` - `{text}`");
        }
    }
}
