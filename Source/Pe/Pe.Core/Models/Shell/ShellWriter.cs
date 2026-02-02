using System;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.Shell
{
    public enum ShellIndent
    {
        Increment,
        Decrement,
    }

    public interface IShellWriter
    {
        #region function

        void WriteLine();

        void Write(char value);
        void Write(int value);
        void Write(string value);

        void Indent(ShellIndent indent);

        #endregion
    }

    public abstract class ShellWriterBase<TShellOptions>: IShellWriter
        where TShellOptions : ShellOptions
    {
        protected ShellWriterBase(TShellOptions options)
        {
            Options = options;
        }

        #region property

        protected TShellOptions Options { get; }

        #endregion

        #region IShellWriter

        public abstract void WriteLine();

        public abstract void Write(char value);
        public abstract void Write(int value);
        public abstract void Write(string value);

        public abstract void Indent(ShellIndent indent);

        #endregion
    }

    public class DefaultShellWriter<TShellOptions>: ShellWriterBase<TShellOptions>
        where TShellOptions : ShellOptions
    {
        public DefaultShellWriter(TShellOptions options)
            : base(options)
        { }

        #region property

        private StringBuilder Worker { get; } = new StringBuilder();
        private int IndentLevel { get; set; } = 0;

        #endregion

        #region function

        #endregion

        #region ShellWriterBase

        public override string ToString()
        {
            return Worker.ToString();
        }

        public override void WriteLine()
        {
            Worker.Append(Options.NewLine);
        }

        public override void Write(char value)
        {
            Worker.Append(value);
        }
        public override void Write(int value)
        {
            Worker.Append(value);
        }
        public override void Write(string value)
        {
            Worker.Append(value);
        }

        public override void Indent(ShellIndent indent)
        {
            switch(indent) {
                case ShellIndent.Increment:
                    IndentLevel += 1;
                    break;

                case ShellIndent.Decrement:
                    IndentLevel -= 1;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
