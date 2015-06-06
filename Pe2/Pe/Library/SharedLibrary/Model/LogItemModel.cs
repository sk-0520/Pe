﻿namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;

	/// <summary>
	/// ログとして出力するデータ。
	/// </summary>
	public class LogItemModel: ModelBase
	{
		/// <summary>
		/// 発生日。
		/// </summary>
		public DateTime DateTime { get; set; }
		/// <summary>
		/// ログ種別。
		/// </summary>
		public LogKind LogKind { get; set; }
		/// <summary>
		/// ログメッセージ。
		/// </summary>
		public string Message { get; set; }
		/// <summary>
		/// 詳細。
		/// <para>nullで何もなし。</para>
		/// <para>判定にはHasDetailを使用。</para>
		/// </summary>
		public object Detail { get; set; }
		/// <summary>
		/// 詳細はあるか。
		/// </summary>
		public bool HasDetail { get { return Detail == null; } }
		/// <summary>
		/// スタックトレース情報。
		/// </summary>
		public StackTrace StackTrace { get; set; }
		/// <summary>
		/// 呼び出しファイルパス。
		/// </summary>
		public string CallerFile { get; set; }
		/// <summary>
		/// 呼び出し行番号。
		/// </summary>
		public int CallerLine { get; set; }
		/// <summary>
		/// 呼び出しメンバ。
		/// </summary>
		public string CallerMember { get; set; }
		/// <summary>
		/// 呼び出しアセンブリ。
		/// </summary>
		public Assembly CallerAssembly { get; set; }
		/// <summary>
		/// 呼び出しスレッド。
		/// </summary>
		public Thread CallerThread { get; set; }
	}
}
