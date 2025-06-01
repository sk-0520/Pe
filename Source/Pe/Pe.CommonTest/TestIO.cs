using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace ContentTypeTextNet.Pe.CommonTest
{
    /// <summary>
    /// テストディレクトリ基底。
    /// </summary>
    public abstract class TestDirectoryBase
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="directory"><see cref="Directory"/></param>
        protected TestDirectoryBase(DirectoryInfo directory)
        {
            Directory = directory;
        }

        #region property

        /// <summary>
        /// 対象ディレクトリ。
        /// </summary>
        public DirectoryInfo Directory { get; }

        #endregion


        #region function

        /// <summary>
        /// パス結合。
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        private static string CombinePathCore(string[] tree)
        {
            return Path.Combine(tree);
        }

        /// <summary>
        /// 現在ディレクトリにパスを結合。
        /// </summary>
        /// <param name="sub">サブパス。</param>
        /// <param name="children">さらに下のパス。</param>
        /// <returns>結合されたパス文字列。</returns>
        public string CombinePath(string sub, params string[] children)
        {
            return CombinePathCore([Directory.FullName, sub, .. children]);
        }

        /// <summary>
        /// 存在するファイルの取得。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <returns></returns>
        /// <exception cref="TestIOException"></exception>
        public FileInfo GetFile(string name)
        {
            var path = CombinePath(name);
            var file = new FileInfo(path);
            file.Refresh();
            if(!file.Exists) {
                throw new TestIOException($"File not found: {path}.");
            }
            return file;
        }

        /// <summary>
        /// 存在するディレクトリの取得。
        /// </summary>
        /// <param name="name">ディレクトリ名。</param>
        /// <returns></returns>
        /// <exception cref="TestIOException"></exception>
        public DirectoryInfo GetDirectory(string name)
        {
            var path = CombinePath(name);
            var dir = new DirectoryInfo(path);
            dir.Refresh();
            if(!dir.Exists) {
                throw new TestIOException($"Directory not found: {path}.");
            }
            return dir;
        }

        /// <summary>
        /// ファイルを開く。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <returns></returns>
        public FileStream Open(string name)
        {
            var path = CombinePath(name);
            return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        /// <summary>
        /// テキストファイルを開く。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <param name="encoding">エンコーディング。</param>
        /// <returns></returns>
        public StreamReader Read(string name, Encoding encoding)
        {
            var stream = Open(name);
            return new StreamReader(stream, encoding, true);
        }
        /// <inheritdoc cref="Read(string, Encoding)"/>
        public StreamReader Read(string name) => Read(name, Encoding.UTF8);

        #endregion
    }

    /// <summary>
    /// コピーされたテスト用データディレクトリ。
    /// </summary>
    public class TestDataDirectory: TestDirectoryBase
    {
        public TestDataDirectory(DirectoryInfo directory)
            : base(directory)
        { }
    }

    /// <summary>
    /// テスト用に生成された作業ディレクトリ。
    /// </summary>
    public class TestWorkDirectory: TestDirectoryBase
    {
        public TestWorkDirectory(DirectoryInfo directory)
            : base(directory)
        { }

        #region function

        /// <summary>
        /// サブディレクトリの作成。
        /// </summary>
        /// <param name="subDirectoryName">サブディレクトリ名。</param>
        /// <returns></returns>
        public TestWorkDirectory CreateDirectory(string subDirectoryName)
        {
            var dirPath = CombinePath(subDirectoryName);
            var dir = System.IO.Directory.CreateDirectory(dirPath);
            return new TestWorkDirectory(dir);
        }

        /// <summary>
        /// ファイルパスの生成。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <returns>実ファイルの生成されていない <see cref="FileInfo"/> </returns>
        public FileInfo NewFile(string name)
        {
            var filePath = CombinePath(name);
            return new FileInfo(filePath);
        }

        /// <summary>
        /// 空ファイルの作成。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <returns></returns>
        public FileInfo CreateEmptyFile(string name)
        {
            var file = NewFile(name);
            file.Create().Dispose();
            return file;
        }

        /// <summary>
        /// テキストファイルの生成。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <param name="content">内容。</param>
        /// <param name="encoding">エンコーディング。</param>
        /// <returns></returns>
        public FileInfo CreateTextFile(string name, string content, Encoding encoding)
        {
            var filePath = CombinePath(name);

            using(var stream = File.Create(filePath)) {
                using var writer = new StreamWriter(stream, encoding);
                writer.Write(content);
            }

            return new FileInfo(filePath);
        }
        /// <inheritdoc cref="CreateTextFile(string, string, Encoding)"/>/>
        public FileInfo CreateTextFile(string name, string content) => CreateTextFile(name, content, Encoding.UTF8);

        #endregion
    }

    /// <summary>
    /// 再初期化抑制用。
    /// </summary>
    /// <remarks><para>今のところメソッドだけなのでそんなに有用ではない。</para></remarks>
    public class TestInitializedStore
    {
        #region property

        private static HashSet<string> WorkDirectory { get; set; } = [];

        #endregion

        #region function

        private static void InitializeDirectory(DirectoryInfo dir)
        {
            dir.Refresh();
            if(dir.Exists) {
                dir.Delete(true);
            }
            dir.Create();
        }

        public void Initialize(TestIO testIO)
        {
            if(WorkDirectory.Contains(testIO.Work.Directory.FullName)) {
                throw new TestIOInitializedException(testIO.Work.Directory.FullName);
            }

            InitializeDirectory(testIO.Work.Directory);

            WorkDirectory.Add(testIO.Work.Directory.FullName);
        }

        #endregion
    }

    /// <summary>
    /// IO系のテスト系インフラ。
    /// </summary>
    public class TestIO
    {
        private TestIO(TestDataDirectory data, TestWorkDirectory work)
        {
            Data = data;
            Work = work;
        }

        #region property

        private static DirectoryInfo? _ProjectDirectory;
        private static DirectoryInfo ProjectDirectory
        {
            get
            {
                return _ProjectDirectory ??= new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new TestException("どうやって到達するのか知らん例外"));
            }
        }

        private static string WorkRootDirectoryName { get; } = "_test_io_";

        private static TestInitializedStore InitializedMethod { get; } = new TestInitializedStore();


        /// <inheritdoc cref="TestDataDirectory"/>
        public TestDataDirectory Data { get; }
        /// <inheritdoc cref="TestWorkDirectory"/>
        public TestWorkDirectory Work { get; }

        #endregion

        #region function

        private static string GetDataClassDirectoryName(object test, string callerFilePath)
        {
            var callerFilePathSpan = callerFilePath.AsSpan();
            var firstSepIndex = callerFilePathSpan.IndexOf(Path.DirectorySeparatorChar);
            var secondSepIndex = callerFilePathSpan.Slice(firstSepIndex + 1).IndexOf(Path.DirectorySeparatorChar);
            var classPath = callerFilePathSpan.Slice(firstSepIndex + 1 + secondSepIndex + 1);
            var extIndex = classPath.LastIndexOf(".cs");
            var classTree = classPath.Slice(0, extIndex);

            return classTree.ToString();
        }

        private static string GetWorkClassDirectoryName(object test)
        {
            return test.GetType().FullName ?? throw new TestException(test.ToString()!);
        }

        private static string GetDataMethodDirectoryName(string extension, string callerMemberName)
        {
            return callerMemberName + "." + extension;
        }

        private static string GetWorkMethodDirectoryName(string suffix, string callerMemberName, int callerLineNumber)
        {
            var path = callerMemberName + "-L" + callerLineNumber.ToString(CultureInfo.InvariantCulture);
            if(string.IsNullOrWhiteSpace(suffix)) {
                return path;
            }
            return path + "@" + suffix;
        }

        /// <summary>
        /// テストメソッドの初期化。
        /// </summary>
        /// <param name="test">テスト実行中のインスタンス。</param>
        /// <param name="workSuffix">作業ディレクトリのサフィックス。指定した場合 「作業ディレクトリ@<paramref name="workSuffix"/>」となる。パラメータを使用した場合のテストではこれがないとダメ。</param>
        /// <param name="dataExtension">データディレクトリの拡張子。指定した場合 「データディレクトリ@<paramref name="dataExtension"/>」となる。</param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <param name="callerMemberName"></param>
        /// <returns></returns>
        public static TestIO InitializeMethod(object test, string workSuffix = "", string dataExtension = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0, [CallerMemberName] string callerMemberName = "")
        {
            var data = new TestDataDirectory(new DirectoryInfo(Path.Combine(
                ProjectDirectory.FullName,
                GetDataClassDirectoryName(test, callerFilePath),
                GetDataMethodDirectoryName(dataExtension, callerMemberName)
            )));
            var work = new TestWorkDirectory(new DirectoryInfo(Path.Combine(
                ProjectDirectory.FullName,
                WorkRootDirectoryName,
                GetWorkClassDirectoryName(test),
                GetWorkMethodDirectoryName(workSuffix, callerMemberName, callerLineNumber)
            )));

            var testIO = new TestIO(data, work);

            InitializedMethod.Initialize(testIO);

            return testIO;
        }

        #endregion
    }
}
