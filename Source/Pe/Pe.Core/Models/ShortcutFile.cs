using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// ショートカットファイルの読み書き。
    /// </summary>
    public class ShortcutFile: DisposerBase
    {
        #region define

        private readonly struct IconPathData
        {
            public IconPathData(string path)
                : this(path, 0)
            { }

            public IconPathData(string path, int index)
            {
                Path = path;
                Index = index;
            }

            #region property

            public string Path { get; }
            public int Index { get; }

            #endregion
        }

        #endregion

        #region variable

        private ComWrapper<IPersistFile>? _persistFile;

        #endregion

        /// <summary>
        /// ショートカットを作成するためにオブジェクト生成。
        /// </summary>
        public ShortcutFile()
            : base()
        {
            ShellLink = CreateShellLink();
        }

        /// <summary>
        /// ショートカットを読み込むためにオブジェクト生成。
        /// </summary>
        /// <param name="path">読み込むショートカットファイルパス。</param>
        public ShortcutFile(string path)
            : this()
        {
            PersistFile.Raw.Load(path, 0);
        }

        #region property

        /// <summary>
        /// 各種パスプロパティのバッファサイズ。
        /// </summary>
        public int PathLength { get; set; } = (int)MAX.MAX_PATH;
        /// <summary>
        /// 引数のバッファサイズ。
        /// </summary>
        public int ArgumentLength { get; set; } = 1024;
        /// <summary>
        /// コメントのバッファサイズ。
        /// </summary>
        public int DescriptionLength { get; set; } = 1024 * 4;

        protected ComWrapper<IShellLink> ShellLink { get; }

        protected ComWrapper<IPersistFile> PersistFile => this._persistFile ??= ShellLink.Cast<IPersistFile>();

        /// <summary>
        /// ショートカット先パス。
        /// </summary>
        public string TargetPath
        {
            get
            {
                ThrowIfDisposed();

                var resultBuffer = CreateStringBuffer(PathLength);

                ShellLink.Raw.GetPath(resultBuffer, resultBuffer.MaxCapacity, out _, SLGP_FLAGS.SLGP_UNCPRIORITY);

                return resultBuffer.ToString();
            }
            set
            {
                ThrowIfDisposed();

                ShellLink.Raw.SetPath(value);
            }
        }

        /// <summary>
        /// 引数。
        /// </summary>
        public string Arguments
        {
            get
            {
                ThrowIfDisposed();

                var resultBuffer = CreateStringBuffer(ArgumentLength);

                ShellLink.Raw.GetArguments(resultBuffer, resultBuffer.Capacity);

                return resultBuffer.ToString();
            }
            set
            {
                ThrowIfDisposed();

                ShellLink.Raw.SetArguments(value);
            }
        }

        /// <summary>
        /// コメント。
        /// </summary>
        public string Description
        {
            get
            {
                ThrowIfDisposed();

                var resultBuffer = CreateStringBuffer(DescriptionLength);

                ShellLink.Raw.GetDescription(resultBuffer, resultBuffer.Capacity);

                return resultBuffer.ToString();
            }
            set
            {
                ThrowIfDisposed();

                ShellLink.Raw.SetDescription(value);
            }
        }

        /// <summary>
        /// 作業ディレクトリ。
        /// </summary>
        public string WorkingDirectory
        {
            get
            {
                ThrowIfDisposed();

                var resultBuffer = CreateStringBuffer(PathLength);

                ShellLink.Raw.GetWorkingDirectory(resultBuffer, resultBuffer.MaxCapacity);

                return resultBuffer.ToString();
            }
            set
            {
                ThrowIfDisposed();

                ShellLink.Raw.SetWorkingDirectory(value);
            }
        }

        public string IconPath
        {
            get => GetIcon().Path;
            set => SetIcon(new IconPathData(value));
        }
        public int IconIndex
        {
            get => GetIcon().Index;
            set => SetIcon(new IconPathData(IconPath, value));
        }

        /// <summary>
        /// 表示方法。
        /// </summary>
        public SW ShowCommand
        {
            get
            {
                ThrowIfDisposed();

                ShellLink.Raw.GetShowCmd(out var rawShowCommand);
                return (SW)rawShowCommand;
            }
            set
            {
                ThrowIfDisposed();

                ShellLink.Raw.SetShowCmd((int)value);
            }
        }

        #endregion

        #region function

        private static StringBuilder CreateStringBuffer(int max)
        {
            return new StringBuilder(max, max);
        }

        private static ComWrapper<IShellLink> CreateShellLink()
        {
            return new ComWrapper<IShellLink>((IShellLink)new ShellLinkObject());
        }

        /// <summary>
        /// アイコン取得。
        /// </summary>
        /// <returns></returns>
        private IconPathData GetIcon()
        {
            ThrowIfDisposed();

            var resultBuffer = CreateStringBuffer(PathLength);
            ShellLink.Raw.GetIconLocation(resultBuffer, resultBuffer.Capacity, out var iconIndex);

            return new IconPathData(resultBuffer.ToString(), iconIndex);
        }

        /// <summary>
        /// アイコン設定。
        /// </summary>
        /// <param name="iconPath"></param>
        private void SetIcon(in IconPathData iconPath)
        {
            ThrowIfDisposed();

            ShellLink.Raw.SetIconLocation(iconPath.Path, iconPath.Index);
        }

        /// <summary>
        /// ショートカットを保存。
        /// </summary>
        /// <param name="path">保存先ショートカットパス。</param>
        public void Save(string path)
        {
            ThrowIfDisposed();

            PersistFile.Raw.Save(path, true);
        }

        #endregion

        #region ModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(this._persistFile != null) {
                        this._persistFile.Dispose();
                    }

                    ShellLink.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
