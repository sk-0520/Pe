using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
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

        public ProjectPath Create(string baseDirectoryName, bool resetDirectory, TestDirectoryKind directoryKind)
        {
            var rootDirectoryName = string.IsNullOrWhiteSpace(baseDirectoryName)
                ? ProjectTestPath
                : Path.Combine(ProjectTestPath, baseDirectoryName)
            ;

            return new ProjectPath(rootDirectoryName, resetDirectory, directoryKind);
        }

        public ProjectPath CreateOutput() => Create(string.Empty, false, TestDirectoryKind.Tree);

        public ProjectPath CreateIO() => Create("_test_io_", true, TestDirectoryKind.Flat);

        #endregion
    }

    public abstract class DirectoryPath
    {
        protected DirectoryPath(string rootDirectoryName, bool isResetDirectory, TestDirectoryKind directoryKind)
        {
            RootDirectoryPath = rootDirectoryName;
            IsResetDirectory = isResetDirectory;
            DirectoryKind = directoryKind;

            if(!InitializedDirectories.Contains(RootDirectoryPath)) {
                if(IsResetDirectory) {
                    Directory.Delete(RootDirectoryPath, true);
                }
                Directory.CreateDirectory(RootDirectoryPath);
                InitializedDirectories.Add(RootDirectoryPath);
            }
        }

        #region property

        private static HashSet<string> InitializedDirectories = new HashSet<string>();

        public string RootDirectoryPath { get; }
        public bool IsResetDirectory { get; }
        public TestDirectoryKind DirectoryKind { get; }

        #endregion
    }

    public class MethodDirectoryPath: DirectoryPath
    {
        internal MethodDirectoryPath(string rootDirectoryName, bool resetDirectory, TestDirectoryKind directoryKind)
           : base(rootDirectoryName, resetDirectory, directoryKind)
        { }
    }

    public class ClassDirectoryPath: DirectoryPath
    {
        internal ClassDirectoryPath(string rootDirectoryName, bool resetDirectory, TestDirectoryKind directoryKind)
           : base(rootDirectoryName, resetDirectory, directoryKind)
        { }
    }

    public class ProjectPath: DirectoryPath
    {
        #region variable

        private static ProjectPathFactory? _factory;

        #endregion

        internal ProjectPath(string rootDirectoryName, bool resetDirectory, TestDirectoryKind directoryKind)
            : base(rootDirectoryName, resetDirectory, directoryKind)
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

        public ClassDirectoryPath GetClassDirectory(Type type)
        {
            var dirPath = type.FullName!;
            if(DirectoryKind == TestDirectoryKind.Tree) {
                dirPath = dirPath.Replace('.', Path.DirectorySeparatorChar);
            }
            var path = Path.Combine(RootDirectoryPath, dirPath);
            return new ClassDirectoryPath(path, IsResetDirectory, DirectoryKind);
        }

        #endregion
    }
}
