﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/10/31
 * 時刻: 0:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using NUnit.Framework;

namespace PeSetting
{
	[TestFixture]
	public class Test_Item
	{
		[Test]
		public void IsSafeName()
		{
			Assert.IsTrue(Item.IsSafeName("4"), "4");
			Assert.IsTrue(Item.IsSafeName("1"), "1");
			Assert.IsTrue(Item.IsSafeName("2"), "2");
			Assert.IsTrue(Item.IsSafeName("3"), "3");
		}
	}
}
