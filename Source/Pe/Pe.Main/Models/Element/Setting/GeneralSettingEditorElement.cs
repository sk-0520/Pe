using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public abstract class GeneralSettingEditorElementBase : ElementBase
    {
        public GeneralSettingEditorElementBase(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected IFileDatabaseBarrier FileDatabaseBarrier { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }
        #endregion

        #region function

        public void Save(DatabaseCommandPack commadPack)
        {
            SaveImpl(commadPack);
        }

        protected abstract void SaveImpl(DatabaseCommandPack commadPack);

        #endregion
    }

    public class AppExecuteSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppExecuteSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property

        public string UserId { get; set; } = string.Empty;
        public bool IsEnabledTelemetry { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppExecuteSettingData setting;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var appExecuteSettingEntityDao = new AppExecuteSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                setting = appExecuteSettingEntityDao.SelectSettingExecuteSetting();
            }

            UserId = setting.UserId;
            IsEnabledTelemetry = setting.IsEnabledTelemetry;

            var userIdManager = new UserIdManager(LoggerFactory);
            if(!userIdManager.IsValidUserId(UserId)) {
                UserId = userIdManager.CreateFromEnvironment();
            }
        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
            var appExecuteSettingEntityDao = new AppExecuteSettingEntityDao(commandPack.Main.Commander, StatementLoader, commandPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppExecuteSettingData() {
                IsEnabledTelemetry = IsEnabledTelemetry,
                UserId = UserId,
            };
            appExecuteSettingEntityDao.UpdateSettingExecuteSetting(data, commandPack.CommonStatus);
        }

        #endregion
    }


    public class AppGeneralSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppGeneralSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property

        public CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;
        public string UserBackupDirectoryPath { get; set; } = string.Empty;


        public bool IsRegisterStartup { get; set; }
        public bool DelayStartup { get; set; }
        public TimeSpan StartupWaitTime { get; set; }
        public string StartupArgument { get; set; } = string.Empty;


        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppGeneralSettingData setting;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var appGeneralSettingEntityDao = new AppGeneralSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                setting = appGeneralSettingEntityDao.SelectSettingGeneralSetting();
            }

            CultureInfo = CultureInfo.GetCultureInfo(setting.Language);

            UserBackupDirectoryPath = setting.UserBackupDirectoryPath;

            // スタートアップ取得
            var startupRegister = new StartupRegister(LoggerFactory);
            var startupParameterResult = startupRegister.GetStartupParameter();
            if(startupParameterResult.Success) {
                var startupParameter = startupParameterResult.SuccessValue!;
                IsRegisterStartup = true;
                DelayStartup = startupParameter.DelayStartup;
                StartupWaitTime = startupParameter.StartupWaitTime;
                StartupArgument = startupParameter.Argument;
            } else {
                IsRegisterStartup = false;
                DelayStartup = false;
                StartupWaitTime = TimeSpan.FromSeconds(3);
                StartupArgument = string.Empty;
            }
        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
            var appGeneralSettingEntityDao = new AppGeneralSettingEntityDao(commandPack.Main.Commander, StatementLoader, commandPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppGeneralSettingData() {
                Language = CultureInfo.Name,
                UserBackupDirectoryPath = UserBackupDirectoryPath,
            };
            appGeneralSettingEntityDao.UpdateSettingGeneralSetting(data, commandPack.CommonStatus);

            var startupRegister = new StartupRegister(LoggerFactory);
            if(IsRegisterStartup) {
                var startupParameter = new StartupParameter() {
                    DelayStartup = DelayStartup,
                    StartupWaitTime = StartupWaitTime,
                    Argument = StartupArgument,
                };
                startupRegister.Register(startupParameter);
            } else {
                startupRegister.Unregister();
            }
        }

        #endregion
    }


    public class AppUpdateSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppUpdateSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property

        public UpdateKind UpdateKind { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppUpdateSettingData setting;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var appUpdateSettingEntityDao = new AppUpdateSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                setting = appUpdateSettingEntityDao.SelectSettingUpdateSetting();
            }

            UpdateKind = setting.UpdateKind;
        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
            var appUpdateSettingEntityDao = new AppUpdateSettingEntityDao(commandPack.Main.Commander, StatementLoader, commandPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppUpdateSettingData() {
                UpdateKind = UpdateKind,
            };
            appUpdateSettingEntityDao.UpdateSettingUpdateSetting(data, commandPack.CommonStatus);
        }

        #endregion
    }



    public class AppCommandSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppCommandSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property

        //public Guid FontId { get; set; }
        public FontElement? Font { get; private set; }
        public IconBox IconBox { get; set; }
        public double Width { get; set; }
        public TimeSpan HideWaitTime { get; set; }
        public bool FindTag { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppCommandSettingData setting;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var appCommandSettingEntityDao = new AppCommandSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                setting = appCommandSettingEntityDao.SelectSettingCommandSetting();
            }

            Font = new FontElement(setting.FontId, MainDatabaseBarrier, StatementLoader, LoggerFactory);
            Font.Initialize();

            IconBox = setting.IconBox;
            Width = setting.Width;
            HideWaitTime = setting.HideWaitTime;
            FindTag = setting.FindTag;
        }

        protected override void SaveImpl(DatabaseCommandPack commadPack)
        {
            Debug.Assert(Font != null);

            var appCommandSettingEntityDao = new AppCommandSettingEntityDao(commadPack.Main.Commander, StatementLoader, commadPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppCommandSettingData() {
                FontId = Font.FontId,
                IconBox = IconBox,
                Width = Width,
                HideWaitTime = HideWaitTime,
                FindTag = FindTag,
            };
            appCommandSettingEntityDao.UpdateSettingCommandSetting(data, commadPack.CommonStatus);

            var fontsEntityDao = new FontsEntityDao(commadPack.Main.Commander, StatementLoader, commadPack.Main.Implementation, LoggerFactory);
            fontsEntityDao.UpdateFont(Font.FontId, Font.FontData, commadPack.CommonStatus);

        }

        #endregion
    }



    public class AppNoteSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppNoteSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property

        public FontElement? Font { get; private set; }
        public NoteCreateTitleKind TitleKind { get; set; }
        public NoteLayoutKind LayoutKind { get; set; }
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public bool IsTopmost { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppNoteSettingData setting;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var appNoteSettingEntityDao = new AppNoteSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                setting = appNoteSettingEntityDao.SelectSettingNoteSetting();
            }

            Font = new FontElement(setting.FontId, MainDatabaseBarrier, StatementLoader, LoggerFactory);
            Font.Initialize();

            TitleKind = setting.TitleKind;
            LayoutKind = setting.LayoutKind;
            ForegroundColor = setting.ForegroundColor;
            BackgroundColor = setting.BackgroundColor;
            IsTopmost = setting.IsTopmost;
        }

        protected override void SaveImpl(DatabaseCommandPack commadPack)
        {
            Debug.Assert(Font != null);

            var appNoteSettingEntityDao = new AppNoteSettingEntityDao(commadPack.Main.Commander, StatementLoader, commadPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppNoteSettingData() {
                FontId = Font.FontId,
                TitleKind = TitleKind,
                LayoutKind = LayoutKind,
                ForegroundColor = ForegroundColor,
                BackgroundColor = BackgroundColor,
                IsTopmost = IsTopmost,
            };
            appNoteSettingEntityDao.UpdateSettingNoteSetting(data, commadPack.CommonStatus);

            var fontsEntityDao = new FontsEntityDao(commadPack.Main.Commander, StatementLoader, commadPack.Main.Implementation, LoggerFactory);
            fontsEntityDao.UpdateFont(Font.FontId, Font.FontData, commadPack.CommonStatus);

        }

        #endregion
    }



    public class AppStandardInputOutputSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppStandardInputOutputSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property

        public FontElement? Font { get; private set; }
        public Color OutputForegroundColor { get; set; }
        public Color OutputBackgroundColor { get; set; }
        public Color ErrorForegroundColor { get; set; }
        public Color ErrorBackgroundColor { get; set; }
        public bool IsTopmost { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppStandardInputOutputSettingData setting;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var appStandardInputOutputSettingEntityDao = new AppStandardInputOutputSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                setting = appStandardInputOutputSettingEntityDao.SelectSettingStandardInputOutputSetting();
            }

            Font = new FontElement(setting.FontId, MainDatabaseBarrier, StatementLoader, LoggerFactory);
            Font.Initialize();

            OutputForegroundColor = setting.OutputForegroundColor;
            OutputBackgroundColor = setting.OutputBackgroundColor;
            ErrorForegroundColor = setting.ErrorForegroundColor;
            ErrorBackgroundColor = setting.ErrorBackgroundColor;
            IsTopmost = setting.IsTopmost;
        }

        protected override void SaveImpl(DatabaseCommandPack commadPack)
        {
            Debug.Assert(Font != null);

            var appStandardInputOutputSettingEntityDao = new AppStandardInputOutputSettingEntityDao(commadPack.Main.Commander, StatementLoader, commadPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppStandardInputOutputSettingData() {
                FontId = Font.FontId,
                OutputForegroundColor = OutputForegroundColor,
                OutputBackgroundColor = OutputBackgroundColor,
                ErrorForegroundColor = ErrorForegroundColor,
                ErrorBackgroundColor = ErrorBackgroundColor,
                IsTopmost = IsTopmost,
            };
            appStandardInputOutputSettingEntityDao.UpdateSettingStandardInputOutputSetting(data, commadPack.CommonStatus);

            var fontsEntityDao = new FontsEntityDao(commadPack.Main.Commander, StatementLoader, commadPack.Main.Implementation, LoggerFactory);
            fontsEntityDao.UpdateFont(Font.FontId, Font.FontData, commadPack.CommonStatus);

        }


        #endregion
    }



}