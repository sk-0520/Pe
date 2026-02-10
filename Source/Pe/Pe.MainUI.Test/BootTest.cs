using System.Linq;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Pe.MainUI.Test.Test;
using FlaUI.UIA3;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.UI
{
    public class BootTest
    {
        #region function

        // 引数なしだとローカル開発環境と競合する可能性があるため CI でのみ実行できるものと考えた方が良い
        // 正直なくても良いと思いつつ、未指定実行で死ぬことがないようにしたいのであえて実施
        [Fact]
        public void DefaultBootKillTest()
        {
            using var testApp = TestUI.Launch(TestUI.EmptyOptions, TestUI.EmptySwitches);
            using(var automation = new UIA3Automation()) {
                var window = TestUI.GetMainWindow(testApp, automation);
                Assert.Equal("AcceptWindow", window.Properties.AutomationId);
                Assert.Contains(
                    global::ContentTypeTextNet.Pe.Main.Properties.Resources.String_Accept_Caption,
                    window.Title
                );
            }
        }

        [Fact]
        public void BootCancelTest()
        {
            var testIO = TestIO.InitializeMethod(this);
            using(var testApp = TestUI.Launch(testIO, LaunchMode.None))
            using(var automation = new UIA3Automation()) {
                var window = TestUI.GetMainWindow(testApp, automation);
                Assert.Equal("AcceptWindow", window.Properties.AutomationId);

                TestUI.WaitUntilTrue(() => window.IsAvailable);

                var negativeElement = TestUI.GetElementById("NegativeCommand", window);
                negativeElement.Click();

                TestUI.WaitUntilFalse(() => window.IsAvailable);
            }

            // キャンセルされたのでログファイルのみ作られた
            // 各種データディレクトリは TestIO で作られたのでこのテストではファイルのみを見る
            var workFiles = testIO.Work.Directory.EnumerateFiles("*", System.IO.SearchOption.AllDirectories).ToArray();
            Assert.Single(workFiles);
            Assert.Equal("log.log", workFiles[0].Name);
        }

        [Fact]
        public void BootExecuteTest()
        {
            var testIO = TestIO.InitializeMethod(this);
            using var testApp = TestUI.Launch(testIO, LaunchMode.None);

            // 使用許諾 同意
            using(var automation = new UIA3Automation()) {
                var window = TestUI.GetMainWindow(testApp, automation);
                Assert.Equal("AcceptWindow", window.Properties.AutomationId);

                TestUI.WaitUntilTrue(() => window.IsAvailable);

                var affirmativeElement = TestUI.GetElementById("AffirmativeCommand", window);
                affirmativeElement.Click();

                TestUI.WaitUntilClosed(window);
            }

            // スタートアップウィンドウの表示確認と終了
            using(var automation = new UIA3Automation()) {
                var window = TestUI.GetMainWindow(testApp, automation);
                Assert.Equal("StartupWindow", window.Properties.AutomationId);

                var closeCommand = TestUI.GetElementById("CloseCommand", window);
                closeCommand.Click();

                TestUI.WaitUntilClosed(window);
            }

            // ランチャーツールバーのみが表示される
            using(var automation = new UIA3Automation()) {
                var windows = TestUI.GetAllTopLevelWindows(testApp, automation);
                Assert.Single(windows);
                Assert.Equal("LauncherToolbarWindow", windows[0].Properties.AutomationId);
            }
        }

        [Fact]
        public void BootExecute_LaunchMode_SkipAccept_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            using var testApp = TestUI.Launch(testIO, LaunchMode.SkipAccept);

            using(var automation = new UIA3Automation()) {
                var window = TestUI.GetMainWindow(testApp, automation);
                // 使用許諾は表示されない
                Assert.NotEqual("AcceptWindow", window.Properties.AutomationId);
                // スタートアップが表示される
                Assert.Equal("StartupWindow", window.Properties.AutomationId);
            }
        }

        [Fact]
        public void BootExecute_LaunchMode_SkipStartup_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            using var testApp = TestUI.Launch(testIO, LaunchMode.SkipStartup);

            using(var automation = new UIA3Automation()) {
                var window = TestUI.GetMainWindow(testApp, automation);
                // 使用許諾が表示される
                Assert.Equal("AcceptWindow", window.Properties.AutomationId);
                // スタートアップは表示されない
                Assert.NotEqual("StartupWindow", window.Properties.AutomationId);
            }
        }

        [Fact]
        public void BootExecute_LaunchMode_Default_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            using var testApp = TestUI.Launch(testIO, LaunchMode.Default);

            // ランチャーツールバーのみが表示される
            using(var automation = new UIA3Automation()) {
                var windows = TestUI.GetAllTopLevelWindows(testApp, automation);
                Assert.Single(windows);
                Assert.Equal("LauncherToolbarWindow", windows[0].Properties.AutomationId);
            }
        }

        #endregion
    }
}
