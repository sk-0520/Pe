using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Library.Common;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    /// <summary>
    /// 環境変数編集処理。
    /// </summary>
    /// <remarks>編集 UI とデータ変換処理の間に位置する。</remarks>
    public class EnvironmentVariableEditor
    {
        public EnvironmentVariableEditor(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

        #endregion

        #region function

        /// <summary>
        /// 文字列から編集用環境変数データに変換する。
        /// </summary>
        /// <param name="text">入力。</param>
        /// <returns>環境変数データ一覧。</returns>
        public IEnumerable<LauncherEnvironmentVariableData> ParseMergeItems(string text)
        {
            return TextUtility.ReadLines(text)
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i => i.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToArray())
                .Where(i => i.Length == 2)
                .Select(i => new LauncherEnvironmentVariableData(i[0], i[1]))
            ;
        }

        /// <summary>
        /// 文字列から削除用環境変数データに変換する。
        /// </summary>
        /// <param name="text">入力。</param>
        /// <returns>環境変数データ一覧。</returns>
        public IEnumerable<string> ParseRemoveItems(string text)
        {
            return TextUtility.ReadLines(text)
                 .Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i => i.Trim())
            ;
        }

        /// <summary>
        /// 編集用環境変数データと削除用環境変数データを結合。
        /// </summary>
        /// <param name="mergeItems">編集用環境変数データ一覧。</param>
        /// <param name="removeItems">削除用環境変数データ一覧。</param>
        /// <returns>環境変数データ一覧。</returns>
        public IReadOnlyList<LauncherEnvironmentVariableData> Join(IEnumerable<LauncherEnvironmentVariableData> mergeItems, IEnumerable<string> removeItems)
        {
            var envVarItems = mergeItems.ToList();
            foreach(var item in removeItems) {
                var index = envVarItems.FindIndex(i => i.Name == item);
                if(index != -1) {
                    envVarItems.RemoveAt(index);
                }

                envVarItems.Add(new LauncherEnvironmentVariableData(item));
            }

            return envVarItems;
        }

        /// <summary>
        /// 編集用環境変数データから文字列に変換する。
        /// </summary>
        /// <param name="items">環境変数データ一覧。</param>
        /// <returns></returns>
        public string ConvertMergeText(IEnumerable<LauncherEnvironmentVariableData> items)
        {
            var mergeItems = items
                .Where(i => !i.IsRemove)
                .Select(i => $"{i.Name}={i.Value}")
            ;

            return string.Join(Environment.NewLine, mergeItems);
        }

        /// <summary>
        /// 削除用環境変数データから文字列に変換する。
        /// </summary>
        /// <param name="items">環境変数データ一覧。</param>
        /// <returns></returns>
        public string ConvertRemoveText(IEnumerable<LauncherEnvironmentVariableData> items)
        {
            var removeItems = items
                .Where(i => i.IsRemove)
                .Select(i => i.Name)
            ;

            return string.Join(Environment.NewLine, removeItems);
        }

        public IEnumerable<string> ValidateMergeDocument(TextDocument textDocument)
        {
            yield break;
        }

        public IEnumerable<string> ValidateRemoveDocument(TextDocument textDocument)
        {
            yield break;
        }

        public void SetValidateCommon(TextDocument textDocument, Func<TextDocument, IEnumerable<string>> func, Action<IEnumerable<string>> addErrors, ICollection<string> collection)
        {
            collection.Clear();

            var errors = func(textDocument!).ToArray();
            if(errors.Length != 0) {
                addErrors(errors);
                foreach(var err in errors) {
                    collection.Add(err);
                }
            }
        }

        #endregion
    }
}
