using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    public class EnvironmentVariableConfigurationTest
    {
        #region define

        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "GetMergeItemsCore")]
        private static extern IEnumerable<LauncherEnvironmentVariableData> EnvironmentVariableConfiguration_GetMergeItemsCore(EnvironmentVariableConfiguration evc, string text);
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "GetRemoveItemsCore")]
        private static extern IEnumerable<string> EnvironmentVariableConfiguration_GetRemoveItemsCore(EnvironmentVariableConfiguration evc, string text);

        #endregion

        #region function

        public static TheoryData<IEnumerable<LauncherEnvironmentVariableData>, string> GetMergeItemsCoreTestData => new() {
            {
                [],
                ""
            },
            {
                [
                    new LauncherEnvironmentVariableData() { Name = "A", Value = "1" }
                ],
                "A=1"
            },
            {
                [
                    new LauncherEnvironmentVariableData() { Name = "A", Value = "1" },
                    new LauncherEnvironmentVariableData() { Name = "B", Value = "2" },
                ],
                """
                A=1
                B=2
                """
            },
            {
                [
                    new LauncherEnvironmentVariableData() { Name = "A", Value = "1" },
                    new LauncherEnvironmentVariableData() { Name = "B", Value = "2" },
                ],
                """
                A=1

                B=2
                """
            },
            {
                [
                    new LauncherEnvironmentVariableData() { Name = "A", Value = "1" },
                    new LauncherEnvironmentVariableData() { Name = "E", Value = "5" },
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
        [MemberData(nameof(GetMergeItemsCoreTestData))]
        public void GetMergeItemsCoreTest(IEnumerable<LauncherEnvironmentVariableData> expected, string text)
        {
            var evc = new EnvironmentVariableConfiguration(NullLoggerFactory.Instance);
            var actual = EnvironmentVariableConfiguration_GetMergeItemsCore(evc, text);
            Assert.Equal(expected.Count(), actual.Count());
            foreach(var (e, a) in expected.Zip(actual)) {
                Assert.Equal(e.Name, a.Name);
                Assert.Equal(e.Value, a.Value);
                Assert.Equal(e.IsRemove, a.IsRemove);
            }
        }

        public static TheoryData<IEnumerable<string>, string> GetRemoveItemsCoreData => new() {
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
        [MemberData(nameof(GetRemoveItemsCoreData))]
        public void GetRemoveItemsCoreTest(IEnumerable<string> expected, string text)
        {
            var evc = new EnvironmentVariableConfiguration(NullLoggerFactory.Instance);
            var actual = EnvironmentVariableConfiguration_GetRemoveItemsCore(evc, text);
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
                    new LauncherEnvironmentVariableData() { Name = "A", Value = "1" },
                ],
                [
                    new LauncherEnvironmentVariableData() { Name = "A", Value = "1" },
                ],
                []
            },
            {
                [
                    new LauncherEnvironmentVariableData() { Name = "B" },
                ],
                [],
                [
                    "B"
                ]
            },
            {
                [
                    new LauncherEnvironmentVariableData() { Name = "A", Value = "1" },
                    new LauncherEnvironmentVariableData() { Name = "B" },
                ],
                [
                    new LauncherEnvironmentVariableData() { Name = "A", Value = "1" },
                    new LauncherEnvironmentVariableData() { Name = "B", Value = "1" }
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
            var evc = new EnvironmentVariableConfiguration(NullLoggerFactory.Instance);
            var actual = evc.Join(mergeItems, removeItems);

            Assert.Equal(expected.Count(), actual.Count());
            foreach(var (e, a) in expected.Zip(actual)) {
                Assert.Equal(e.Name, a.Name);
                Assert.Equal(e.Value, a.Value);
                Assert.Equal(e.IsRemove, a.IsRemove);
            }
        }

        #endregion
    }
}
