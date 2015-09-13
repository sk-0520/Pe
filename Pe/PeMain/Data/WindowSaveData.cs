﻿namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class WindowSaveData: ItemModelBase
	{
		public WindowSaveData()
		{
			TimerItems = new FixedSizeCollectionModel<WindowItemCollectionModel>();
			SystemItems = new FixedSizeCollectionModel<WindowItemCollectionModel>();
		}

		#region property

		public WindowItemCollectionModel TemporaryItem { get; set; }

		/// <summary>
		/// タイマー保存。
		/// </summary>
		public FixedSizeCollectionModel<WindowItemCollectionModel> TimerItems { get; private set; }
		/// <summary>
		/// 環境変更時の保存。
		/// </summary>
		public FixedSizeCollectionModel<WindowItemCollectionModel> SystemItems { get; private set; }

		#endregion
	}
}
