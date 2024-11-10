using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Args.Test
{
    public class CommandLineTest
    {
        [Theory]
        [InlineData(new[] { "a" }, true, "a", 0)]
        [InlineData(new[] { "A", "a" }, true, "A", 1)]
        [InlineData(new[] { "A", "a" }, false, "", 2)]
        [InlineData(new string[] { }, true, "", 0)]
        [InlineData(new string[] { }, false, "", 0)]
        public void ConstructorTest(string[] args, bool withCommand, string expectedProgramName, int expectedArgumentCount)
        {
            var commandLine = new CommandLine(args, withCommand);
            Assert.True(commandLine.CommandName == expectedProgramName);
            Assert.True(commandLine.Arguments.Count == expectedArgumentCount);
        }

        //TODO: 例外と分けた方がいい
        [Theory]
        [InlineData(true, "aa")]
        [InlineData(false, "")]
        [InlineData(false, "a")]
        public void AddTest(bool expected, string longKey)
        {
            var commandLine = new CommandLine();
            try {
                var key = commandLine.Add(longKey);
                Assert.True(expected);
            } catch(ArgumentException ex) {
                Assert.False(expected, ex.ToString());
            }
        }
        //TOOD: AddTest の観点と同じ
        [Theory]
        [InlineData(false, "")]
        [InlineData(false, "aa")]
        [InlineData(false, "bb")]
        [InlineData(true, "aaa")]
        public void AddTest_Exists(bool expected, string longKey)
        {
            var commandLine = new CommandLine();
            commandLine.Add("aa");
            commandLine.Add("bb");

            try {
                var key = commandLine.Add(longKey);
                Assert.True(expected);
            } catch(ArgumentException ex) {
                Assert.False(expected, ex.ToString());
            }
        }

        [Theory]
        //[InlineData("A", new[] { "/a", "A" }, "aaa")]
        [InlineData("A", new[] { "/aaa", "A" }, "aaa")]
        [InlineData("AA", new[] { "/aaa", "AA", "/a", "A" }, "aaa")]
        [InlineData("A", new[] { "--aaa", "A" }, "aaa")]
        [InlineData("AA", new[] { "--aaa", "AA", "-a", "A" },"aaa")]
        //[InlineData("A", new[] { "/a=A" }, "aaa")]
        [InlineData("A", new[] { "/aaa=A" }, "aaa")]
        [InlineData("AA", new[] { "/aaa=AA", "/a=A" }, "aaa")]
        [InlineData("A", new[] { "--aaa=A" }, "aaa")]
        [InlineData("AA", new[] { "--aaa=AA", "-a=A" }, "aaa")]
        [InlineData("A", new[] { "/aaa=\"A\"" }, "aaa")]
        [InlineData("AA", new[] { "/aaa=\"AA\"", "/a=\"A\"" }, "aaa")]
        [InlineData("A", new[] { "--aaa=\"A\"" }, "aaa")]
        [InlineData("AA", new[] { "--aaa=\"AA\"", "-a=\"A\"" }, "aaa")]
        public void ExecuteTest_Simple(string expected, string[] args, string longKey)
        {
            var commandLine = new CommandLine(args, false);
            var commandKey = commandLine.Add(longKey, true);

            Assert.True(commandLine.Parse());
            var value = commandLine.Values[commandKey];
            Assert.True(value.First == expected);
        }

        [Theory]
        [InlineData(false, new[] { "/a" }, "aaa")]
        [InlineData(true, new[] { "/aaa" }, "aaa")]
        //[InlineData(true, new[] { "-a" }, 'a', "aaa")]
        [InlineData(true, new[] { "--aaa" }, "aaa")]
        [InlineData(false, new[] { "--aaa" }, "AAA")]
        public void ExecuteTest_Switch(bool expected, string[] args, string longKey)
        {
            var commandLine = new CommandLine(args, false);
            var commandKey = commandLine.Add(longKey, false);

            Assert.True(commandLine.Parse());
            var has = commandLine.Switches.Contains(commandKey);
            Assert.True(has == expected);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("a", " a")]
        [InlineData("a", "a ")]
        [InlineData("a", " a ")]
        [InlineData("\"a a\"", "a a")]
        [InlineData("a\"\"b", "a\"b")]
        [InlineData("a\"\"\"\"\"\"b", "a\"\"\"b")]
        [InlineData("\"a \"\"\"\"\"\" b\"", "a \"\"\" b")]
        public void Escape(string expected, string input)
        {
            var actual = CommandLine.Escape(input);
            Assert.Equal(expected, actual);
        }
    }
}
