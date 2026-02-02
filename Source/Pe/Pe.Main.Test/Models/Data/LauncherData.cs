using ContentTypeTextNet.Pe.Main.Models.Data;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Data
{
    public class LauncherEnvironmentVariableDataTest
    {
        #region function

        [Fact]
        public void New_Default_Test()
        {
            var test = new LauncherEnvironmentVariableData();
            Assert.Empty(test.Name);
            Assert.Empty(test.Value);
            Assert.True(test.IsRemove);
        }

        [Fact]
        public void New_Name_Test()
        {
            var test = new LauncherEnvironmentVariableData() {
                Name = nameof(LauncherEnvironmentVariableData.Name),
            };
            Assert.Equal(nameof(LauncherEnvironmentVariableData.Name), test.Name);
            Assert.Empty(test.Value);
            Assert.True(test.IsRemove);
        }


        [Fact]
        public void New_Value_Test()
        {
            var test = new LauncherEnvironmentVariableData() {
                Value = nameof(LauncherEnvironmentVariableData.Value),
            };
            Assert.Empty(test.Name);
            Assert.Equal(nameof(LauncherEnvironmentVariableData.Value), test.Value);
            Assert.False(test.IsRemove);
        }

        #endregion
    }
}
