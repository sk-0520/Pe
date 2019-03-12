using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.View.Extend;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Toolbar
{
    public class LauncherToolbarElement : ContextElementBase, IAppDesktopToolbarExtendData
    {
        public LauncherToolbarElement(Screen dockScreen, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            DockScreen = dockScreen;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            IdFactory = idFactory;
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IIdFactory IdFactory { get; }

        /// <summary>
        /// 表示されているか。
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// 表示アイコンサイズ。
        /// </summary>
        public IconScale IconScale { get; private set; }

        /// <summary>
        /// アイコンの余白。
        /// <para>CSS の margin と同じ考え方。</para>
        /// </summary>
        public Thickness IconMargin { get; private set; }

        /// <summary>
        /// ボタンの詰め領域。
        /// <para>CSS の padding と同じ考え方。</para>
        /// </summary>
        public Thickness ButtonPadding { get; private set; }

        /// <summary>
        /// アイコンのみを表示するか。
        /// </summary>
        public bool IsIconOnly { get; private set; }

        /// <summary>
        /// テキスト表示の際の表示幅。
        /// </summary>
        [PixelKind(Px.Logical)]
        public double TextWidth { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// <see cref="DockScreen"/> から近しい ツールバー設定を読み込む。
        /// </summary>
        /// <param name="rows"></param>
        /// <returns>見つかったツールバー。見つからない場合は<see cref="Guid.Empty"/>を返す。</returns>
        Guid FindMaybeToolbarId(IEnumerable<ToolbarsScreenRowDto> rows)
        {
            foreach(var row in rows) {
                if(row.Screen == DockScreen.DeviceName) {
                    return row.ToolbarId;
                }

                var deviceBounds = DockScreen.DeviceBounds;
                // 完全一致パターン: ドライバ更新でも大抵は大丈夫だと思う
                if(row.X == deviceBounds.X && row.Y == deviceBounds.Y && row.Width == deviceBounds.Width && row.Height == deviceBounds.Height) {
                    return row.ToolbarId;
                }
            }

            return Guid.Empty;
        }

        Guid GetToolbarId()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new ToolbarsDao(commander, StatementLoader, Logger.Factory);
                var screenToolbars = dao.SelectAllToolbars().ToList();
                var toolbarId = FindMaybeToolbarId(screenToolbars);
                return toolbarId;
            }
        }

        Guid CreateToolbar()
        {
            var toolbarId = IdFactory.CreateToolbarId();
            Logger.Debug($"new toolbar: {toolbarId}");

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var dao = new ToolbarsDao(commander, StatementLoader, Logger.Factory);
                dao.InsertNewToolbar(toolbarId, DockScreen);

                commander.Commit();
            }

            return toolbarId;
        }

        void LoadToolbar(Guid toolbarId)
        {
            Logger.Information($"toolbar id: {toolbarId}");

            ToolbarPosition = AppDesktopToolbarPosition.Right;
            DisplaySize = new Size(40, 40);
            HiddenSize = new Size(4, 4);
        }

        public void Initialize()
        {
            Logger.Information($"initialize {DockScreen.DeviceName}:{DockScreen.DeviceBounds}, {nameof(DockScreen.Primary)}: {DockScreen.Primary}");

            var toolbarId = GetToolbarId();
            if(toolbarId == Guid.Empty) {
                toolbarId = CreateToolbar();
            }
            LoadToolbar(toolbarId);
        }

        #endregion

        #region IAppDesktopToolbarExtendData

        /// <summary>
        /// ツールバー位置。
        /// </summary>
        public AppDesktopToolbarPosition ToolbarPosition { get; set; }
        /// <summary>
        /// ドッキング中か。
        /// </summary>
        public bool IsDocking { get; set; }
        /// <summary>
        /// 自動的に隠すか。
        /// </summary>
        public bool IsAutoHide { get; set; }
        /// <summary>
        /// 隠れているか。
        /// </summary>
        public bool IsHiding { get; set; }
        /// <summary>
        /// 自動的に隠れるまでの時間。
        /// </summary>
        public TimeSpan AutoHideTime { get; private set; }

        /// <summary>
        /// 表示中のサイズ。
        /// <para><see cref="AppDesktopToolbarPosition"/>の各辺に対応</para>
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size DisplaySize { get; set; }

        /// <summary>
        /// 表示中の論理バーサイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect DisplayBarArea { get; set; }
        /// <summary>
        /// 隠れた状態のバー論理サイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size HiddenSize { get; private set; }
        /// <summary>
        /// 表示中の隠れたバーの論理領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect HiddenBarArea { get; set; }

        /// <summary>
        /// フルスクリーンウィンドウが存在するか。
        /// </summary>
        public bool ExistsFullScreenWindow { get; set; }


        /// <summary>
        /// 対象ディスプレイ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Screen DockScreen { get; }

        #endregion
    }
}
