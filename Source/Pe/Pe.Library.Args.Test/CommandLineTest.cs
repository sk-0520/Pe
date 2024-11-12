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

        [Fact]
        public void Add_Key_Test()
        {
            var commandLine = new CommandLine();
            var key = commandLine.Add(new CommandLineKey("aa", true, ""));
            Assert.Equal("aa", key.LongKey);
        }

        public static TheoryData<Type, string, CommandLineKey> Add_Key_Throw_Data => new() {
            {
                typeof(ArgumentNullException),
                "Value cannot be null. (Parameter 'key')",
                null!
            },
            {
                typeof(ArgumentException),
                "IsEnabledLongKey and LongKey.Length == 1 (Parameter 'key')",
                new CommandLineKey("a", true, "")
            }
        };

        [Theory]
        [MemberData(nameof(Add_Key_Throw_Data))]
        public void Add_Key_Throw_Test(Type expectedException, string expectedMessage, CommandLineKey input)
        {
            var commandLine = new CommandLine();
            var exception = Assert.Throws(expectedException, () => commandLine.Add(input));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Add_Params_Test()
        {
            var commandLine = new CommandLine();
            var key = commandLine.Add("aa");
            Assert.Equal("aa", key.LongKey);
        }

        [Theory]
        [InlineData(typeof(ArgumentNullException), default)]
        [InlineData(typeof(ArgumentException), "")]
        [InlineData(typeof(ArgumentException), " ")]
        public void Add_Params_Throw_Test(Type expectedException, string? longKey)
        {
            var commandLine = new CommandLine();
            Assert.Throws(expectedException, () => commandLine.Add(longKey!));
        }

        [Fact]
        public void AddTest_Exists_Test()
        {
            var commandLine = new CommandLine();
            commandLine.Add("aa");

            Assert.Throws<ArgumentException>(() => commandLine.Add("aa"));
            Assert.Throws<ArgumentException>(() => commandLine.Add(new CommandLineKey("aa", true, "")));
        }

        [Theory]
        [InlineData("A", new[] { "--aaa", "A" }, "aaa")]
        [InlineData("A A", new[] { "--aaa", "A A", "-a", "A" }, "aaa")]
        [InlineData("A", new[] { "--aaa=A" }, "aaa")]
        [InlineData("A A", new[] { "--aaa=A A", "-a=A" }, "aaa")]
        [InlineData("A", new[] { "--aaa=\"A\"" }, "aaa")]
        [InlineData("A A", new[] { "--aaa=\"A A\"", "-a=\"A\"" }, "aaa")]
        [InlineData("", new[] { "--aaa" }, "aaa")]
        [InlineData("", new[] { "--aaa=" }, "aaa")]
        public void Execute_Simple_Test(string expected, string[] args, string longKey)
        {
            var commandLine = new CommandLine(args, false);
            var commandKey = commandLine.Add(longKey, true);

            Assert.True(commandLine.Parse());
            var value = commandLine.Values[commandKey];
            Assert.True(value.First == expected);
        }

        [Theory]
        [InlineData(false, new[] { "/aaa" }, "aaa")]
        [InlineData(true, new[] { "--aaa" }, "aaa")]
        [InlineData(false, new[] { "--aaa" }, "AAA")]
        public void Execute_Switch_Test(bool expected, string[] args, string longKey)
        {
            var commandLine = new CommandLine(args, false);
            var commandKey = commandLine.Add(longKey, false);

            Assert.True(commandLine.Parse());
            var has = commandLine.Switches.Contains(commandKey);
            Assert.True(has == expected);
        }

        [Theory]
        [InlineData(new[] { "--aaa b" }, "aaa")]
        [InlineData(new[] { "--aaa=b" }, "aaa")]
        [InlineData(new[] { "--aaa" }, "aaa")]
        public void GetKey_Single_Test(string[] args, string longKey)
        {
            var commandLine = new CommandLine(args, false);
            var commandKey = commandLine.Add("aaa", true);

            Assert.True(commandLine.Parse());
            var actual = commandLine.GetKey(longKey);
            Assert.Equal(commandKey, actual);
        }

        [Theory]
        [InlineData(new[] { "--aaa=b" }, "aa")]
        [InlineData(new[] { "--aaa=b" }, "AAA")]
        public void GetKey_Throw_Test(string[] args, string longKey)
        {
            var commandLine = new CommandLine(args, false);
            var commandKey = commandLine.Add("aaa", true);

            Assert.True(commandLine.Parse());
            Assert.Throws<InvalidOperationException>(() => commandLine.GetKey(longKey));
        }
    }
}
