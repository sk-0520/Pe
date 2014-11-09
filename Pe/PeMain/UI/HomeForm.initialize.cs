﻿namespace PeMain.UI
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using PeMain.Logic;

	partial class HomeForm
	{
		void InitializeUI()
		{
			UIUtility.ShowCenterInPrimaryScreen(this);
		}
		
		void Initialize()
		{
			InitializeUI();
		}
	}
}
