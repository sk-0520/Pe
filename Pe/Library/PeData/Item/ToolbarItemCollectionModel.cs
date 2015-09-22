﻿namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	/// <summary>
	/// ツールバーアイテムの統括データ。
	/// </summary>
	[Serializable]
	public class ToolbarItemCollectionModel: TIdCollectionModel<string,ToolbarItemModel>, ISettingModel
	{
		public ToolbarItemCollectionModel()
			: base()
		{ }
	}
}
