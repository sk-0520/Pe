using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators;

namespace ContentTypeTextNet.Pe.CommonTest
{
    public enum TestDirectoryKind
    {
        Tree,
        Flat,
    }

    public class ProjectPathFactory
    {
        public ProjectPathFactory()
        {
            ProjectTestPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        }

        #region property

        private string ProjectTestPath { get; }

        #endregion

        #region function

        public ProjectPath Create(string baseDirectoryName, bool directoryClean, TestDirectoryKind directoryKind)
        {
            var rootDirectoryName = string.IsNullOrWhiteSpace(baseDirectoryName)
                ? ProjectTestPath
                : Path.Combine(ProjectTestPath, baseDirectoryName)
            ;

            return new ProjectPath(rootDirectoryName, directoryClean, directoryKind);
        }

        public ProjectPath CreateOutput() => Create(string.Empty, false, TestDirectoryKind.Tree);

        public ProjectPath CreateIO() => Create("_test_io_", true, TestDirectoryKind.Flat);

        #endregion
    }

    public abstract class DirectoryPath
    {
        protected DirectoryPath(string rootDirectoryName, bool directoryClean, TestDirectoryKind directoryKind)
        {
            Directory = new DirectoryInfo(rootDirectoryName);
            DirectoryClean = directoryClean;
            DirectoryKind = directoryKind;

            if(!InitializedDirectories.Contains(Directory.FullName)) {
                if(DirectoryClean) {
                    Directory.Refresh();
                    if(Directory.Exists) {
                        Directory.Delete(true);
                    }
                }
                Directory.Create();
                InitializedDirectories.Add(Directory.FullName);
            }
        }

        #region property

        private static HashSet<string> InitializedDirectories = new HashSet<string>();

        public DirectoryInfo Directory { get; }
        public bool DirectoryClean { get; }
        public TestDirectoryKind DirectoryKind { get; }

        #endregion

        #region function

        //public static DirectoryInfo CreateDirectory(DirectoryInfo directory, string name)
        //{
        //    var dirPath = Path.Combine(directory.FullName, name);
        //    return Directory.CreateDirectory(dirPath);
        //}

        //public static FileInfo CreateEmptyFile(DirectoryInfo directory, string name)
        //{
        //    var filePath = Path.Combine(directory.FullName, name);
        //    File.Create(filePath).Dispose();
        //    return new FileInfo(filePath);
        //}

        //public static FileInfo CreateTextFile(DirectoryInfo directory, string name, string content, Encoding encoding)
        //{
        //    var filePath = Path.Combine(directory.FullName, name);

        //    using(var stream = File.Create(filePath)) {
        //        using var writer = new StreamWriter(stream, encoding);
        //        writer.Write(content);
        //    }

        //    return new FileInfo(filePath);
        //}
        //public static FileInfo CreateTextFile(DirectoryInfo directory, string name, string content) => CreateTextFile(directory, name, content, Encoding.UTF8);

        #endregion
    }

    public class MethodDirectoryPath: DirectoryPath
    {
        public MethodDirectoryPath(string rootDirectoryName, bool directoryClean, TestDirectoryKind directoryKind)
           : base(rootDirectoryName, directoryClean, directoryKind)
        { }
    }

    public class ClassDirectoryPath: DirectoryPath
    {
        public ClassDirectoryPath(string rootDirectoryName, bool resetDirectory, TestDirectoryKind directoryKind)
           : base(rootDirectoryName, resetDirectory, directoryKind)
        { }

        #region function

        public MethodDirectoryPath CreateMethodDirectory(string suffix = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            var dirName = DirectoryClean
                ? Path.Combine(Directory.FullName, $"{suffix}{callerMemberName}-L{callerLineNumber}")
                : Path.Combine(Directory.FullName, $"{suffix}{callerMemberName}")
            ;
            var path = Path.Combine(Directory.FullName, dirName);
            return new MethodDirectoryPath(path, DirectoryClean, DirectoryKind);
        }

        #endregion
    }

    public class ProjectPath: DirectoryPath
    {
        #region variable

        private static ProjectPathFactory? _factory;

        #endregion

        public ProjectPath(string rootDirectoryName, bool directoryClean, TestDirectoryKind directoryKind)
            : base(rootDirectoryName, directoryClean, directoryKind)
        { }

        #region property

        public static ProjectPathFactory Factory
        {
            get
            {
                return _factory ??= new ProjectPathFactory();
            }
        }

        #endregion

        #region function

        public ClassDirectoryPath CreateClassDirectory(object test)
        {
            var dirName = test.GetType().FullName!;
            if(DirectoryKind == TestDirectoryKind.Tree) {
                dirName = dirName.Replace('.', Path.DirectorySeparatorChar);
            }
            var path = Path.Combine(Directory.FullName, dirName);
            return new ClassDirectoryPath(path, DirectoryClean, DirectoryKind);
        }

        public MethodDirectoryPath CreateMethodDirectory(object test, string suffix = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            var classPath = CreateClassDirectory(test);
            return classPath.CreateMethodDirectory(suffix, callerMemberName, callerLineNumber);
        }

        #endregion
    }
}
