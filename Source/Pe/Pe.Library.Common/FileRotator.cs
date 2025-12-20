using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ContentTypeTextNet.Pe.Library.Common.Linq;
using ContentTypeTextNet.Pe.Library.Common.Service;

namespace ContentTypeTextNet.Pe.Library.Common
{
    /// <summary>
    /// ファイルのローテートを行う。
    /// </summary>
    public class FileRotator
    {
        public FileRotator()
            : this(FileSystemProvider.Default)
        { }

        public FileRotator(FileSystemProvider fileSystemProvider)
        {
            FileSystemProvider = fileSystemProvider;
        }

        #region property

        private FileSystemProvider FileSystemProvider { get; }

        #endregion

        #region function

        /// <summary>
        /// ローテート処理。
        /// </summary>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="regex"><paramref name="parentDirectory"/>直下の対象ファイル。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="order">ソート。</param>
        /// <param name="exceptionCatcher">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        private int ExecuteCore(DirectoryInfo parentDirectory, Regex regex, int leaveCount, Order order, Func<Exception, bool> exceptionCatcher)
        {
            parentDirectory.Refresh();
            if(!parentDirectory.Exists) {
                return -1;
            }

            var targetFiles = parentDirectory
                .EnumerateFiles()
                .Where(i => regex.IsMatch(i.Name))
                .OrderBy(order, i => i.Name)
                .Skip(leaveCount)
                .ToArray()
            ;

            var removedCount = 0;
            for(var i = 0; i < targetFiles.Length; i++) {
                try {
                    FileSystemProvider.Delete(targetFiles[i]);
                    removedCount += 1;
                } catch(Exception ex) {
                    if(!exceptionCatcher(ex)) {
                        return removedCount;
                    }
                }
            }

            return removedCount;
        }

        /// <summary>
        /// 正規表現に該当したファイルのローテート処理。
        /// </summary>
        /// <remarks>
        /// <para>降順で列挙する。</para>
        /// </remarks>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="regex"><paramref name="parentDirectory"/>直下の対象ファイル。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="exceptionCatcher">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        public int ExecuteRegex(DirectoryInfo parentDirectory, Regex regex, int leaveCount, Func<Exception, bool> exceptionCatcher)
        {
            return ExecuteCore(parentDirectory, regex, leaveCount, Order.Descending, exceptionCatcher);
        }
        /// <summary>
        /// 正規表現に該当したファイルのローテート処理。
        /// </summary>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="regex"><paramref name="parentDirectory"/>直下の対象ファイル。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="leaveOrder">ソート。</param>
        /// <param name="exceptionCatcher">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        public int ExecuteRegex(DirectoryInfo parentDirectory, Regex regex, int leaveCount, Order leaveOrder, Func<Exception, bool> exceptionCatcher)
        {
            return ExecuteCore(parentDirectory, regex, leaveCount, leaveOrder, exceptionCatcher);
        }

        /// <summary>
        /// ワイルドカードに該当したファイルのローテート処理。
        /// </summary>
        /// <remarks>
        /// <para>降順で列挙する。</para>
        /// </remarks>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="wildCard"><paramref name="parentDirectory"/>直下の対象ファイル。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="exceptionCatcher">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        public int ExecuteWildcard(DirectoryInfo parentDirectory, string wildCard, int leaveCount, Func<Exception, bool> exceptionCatcher)
        {
            return ExecuteWildcard(parentDirectory, wildCard, leaveCount, Order.Descending, exceptionCatcher);
        }
        /// <summary>
        /// ワイルドカードに該当したファイルのローテート処理。
        /// </summary>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="wildCard"><paramref name="parentDirectory"/>直下の対象ファイル。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="leaveOrder">ソート。</param>
        /// <param name="exceptionCatcher">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        public int ExecuteWildcard(DirectoryInfo parentDirectory, string wildCard, int leaveCount, Order leaveOrder, Func<Exception, bool> exceptionCatcher)
        {
            var wildcardPattern = "^" + Regex.Escape(wildCard).Replace("\\?", ".").Replace("\\*", ".*") + "$";
            var wildcardRegex = new Regex(wildcardPattern, RegexOptions.IgnoreCase, Timeout.InfiniteTimeSpan);

            return ExecuteCore(parentDirectory, wildcardRegex, leaveCount, leaveOrder, exceptionCatcher);
        }

        /// <summary>
        /// 拡張子に該当したファイルのローテート処理。
        /// </summary>
        /// <remarks>
        /// <para>降順で列挙する。</para>
        /// </remarks>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="extensions"><paramref name="parentDirectory"/>直下の拡張子。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="exceptionCatcher">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        public int ExecuteExtensions(DirectoryInfo parentDirectory, IEnumerable<string> extensions, int leaveCount, Func<Exception, bool> exceptionCatcher)
        {
            return ExecuteExtensions(parentDirectory, extensions, leaveCount, Order.Descending, exceptionCatcher);
        }
        /// <summary>
        /// 拡張子に該当したファイルのローテート処理。
        /// </summary>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="extensions"><paramref name="parentDirectory"/>直下の拡張子。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="leaveOrder">ソート。</param>
        /// <param name="exceptionCatcher">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        public int ExecuteExtensions(DirectoryInfo parentDirectory, IEnumerable<string> extensions, int leaveCount, Order leaveOrder, Func<Exception, bool> exceptionCatcher)
        {
            var extensionPatterns = extensions
                .Select(i => Regex.Escape(i))
                .Select(i => "(" + i + ")")
                .JoinString("|")
            ;
            var extensionPattern = "(" + extensionPatterns + ")";
            var extensionRegex = new Regex(extensionPattern, RegexOptions.IgnoreCase, Timeout.InfiniteTimeSpan);

            return ExecuteCore(parentDirectory, extensionRegex, leaveCount, leaveOrder, exceptionCatcher);
        }

        #endregion
    }
}
