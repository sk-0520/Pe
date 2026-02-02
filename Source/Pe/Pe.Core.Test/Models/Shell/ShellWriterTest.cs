using ContentTypeTextNet.Pe.Core.Models.Shell;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell
{
    public class DefaultShellWriterTest
    {
        #region function

        private static DefaultShellWriter<ShellOptions> CreateDefaultShellWriter(ShellOptions options)
        {
            var writer = new DefaultShellWriter<ShellOptions>(options);
            return writer;
        }

        [Theory]
        [InlineData("\r", "\r")]
        [InlineData("\n", "\n")]
        [InlineData("\r\n", "\r\n")]
        public void WriteLineTest(string expected, string newline)
        {
            var writer = CreateDefaultShellWriter(new ShellOptions() {
                NewLine = newline,
            });
            writer.WriteLine();
            var actual = writer.ToString();
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
