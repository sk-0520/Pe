using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.Shell
{
    public interface IShellWriter
    {
        #region function

        void WriteLine();

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

        #endregion
    }

    public abstract class DefaultShellWriter<TShellOptions>: ShellWriterBase<TShellOptions>
        where TShellOptions : ShellOptions
    {
        protected DefaultShellWriter(TShellOptions options)
            : base(options)
        { }

        #region property

        private StringBuilder Worker { get; } = new StringBuilder();

        #endregion

        #region ShellWriterBase

        public override void WriteLine()
        {
            Worker.Append(Options.NewLine);
        }

        #endregion
    }
}
