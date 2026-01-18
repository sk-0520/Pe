using ContentTypeTextNet.Pe.CommonTest;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.CommandLine.Test
{
    public class CommandLineParserExtensionsTest
    {
        #region function

        [Fact]
        public void Add_4_Test()
        {
            var test = new CommandLineParser();
            test.Add("key", CommandLineOptionKind.Value, "description");

            var actual = test.Options["key"];
            Assert.Equal("key", actual.Key);
            Assert.Equal(CommandLineOptionKind.Value, actual.Kind);
            Assert.Equal("description", actual.Description);
        }

        [Fact]
        public void Add_4_Throw_Test()
        {
            var test = new CommandLineParser();
            Assert.Throws<CommandLineInvalidKeyException>(() => test.Add("", CommandLineOptionKind.Value, "description"));
        }


        [Fact]
        public void Add_3_Test()
        {
            var test = new CommandLineParser();
            test.Add("key", CommandLineOptionKind.Value);

            var actual = test.Options["key"];
            Assert.Equal("key", actual.Key);
            Assert.Equal(CommandLineOptionKind.Value, actual.Kind);
            Assert.Empty(actual.Description);
        }

        [Fact]
        public void Add_3_Throw_Test()
        {
            var test = new CommandLineParser();
            Assert.Throws<CommandLineInvalidKeyException>(() => test.Add("", CommandLineOptionKind.Value));
        }

        [Fact]
        public void ToUsage_Simple1_Test()
        {
            var test = new CommandLineParser();
            test.Add(new CommandLineOption("a", CommandLineOptionKind.Value, string.Empty));

            var actual = test.ToUsage();

            AssertEx.EqualMultiLineTextIgnoreNewline(
                """
                [--a=<value>]
                """,
                actual
            );
        }

        [Fact]
        public void ToUsage_Simple2_Test()
        {
            var test = new CommandLineParser();
            test.Add(new CommandLineOption("a", CommandLineOptionKind.Value, string.Empty) { ValueName = "A" });
            test.Add(new CommandLineOption("b", CommandLineOptionKind.Value, string.Empty) { Required = true });

            var actual = test.ToUsage();

            AssertEx.EqualMultiLineTextIgnoreNewline(
                """
                --b=<value>
                [--a=<A>]
                """,
                actual
            );
        }

        [Fact]
        public void ToUsage_Simple3_Test()
        {
            var test = new CommandLineParser();
            test.Add(new CommandLineOption("a", CommandLineOptionKind.Value, string.Empty) { ValueName = "A" });
            test.Add(new CommandLineOption("b", CommandLineOptionKind.Value, string.Empty) { Required = true });
            test.Add(new CommandLineOption("c", CommandLineOptionKind.Switch, string.Empty));

            var actual = test.ToUsage();

            AssertEx.EqualMultiLineTextIgnoreNewline(
                """
                --b=<value>
                [--a=<A>]
                [--c]
                """,
                actual
            );
        }

        [Fact]
        public void ToUsage_Description3_Test()
        {
            var test = new CommandLineParser();
            test.Add(new CommandLineOption("a", CommandLineOptionKind.Value, "AAA") { ValueName = "A" });
            test.Add(new CommandLineOption("b", CommandLineOptionKind.Value, string.Empty) { Required = true });
            test.Add(new CommandLineOption("c", CommandLineOptionKind.Switch, string.Empty));

            var actual = test.ToUsage();

            AssertEx.EqualMultiLineTextIgnoreNewline(
                $"""
                --b=<value>
                [--a=<A>]
                {test.Helper.DescriptionIndent}AAA
                [--c]
                """,
                actual
            );
        }

        #endregion
    }
}
