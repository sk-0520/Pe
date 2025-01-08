using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    public class ArchiveExtractorTest
    {
        #region define

        private enum NodeType
        {
            File,
            Directory,
        }

        private record class Node(
            NodeType Type,
            string Name
        )
        {
            public Node[] Children { get; init; } = Array.Empty<Node>();
        };

        #endregion

        #region property

        private Test Test { get; } = Test.Create();

        private Node[] Nodes { get; } = [
            new Node(NodeType.Directory, "empty"),
            new Node(NodeType.Directory, "sub") {
                Children = [
                    new Node(NodeType.File, "sub-file.txt"),
                ]
            },
            new Node(NodeType.File, "file.txt"),
            new Node(NodeType.File, "ファイル.txt"),
            new Node(NodeType.File, "🍙.txt"),
        ];

        #endregion

        #region function

        [Theory]
        [InlineData("win11explorer.zip")]
        [InlineData("7zip.zip")]
        [InlineData("7zip.7z")]
        public void ExtractTest(string archiveFileName)
        {
            var testDir = TestIO.InitializeMethod(this, archiveFileName);

            var dir = Test.GetCopiedDirectoryPath(this, "dat");
            var archivePath = Path.Combine(dir, archiveFileName);
            var archiveFile = new FileInfo(archivePath);

            var ext = archiveFile.Extension.Substring(1).ToLowerInvariant();

            var archiveExtractor = new ArchiveExtractor(Test.LoggerFactory);
            archiveExtractor.Extract(new FileInfo(archivePath), testDir, ext, new NullNotifyProgress(Test.LoggerFactory));

            var flatFiles = testDir.EnumerateFiles().Select(a => a.Name).ToHashSet();
            var flatFileNodes = Nodes.Where(a => a.Type == NodeType.File).ToArray();
            Assert.Equal(flatFileNodes.Length, flatFiles.Count);

            var flatDirs = testDir.EnumerateDirectories().Select(a => a.Name).ToHashSet();
            var flatDirNodes = Nodes.Where(a => a.Type == NodeType.Directory).ToArray();
            Assert.Equal(flatDirNodes.Length, flatDirs.Count);
        }

        #endregion
    }
}
