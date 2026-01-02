using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test
{
    public class TextUtilityTest
    {
        [Theory]
        [InlineData("", "", "<", ">")]
        [InlineData("aa", "aa", "", ">")]
        [InlineData("aa", "aa", "<", "")]
        [InlineData("a", "a", "<", ">")]
        [InlineData("<a", "<a", "<", ">")]
        [InlineData("a>", "a>", "<", ">")]
        [InlineData("[a]", "<a>", "<", ">")]
        [InlineData("<>", "<>", "<", ">")] // 何もない場合は何もしない
        [InlineData("[<a]>", "<<a>>", "<", ">")] // 近しい範囲
        [InlineData("[a][b]", "<a><b>", "<", ">")]
        public void ReplacePlaceholderTest(string expected, string src, string head, string tail)
        {
            var actual = TextUtility.ReplacePlaceholder(src, head, tail, (s, w) => w(string.Concat("[", s, "]")));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReplacePlaceholder_Throw_Test()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => TextUtility.ReplacePlaceholder("s", "{", "}", null!));
            Assert.Equal("writer", exception.ParamName);
        }

        [Theory]
        [InlineData("a", "a", "<", ">")]
        [InlineData("A", "<a>", "<", ">")]
        [InlineData("<aa>", "<aa>", "<", ">")]
        [InlineData("AB", "<a><b>", "<", ">")]
        [InlineData("<a<a>>B", "<a<a>><b>", "<", ">")]
        [InlineData("a", "a", "@[", "]")]
        [InlineData("A", "@[a]", "@[", "]")]
        [InlineData("@[aa]", "@[aa]", "@[", "]")]
        [InlineData("AB", "@[a]@[b]", "@[", "]")]
        [InlineData("@[a@[a]]B", "@[a@[a]]@[b]", "@[", "]")]
        public void ReplaceRangeFromDictionaryTest(string expected, string src, string head, string tail)
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
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "${A}")]
        public void ReplaceFromDictionaryTest(string expected, string src)
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
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new string[] { }, "")]
        [InlineData(new[] { "a" }, "a")]
        [InlineData(new[] { "a" }, "a\r\n")]
        [InlineData(new[] { "a", "b" }, "a\r\nb")]
        [InlineData(new[] { "a", "b" }, "a\rb")]
        [InlineData(new[] { "a", "b" }, "a\nb")]
        [InlineData(new[] { " a ", " b " }, " a \r b ")]
        [InlineData(new[] { " a ", " b " }, " a \n b ")]
        [InlineData(new[] { " a ", " b " }, " a \r\n b ")]
        public void ReadLines_String_Test(string[] expected, string s)
        {
            int i = 0;
            foreach(var line in TextUtility.ReadLines(s)) {
                Assert.Equal(expected[i++], line);
            }
            Assert.Equal(expected.Length, i);
        }

        [Theory]
        [InlineData(new string[] { }, "")]
        [InlineData(new[] { "a" }, "a")]
        [InlineData(new[] { "a" }, "a\r\n")]
        [InlineData(new[] { "a", "b" }, "a\r\nb")]
        [InlineData(new[] { "a", "b" }, "a\rb")]
        [InlineData(new[] { "a", "b" }, "a\nb")]
        [InlineData(new[] { " a ", " b " }, " a \r b ")]
        [InlineData(new[] { " a ", " b " }, " a \n b ")]
        [InlineData(new[] { " a ", " b " }, " a \r\n b ")]
        public void ReadLines_Reader_Test(string[] expected, string s)
        {
            using var reader = new StringReader(s);
            int i = 0;
            foreach(var line in TextUtility.ReadLines(reader)) {
                Assert.Equal(expected[i++], line);
            }
            Assert.Equal(expected.Length, i);
        }

        [Fact]
        public void ReadLines_Span_None_Test()
        {
            var span = "".AsSpan();
            var test = TextUtility.ReadLines(span);
            var enumerator = test.GetEnumerator();
            Assert.Equal("", enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal("", enumerator.Current);
        }

        [Fact]
        public void ReadLines_Span_1Line_Test()
        {
            var span = "abc".AsSpan();
            var test = TextUtility.ReadLines(span);
            var enumerator = test.GetEnumerator();
            Assert.True(enumerator.MoveNext());
            Assert.Equal("abc", enumerator.Current);
            Assert.False(enumerator.MoveNext());
        }

        [Theory]
        [InlineData(new string[] { }, "")]
        [InlineData(new string[] { "abc" }, "abc")]
        [InlineData(new[] { "line1", "line2" }, "line1\r\nline2")]
        [InlineData(new[] { "line1", "line2" }, "line1\rline2")]
        [InlineData(new[] { "line1", "line2" }, "line1\nline2")]
        [InlineData(new[] { "line1" }, "line1\r\n")]
        [InlineData(new[] { "line1" }, "line1\r")]
        [InlineData(new[] { "line1" }, "line1\n")]
        [InlineData(new[] { "line1", "line2", "line3" }, "line1\r\nline2\r\nline3")]
        [InlineData(new[] { "line1", "line2", "line3" }, "line1\rline2\r\nline3")]
        [InlineData(new[] { "line1", "line2", "line3" }, "line1\nline2\r\nline3")]
        [InlineData(new[] { "line1", "line2", "line3" }, "line1\r\nline2\rline3")]
        [InlineData(new[] { "line1", "line2", "line3" }, "line1\r\nline2\nline3")]
        [InlineData(new[] { "line1", "", "line3" }, "line1\r\n\r\nline3")]
        [InlineData(new[] { "line1", "", "line3" }, "line1\r\n\rline3")]
        [InlineData(new[] { "line1", "", "line3" }, "line1\r\n\nline3")]
        [InlineData(new[] { "line1", "", "line3" }, "line1\r\r\nline3")]
        [InlineData(new[] { "line1", "", "line3" }, "line1\n\r\nline3")]
        [InlineData(new[] { "line1", "", "line3" }, "line1\n\nline3")]
        public void ReadLines_Span_Lines_Test(string[] expected, string input)
        {
            var span = input.AsSpan();
            int i = 0;
            foreach(var line in TextUtility.ReadLines(span)) {
                Assert.Equal(expected[i++], line);
            }
            Assert.Equal(expected.Length, i);
        }

        [Fact]
        public void ReadLines_String_Null_Test()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => TextUtility.ReadLines(default(string)!).ToArray());
            Assert.Equal("s", exception.ParamName);
        }

        [Fact]
        public void ReadLines_Reader_Null_Test()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => TextUtility.ReadLines(default(StringReader)!).ToArray());
            Assert.Equal("reader", exception.ParamName);
        }

        [Fact]
        public void ToUnique_Throw_Source_Test()
        {
            var actual = () => TextUtility.ToUnique(null!, [], StringComparison.Ordinal, (s, i) => s + i);
            var exception = Assert.Throws<ArgumentNullException>(actual);
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void ToUnique_Throw_Sequence_Test()
        {
            var actual = () => TextUtility.ToUnique("", null!, StringComparison.Ordinal, (s, i) => s + i);
            var exception = Assert.Throws<ArgumentNullException>(actual);
            Assert.Equal("sequence", exception.ParamName);
        }

        [Fact]
        public void ToUnique_Throw_Converter_Test()
        {
            var actual = () => TextUtility.ToUnique("", [], StringComparison.Ordinal, null!);
            var exception = Assert.Throws<ArgumentNullException>(actual);
            Assert.Equal("converter", exception.ParamName);
        }

        [Theory]
        [InlineData("a", "a", new[] { "" })]
        [InlineData("a", "a", new[] { "b" })]
        [InlineData("a(2)", "a", new[] { "a" })]
        [InlineData("A", "A", new[] { "A(2)" })]
        [InlineData("a(3)", "a", new[] { "a(5)", "a(2)", "a(4)", "a" })]
        public void ToUniqueDefaultTest(string expected, string src, string[] list)
        {
            var actual = TextUtility.ToUniqueDefault(src, list, StringComparison.Ordinal);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(0, default(string))]
        [InlineData(1, "1")]
        [InlineData(2, "22")]
        [InlineData(1, "あ")]
        [InlineData(1, "🐙")]
        public void GetWidthTest(int expected, string? text)
        {
            var actual = TextUtility.GetWidth(text!);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "a")]
        [InlineData(1, "あ")]
        [InlineData(1, "亜")]
        [InlineData(2, "ab")]
        [InlineData(2, "あい")]
        [InlineData(2, "亜伊")]
        [InlineData(5, "ァｲu工ぉ")]
        [InlineData(1, "〇")]
        [InlineData(1, "⛄")]
        [InlineData(1, "🐎")]
        [InlineData(2, "🐎🐎")]
        public void GetCharactersText(int expected, string text)
        {
            var actual = TextUtility.GetCharacters(text);
            Assert.Equal(expected, actual.Count());
        }

        [Theory]
        [InlineData("", "", new[] { 'a', 'b', 'c' })]
        [InlineData("def", "def", new[] { 'a', 'b', 'c' })]
        [InlineData("", "abc", new[] { 'a', 'b', 'c' })]
        [InlineData("def", "abcdef", new[] { 'a', 'b', 'c' })]
        [InlineData("def", "abcdefabc", new[] { 'a', 'b', 'c' })]
        [InlineData("aいbえ-aいbえ", "あaいbうcえdお-あaいbうcえdお", new[] { 'あ', 'う', 'お', 'c', 'd' })]
        public void RemoveCharactersTest(string expected, string input, char[] cs)
        {
            var actual = TextUtility.RemoveCharacters(input, cs.ToHashSet());
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("def", "def")]
        [InlineData("ABCdef", "ABCdef")]
        [InlineData("ABCdefABC", "ABCdefABC")]
        [InlineData("ABCdef", "abcdef")]
        [InlineData("ABCdefABC", "abcdefabc")]
        public void ReplaceCharactersTest(string expected, string input)
        {
            var map = new Dictionary<char, char>() {
                ['a'] = 'A',
                ['b'] = 'B',
                ['c'] = 'C',
            };
            var actual = TextUtility.ReplaceCharacters(input, map);
            Assert.Equal(expected, actual);
        }
    }

}
