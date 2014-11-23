﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/08/02
 * 時刻: 17:59
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;
using PeMain.Data;
using PeMain.IF;

namespace PeMain.Logic
{
	/// <summary>
	/// Description of FileLogger.
	/// </summary>
	public class FileLogger: ILogger, IDisposable
	{
		private StreamWriter _stream;
		
		public FileLogger() { }
		
		public FileLogger(string path)
		{
			this._stream = new StreamWriter(new FileStream(path, FileMode.CreateNew));
			Puts(LogType.None, "FileLogger", "Start");
		}

		protected virtual void Dispose(bool disposing)
		{
			Puts(LogType.None, "FileLogger", "Dispose");

			if(this._stream != null) {
				this._stream.Dispose();
			}
		}
		
		public void Dispose()
		{
			Dispose(true);
		}
		
		public void Puts(LogType logType, string title, object detail, int frame = 2)
		{
			var logItem = new LogItem(logType, title, detail, frame);
			WiteItem(logItem);
		}
		
		public void WiteItem(LogItem logItem)
		{
			if(this._stream != null) {
				this._stream.WriteLine(logItem.ToString());
				this._stream.Flush();
			}
		}
	}
}
