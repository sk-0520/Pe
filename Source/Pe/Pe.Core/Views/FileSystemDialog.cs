using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Common;

namespace ContentTypeTextNet.Pe.Core.Views
{
    [ComImport]
    [Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")]
    internal class FileOpenDialogImpl
    { }

    [ComImport]
    [Guid("C0B4E2F3-BA21-4773-8DBA-335EC946EB8B")]
    internal class FileSaveDialogImpl
    { }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    internal class FileSystemFosAttribute: Attribute
    {
        public FileSystemFosAttribute(FOS fos)
        {
            Fos = fos;
        }

        #region property

        public FOS Fos { get; }

        #endregion
    }

    public abstract class FileSystemDialogBase: DisposerBase
    {
        protected private FileSystemDialogBase(FileOpenDialogImpl openDialogImpl)
        {
            FileDialogImpl = new Com<object>(openDialogImpl);
            FileDialog = FileDialogImpl.Cast<IFileDialog>();//(IFileDialog)openDialogImpl;
            FileDialogCustomize = FileDialogImpl.Cast<IFileDialogCustomize>();
        }

        protected private FileSystemDialogBase(FileSaveDialogImpl saveDialogImpl)
        {
            FileDialogImpl = new Com<object>(saveDialogImpl);
            FileDialog = FileDialogImpl.Cast<IFileDialog>();
            FileDialogCustomize = FileDialogImpl.Cast<IFileDialogCustomize>();
        }

        #region property

        private Com<object> FileDialogImpl { get; }
        private Com<IFileDialog> FileDialog { get; }
        private Com<IFileDialogCustomize> FileDialogCustomize { get; }

        /// <summary>
        /// フォルダ選択を行うか。
        /// </summary>
        [FileSystemFos(FOS.FOS_PICKFOLDERS)]
        public bool PickFolders { get; set; }

        /// <summary>
        /// ファイルシステムを強制する。
        /// </summary>
        [FileSystemFos(FOS.FOS_FORCEFILESYSTEM)]
        public bool ForceFileSystem { get; set; } = true;

        /// <summary>
        /// 新規作成時に確認するか。
        /// </summary>
        [FileSystemFos(FOS.FOS_CREATEPROMPT)]
        public bool CreatePrompt { get; set; } = true;
        /// <summary>
        /// 上書き時に確認するか。
        /// </summary>
        [FileSystemFos(FOS.FOS_OVERWRITEPROMPT)]
        public bool OverwritePrompt { get; set; } = true;
        /// <summary>
        /// 無効なパスに警告。
        /// </summary>
        [FileSystemFos(FOS.FOS_FILEMUSTEXIST)]
        public bool CheckPathExists { get; set; } = true;
        /// <summary>
        /// 有効な Win32 ファイル名だけを受け入れる。
        /// </summary>
        [FileSystemFos(FOS.FOS_PATHMUSTEXIST)]
        public bool ValidateNames { get; set; } = true;
        [FileSystemFos(FOS.FOS_NOCHANGEDIR)]
        public bool NoChangeDirectory { get; set; } = false;
        /// <summary>
        /// ショートカットで参照されたファイルの場所を返すか。
        /// </summary>
        [FileSystemFos(FOS.FOS_NODEREFERENCELINKS)]
        public bool DereferenceLinks { get; set; } = true;
        /// <summary>
        /// 最近使用したアイテムを隠すか。
        /// </summary>
        [FileSystemFos(FOS.FOS_HIDEMRUPLACES)]
        public bool HideMruPlaces { get; set; } = false;
        /// <summary>
        /// デフォルトで表示されるアイテムを隠すか。
        /// </summary>
        [FileSystemFos(FOS.FOS_HIDEPINNEDPLACES)]
        public bool HidePinnedPlaces { get; set; } = false;
        /// <summary>
        /// 読み取り専用は返さない？。
        /// </summary>
        [FileSystemFos(FOS.FOS_NOREADONLYRETURN)]
        public bool NoReadOnlyReturn { get; set; } = false;

        public string Title { get; set; } = string.Empty;

        public string InitialDirectory { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public DialogFilterList Filters { get; } = new DialogFilterList();

        public CustomizeDialogManager CustomizeDialogManager { get; } = new CustomizeDialogManager();

        #endregion

        #region function

        protected FOS GetFos()
        {
            var options = default(FOS);
            var type = GetType();
            var propertyInfoItems = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var fosAttributes = propertyInfoItems
                .Select(i => new { Property = i, Attribute = i.GetCustomAttribute<FileSystemFosAttribute>() })
                .Where(i => i.Attribute != null)
            ;
            foreach(var item in fosAttributes) {
                var isChecked = (bool)item.Property.GetValue(this)!;
                if(isChecked) {
                    options |= item.Attribute!.Fos;
                }
            }

            return options;
        }

        private static Com<IShellItem>? CreateFileItem(string path)
        {
            IShellItem item;
            IntPtr idl;
            uint atts = 0;
            if(NativeMethods.SHILCreateFromPath(path, out idl, ref atts) == 0) {
                if(NativeMethods.SHCreateShellItem(IntPtr.Zero, IntPtr.Zero, idl, out item) == 0) {
                    return ComWrapper.Create(item);
                }
            }

            return null;
        }

        private string AdjustExtension(string path)
        {
            var dotExt = Path.GetExtension(path);
            if(!string.IsNullOrWhiteSpace(dotExt)) {
                return path;
            }

            if(!Filters.Any()) {
                return path;
            }

            FileDialog.Instance.GetFileTypeIndex(out var filterIndex);
            var filter = Filters[(int)filterIndex - 1];
            if(string.IsNullOrWhiteSpace(filter.DefaultExtension)) {
                return path;
            }

            return PathUtility.AddExtension(path, filter.DefaultExtension);
        }

        private static void ApplyFos(Com<IFileDialog> result, FOS options)
        {
            result.Instance.SetOptions(options);
        }

        private static void ApplyTitle(Com<IFileDialog> result, string title)
        {
            if(!string.IsNullOrEmpty(title)) {
                result.Instance.SetTitle(title);
            }
        }

        private static IDisposable? ApplyInitialDirectory(Com<IFileDialog> result, string initialDirectory)
        {
            if(!string.IsNullOrEmpty(initialDirectory)) {
                var item = CreateFileItem(initialDirectory);
                if(item != null) {
                    result.Instance.SetDefaultFolder(item.Instance);
                    return item;
                }
            }

            return null;
        }

        private static IDisposable? ApplyPath(Com<IFileDialog> result, string path)
        {
            IDisposable? disposer = null;

            if(!string.IsNullOrEmpty(path)) {
                var parentDirPath = Path.GetDirectoryName(path);
                if(parentDirPath != null) {
                    var item = CreateFileItem(parentDirPath);
                    if(item != null) {
                        result.Instance.SetFolder(item.Instance);
                        disposer = item;

                        var fileName = Path.GetFileName(path);
                        result.Instance.SetFileName(fileName);
                    } else {
                        result.Instance.SetFileName(path);
                    }
                } else {
                    result.Instance.SetFileName(path);
                }
            }

            return disposer;
        }

        public bool? ShowDialog(Window parent) => ShowDialog(HandleUtility.GetWindowHandle(parent));
        public bool? ShowDialog(IntPtr hWnd)
        {
            using var cleaner = new DisposableCollection();

            ApplyFos(FileDialog, GetFos());
            ApplyTitle(FileDialog, Title);

            var initialDirectoryDisposer = ApplyInitialDirectory(FileDialog, InitialDirectory);
            if(initialDirectoryDisposer is not null) {
                cleaner.Add(initialDirectoryDisposer);
            }

            var pathDisposer = ApplyPath(FileDialog, FileName);
            if(pathDisposer is not null) {
                cleaner.Add(pathDisposer);
            }

            var filters = Filters
                .Select(i => new COMDLG_FILTERSPEC() { pszName = i.Display, pszSpec = string.Join(";", i.Wildcards) })
                .ToArray()
            ;
            if(filters.Any()) {
                FileDialog.Instance.SetFileTypes((uint)filters.Length, filters);
            }

            CustomizeDialogManager.Build(FileDialogCustomize);

            var result = FileDialog.Instance.Show(hWnd);
            if(result == (uint)ERROR.ERROR_CANCELLED) {
                //TODO: バグってる
                return false;
            }
            if(result != 0) {
                return null;
            }

            FileDialog.Instance.GetResult(out IShellItem resultItem);
            cleaner.Add(ComWrapper.Create(resultItem));
            resultItem.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out var pszPath);
            if(pszPath != IntPtr.Zero) {
                var path = Marshal.PtrToStringAuto(pszPath);
                Marshal.FreeCoTaskMem(pszPath);
                if(path != null) {
                    if(PickFolders) {
                        FileName = path;
                    } else {
                        FileName = AdjustExtension(path);
                    }

                    CustomizeDialogManager.ChangeStatus();
                    return true;
                }
            }

            return null;
        }

        #endregion

        #region DiposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    FileDialogCustomize.Dispose();
                    FileDialog.Dispose();
                    FileDialogImpl.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public class OpenFileDialog: FileSystemDialogBase
    {
        public OpenFileDialog()
            : base(new FileOpenDialogImpl())
        {
        }
    }

    public class SaveFileDialog: FileSystemDialogBase
    {
        public SaveFileDialog()
            : base(new FileSaveDialogImpl())
        {
            CreatePrompt = false;
            OverwritePrompt = true;
            NoReadOnlyReturn = true;
        }
    }

    public class FolderBrowserDialog: FileSystemDialogBase
    {
        public FolderBrowserDialog()
            : base(new FileOpenDialogImpl())
        {
            PickFolders = true;
            CreatePrompt = false;
            OverwritePrompt = false;
        }
    }
}
