using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.CliProxy
{
    public interface ISourceSetting
    {
        #region property

        string IndentText { get; }

        #endregion
    }

    file sealed class DefaultSourceSetting: ISourceSetting
    {
        #region ISourceSetting

        public string IndentText { get; } = "    ";

        #endregion
    }

    public class SourceWriter: ISourceSetting
    {
        #region define

        private class BlockSource: IDisposable
        {
            public BlockSource(SourceWriter parent, bool levelIncrement, string head, string tail)
            {
                Parent = parent;
                Head = head;
                Tail = tail;
                LevelIncrement = levelIncrement;

                Parent.AppendLine(Head);
                if(LevelIncrement) {
                    Parent.IndentLevel += 1;
                }
            }

            #region property

            private string Head { get; }
            private string Tail { get; }
            private bool LevelIncrement { get; }

            private SourceWriter Parent { get; }

            #endregion

            #region IDisposable

            private bool _disposedValue;

            protected virtual void Dispose(bool disposing)
            {
                if(!this._disposedValue) {
                    if(disposing) {
                        if(LevelIncrement) {
                            Parent.IndentLevel -= 1;
                        }
                        Parent.AppendLine(Tail);
                    }

                    this._disposedValue = true;
                }
            }

            public void Dispose()
            {
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            #endregion
        }

        #endregion

        public SourceWriter()
            : this(new DefaultSourceSetting())
        { }

        public SourceWriter(ISourceSetting setting)
        {
            IndentText = setting.IndentText;
        }

        #region property

        private StringBuilder Buffer { get; } = new StringBuilder();

        private int IndentLevel { get; set; }

        #endregion

        #region function

        private string GetIndent() => string.Concat(Enumerable.Repeat(IndentText, IndentLevel));

        public void Append(string s)
        {
            var indent = GetIndent();
            using(var reader = new StringReader(s)) {
                string? line = null;
                while((line = reader.ReadLine()) is not null) {
                    Buffer.Append(indent);
                    Buffer.AppendLine(line);
                }
            }
        }

        public void AppendLine(string s)
        {
            Append(s);
            Buffer.AppendLine();
        }

        public void AppendEmptyLine()
        {
            Buffer.Append(GetIndent());
            Buffer.AppendLine();
        }

        public IDisposable Block() => Block("{", "}");

        public IDisposable Block(string head, string tail)
        {
            return new BlockSource(this, true, head, tail);
        }

        public IDisposable Region(string name)
        {
            return new BlockSource(this, false, $"#region {name}", "#" + "endregion"); // VS の拡張機能都合でソース上は # を分割
        }

        #endregion

        #region object

        public override string ToString()
        {
            return Buffer.ToString();
        }

        #endregion

        #region ISourceSetting

        public string IndentText { get; }

        #endregion
    }

}
