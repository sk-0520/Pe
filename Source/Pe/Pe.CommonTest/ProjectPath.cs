using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.CommonTest
{
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

        public ProjectPath Create(string baseDirectoryName)
        {
            var rootDirectoryName = string.IsNullOrWhiteSpace(baseDirectoryName)
                ? ProjectTestPath
                : Path.Combine(ProjectTestPath, baseDirectoryName)
            ;

            return new ProjectPath(rootDirectoryName);
        }

        public ProjectPath CreateOutput() => Create(string.Empty);

        public ProjectPath CreateIO() => Create("_test_io_");

        #endregion
    }

    public class ProjectDirectoryPath
    { }

    public abstract class DirectoryPath
    {
        protected DirectoryPath(string rootDirectoryName)
        {
            RootDirectoryPath = rootDirectoryName;

            if(!InitializedDirectories.Contains(RootDirectoryPath)) {
                Directory.CreateDirectory(RootDirectoryPath);
                InitializedDirectories.Add(RootDirectoryPath);
            }
        }

        #region property

        private static HashSet<string> InitializedDirectories = new HashSet<string>();

        public string RootDirectoryPath { get; }

        #endregion
    }

    public class MethodPath: DirectoryPath
    {
        internal MethodPath(string rootDirectoryName)
           : base(rootDirectoryName)
        { }
    }

    public class ClassPath: DirectoryPath
    {
        internal ClassPath(string rootDirectoryName)
            : base(rootDirectoryName)
        { }
    }

    public class ProjectPath: DirectoryPath
    {
        #region variable

        private static ProjectPathFactory? _factory;

        #endregion

        internal ProjectPath(string rootDirectoryName)
            : base(rootDirectoryName)
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

        #endregion
    }
}
