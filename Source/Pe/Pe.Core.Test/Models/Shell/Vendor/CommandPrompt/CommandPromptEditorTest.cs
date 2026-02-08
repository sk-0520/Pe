using System;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Core.Models.Shell;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt
{
    public class CommandPromptEditorTest
    {
        #region function

        [Fact]
        public void CreateEmptyLineTest()
        {
            var editor = new CommandPromptEditor();
            var actual = editor.CreateEmptyLine();
            Assert.IsAssignableFrom<EmptyLine>(actual);
            Assert.Empty(editor.Commands);
        }

        [Fact]
        public void AddEmptyLineTest()
        {
            var editor = new CommandPromptEditor();
            var actual = editor.AddEmptyLine();
            Assert.IsAssignableFrom<EmptyLine>(actual);
            Assert.Single(editor.Commands);
            Assert.Same(actual, editor.Commands[0]);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public void AddEmptyLinesTest(int count)
        {
            var editor = new CommandPromptEditor();
            var actual = editor.AddEmptyLines(count);
            for(var i = 0; i < count; i++) {
                Assert.IsAssignableFrom<EmptyLine>(actual[i]);
                Assert.Same(actual[i], editor.Commands[i]);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddEmptyLines_Throw_Test(int count)
        {
            var editor = new CommandPromptEditor();
            Assert.Throws<ArgumentOutOfRangeException>(() => editor.AddEmptyLines(count));
        }

        [Fact]
        public void CreateShellCommandTest()
        {
            var editor = new CommandPromptEditor();
            var actual = editor.CreateShellCommand(impl => new EmptyLine());
            Assert.IsAssignableFrom<EmptyLine>(actual);
            Assert.Empty(editor.Commands);
        }

        [Fact]
        public void AddShellCommandTest()
        {
            var editor = new CommandPromptEditor();
            var actual = editor.AddShellCommand(impl => new EmptyLine());
            Assert.IsAssignableFrom<EmptyLine>(actual);
            Assert.Single(editor.Commands);
            Assert.Same(editor.Commands[0], actual);
        }


        [Fact]
        public void CreateCommandTest()
        {
            var editor = new CommandPromptEditor();
            var actual = editor.CreateCommand(impl => new EchoCommand() {
                Implementation = impl,
                Value = new Text("Hello, World!"),
            });
            Assert.IsAssignableFrom<EchoCommand>(actual);
            Assert.Empty(editor.Commands);
        }

        [Fact]
        public void AddCommandTest()
        {
            var editor = new CommandPromptEditor();
            var actual = editor.AddCommand(impl => new EchoCommand() {
                Implementation = impl,
                Value = new Text("Hello, World!"),
            });
            Assert.IsAssignableFrom<EchoCommand>(actual);
            Assert.Single(editor.Commands);
            Assert.Same(editor.Commands[0], actual);
        }

        [Fact]
        public async Task WriteAsyncTest()
        {
            var editor = new CommandPromptEditor();
            editor.AddCommand(iml => new EchoCommand() {
                Implementation = iml,
                Value = new Text("Hello, World!"),
            });

            var setCommand = editor.AddCommand(iml => new SetVariableCommand() {
                VariableName = "VAR",
                Value = Express.Create("value"),
            });

            var ifCommand = editor.AddCommand(iml => new IfExpressCommand() {
                Implementation = iml,
                Left = setCommand.Variable,
                Right = new Text("1"),
            });

            ifCommand.TrueBlock.Add(
                editor.CreateCommand(iml => new EchoCommand() {
                    Implementation = iml,
                    Value = new Text("VAR is 1"),
                })
            );
            ifCommand.FalseBlock.Add(
                editor.CreateCommand(iml => new EchoCommand() {
                    Implementation = iml,
                    Value = new Text("VAR is not 1"),
                })
            );

            using var stream = new System.IO.MemoryStream();
            await editor.WriteAsync(stream, TestContext.Current.CancellationToken);
            stream.Position = 0;
            using var reader = new System.IO.StreamReader(stream);
            var actual = await reader.ReadToEndAsync(TestContext.Current.CancellationToken);

            AssertEx.EqualMultiLineTextIgnoreNewline(
                """
                echo "Hello, World!"
                set VAR=value
                if "%VAR%" == 1 (
                	echo "VAR is 1"
                ) else (
                	echo "VAR is not 1"
                )
                """,
                actual
            );
        }

        [Fact]
        public void ToStringTest()
        {
            var editor = new CommandPromptEditor();
            editor.AddCommand(iml => new EchoCommand() {
                Implementation = iml,
                Value = new Text("Hello, World!"),
            });

            var setCommand = editor.AddCommand(iml => new SetVariableCommand() {
                VariableName = "VAR",
                Value = Express.Create("value"),
            });

            var ifCommand = editor.AddCommand(iml => new IfExpressCommand() {
                Implementation = iml,
                Left = setCommand.Variable,
                Right = new Text("1"),
            });

            ifCommand.TrueBlock.Add(
                editor.CreateCommand(iml => new EchoCommand() {
                    Implementation = iml,
                    Value = new Text("VAR is 1"),
                })
            );
            ifCommand.FalseBlock.Add(
                editor.CreateCommand(iml => new EchoCommand() {
                    Implementation = iml,
                    Value = new Text("VAR is not 1"),
                })
            );

            var actual = editor.ToString();
            AssertEx.EqualMultiLineTextIgnoreNewline(
                """
                echo "Hello, World!"
                set VAR=value
                if "%VAR%" == 1 (
                	echo "VAR is 1"
                ) else (
                	echo "VAR is not 1"
                )
                """,
                actual
            );
        }

        #endregion
    }
}
