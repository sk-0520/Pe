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
            using(var testApp = TestUI.Launch(testIO))
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
            using var testApp = TestUI.Launch(testIO);

            // 使用許諾 同意
            using(var automation = new UIA3Automation()) {
                var window = TestUI.GetMainWindow(testApp, automation);
                Assert.Equal("AcceptWindow", window.Properties.AutomationId);

                TestUI.WaitUntilTrue(() => window.IsAvailable);

                var affirmativeElement = TestUI.GetElementById("AffirmativeCommand", window);
                affirmativeElement.Click();

                TestUI.WaitUntilClosed(window);
            }

            TestUI.SilentStartup(testApp);
            TestUI.SilentLauncherToolbar(testApp);
        }

        [Fact]
        public void BootExecute_EasyLaunch_Test()
        {
            var testIO = TestIO.InitializeMethod(this);
            using(var testApp = TestUI.EasyLaunch(testIO)) {
                //NOP
            }
            Assert.True(true);
        }

        #endregion
    }
}
