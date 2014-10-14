﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 03/07/2014
 * 時刻: 00:34
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using PeUtility;

namespace PeMain.Logic
{
	/// <summary>
	/// Description of Information.
	/// </summary>
	public class AppInformation: Information
	{
		public override FileVersionInfo GetVersionInfo
		{
			get { return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location); }
		}
	}
}