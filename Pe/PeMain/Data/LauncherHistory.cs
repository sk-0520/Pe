﻿namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// 実行履歴
	/// </summary>
	[Serializable]
	public class LauncherHistory: Item, ICloneable
	{
		public LauncherHistory()
		{
			WorkDirs = new List<string>();
			Options = new List<string>();
			DateHistory = new DateHistory();
		}
		/// <summary>
		/// 実行回数
		/// </summary>
		public uint ExecuteCount { get; set; }
		/// <summary>
		/// 作業ディレクトリ
		/// </summary>
		public List<string> WorkDirs { get; set; }
		/// <summary>
		/// オプション
		/// </summary>
		public List<string> Options { get; set; }
		/// <summary>
		/// アイテム登録及び更新日時
		/// </summary>
		public DateHistory DateHistory { get; set; }

		/// <summary>
		/// 複製
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			var result = new LauncherHistory();
			result.ExecuteCount = ExecuteCount;
			result.WorkDirs.AddRange(WorkDirs);
			result.Options.AddRange(Options);
			result.DateHistory = (DateHistory)DateHistory.Clone();

			return result;
		}
	}

}