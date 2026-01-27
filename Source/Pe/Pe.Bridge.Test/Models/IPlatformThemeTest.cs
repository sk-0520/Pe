using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using NSubstitute;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models
{
    public class PlatformThemeColorsTest
    {
        #region function

        [Fact]
        public void Test()
        {
            var actual = new PlatformThemeColors(Colors.Lime, Colors.Red, Colors.Gray, Colors.DarkGray);

            Assert.Equal(Colors.Lime, actual.Background);
            Assert.Equal(Colors.Red, actual.Foreground);
            Assert.Equal(Colors.Gray, actual.Control);
            Assert.Equal(Colors.DarkGray, actual.Border);
        }

        #endregion
    }

    public class PlatformAccentColorsTest
    {
        #region function

        [Fact]
        public void Test()
        {
            var actual = new PlatformAccentColors(Colors.Lime, Colors.Red, Colors.Gray, Colors.DarkGray, Colors.Yellow);

            Assert.Equal(Colors.Lime, actual.Accent);
            Assert.Equal(Colors.Red, actual.Base);
            Assert.Equal(Colors.Gray, actual.Highlight);
            Assert.Equal(Colors.DarkGray, actual.Active);
            Assert.Equal(Colors.Yellow, actual.Disable);
        }

        #endregion
    }

    public class IPlatformThemeLoaderExtensionsTest
    {
        #region function

        [Fact]
        public void GetTaskbarColor_ColorPrevalence_Test()
        {
            var mockPlatformTheme = Substitute.For<IPlatformTheme>();
            mockPlatformTheme.ColorPrevalence.Returns(true);
            mockPlatformTheme.AccentColor.Returns(Colors.Red);
            mockPlatformTheme.EnableTransparency.Returns(true);

            var actual = mockPlatformTheme.GetTaskbarColor();
            Assert.Equal(Colors.Red, actual);
        }

        [Fact]
        public void GetTaskbarColor_not_ColorPrevalence_Test()
        {
            var mockPlatformTheme = Substitute.For<IPlatformTheme>();
            mockPlatformTheme.ColorPrevalence.Returns(false);
            mockPlatformTheme.AccentColor.Returns(Colors.White);
            mockPlatformTheme.GetWindowsThemeColors(Arg.Any<PlatformThemeKind>()).Returns(new PlatformThemeColors(Colors.Lime, Colors.Red, Colors.Gray, Colors.DarkGray));
            mockPlatformTheme.EnableTransparency.Returns(true);

            var actual = mockPlatformTheme.GetTaskbarColor();
            Assert.Equal(Colors.Lime, actual);
        }

        [Fact]
        public void GetTaskbarColor_EnableTransparency_Test()
        {
            var mockPlatformTheme = Substitute.For<IPlatformTheme>();
            mockPlatformTheme.ColorPrevalence.Returns(false);
            mockPlatformTheme.AccentColor.Returns(Color.FromArgb(0x99, 0xff, 0xff, 0xff));
            mockPlatformTheme.GetWindowsThemeColors(Arg.Any<PlatformThemeKind>()).Returns(new PlatformThemeColors(Colors.Lime, Colors.Red, Colors.Gray, Colors.DarkGray));
            mockPlatformTheme.EnableTransparency.Returns(false);

            var actual = mockPlatformTheme.GetTaskbarColor();
            Assert.Equal(Colors.Lime, actual);
        }

        #endregion
    }
}
