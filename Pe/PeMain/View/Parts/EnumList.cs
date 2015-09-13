﻿namespace ContentTypeTextNet.Pe.PeMain.View.Parts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Pe.Library.PeData.Define;

	public static class EnumList
	{
		public static IEnumerable<DockType> DockTypeList
		{
			get
			{
				var result = new[] {
					DockType.None,
					DockType.Left,
					DockType.Top,
					DockType.Right,
					DockType.Bottom,
				};

				return result;
			}
		}

		public static IEnumerable<IconScale> PlainIconScaleList
		{
			get
			{
				var result = new[] {
					IconScale.Small,
					IconScale.Normal,
					IconScale.Big,
				};

				return result;
			}
		}

		public static IEnumerable<TemplateReplaceMode> TemplateReplaceModeList
		{
			get
			{
				var result = new[] {
					TemplateReplaceMode.None,
					TemplateReplaceMode.Text,
					TemplateReplaceMode.Program,
				};

				return result;
			}
		}
	}
}
