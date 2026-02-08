using System.Collections.Generic;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    public class EnvironmentVariableEditorTest
    {
        #region function

        public static TheoryData<IEnumerable<LauncherEnvironmentVariableData>, string> ParseMergeItemsData => new() {
            {
                [],
                ""
            },
            {
                [
                    new LauncherEnvironmentVariableData("A", "1")
                ],
                "A=1"
            },
            {
                [
                    new LauncherEnvironmentVariableData("A", "1"),
                    new LauncherEnvironmentVariableData("B", "2"),
                ],
                """
                A=1
                B=2
                """
            },
            {
                [
                    new LauncherEnvironmentVariableData("A", "1"),
                    new LauncherEnvironmentVariableData("B", "2"),
                ],
                """
                A=1

                B=2
                """
            },
            {
                [
                    new LauncherEnvironmentVariableData("A", "1"),
                    new LauncherEnvironmentVariableData("E", "5"),
                ],
                """

                A=1

                B=

                =3

                D

                E=5

                """
            },
        };

        [Theory]
        [MemberData(nameof(ParseMergeItemsData))]
        public void ParseMergeItemsTest(IEnumerable<LauncherEnvironmentVariableData> expected, string text)
        {
            var test = new EnvironmentVariableEditor(NullLoggerFactory.Instance);
            var actual = test.ParseMergeItems(text);
            Assert.Equal(expected, actual);
        }

        public static TheoryData<IEnumerable<string>, string> ParseRemoveItemsData => new() {
            {
                [],
                ""
            },
            {
                [],
                " "
            },
            {
                [
                    "a"
                ],
                " a "
            },
        };

        [Theory]
        [MemberData(nameof(ParseRemoveItemsData))]
        public void ParseRemoveItemsTest(IEnumerable<string> expected, string text)
        {
            var test = new EnvironmentVariableEditor(NullLoggerFactory.Instance);
            var actual = test.ParseRemoveItems(text);
            Assert.Equal(expected, actual);
        }

        public static TheoryData<IEnumerable<LauncherEnvironmentVariableData>, IEnumerable<LauncherEnvironmentVariableData>, IEnumerable<string>> JoinData => new() {
            {
                [],
                [],
                []
            },
            {
                [
                    new LauncherEnvironmentVariableData("A", "1"),
                ],
                [
                    new LauncherEnvironmentVariableData("A", "1"),
                ],
                []
            },
            {
                [
                    new LauncherEnvironmentVariableData("B"),
                ],
                [],
                [
                    "B"
                ]
            },
            {
                [
                    new LauncherEnvironmentVariableData("A", "1"),
                    new LauncherEnvironmentVariableData("B"),
                ],
                [
                    new LauncherEnvironmentVariableData("A", "1"),
                    new LauncherEnvironmentVariableData("B", "2")
                ],
                [
                    "B"
                ]
            },
        };

        [Theory]
        [MemberData(nameof(JoinData))]
        public void JoinTest(IEnumerable<LauncherEnvironmentVariableData> expected, IEnumerable<LauncherEnvironmentVariableData> mergeItems, IEnumerable<string> removeItems)
        {
            var test = new EnvironmentVariableEditor(NullLoggerFactory.Instance);
            var actual = test.Join(mergeItems, removeItems);

            Assert.Equal(expected, actual);
        }

        public static TheoryData<string, IEnumerable<LauncherEnvironmentVariableData>> ConvertMergeTextData => new() {
            {
                "",
                []
            },
            {
                "A=1",
                [
                    new LauncherEnvironmentVariableData("A", "1"),
                ]
            },
            {
                "",
                [
                    new LauncherEnvironmentVariableData("A"),
                ]
            },
            {
                """
                A=1
                B=2
                """,
                [
                    new LauncherEnvironmentVariableData("A", "1"),
                    new LauncherEnvironmentVariableData("B", "2"),
                ]
            },
        };

        [Theory]
        [MemberData(nameof(ConvertMergeTextData))]
        public void ConvertMergeTextTest(string expected, IEnumerable<LauncherEnvironmentVariableData> mergeItems)
        {
            var test = new EnvironmentVariableEditor(NullLoggerFactory.Instance);
            var actual = test.ConvertMergeText(mergeItems);
            Assert.Equal(expected, actual);
        }

        public static TheoryData<string, IEnumerable<LauncherEnvironmentVariableData>> ConvertRemoveTextData => new() {
            {
                "",
                []
            },
            {
                "",
                [
                    new LauncherEnvironmentVariableData("A", "1"),
                ]
            },
            {
                "A",
                [
                    new LauncherEnvironmentVariableData("A"),
                ]
            },
            {
                """
                A
                B
                """,
                [
                    new LauncherEnvironmentVariableData("A"),
                    new LauncherEnvironmentVariableData("B"),
                ]
            },
        };

        [Theory]
        [MemberData(nameof(ConvertRemoveTextData))]
        public void ConvertRemoveTextTest(string expected, IEnumerable<LauncherEnvironmentVariableData> mergeItems)
        {
            var test = new EnvironmentVariableEditor(NullLoggerFactory.Instance);
            var actual = test.ConvertRemoveText(mergeItems);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
