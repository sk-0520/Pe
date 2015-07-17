﻿namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;

	public class ClipboardIndexItemModel: IndexItemModelBase
	{
		public ClipboardIndexItemModel()
			: base()
		{ }

		public ClipboardType Type { get; set; }
	}
}
