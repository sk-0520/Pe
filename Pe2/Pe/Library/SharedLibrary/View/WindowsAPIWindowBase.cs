﻿namespace ContentTypeTextNet.Library.SharedLibrary.View
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Interop;

	public abstract class WindowsAPIWindowBase: OnLoadedWindowBase
	{
		public WindowsAPIWindowBase()
			:base()
		{ }

		#region property

		protected WindowInteropHelper WindowInteropHelper { get; private set; }

		protected IntPtr Handle { get { return WindowInteropHelper.Handle; } }

		#endregion

		#region OnLoadedWindowBase

		protected override void OnSourceInitialized(EventArgs e)
		{
			WindowInteropHelper = new WindowInteropHelper(this);
			
			base.OnSourceInitialized(e);
		}

		#endregion
	}
}
