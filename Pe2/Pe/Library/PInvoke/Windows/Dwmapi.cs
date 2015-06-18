﻿using System;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Library.PInvoke.Windows
{
	[StructLayout(LayoutKind.Sequential)]
	public struct MARGINS
	{
		public int leftWidth;
		public int rightWidth;
		public int topHeight;
		public int bottomHeight;
	}
	
	[Flags]
	public enum DWM_BB
	{
		DWM_BB_ENABLE = 1,
		DWM_BB_BLURREGION = 2,
		DWM_BB_TRANSITIONONMAXIMIZED = 4
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public struct DWM_BLURBEHIND
	{
		public DWM_BB dwFlags;
		public bool fEnable;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security","CA2111:PointersShouldNotBeVisible")]
		public IntPtr hRgnBlur;
		public int fTransitionOnMaximized;

		public DWM_BLURBEHIND(bool enabled)
		{
			fEnable = enabled;
			hRgnBlur = IntPtr.Zero;
			fTransitionOnMaximized = 0;
			dwFlags = DWM_BB.DWM_BB_ENABLE;
		}

		//public System.Drawing.Region Region
		//{
		//	get { return System.Drawing.Region.FromHrgn(hRgnBlur); }
		//}

		public bool TransitionOnMaximized
		{
			get { return fTransitionOnMaximized > 0; }
			set
			{
				fTransitionOnMaximized = value ? 1 : 0;
				dwFlags |= DWM_BB.DWM_BB_TRANSITIONONMAXIMIZED;
			}
		}

		//public void SetRegion(System.Drawing.Graphics graphics, System.Drawing.Region region)
		//{
		//	hRgnBlur = region.GetHrgn(graphics);
		//	dwFlags |= DWM_BB.DWM_BB_BLURREGION;
		//}
	}

	public enum DWMWINDOWATTRIBUTE
	{
		DWMWA_NCRENDERING_ENABLED = 1,
		DWMWA_NCRENDERING_POLICY,
		DWMWA_TRANSITIONS_FORCEDISABLED,
		DWMWA_ALLOW_NCPAINT,
		DWMWA_CAPTION_BUTTON_BOUNDS,
		DWMWA_NONCLIENT_RTL_LAYOUT,
		DWMWA_FORCE_ICONIC_REPRESENTATION,
		DWMWA_FLIP3D_POLICY,
		DWMWA_EXTENDED_FRAME_BOUNDS,
		DWMWA_HAS_ICONIC_BITMAP,
		DWMWA_DISALLOW_PEEK,
		DWMWA_EXCLUDED_FROM_PEEK,
		DWMWA_CLOAK,
		DWMWA_CLOAKED,
		DWMWA_FREEZE_REPRESENTATION,
		DWMWA_LAST
	}

	public enum DWMNCRENDERINGPOLICY
	{
		DWMNCRP_USEWINDOWSTYLE,
		DWMNCRP_DISABLED,
		DWMNCRP_ENABLED,
		DWMNCRP_LAST
	}

	partial class NativeMethods
	{
		/// <summary>
		/// http://www.pinvoke.net/default.aspx/dwmNativeMethods.dwmiscompositionenabled
		/// </summary>
		/// <param name="enabled"></param>
		/// <returns></returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("dwmapi.dll")]
		public static extern int DwmIsCompositionEnabled(out bool enabled);

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("dwmapi.dll", PreserveSig = true)]
		public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("dwmapi.dll")]
		public static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);
		
		/// <summary>
		/// http://www.pinvoke.net/default.aspx/dwmNativeMethods.dwmgetcolorizationcolor
		/// </summary>
		/// <param name="ColorizationColor"></param>
		/// <param name="ColorizationOpaqueBlend"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmGetColorizationColor(out uint ColorizationColor, [MarshalAs(UnmanagedType.Bool)]out bool ColorizationOpaqueBlend);

		[DllImport("dwmapi.dll")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern bool DwmDefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref IntPtr plResult);


		[DllImport("dwmapi.dll", PreserveSig = true)]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attr, ref DWMNCRENDERINGPOLICY attrValue, int attrSize);

	}
}
