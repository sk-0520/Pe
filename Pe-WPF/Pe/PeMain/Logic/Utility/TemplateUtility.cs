﻿namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Remoting;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public static class TemplateUtility
	{
		static IReadOnlyDictionary<string, string> GetTemplateMap(INonProcess nonProcess)
		{
			var map = new Dictionary<string, string>();

			var clipboardItem = ClipboardUtility.GetClipboardData(ClipboardType.Text | ClipboardType.File, IntPtr.Zero, nonProcess);
			if(clipboardItem.Type != ClipboardType.None) {
				var clipboardText = clipboardItem.Body.Text;
				// そのまんま
				map[TemplateReplaceKey.textClipboard] = clipboardText;

				var lines = clipboardText.SplitLines().ToList();
				// 改行を削除
				map[TemplateReplaceKey.textClipboardNobreak] = string.Join(string.Empty, lines);
				// 先頭行
				map[TemplateReplaceKey.textClipboardHead] = lines.FirstOrDefault();
				// 最終行
				map[TemplateReplaceKey.textClipboardTail] = lines.LastOrDefault();
			}

			return map;
		}

		/// <summary>
		/// テンプレートアイテムからテンプレートプロセッサ作成。
		/// </summary>
		/// <param name="item">テンプレートアイテム。テンプレートプロセッサが設定される。</param>
		/// <param name="language">使用言語。</param>
		/// <returns>作成されたテンプレートプロセッサ。</returns>
		public static ProgramTemplateProcessor MakeTemplateProcessor(string source, ProgramTemplateProcessor processor, INonProcess appNonProcess)
		{
			if(processor != null) {
				//processor.Language = language;
				try {
					processor.CultureCode = appNonProcess.Language.CultureCode;
					processor.TemplateSource = source;
				} catch(RemotingException ex) {
					appNonProcess.Logger.Error(ex);
				}

				return processor;
			}

			var result = new ProgramTemplateProcessor() {
				CultureCode = appNonProcess.Language.CultureCode,
				TemplateSource = source,
			};

			return result;
		}

		static string ToPlainTextProgrammable(TemplateIndexItemModel indexModel, TemplateBodyItemModel bodyModel, ProgramTemplateProcessor processor, DateTime dateTime, INonProcess appNonProcess)
		{
			if(processor.Compiled) {
				return processor.TransformText();
			}
			processor.AllProcess();
			if(processor.Error != null || processor.GeneratedErrorList.Any() || processor.CompileErrorList.Any()) {
				// エラーあり
				if(processor.Error != null) {
					return processor.Error.ToString() + Environment.NewLine + string.Join(Environment.NewLine, processor.GeneratedErrorList.Concat(processor.CompileErrorList).Select(e => e.ToString()));
				} else {
					return string.Join(Environment.NewLine, processor.GeneratedErrorList.Concat(processor.CompileErrorList).Select(e => string.Format("[{0},{1}] {2}: {3}", e.Line - processor.FirstLineNumber, e.Column, e.ErrorNumber, e.ErrorText)));
				}
			}
			return processor.TransformText();
		}

		static string ToPlainTextReplace(TemplateIndexItemModel indexModel, TemplateBodyItemModel bodyModel, DateTime dateTime, INonProcess appNonProcess)
		{
			var src = bodyModel.Source ?? string.Empty;
			if(string.IsNullOrWhiteSpace(src)) {
				return src;
			}

			var templateMap = GetTemplateMap(appNonProcess);
			var appMap = AppLanguageManager.GetAppMap(DateTime.Now, appNonProcess.Language);

			var map = templateMap.Concat(appMap).ToDictionary(p => p.Key, p => p.Value);

			var result = src.ReplaceRangeFromDictionary("@[", "]", map);

			return result;
		}

		public static string ToPlainText(TemplateIndexItemModel indexModel, TemplateBodyItemModel bodyModel, ProgramTemplateProcessor processor, DateTime dateTime, INonProcess appNonProcess)
		{
			if(!indexModel.IsReplace) {
				return bodyModel.Source ?? string.Empty;
			}
			if(indexModel.IsProgrammableReplace) {
				CheckUtility.EnforceNotNull(processor);
				return ToPlainTextProgrammable(indexModel, bodyModel, processor, dateTime, appNonProcess);
			} else {
				return ToPlainTextReplace(indexModel, bodyModel, dateTime, appNonProcess);
			}
		}
	}
}
