using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    public class LauncherIconLoader : IconImageLoaderBase, ILauncherItemId
    {
        public LauncherIconLoader(Guid launcherItemId, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IDispatcherWrapper? dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(dispatcherWrapper, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }
        static EnvironmentPathExecuteFileCache EnvironmentPathExecuteFileCache { get; } = EnvironmentPathExecuteFileCache.Instance;

        int RetryMaxCount { get; } = 5;
        TimeSpan RetryWaitTime { get; } = TimeSpan.FromSeconds(1);

        #endregion

        #region function

        Task<ResultSuccessValue<BitmapSource>> LoadExistsImageAsync(IconScale iconScale)
        {
            ThrowIfDisposed();

            return Task.Run(() => {
                IReadOnlyList<byte[]>? imageBinary;
                using(var commander = FileDatabaseBarrier.WaitRead()) {
                    var dao = new LauncherItemIconsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                    imageBinary = dao.SelectImageBinary(LauncherItemId, iconScale);
                }

                if(imageBinary != null && imageBinary.Count == 0) {
                    return ResultSuccessValue.Failure<BitmapSource>();
                }
                var image = ToImage(imageBinary);

                if(image == null) {
                    return ResultSuccessValue.Failure<BitmapSource>();
                }

                return ResultSuccessValue.Success(image);
            });
        }

        protected virtual LauncherIconData GetIconData()
        {
            ThrowIfDisposed();

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherItemDomainDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectFileIcon(LauncherItemId);
            }
        }

        protected virtual Task<BitmapSource?> GetImageCoreAsync(LauncherItemKind kind, IReadOnlyIconData iconData, IconScale iconScale, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var editIconData = new IconData() {
                Path = Environment.ExpandEnvironmentVariables(iconData.Path),
                Index = iconData.Index,
            };

            if(Path.IsPathFullyQualified(editIconData.Path)) {
                return GetIconImageAsync(editIconData, iconScale, cancellationToken);
            }

            if(string.IsNullOrWhiteSpace(editIconData.Path)) {
                // 無効パスのため後続処理不要
                return Task.FromResult(default(BitmapSource));
            }

            var pathItems = EnvironmentPathExecuteFileCache.GetItems(LoggerFactory);
            var environmentExecuteFile = new EnvironmentExecuteFile(LoggerFactory);
            var pathItem = environmentExecuteFile.Get(editIconData.Path, pathItems);
            if(pathItem == null) {
                Logger.LogWarning("指定されたコマンドからパス取得失敗: {0}", editIconData.Path);
                return Task.FromResult(default(BitmapSource));
            }

            editIconData.Path = pathItem.File.FullName;
            editIconData.Index = 0;
            return GetIconImageAsync(editIconData, iconScale, cancellationToken);
        }

        protected async Task<BitmapSource?> GetImageAsync(LauncherIconData launcherIconData, IconScale iconScale, bool tuneSize, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var iconImage = await GetImageCoreAsync(launcherIconData.Kind, launcherIconData.Icon, iconScale, cancellationToken).ConfigureAwait(false);
            if(iconImage != null) {
                if(tuneSize) {
                    return ResizeImage(iconImage, iconScale);
                }
                return iconImage;
            }

            //if(launcherIconData.Kind == LauncherItemKind.StoreApp) {

            //}

            var commandImage = await GetImageCoreAsync(launcherIconData.Kind, launcherIconData.Path, iconScale, cancellationToken).ConfigureAwait(false);
            if(commandImage != null) {
                if(tuneSize) {
                    return ResizeImage(commandImage, iconScale);
                }
                return commandImage;
            }

            // 標準のやつ。。。 xaml でやるべき？
            return null;
        }

        protected Task WriteStreamAsync(BitmapSource iconImage, Stream stream)
        {
            Debug.Assert(iconImage.IsFrozen);
            ThrowIfDisposed();

            return Task.Run(() => {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(iconImage));
                encoder.Save(stream);
                Dispatcher.CurrentDispatcher.InvokeShutdown();
            });
        }

        protected async Task SaveImageAsync(BitmapSource iconImage, IconScale iconScale)
        {
            ThrowIfDisposed();

            using(var stream = new BinaryChunkedStream()) {
                await WriteStreamAsync(iconImage, stream);
#if DEBUG
                using(var debugStream = new MemoryStream()) {
                    await WriteStreamAsync(iconImage, debugStream);
                    Debug.Assert(stream.Position == debugStream.Position, $"{nameof(stream)}: {stream.Length}, {nameof(debugStream)}: {debugStream.Length}");
                }
#endif
                DateTime iconUpdatedTimestamp = DateTime.UtcNow;
                using(var commander = FileDatabaseBarrier.WaitWrite()) {
                    var launcherItemIconStatusEntityDao = new LauncherItemIconStatusEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                    var existIconState = launcherItemIconStatusEntityDao.SelecteExistLauncherItemIconState(LauncherItemId, iconScale);
                    if(existIconState) {
                        launcherItemIconStatusEntityDao.UpdateLastUpdatedIconTimestamp(LauncherItemId, iconScale, iconUpdatedTimestamp, DatabaseCommonStatus.CreateCurrentAccount());
                    } else {
                        launcherItemIconStatusEntityDao.InsertLastUpdatedIconTimestamp(LauncherItemId, iconScale, iconUpdatedTimestamp, DatabaseCommonStatus.CreateCurrentAccount());
                    }

                    var launcherItemIconsEntityDao = new LauncherItemIconsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                    launcherItemIconsEntityDao.DeleteImageBinary(LauncherItemId, iconScale);
                    launcherItemIconsEntityDao.InsertImageBinary(LauncherItemId, iconScale, stream.BinaryChunkedList, DatabaseCommonStatus.CreateCurrentAccount());
                    commander.Commit();
                }

            }


        }

        async Task<BitmapSource?> MakeImageAsync(IconScale iconScale, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            // アイコンパス取得
            var launcherIconData = GetIconData();

            // アイコン取得
            var iconImage = await GetImageAsync(launcherIconData, iconScale, true, cancellationToken).ConfigureAwait(false);
            if(iconImage != null) {
                // データ書き込み(失敗してもアイコンが取得できてるならOK)
                var _ = SaveImageAsync(iconImage, iconScale);
            } else {
                Logger.LogWarning("アイコン取得失敗: {0}, {1}", LauncherItemId, ObjectDumper.GetDumpString(launcherIconData));
            }

            return iconImage;
        }


        #endregion

        #region IconImageLoaderBase

        protected override async Task<BitmapSource?> LoadImplAsync(IconScale iconScale, CancellationToken cancellationToken)
        {
            var counter = new Counter(RetryMaxCount);
            foreach(var count in counter) {
                try {
                    var existisResult = await LoadExistsImageAsync(iconScale).ConfigureAwait(false);
                    if(existisResult.Success) {
                        return existisResult.SuccessValue;
                    }

                    var image = await MakeImageAsync(iconScale, cancellationToken).ConfigureAwait(false);
                    return image;
                } catch(SynchronizationLockException ex) {
                    if(count.IsLast) {
                        Logger.LogError(ex, "アイコン取得待機失敗: 全試行 {0}回 失敗: {1}", count.MaxCount, LauncherItemId);
                        return null;
                    } else {
                        Logger.LogWarning(ex, "アイコン取得待機失敗: {0}/{1}回 失敗, 再試行待機 {2}, {3}", count.CurrentCount, count.MaxCount, RetryWaitTime, LauncherItemId);
                        await Task.Delay(RetryWaitTime).ConfigureAwait(false);
                    }
                }
            }

            throw new NotImplementedException();
        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion

    }
}