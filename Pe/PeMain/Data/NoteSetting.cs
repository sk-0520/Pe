﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 12:59
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;

namespace PeMain.Data
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class NoteSetting: Item
	{
		public NoteSetting()
		{
			CreateHotKey = new HotKeySetting();
		}
		
		public HotKeySetting CreateHotKey { get; set; }
	}
}
