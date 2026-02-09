using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Library.CommandLine;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Library.Common.Linq;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using Xunit;

namespace ContentTypeTextNet.Pe.Pe.MainUI.Test.Test
{
    /// <summary>
    /// <see cref="Application"/> のヘルパー処理。
    /// </summary>
    /// <remarks>
    /// 暗黙的に <see cref="FlaUI.Core.Application"/> へキャストされる。
    /// </remarks>
    public class TestAutomation: DisposerBase
    {
        public TestAutomation(Application application)
        {
            Application = application;
        }

        #region property

        public Application Application { get; }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(!Application.HasExited) {
                        Application.Close();
                    }
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region operator

        public static implicit operator Application(TestAutomation arg)
        {
            return arg.Application;
        }

        #endregion
    }

    /// <summary>
    /// タイムアウト設定。
    /// </summary>
    /// <remarks>個別に設定することはあまり考慮していない</remarks>
    //NOTE: テスト環境により待ち時間の指針は変わるので外部(コマンドライン引数や環境変数など)から設定できることも考慮した方が良いかもしれない
    public record class TestTimeout
    {
        #region property

        /// <summary>
        /// <see cref="TotalTimeout"/> のデフォルト値。
        /// </summary>
        public static readonly TimeSpan DefaultTotalTimeout = TimeSpan.FromSeconds(10);
        /// <summary>
        /// <see cref="Interval"/> のデフォルト値。
        /// </summary>
        public static readonly TimeSpan DefaultInterval = TimeSpan.FromMilliseconds(200);

        /// <summary>
        /// 共通使用されるタイムアウト設定。
        /// </summary>
        /// <remarks>
        /// <para>各テストにおいて部分的に変更したい場合は、<c><see cref="TestTimeout.Default"/> <see langword="with"/> { ... }</c>で個別に設定する指針。</para>
        /// </remarks>
        public static TestTimeout Default => field ??= new TestTimeout() {
            // UIテストは直列なのでこの程度のキャッシュで良いのだ
            // そもそもキャッシュ自体が目的ではなく各テストにおいて new TestTimeout() { ... } が面倒なので null から共通値を取得する目的
            TotalTimeout = DefaultTotalTimeout,
            Interval = DefaultInterval,
        };

        /// <summary>
        /// 全体タイムアウト時間。
        /// </summary>
        public required TimeSpan TotalTimeout { get; init; }
        /// <summary>
        /// チェック間隔。
        /// </summary>
        public required TimeSpan Interval { get; init; }

        #endregion

        #region function

        public static TestTimeout Normalize(TestTimeout? timeout)
        {
            if(timeout is null) {
                return Default;
            }

            return timeout;
        }

        #endregion
    }

    /// <summary>
    /// UIテスト用ヘルパー。
    /// </summary>
    public static class TestUI
    {
        #region property

        public static IReadOnlyDictionary<string, string> EmptyOptions => field ??= new Dictionary<string, string>();
        public static IReadOnlySet<string> EmptySwitches => field ??= new HashSet<string>();

        #endregion

        #region function

        public static void SilentStartup(TestAutomation testApp)
        {
            // スタートアップウィンドウの表示確認と終了
            using(var automation = new UIA3Automation()) {
                var window = GetMainWindow(testApp, automation);
                Assert.Equal("StartupWindow", window.Properties.AutomationId);

                var closeCommand = TestUI.GetElementById("CloseCommand", window);
                closeCommand.Click();

                WaitUntilClosed(window);
            }
        }

        public static void SilentLauncherToolbar(TestAutomation testApp)
        {
            // ランチャーツールバーのみが表示される
            using(var automation = new UIA3Automation()) {
                var windows = Get(
                    () => testApp.Application.GetAllTopLevelWindows(automation),
                    a => 1 <= a.Length
                );
                Assert.Single(windows);
                Assert.Equal("LauncherToolbarWindow", windows[0].Properties.AutomationId);
            }
        }

        /// <summary>
        /// プログラムの実行。
        /// </summary>
        /// <param name="options">オプション一覧。</param>
        /// <returns><see cref="TestAutomation"/></returns>
        public static TestAutomation Launch(IReadOnlyDictionary<string, string> options, IReadOnlySet<string> switches)
        {
            var commandLineHelper = new CommandLineHelper();
            var arguments = commandLineHelper.ToCommandLineArguments(options)
                .Concat(switches.Select(a => $"--{a}"))
                .JoinString(" ")
            ;

            var application = Application.Launch("Pe.Main.exe", arguments);
            Assert.NotNull(application);

            return new TestAutomation(application);
        }

        /// <summary>
        /// <inheritdoc cref="Launch(IReadOnlyDictionary{string, string})"/>
        /// 基本的にはこちらを使用して、テストごとのデータを分離する。
        /// </summary>
        /// <param name="testIO">このテスト用のIOを用いてユーザーディレクトリなどのデータディレクトリが構築される。</param>
        /// <param name="skipAccept">使用許諾同意をスキップするか。</param>
        /// <param name="extensionOptions">追加するオプション一覧。</param>
        /// <param name="extensionSwitches">追加するスイッチ一覧。</param>
        /// <returns><inheritdoc cref="Launch(IReadOnlyDictionary{string, string})"/></returns>
        public static TestAutomation Launch(TestIO testIO, IReadOnlyDictionary<string, string>? extensionOptions = null, IReadOnlySet<string>? extensionSwitches = null)
        {
            var data = testIO.Work.CreateDirectory("data");
            var user = data.CreateDirectory("user");
            var machine = data.CreateDirectory("machine");
            var temp = data.CreateDirectory("temp");
            var log = data.CreateEmptyFile("log.log");

            var options = new Dictionary<string, string>() {
                { "user-dir", user.Directory.FullName },
                { "machine-dir", machine.Directory.FullName },
                { "temp-dir", temp.Directory.FullName },
                { "log", log.FullName },
            };
            if(extensionOptions is not null) {
                foreach(var p in extensionOptions) {
                    options[p.Key] = p.Value;
                }
            }

            var switches = new HashSet<string>();
            if(extensionSwitches is not null) {
                foreach(var s in extensionSwitches) {
                    switches.Add(s);
                }
            }

            return Launch(options, switches);
        }

        public static TestAutomation EasyLaunch(TestIO testIO, IReadOnlyDictionary<string, string>? extensionOptions = null, IReadOnlySet<string>? extensionSwitches = null)
        {
            var switches = new HashSet<string>() {
                "skip-accept",
            };
            if(extensionSwitches is not null) {
                foreach(var s in extensionSwitches) {
                    switches.Add(s);
                }
            }

            var testApp = Launch(testIO, extensionOptions, switches);

            SilentStartup(testApp);
            SilentLauncherToolbar(testApp);

            return testApp;
        }



        /// <summary>
        /// UI要素的な何かを取得。
        /// </summary>
        /// <typeparam name="TResult">取得対象の型。</typeparam>
        /// <param name="func">取得処理。</param>
        /// <param name="when">取得データの判定処理。<see langword="true"/>を返した場合に取得成功とみなす。</param>
        /// <param name="timeout">タイムアウト設定。</param>
        /// <returns>取得データ。</returns>
        /// <remarks>
        /// <para>戻り値は非 <see langword="null"/> を強制する。</para>
        /// <para><see cref="Retry.While"/>のチェックが <see langword="false"/> を返すことで成功とみなすのがどうにもしんどいので <paramref name="when"/> は <see langword="true"/> を返すヘルパー的なもの。</para>
        /// <para>UIチェックにおける <see langword="null"/> は許容すべきでないと思うんよ。</para>
        /// </remarks>
        [return: NotNull]
        public static TResult Get<TResult>(Func<TResult> func, Func<TResult, bool> when, TestTimeout? timeout = null)
        {
            var normalizedTimeout = TestTimeout.Normalize(timeout);
            var setting = new RetrySettings {
                Timeout = normalizedTimeout.TotalTimeout,
                Interval = normalizedTimeout.Interval,
            };
            var result = Retry.While(
                func,
                a => {
                    if(a is null) {
                        return true;
                    }
                    return !when(a);
                },
                setting
            ).Result;

            Assert.NotNull(result);

            return result;
        }

        /// <summary>
        /// チェック処理なし版。
        /// </summary>
        /// <inheritdoc cref="Get{TResult}(Func{TResult}, Func{TResult, bool}, TestTimeout?)"/>
        [return: NotNull]
        public static TResult Get<TResult>(Func<TResult> func, TestTimeout? timeout = null)
        {
            return Get(func, a => true, timeout);
        }

        /// <summary>
        /// <see cref="Application.GetMainWindow(UIA3Automation)"/> ラッパー。
        /// </summary>
        /// <param name="application"></param>
        /// <param name="automation"></param>
        /// <param name="timeout">タイムアウト設定。</param>
        /// <returns></returns>
        public static Window GetMainWindow(Application application, UIA3Automation automation, TestTimeout? timeout = null)
        {
            return Get(() => application.GetMainWindow(automation), timeout);
        }

        /// <summary>
        /// オートメーションIDで要素を取得。
        /// </summary>
        /// <param name="automationId">対象ID。</param>
        /// <param name="owner">IDを持つ要素。</param>
        /// <returns>要素。</returns>
        public static AutomationElement GetElementById(string automationId, AutomationElement owner)
        {
            var element = owner.FindFirstDescendant(a => a.ByAutomationId(automationId));

            Assert.NotNull(element);

            return element;
        }

        /// <summary>
        /// <paramref name="condition"/>が <see langword="true"/> になるまで待機。
        /// </summary>
        /// <param name="condition">判定処理。<c>() => obj.IsXXX</c> 程度を想定。</param>
        /// <param name="timeout">タイムアウト設定。</param>
        public static void WaitUntilTrue(Func<bool> condition, TestTimeout? timeout = null)
        {
            var normalizedTimeout = TestTimeout.Normalize(timeout);
            var result = Retry.WhileFalse(
                condition,
                normalizedTimeout.TotalTimeout,
                normalizedTimeout.Interval
            );

            Assert.True(result.Success);
        }

        /// <summary>
        /// <paramref name="condition"/>が <see langword="false"/> になるまで待機。
        /// </summary>
        /// <param name="condition">判定処理。<c>() => obj.IsXXX</c> 程度を想定。</param>
        /// <param name="timeout">タイムアウト設定。</param>
        public static void WaitUntilFalse(Func<bool> condition, TestTimeout? timeout = null)
        {
            var normalizedTimeout = TestTimeout.Normalize(timeout);
            var result = Retry.WhileTrue(
                condition,
                normalizedTimeout.TotalTimeout,
                normalizedTimeout.Interval
            );

            Assert.True(result.Success);
        }

        /// <summary>
        /// ウィンドウが閉じられるまで待機。
        /// </summary>
        /// <param name="window">対象ウィンドウ。</param>
        /// <param name="timeout">タイムアウト設定。</param>
        public static void WaitUntilClosed(Window window, TestTimeout? timeout = null)
        {
            WaitUntilFalse(() => NativeMethods.IsWindow(window.Properties.NativeWindowHandle.Value), timeout);
        }

        #endregion
    }
}
