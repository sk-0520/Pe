using System.Collections.Generic;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Args.Test
{
    public class CommandLineParserTest
    {
        #region function

        [Fact]
        public void AddKeyTest()
        {
            var test = new CommandLineParser();
            Assert.Empty(test.Options);
            test.Add(new CommandLineOption("a", CommandLineOptionKind.Value, ""));
            test.Add(new CommandLineOption("b", CommandLineOptionKind.Value, ""));

            Assert.Equal(2, test.Options.Count);
            Assert.Contains("a", test.Options);
            Assert.Contains("b", test.Options);
        }

        [Fact]
        public void AddKey_Throw_InvalidKey_Test()
        {
            var test = new CommandLineParser();
            Assert.Throws<CommandLineInvalidKeyException>(() => test.Add(new CommandLineOption("", CommandLineOptionKind.Value, "")));
        }

        [Fact]
        public void AddKey_Throw_RequiredSwitch_Test()
        {
            var test = new CommandLineParser();
            Assert.Throws<CommandLineRequiredSwitchException>(() => test.Add(new CommandLineOption("switch", CommandLineOptionKind.Switch, "") { Required = true, }));
        }

        [Fact]
        public void AddKey_Throw_DuplicateKey_Test()
        {
            var test = new CommandLineParser();
            test.Add(new CommandLineOption("dup", CommandLineOptionKind.Value, ""));
            var exception = Assert.Throws<CommandLineDuplicateKeyException>(() => test.Add(new CommandLineOption("dup", CommandLineOptionKind.Value, "")));
            Assert.Equal("dup", exception.Key);
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
        public void Parse_Simple_Test(string expected, string[] args, string key)
        {
            var commandLine = new CommandLineParser();
            var commandKey = commandLine.Add(new CommandLineOption(key, CommandLineOptionKind.Value, string.Empty));

            var actual = commandLine.Parse(string.Empty, args);

            var value = actual.Values[commandKey.Key];
            Assert.True(value.First == expected);
        }

        [Theory]
        [InlineData(false, new[] { "/aaa" }, "aaa")]
        [InlineData(true, new[] { "--aaa" }, "aaa")]
        [InlineData(false, new[] { "--aaa" }, "AAA")]
        public void Parse_Switch_Test(bool expected, string[] args, string key)
        {
            var commandLine = new CommandLineParser();
            var commandKey = commandLine.Add(new CommandLineOption(key, CommandLineOptionKind.Switch, string.Empty));

            var actual = commandLine.Parse(string.Empty, args);

            var has = actual.Switches.Contains(commandKey.Key);
            Assert.True(has == expected);
        }

        [Theory]
        [InlineData("b", new[] { "--aaa", "b" }, "aaa", '=')]
        [InlineData("b", new[] { "--bbb=b" }, "bbb", '=')]
        [InlineData("b", new[] { "--bbb=\"b\"" }, "bbb", '=')]
        [InlineData("", new[] { "--ccc" }, "ccc", '=')]
        [InlineData("b", new[] { "--aaa", "b" }, "aaa", ':')]
        [InlineData("b", new[] { "--bbb:b" }, "bbb", ':')]
        [InlineData("b", new[] { "--bbb:\"b\"" }, "bbb", ':')]
        [InlineData("", new[] { "--ccc" }, "ccc", ':')]
        // セパレータがスペースはプログラム的にはこうなるが、引数の渡し方として現実的じゃないのでライブラリとしてはあまり考慮しない
        [InlineData("b", new[] { "--aaa", "b" }, "aaa", ' ')]
        [InlineData("b", new[] { "--bbb b" }, "bbb", ' ')]
        [InlineData("", new[] { "--ccc" }, "ccc", ' ')]
        public void Parse_SingleValues_Test(string expected, string[] args, string key, char separator)
        {
            var commandLine = new CommandLineParser(new CommandLineHelper() { Separator = separator });
            var commandKey = commandLine.Add(new CommandLineOption(key, CommandLineOptionKind.Value, string.Empty));

            var actual = commandLine.Parse(string.Empty, args);

            Assert.Contains(commandKey.Key, actual.Values);
            Assert.Contains(expected, actual.Values[commandKey.Key].Data);
        }

        [Fact]
        public void Parse_MultiValues_Test()
        {
            var commandLine = new CommandLineParser(new CommandLineHelper());
            var commandKey1 = commandLine.Add(new CommandLineOption("key1", CommandLineOptionKind.Value, string.Empty));
            var commandKey2 = commandLine.Add(new CommandLineOption("key2", CommandLineOptionKind.Value, string.Empty));
            var commandKey3 = commandLine.Add(new CommandLineOption("key3", CommandLineOptionKind.Value, string.Empty));

            var actual = commandLine.Parse(
                string.Empty,
                [
                    "--key1", "key1-value-1",
                    "--key1=key1-value-2",
                    "--key2", "key2-value-1",
                    "--key2", "key2 value 2",
                    "--key3", "key3-value-1",
                    "--key3=\"key3-value-2\"",
                ]
            );

            Assert.Equal(2, actual.Values[commandKey1.Key].Data.Count);
            Assert.Equal("key1-value-1", actual.Values[commandKey1.Key].Data[0]);
            Assert.Equal("key1-value-2", actual.Values[commandKey1.Key].Data[1]);

            Assert.Equal(2, actual.Values[commandKey2.Key].Data.Count);
            Assert.Equal("key2-value-1", actual.Values[commandKey2.Key].Data[0]);
            Assert.Equal("key2 value 2", actual.Values[commandKey2.Key].Data[1]);

            Assert.Equal(2, actual.Values[commandKey3.Key].Data.Count);
            Assert.Equal("key3-value-1", actual.Values[commandKey3.Key].Data[0]);
            Assert.Equal("key3-value-2", actual.Values[commandKey3.Key].Data[1]);
        }

        [Theory]
        [InlineData(new[] { "--aaa=b" }, "aa")]
        [InlineData(new[] { "--aaa=b" }, "AAA")]
        public void Parse_Throw_Test(string[] args, string key)
        {
            var commandLine = new CommandLineParser();
            var commandKey = commandLine.Add(new CommandLineOption(key, CommandLineOptionKind.Value, string.Empty));

            var actual = commandLine.Parse(string.Empty, args);
            Assert.NotNull(actual);

            Assert.Throws<KeyNotFoundException>(() => actual.Values[commandKey.Key]);
        }

        [Fact]
        public void Parse_Raw_Test()
        {
            var commandLine = new CommandLineParser();
            commandLine.Add(new CommandLineOption("abc", CommandLineOptionKind.Switch, string.Empty));
            commandLine.Add(new CommandLineOption("def", CommandLineOptionKind.Value, string.Empty));
            commandLine.Add(new CommandLineOption("ghi", CommandLineOptionKind.Switch, string.Empty));
            commandLine.Add(new CommandLineOption("jkl", CommandLineOptionKind.Value, string.Empty));

            var actual = commandLine.Parse(
                string.Empty,
                [
                    "--abc",
                    "raw1",
                    "--def=val",
                    "raw2",
                    "--ghi",
                    "--jkl", "val2",
                    "raw3",
                    "--",
                    "raw1",
                    "raw2",
                    "--raw3=raw4",
                    "--raw5",
                    "raw6",
                ]
            );

            Assert.Contains("abc", actual.Switches);
            Assert.Contains("raw1", actual.Unknowns[1]);
            Assert.Contains("val", actual.Values["def"].First);
            Assert.Contains("raw2", actual.Unknowns[3]);
            Assert.Contains("ghi", actual.Switches);
            Assert.Contains("val2", actual.Values["jkl"].First);
            Assert.Contains("raw3", actual.Unknowns[7]);
            Assert.Equal("raw1", actual.Raws[0]);
            Assert.Equal("raw2", actual.Raws[1]);
            Assert.Equal("--raw3=raw4", actual.Raws[2]);
            Assert.Equal("--raw5", actual.Raws[3]);
            Assert.Equal("raw6", actual.Raws[4]);
        }

        [Fact]
        public void Parse_Required_1_Abc_Test()
        {
            var commandLine = new CommandLineParser();
            commandLine.Add(new CommandLineOption("abc", CommandLineOptionKind.Value, string.Empty) { Required = true });
            commandLine.Add(new CommandLineOption("def", CommandLineOptionKind.Value, string.Empty));

            var exception = Record.Exception(() => commandLine.Parse(
                string.Empty,
                [
                    "--abc", "value",
                ]
            ));
            Assert.Null(exception);
        }

        [Fact]
        public void Parse_Required_1_Def_Test()
        {
            var commandLine = new CommandLineParser();
            commandLine.Add(new CommandLineOption("abc", CommandLineOptionKind.Value, string.Empty));
            commandLine.Add(new CommandLineOption("def", CommandLineOptionKind.Value, string.Empty) { Required = true });

            var exception = Record.Exception(() => commandLine.Parse(
                string.Empty,
                [
                    "--def", "value",
                ]
            ));
            Assert.Null(exception);
        }

        [Fact]
        public void Parse_Required_1_Abc_Throw_Test()
        {
            var commandLine = new CommandLineParser();
            commandLine.Add(new CommandLineOption("abc", CommandLineOptionKind.Value, string.Empty) { Required = true });
            commandLine.Add(new CommandLineOption("def", CommandLineOptionKind.Value, string.Empty));

            var exception = Assert.Throws<CommandLineRequiredException>(() => commandLine.Parse(
                string.Empty,
                [
                    "--def", "value",
                ]
            ));
            Assert.Contains("abc", exception.Keys);
        }

        [Fact]
        public void Parse_Required_1_Def_Throw_Test()
        {
            var commandLine = new CommandLineParser();
            commandLine.Add(new CommandLineOption("abc", CommandLineOptionKind.Value, string.Empty));
            commandLine.Add(new CommandLineOption("def", CommandLineOptionKind.Value, string.Empty) { Required = true });

            var exception = Assert.Throws<CommandLineRequiredException>(() => commandLine.Parse(
                string.Empty,
                [
                    "--abc", "value",
                ]
            ));
            Assert.Contains("def", exception.Keys);
        }

        [Fact]
        public void Parse_Required_2_Test()
        {
            var commandLine = new CommandLineParser();
            commandLine.Add(new CommandLineOption("abc", CommandLineOptionKind.Value, string.Empty) { Required = true });
            commandLine.Add(new CommandLineOption("def", CommandLineOptionKind.Value, string.Empty) { Required = true });

            var exception = Record.Exception(() => commandLine.Parse(
                string.Empty,
                [
                    "--abc=value",
                    "--def", "value",
                ]
            ));
            Assert.Null(exception);
        }

        [Fact]
        public void Parse_Required_2_Abc_Throw_Test()
        {
            var commandLine = new CommandLineParser();
            commandLine.Add(new CommandLineOption("abc", CommandLineOptionKind.Value, string.Empty) { Required = true });
            commandLine.Add(new CommandLineOption("def", CommandLineOptionKind.Value, string.Empty) { Required = true });

            var exception = Assert.Throws<CommandLineRequiredException>(() => commandLine.Parse(
                string.Empty,
                [
                    "--def", "value",
                ]
            ));
            Assert.Contains("abc", exception.Keys);
        }

        [Fact]
        public void Parse_Required_2_Def_Throw_Test()
        {
            var commandLine = new CommandLineParser();
            commandLine.Add(new CommandLineOption("abc", CommandLineOptionKind.Value, string.Empty) { Required = true });
            commandLine.Add(new CommandLineOption("def", CommandLineOptionKind.Value, string.Empty) { Required = true });

            var exception = Assert.Throws<CommandLineRequiredException>(() => commandLine.Parse(
                string.Empty,
                [
                    "--abc", "value",
                ]
            ));
            Assert.Contains("def", exception.Keys);
        }


        [Fact]
        public void Parse_Required_2_Abc_Def_Throw_Test()
        {
            var commandLine = new CommandLineParser();
            commandLine.Add(new CommandLineOption("abc", CommandLineOptionKind.Value, string.Empty) { Required = true });
            commandLine.Add(new CommandLineOption("def", CommandLineOptionKind.Value, string.Empty) { Required = true });

            var exception = Assert.Throws<CommandLineRequiredException>(() => commandLine.Parse(
                string.Empty,
                []
            ));
            Assert.Contains("abc", exception.Keys);
            Assert.Contains("def", exception.Keys);
        }

        #endregion
    }
}
