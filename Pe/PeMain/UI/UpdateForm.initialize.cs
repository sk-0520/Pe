﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/28
 * 時刻: 21:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.Application.Logic;

namespace ContentTypeTextNet.Pe.Application.UI
{
	partial class UpdateForm
	{
		void Initialize()
		{
			PointingUtility.AttachmentDefaultButton(this);
			WebBrowserUtility.AttachmentNewWindow(this.webUpdate);
		}
	}
}
