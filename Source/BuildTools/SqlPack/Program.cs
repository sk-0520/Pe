using System;
using System.IO;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Args;
using ContentTypeTextNet.Pe.Library.Common;
using Microsoft.Extensions.Logging.Abstractions;

namespace SqlPack
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLine = new CommandLine();
            var sqlRootDirKey = commandLine.Add(longKey: "sql-root-dir", CommandLineKeyKind.Value);
            var outputPathKey = commandLine.Add(longKey: "output", CommandLineKeyKind.Value);
            if(!commandLine.Parse()) {
                throw commandLine.ParseException!;
            }

            var rootDirPath = commandLine.Values[sqlRootDirKey].First;
            var outPath = commandLine.Values[outputPathKey].First;

            var executor = new Executor(rootDirPath, outPath);
            executor.Run();
        }
    }
}
