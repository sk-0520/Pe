using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element._Debug_;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels._Debug_;
using ContentTypeTextNet.Pe.Main.Views._Debug_;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
#if DEBUG
    partial class ApplicationManager
    {
        #region function

        void DebugExecuteBefore()
        {
            Logger.LogDebug("デバッグ用前処理");

            //DebugColorPicker();
            //Exit();
        }

        void DebugExecuteAfter()
        {
            Logger.LogDebug("デバッグ用後処理");

            //DebugCustomize();
            //DebugExtendsExecute();
            //DebugStdIoExecute();
            //DebugHook();
            DebugKeyAction();
        }

        void DebugCustomize()
        {
            // LauncherGroups.Sequence を調整すること
            var i = LauncherToolbarElements.First().LauncherItems.FirstOrDefault();
            if(i != null) {
                i.OpenCustomizeView(Screen.PrimaryScreen);
            }
        }

        void DebugExtendsExecute()
        {
            var i = LauncherToolbarElements.First().LauncherItems.FirstOrDefault();
            if(i != null) {
                i.OpenExtendsExecuteView(Screen.PrimaryScreen);
            }
        }

        void DebugStdIoExecute()
        {
            var batchPath = @".\temp.bat";
            File.WriteAllText(batchPath, @"
echo test1
echo test2
ping localhost
rem ping 127.0.0.1
rem ping localhost
rem ping 127.0.0.1
rem ping localhost
rem ping 127.0.0.1
rem ping localhost
rem ping 127.0.0.1
test
echo end
            ", Encoding.GetEncoding("shift_jis"));
            var launcherExecutor = new LauncherExecutor(OrderManager, new ApplicationDispatcherWapper(), LoggerFactory);
            var data = new LauncherFileData() {
                //Path = batchPath,
                Path = "cmd",
                Option = "/c " + batchPath,
                IsEnabledStandardInputOutput = true,
            };
            var env = new List<LauncherEnvironmentVariableData>();
            var result = launcherExecutor.Execute(LauncherItemKind.File, data, data, env, Screen.PrimaryScreen);
        }

        KeyboradHooker? dbgKeyboradHooker { get; set; }
        MouseHooker? dbgMouseHooker { get; set; }
        void DebugHook()
        {
            dbgKeyboradHooker = new KeyboradHooker(LoggerFactory);
            dbgKeyboradHooker.KeyDown += (sender, e) => {
                Logger.LogTrace("UP: key = {0}, mods = {1}, {2}", e.Key, e.modifierKeyStatus, e.kbdll);
            };
            dbgKeyboradHooker.KeyUp += (sender, e) => {
                Logger.LogTrace("DW: key = {0}, mods = {1}, {2}", e.Key, e.modifierKeyStatus, e.kbdll);
            };
            dbgKeyboradHooker.Register();

            dbgMouseHooker = new MouseHooker(LoggerFactory);
            //dbgMouseHooker.Register();
        }

        KeyActionChecker? dbgKeyActionChecker { get; set; }
        void DebugKeyAction()
        {
            dbgKeyActionChecker = new KeyActionChecker(LoggerFactory);
            dbgKeyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData() {
                    KeyActionId = Guid.NewGuid(),
                    KeyActionKind = KeyActionKind.Disable,
                },
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.X,
                }
            ));
            dbgKeyActionChecker.DisableJobs.Add(new KeyActionDisableJob(
                new KeyActionDisableData() {
                    KeyActionId = Guid.NewGuid(),
                    KeyActionKind = KeyActionKind.Disable,
                },
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.E,
                    Super = ModifierKey.Any,
                }
            ));
            dbgKeyActionChecker.ReplaceJobs.Add(new KeyActionReplaceJob(
                new KeyActionReplaceData() {
                    KeyActionId = Guid.NewGuid(),
                    KeyActionKind = KeyActionKind.Replace,
                    ReplaceKey = System.Windows.Input.Key.B,
                },
                new KeyMappingData() {
                    Key = System.Windows.Input.Key.A,
                }
            ));

            dbgKeyboradHooker = new KeyboradHooker(LoggerFactory);
            dbgKeyboradHooker.KeyDown += (sender, e) => {
                var jobs = dbgKeyActionChecker.Find(true, e.Key, e.modifierKeyStatus, e.kbdll);
                if(jobs.Any()) {
                    e.Handled = true;
                    Task.Run(() => {
                        foreach(var job in jobs) {
                            Logger.LogTrace("[{0}]: {1}", job.CommonData.KeyActionId, job);
                            if(job.CommonData.KeyActionKind == KeyActionKind.Replace) {
                                var replaceJob = (KeyActionReplaceJob)job;
                                var input = new INPUT() {
                                    type = INPUT_type.INPUT_KEYBOARD,
                                };
                                input.ki.wVk = (ushort)replaceJob.ActionData.ReplaceKey;
                                input.ki.wScan = (ushort)NativeMethods.MapVirtualKey(input.ki.wVk, MAPVK.MAPVK_VK_TO_VSC);
                                input.ki.dwFlags = KEYEVENTF.KEYEVENTF_EXTENDEDKEY| KEYEVENTF.KEYEVENTF_KEYDOWN;
                                input.ki.dwExtraInfo = new UIntPtr(dbgKeyActionChecker.SelfJobInputId);
                                input.ki.time = 0;

                                var inputs = new[] {
                                    input,
                                };

                                NativeMethods.SetLastError(0);
                                NativeMethods.SendInput(1, inputs, Marshal.SizeOf<INPUT>());
                                var e1 = Marshal.GetLastWin32Error();
                                var e2 = NativeMethods.GetLastError();
                                Logger.LogDebug("last error: {0}, {1}", e1, e2);
                            }
                        }
                    });
                }
            };
            dbgKeyboradHooker.Register();
        }

        void DebugColorPicker()
        {
            using(var di = ApplicationDiContainer.CreateChildContainer()) {
                di.RegisterMvvm<DebugColorPickerElement, DebugColorPickerViewModel, DebugColorPickerWindow>();
                var model = di.Build<DebugColorPickerElement>();
                var view = di.Build<DebugColorPickerWindow>();
                var windowItem = new WindowItem(WindowKind.Debug, view);
                WindowManager.Register(windowItem);
                view.ShowDialog();
            }
        }
        #endregion
    }
#endif
}
