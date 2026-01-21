using System;
using ContentTypeTextNet.Pe.Core.Models.Shell.Value;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class CopyCommandTest
    {
        #region function

        public static TheoryData<string, (Express source, Express destination, bool isDecryption, bool isVerify, PromptMode promptMode)> GetStatementData => new() {
            {
                "copy from to",
                (
                    "from",
                    "to",
                    false,
                    false,
                    PromptMode.Default
                )
            },
            {
                "copy \" from \" \" to \"",
                (
                    " from ",
                    " to ",
                    false,
                    false,
                    PromptMode.Default
                )
            },
            {
                "copy /d from to",
                (
                    "from",
                    "to",
                    true,
                    false,
                    PromptMode.Default
                )
            },
            {
                "copy /d /v from to",
                (
                    "from",
                    "to",
                    true,
                    true,
                    PromptMode.Default
                )
            },
            {
                "copy /-y from to",
                (
                    "from",
                    "to",
                    false,
                    false,
                    PromptMode.Confirm
                )
            },
            {
                "copy /y from to",
                (
                    "from",
                    "to",
                    false,
                    false,
                    PromptMode.Silent
                )
            },
        };

        [Theory]
        [MemberData(nameof(GetStatementData))]
        public void GetStatementTest(string expected, (Express source, Express destination, bool isDecryption, bool isVerify, PromptMode promptMode) parameters)
        {
            var (source, destination, isDecryption, isVerify, promptMode) = parameters;
            var command = new CopyCommand() {
                Source = source,
                Destination = destination,
                IsDecryption = isDecryption,
                IsVerify = isVerify,
                PromptMode = promptMode,
            };
            var actual = command.GetStatement();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetStatement_Source_Throw_Test(string? source)
        {
            var command = new CopyCommand() {
                Source = source!,
                Destination = "dest",
            };
            Assert.Throws<InvalidOperationException>(() => command.GetStatement());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetStatement_Destination_Throw_Test(string? destination)
        {
            var command = new CopyCommand() {
                Source = "source",
                Destination = destination!,
            };
            Assert.Throws<InvalidOperationException>(() => command.GetStatement());
        }

        #endregion
    }
}
