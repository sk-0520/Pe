using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    /// <summary>
    /// Windows での低レベル処理用ユーティリティ。
    /// </summary>
    public static class WindowsUtility
    {
        #region define

        public const int classNameLength = 260;

        #endregion

        #region define-function

        public static int GetIntUnchecked(IntPtr value)
        {
            return IntPtr.Size == 8 ? unchecked((int)value.ToInt64()) : value.ToInt32();
        }
        public static int LOWORD(IntPtr value)
        {
            return unchecked((short)GetIntUnchecked(value));
        }
        public static int LOWORD(int value)
        {
            return unchecked((short)value);
        }
        public static int HIWORD(IntPtr value)
        {
            return unchecked((short)(((uint)GetIntUnchecked(value)) >> 16));
        }
        public static int HIWORD(int value)
        {
            return unchecked((short)(value >> 16));
        }

        #endregion

        #region window message convert

        /// <summary>
        /// <paramref name="wParam"/> を <see cref="SC"/> に変換。
        /// </summary>
        /// <param name="wParam"></param>
        /// <returns></returns>
        public static SC ConvertSCFromWParam(IntPtr wParam)
        {
            return (SC)(wParam.ToInt32() & 0xfff0);
        }

        /// <summary>
        /// <paramref name="wParam"/> を <see cref="WMSZ"/> に変換。
        /// </summary>
        /// <param name="wParam"></param>
        /// <returns></returns>
        public static WMSZ ConvertWMSZFromWParam(IntPtr wParam)
        {
            return (WMSZ)(wParam.ToInt32());
        }

        /// <summary>
        /// <paramref name="lParam"/> を <see cref="bool"/> に変換。
        /// </summary>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static bool ConvertBoolFromLParam(IntPtr lParam)
        {
            return Convert.ToBoolean(lParam.ToInt32());
        }

        /// <summary>
        /// <paramref name="lParam"/> を <see cref="RECT"/> に変換。
        /// </summary>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static RECT ConvertRECTFromLParam(IntPtr lParam)
        {
            return (RECT)Marshal.PtrToStructure(lParam, typeof(RECT))!;
        }

        /// <summary>
        /// <paramref name="lParam"/> を <see cref="POINT"/> に変換。
        /// </summary>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static POINT ConvertPOINTFromLParam(IntPtr lParam)
        {
            return new POINT(LOWORD(lParam), HIWORD(lParam));
        }

        /// <summary>
        /// <paramref name="wParam"/> を <see cref="HT"/> に変換。
        /// </summary>
        /// <param name="wParam"></param>
        /// <returns></returns>
        public static HT ConvertHTFromWParam(IntPtr wParam)
        {
            return (HT)wParam;
        }

        /// <summary>
        /// <paramref name="lParam"/> を <see cref="HT"/> に変換。
        /// </summary>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static HT ConvertHTFromLParam(IntPtr lParam)
        {
            return (HT)LOWORD(lParam);
        }

        /// <summary>
        /// <see cref="ModifierKeys" /> を <see cref="WMSZ"/> に変換。
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public static MOD ConvertMODFromModifierKeys(ModifierKeys mod)
        {
            return (MOD)mod;
        }

        /// <summary>
        /// <see cref="MOD" /> を <see cref="ModifierKeys"/> に変換。
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public static ModifierKeys ConvertModifierKeysFromMOD(MOD mod)
        {
            return (ModifierKeys)mod;
        }

        /// <summary>
        /// <paramref name="lParam"/> を <see cref="ModifierKeys"/> に変換。
        /// </summary>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static ModifierKeys ConvertModifierKeysFromLParam(IntPtr lParam)
        {
            return ConvertModifierKeysFromMOD((MOD)unchecked((short)(long)lParam));
        }

        /// <summary>
        /// <paramref name="lParam"/> を <see cref="Key"/> に変換。
        /// </summary>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static Key ConvertKeyFromLParam(IntPtr lParam)
        {
            return KeyInterop.KeyFromVirtualKey(unchecked((ushort)((long)lParam >> 16)));
        }

        #endregion

        #region function

        public static IntPtr GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if(Environment.Is64BitProcess) {
                return NativeMethods.GetWindowLongPtr(hWnd, nIndex);
            } else {
                return new IntPtr(NativeMethods.GetWindowLong(hWnd, nIndex));
            }
        }

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            var result = IntPtr.Zero;

            NativeMethods.SetLastError(0);

            if(Environment.Is64BitProcess) {
                result = NativeMethods.SetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            } else {
                Int32 tempResult = NativeMethods.SetWindowLong(hWnd, nIndex, dwNewLong.ToInt32());
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }

            if((result == IntPtr.Zero) && (error != 0)) {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        public static void Reload(IntPtr handle)
        {
            NativeMethods.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, SWP.SWP_FRAMECHANGED | SWP.SWP_NOSIZE | SWP.SWP_NOMOVE | SWP.SWP_NOZORDER | SWP.SWP_NOOWNERZORDER | SWP.SWP_NOACTIVATE);
        }

        /// <summary>
        /// アクティブ状態を変更せずに最前面に移動させる。
        /// </summary>
        /// <param name="hWnd"></param>
        public static void ShowNoActiveForeground(IntPtr hWnd)
        {
            NativeMethods.SetWindowPos(
                hWnd,
                new IntPtr((int)HWND.HWND_TOP),
                0, 0,
                0, 0,
                SWP.SWP_NOACTIVATE | SWP.SWP_NOMOVE | SWP.SWP_NOSIZE | SWP.SWP_SHOWWINDOW
            );
        }

        public static void ShowActive(IntPtr hWnd)
        {
            NativeMethods.SetWindowPos(
                hWnd,
                new IntPtr((int)HWND.HWND_TOP),
                0, 0,
                0, 0,
                SWP.SWP_NOMOVE | SWP.SWP_NOSIZE | SWP.SWP_SHOWWINDOW
            );
            NativeMethods.SetForegroundWindow(hWnd);
        }

        public static void ShowActiveForeground(IntPtr hWnd)
        {
            var foregroundId = NativeMethods.GetWindowThreadProcessId(NativeMethods.GetForegroundWindow(), out var processId);
            var targetId = NativeMethods.GetWindowThreadProcessId(hWnd, out _);
            NativeMethods.AttachThreadInput(targetId, foregroundId, true);
            NativeMethods.SetForegroundWindow(hWnd);
            ShowActive(hWnd);
        }

        public static void MoveZoderBttom(IntPtr hWnd)
        {
            NativeMethods.SetWindowPos(
                hWnd,
                ToIntPtr(HWND.HWND_BOTTOM),
                0, 0,
                0, 0,
                SWP.SWP_NOACTIVATE | SWP.SWP_NOMOVE | SWP.SWP_NOSIZE
            );
        }

        public static IntPtr ToIntPtr(HWND hWnd)
        {
            return new IntPtr((int)hWnd);
        }

        public static string GetWindowClassName(IntPtr hWnd, int windowClassNameLength = classNameLength)
        {
            var buffer = new StringBuilder(windowClassNameLength);
            NativeMethods.GetClassName(hWnd, buffer, buffer.Capacity);
            var windowClassName = buffer.ToString();
            return windowClassName;
        }

        #endregion
    }
}
