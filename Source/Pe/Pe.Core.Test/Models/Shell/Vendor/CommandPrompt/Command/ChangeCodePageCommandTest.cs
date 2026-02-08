using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Shell.Vendor.CommandPrompt.Command;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Shell.Vendor.CommandPrompt.Command
{
    public class ChangeCodePageCommandTest
    {
        static ChangeCodePageCommandTest()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        #region function

        public static TheoryData<int, Encoding> Data => new() {
            {
                20127, Encoding.ASCII
            },
            {
#pragma warning disable SYSLIB0001 // 型またはメンバーが旧型式です
                65000, Encoding.UTF7
#pragma warning restore SYSLIB0001 // 型またはメンバーが旧型式です
            },
            {
                65001, Encoding.UTF8
            },
            {
                65001, EncodingUtility.UTF8N
            },
            {
                65001, EncodingUtility.UTF8Bom
            },
            {
                (int)NativeMethods.GetACP(), EncodingUtility.GetDefaultEncoding()
            },
            {
                1200, Encoding.Unicode
            },
            {
                1201, Encoding.BigEndianUnicode
            },
            {
                12000, Encoding.UTF32
            },
            {
                12001, new UTF32Encoding(true, false)
            },
            {
                932, Encoding.GetEncoding("shift_jis")
            },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void Test(int excepcted, Encoding encoding)
        {
            var command = new ChangeCodePageCommand() {
                Encoding = encoding,
            };
            var actual = command.GetStatement();
            Assert.Equal($"chcp {excepcted}", actual);
        }

        #endregion
    }
}
