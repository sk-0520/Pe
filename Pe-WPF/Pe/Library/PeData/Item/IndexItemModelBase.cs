﻿namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	[Serializable]
	public class IndexItemModelBase<TValue>: ItemModelBase
		where TValue: ITId<string>
	{
		public IndexItemModelBase()
			: base()
		{
			Items = new TIdCollection<string, TValue>();
		}


		TIdCollection<string, TValue> Items { get; set; }
	}
}
