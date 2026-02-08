using System.Collections.Generic;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Core.Models.Shell;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using ContentTypeTextNet.Pe.Library.Common.Linq;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class IfCommandBaseTest
    {
        #region define

        private sealed class IfCommand: IfCommandBase
        {
            public IfCommand(Express condition)
            {
                Condition = condition;
            }

            #region IfCommandBase

            protected override Express Condition { get; }

            #endregion
        }

        #endregion

        #region function

        public static TheoryData<string, (bool isNot, Express condition, IList<CommandBase> trueBlock, IList<CommandBase> falseBlock)> GetStatementData => new() {
            {
                """
                if condition (
                )
                """,
                (
                    false,
                    "condition",
                    new List<CommandBase>(),
                    new List<CommandBase>()
                )
            },
            {
                """
                if not condition (
                )
                """,
                (
                    true,
                    "condition",
                    new List<CommandBase>(),
                    new List<CommandBase>()
                )
            },
            {
                """
                if condition (
                	echo "true block"
                )
                """,
                (
                    false,
                    "condition",
                    new List<CommandBase>() {
                        new EchoCommand() {
                            Value = new Text("true block"),
                        },
                    },
                    new List<CommandBase>()
                )
            },
            {
                """
                if condition (
                	echo "true block"
                ) else (
                	echo "false block"
                )
                """,
                (
                    false,
                    "condition",
                    new List<CommandBase>() {
                        new EchoCommand() {
                            Value = new Text("true block"),
                        },
                    },
                    new List<CommandBase>() {
                        new EchoCommand() {
                            Value = new Text("false block"),
                        },
                    }
                )
            }
        };

        [Theory]
        [MemberData(nameof(GetStatementData))]
        public void GetStatementTest(string expected, (bool isNot, Express condition, IList<CommandBase> trueBlock, IList<CommandBase> falseBlock) parameters)
        {
            var (isNot, condition, trueBlock, falseBlock) = parameters;
            var command = new IfCommand(condition) {
                IsNot = isNot,
            };
            command.TrueBlock.AddRange(trueBlock);
            command.FalseBlock.AddRange(falseBlock);

            var actual = command.GetStatement();
            AssertEx.EqualMultiLineTextIgnoreNewline(expected, actual);
        }

        #endregion
    }
}
