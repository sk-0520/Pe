// [T4] build 2020-05-01 13:32:17Z(UTC)
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    partial class MouseHooker
    {
        protected override IntPtr HookProcedure(int code, IntPtr wParam, IntPtr lParam)
        {
            if(IsSkipCode(code)) {
                return CallNextProcedure(code, wParam, lParam);
            }

            if(code != (int)HC.HC_ACTION) {
                return CallNextProcedure(code, wParam, lParam);
            }

            MouseHookEventArgs? e = null;
            EventHandler<MouseHookEventArgs>? target = null;


            var wParamValue = wParam.ToInt32();

            switch(wParamValue) {
                case (int)WM.WM_MOUSEMOVE:
                    target = MouseMove;
                    if(target != null) {
                        e = new MouseHookEventArgs(lParam);
                    }
                    break;

                #region 機械生成
                #region 通常ボタン

                case (int)WM.WM_LBUTTONDOWN:
                    target = MouseDown;
                    if(target != null) {
                        e = new MouseHookEventArgs(MouseButton.Left, MouseButtonState.Pressed, lParam);
                    }
                    break;


                case (int)WM.WM_RBUTTONDOWN:
                    target = MouseDown;
                    if(target != null) {
                        e = new MouseHookEventArgs(MouseButton.Right, MouseButtonState.Pressed, lParam);
                    }
                    break;


                case (int)WM.WM_MBUTTONDOWN:
                    target = MouseDown;
                    if(target != null) {
                        e = new MouseHookEventArgs(MouseButton.Middle, MouseButtonState.Pressed, lParam);
                    }
                    break;


                case (int)WM.WM_LBUTTONUP:
                    target = MouseUp;
                    if(target != null) {
                        e = new MouseHookEventArgs(MouseButton.Left, MouseButtonState.Released, lParam);
                    }
                    break;


                case (int)WM.WM_RBUTTONUP:
                    target = MouseUp;
                    if(target != null) {
                        e = new MouseHookEventArgs(MouseButton.Right, MouseButtonState.Released, lParam);
                    }
                    break;


                case (int)WM.WM_MBUTTONUP:
                    target = MouseUp;
                    if(target != null) {
                        e = new MouseHookEventArgs(MouseButton.Middle, MouseButtonState.Released, lParam);
                    }
                    break;


                #endregion

                #region Xボタン

                case (int)WM.WM_XBUTTONDOWN: {
                        var msll = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT))!;
                        var xbutton = WindowsUtility.HIWORD(msll.mouseData);
                        if(xbutton == (int)XBUTTON.XBUTTON1) {
                            target = MouseUp;
                            if(target != null) {
                                e = new MouseHookEventArgs(1, MouseButtonState.Pressed, msll);
                            }
                        } else if(xbutton == (int)XBUTTON.XBUTTON2) {
                            target = MouseUp;
                            if(target != null) {
                                e = new MouseHookEventArgs(2, MouseButtonState.Pressed, msll);
                            }
                        }
                    }
                    break;


                case (int)WM.WM_XBUTTONUP: {
                        var msll = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT))!;
                        var xbutton = WindowsUtility.HIWORD(msll.mouseData);
                        if(xbutton == (int)XBUTTON.XBUTTON1) {
                            target = MouseUp;
                            if(target != null) {
                                e = new MouseHookEventArgs(1, MouseButtonState.Released, msll);
                            }
                        } else if(xbutton == (int)XBUTTON.XBUTTON2) {
                            target = MouseUp;
                            if(target != null) {
                                e = new MouseHookEventArgs(2, MouseButtonState.Released, msll);
                            }
                        }
                    }
                    break;


                #endregion

                #endregion

                default:
                    break;
            }

            if(target != null) {
                Stopwatch? stopwatch = null;
                var logging = Logger.IsEnabled(LogLevel.Trace) || Logger.IsEnabled(LogLevel.Warning);
                if(logging) {
                    stopwatch = new Stopwatch();
                    stopwatch.Start();
                }
                try {
                    Debug.Assert(e != null);
                    target.Invoke(this, e);
                    if(e.Handled) {
                        Logger.LogInformation("マウス入力制御: {0}", (WM)wParamValue);
                        return new IntPtr(1);
                    }
                } finally {
                    if(logging) {
                        Debug.Assert(stopwatch != null);
                        stopwatch.Stop();
                        if(TimeSpan.FromMilliseconds(300) < stopwatch.Elapsed) {
                            Logger.LogWarning("マウス {0} フック 実装部 所要時間長期: {1}", (WM)wParamValue, stopwatch.Elapsed);
                        }
                    }
                }
            }
            return CallNextProcedure(code, wParam, lParam);
        }
    }
}

