﻿namespace ContentTypeTextNet.Pe.PeMain.Logic.Property
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	public static class TopMostProperty
	{
		public static bool GetTopMost(ITopMost model)
		{
			return model.IsTopmost;
		}

		public static bool SetTopMost(ITopMost model, bool value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if(model.IsTopmost != value) {
				model.IsTopmost = value;
				onPropertyChanged(propertyName);

				return true;
			}

			return false;
		}
	}
}
