using ContentTypeTextNet.Pe.Library.CommandLine;

namespace SqlPack
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLineParser = new CommandLineParser();
            var sqlRootDirKey = commandLineParser.Add("sql-root-dir", CommandLineOptionKind.Value);
            var outputPathKey = commandLineParser.Add("output", CommandLineOptionKind.Value);
            var parsedResult = commandLineParser.Parse("SqlPack", args);

            var rootDirPath = parsedResult.Values[sqlRootDirKey.Key].First;
            var outPath = parsedResult.Values[outputPathKey.Key].First;

            var executor = new Executor(rootDirPath, outPath);
            executor.Run();
        }
    }
}
