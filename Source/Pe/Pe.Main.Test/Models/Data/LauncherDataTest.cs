using System;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Data
{
    public class LauncherEnvironmentVariableDataTest
    {
        #region function

        [Theory]
        [InlineData("name", null, null)]
        [InlineData("name", "", null)]
        [InlineData("name", null, nameof(LauncherEnvironmentVariableData.Value))]
        [InlineData("value", nameof(LauncherEnvironmentVariableData.Name), null)]
        public void Constructor_2_Test(string expectedParam, string? name, string? value)
        {
            var exception = Assert.ThrowsAny<ArgumentException>(() => new LauncherEnvironmentVariableData(name!, value!));
            Assert.Equal(expectedParam, exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_1_Test(string? name)
        {
            var exception = Assert.ThrowsAny<ArgumentException>(() => new LauncherEnvironmentVariableData(name!));
            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void New_Name_Test()
        {
            var test = new LauncherEnvironmentVariableData(nameof(LauncherEnvironmentVariableData.Name));

            Assert.Equal(nameof(LauncherEnvironmentVariableData.Name), test.Name);
            Assert.Empty(test.Value);
            Assert.True(test.IsRemove);
        }


        [Fact]
        public void New_Value_Test()
        {
            var test = new LauncherEnvironmentVariableData(nameof(LauncherEnvironmentVariableData.Name), nameof(LauncherEnvironmentVariableData.Value));

            Assert.Equal(nameof(LauncherEnvironmentVariableData.Name), test.Name);
            Assert.Equal(nameof(LauncherEnvironmentVariableData.Value), test.Value);
            Assert.False(test.IsRemove);
        }

        #endregion
    }
}
