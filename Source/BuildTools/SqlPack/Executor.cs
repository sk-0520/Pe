using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using Microsoft.Extensions.Logging.Abstractions;

namespace SqlPack
{
    public class Executor : IDisposable
    {
        public Executor(string sqlRootDirectoryPath, string outputSqlitePath)
        {
            SqlRootDirectory = new DirectoryInfo(sqlRootDirectoryPath);
            DatabaseAccessor = new SqliteAccessor(new SqliteFactory2(outputSqlitePath), NullLogger.Instance); ;
        }

        #region property

        DirectoryInfo SqlRootDirectory { get; }
        DatabaseAccessor DatabaseAccessor { get; }

        #endregion

        #region function

        private void InitializeDatabase()
        {
            DatabaseAccessor.Batch(c => {
                c.Execute(@"
create table Statements
(
    Namespace text not null,
    ClassName text not null,
    MethodName text not null,
    Statement text not null,
    primary key(
        Namespace,
        ClassName,
        MethodName
    )
)
                ");

                return true;
            });
        }

        void ImportSqlFile(IDatabaseContext context, FileInfo sqlFile)
        {
            var fullName = Path.GetRelativePath(SqlRootDirectory.FullName, sqlFile.FullName);
            var parentDirPath = Path.GetDirectoryName(fullName);
            var parameter = new {
                Namespace = Path.GetDirectoryName(parentDirPath)!.Replace('/', '.').Replace('\\', '.'),
                ClassName = Path.GetFileName(parentDirPath),
                MethodName = Path.GetFileNameWithoutExtension(sqlFile.Name),
                Statement = File.ReadAllText(sqlFile.FullName, Encoding.UTF8),
            };
            Console.WriteLine($"{parameter.Namespace} -> {parameter.ClassName} -> {parameter.MethodName}");
            context.Execute(@"
insert into
    Statements
    (
        Namespace,
        ClassName,
        MethodName,
        Statement
    )
    values
    (
        @Namespace,
        @ClassName,
        @MethodName,
        @Statement
    )
            ", parameter);
        }

        void ImportSqlFiles()
        {
            DatabaseAccessor.Batch(c => {
                var sqliFiles = SqlRootDirectory.EnumerateFiles("*.sql", SearchOption.AllDirectories);

                foreach(var sqlifile in sqliFiles) {
                    ImportSqlFile(c, sqlifile);
                }

                return true;
            });
        }

        public void Run()
        {
            InitializeDatabase();

            ImportSqlFiles();

            DatabaseAccessor.Execute("vacuum");
            DatabaseAccessor.Execute("reindex");
            DatabaseAccessor.Execute("analyze");
        }

        #endregion

        #region IDisposable Support
        private bool _disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if(!this._disposedValue) {
                if(disposing) {
                    DatabaseAccessor.Dispose();
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                this._disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~Executor()
        // {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
