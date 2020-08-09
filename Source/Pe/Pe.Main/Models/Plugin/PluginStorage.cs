using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public abstract class PluginFileStorageBase
    {
        protected PluginFileStorageBase(DirectoryInfo directoryInfo)
        {
            DirectoryInfo = directoryInfo;
        }

        #region property

        protected DirectoryInfo DirectoryInfo { get; }

        #endregion

        #region function

        protected string TuneFileName(string name)
        {
            if(name == null) {
                throw new ArgumentNullException(nameof(name));
            }

            if(string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException(nameof(name));
            }

            var s = name.Trim();
            var cs = Path.GetInvalidFileNameChars();
            if(s.Any(i => cs.Any(cc => cc == i))) {
                throw new ArgumentException(nameof(name));
            }

            return s;
        }

        protected string CombinePath(string directoryName, string fileName)
        {
            var path = string.IsNullOrEmpty(directoryName)
                ? Path.Combine(DirectoryInfo.FullName, fileName)
                : Path.Combine(DirectoryInfo.FullName, directoryName, fileName)
            ;
            return path;
        }

        protected bool Exists(string directoryName, string fileName)
        {
            Debug.Assert(directoryName != null);

            var tunedFileName = TuneFileName(fileName);
            var path = CombinePath(directoryName, tunedFileName);

            return File.Exists(path);
        }

        protected void Rename(string directoryName, string sourceName, string destinationName, bool overwrite)
        {
            Debug.Assert(directoryName != null);

            if(sourceName == destinationName) {
                throw new ArgumentException($"{nameof(sourceName)} == {nameof(destinationName)}");
            }

            var tunedSourceFileName = TuneFileName(sourceName);
            var tunedDestinationFileName = TuneFileName(destinationName);

            var tunedSourceFilePath = CombinePath(directoryName, tunedSourceFileName);
            var tunedDestinationFilePath = CombinePath(directoryName, tunedDestinationFileName);

            File.Move(tunedSourceFilePath, tunedDestinationFilePath, overwrite);
        }

        protected void Copy(string directoryName, string sourceName, string destinationName, bool overwrite)
        {
            Debug.Assert(directoryName != null);

            if(sourceName == destinationName) {
                throw new ArgumentException($"{nameof(sourceName)} == {nameof(destinationName)}");
            }

            var tunedSourceFileName = TuneFileName(sourceName);
            var tunedDestinationFileName = TuneFileName(destinationName);

            var tunedSourceFilePath = CombinePath(directoryName, tunedSourceFileName);
            var tunedDestinationFilePath = CombinePath(directoryName, tunedDestinationFileName);

            File.Copy(tunedSourceFilePath, tunedDestinationFilePath, overwrite);
        }

        protected void Delete(string directoryName, string name)
        {
            Debug.Assert(directoryName != null);

            var tunedFileName = TuneFileName(name);
            var path = CombinePath(directoryName, tunedFileName);
            File.Delete(path);
        }

        protected Stream Open(string directoryName, string name, FileMode fileMode)
        {
            Debug.Assert(directoryName != null);

            var tunedFileName = TuneFileName(name);
            var path = CombinePath(directoryName, tunedFileName);
            FileUtility.MakeFileParentDirectory(path);
            var stream = new FileStream(path, fileMode, FileAccess.ReadWrite, FileShare.Read);
            return stream;
        }

        #endregion
    }

    /// <inheritdoc cref="IPluginFileStorage"/>
    public class PluginFileStorage: PluginFileStorageBase, IPluginFileStorage
    {
        public PluginFileStorage(DirectoryInfo directoryInfo)
            : base(directoryInfo)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region IPluginFileStorage

        /// <inheritdoc cref="IPluginFileStorage.Exists(string)"/>
        public bool Exists(string name)
        {
            return Exists(string.Empty, name);
        }

        /// <inheritdoc cref="IPluginFileStorage.Rename(string, string, bool)"/>
        public void Rename(string sourceName, string destinationName, bool overwrite)
        {
            Rename(string.Empty, sourceName, destinationName, overwrite);
        }

        /// <inheritdoc cref="IPluginFileStorage.Copy(string, string, bool)"/>
        public void Copy(string sourceName, string destinationName, bool overwrite)
        {
            Copy(string.Empty, sourceName, destinationName, overwrite);
        }

        /// <inheritdoc cref="IPluginFileStorage.Delete(string)"/>
        public void Delete(string name)
        {
            Delete(string.Empty, name);
        }

        /// <inheritdoc cref="IPluginFileStorage.Open(string, FileMode)"/>
        public Stream Open(string name, FileMode fileMode)
        {
            return Open(string.Empty, name, fileMode);
        }

        #endregion
    }

    public enum PluginPersistentMode
    {
        /// <summary>
        /// 上流に影響される読み書き。
        /// <para>書き込みは状況により不可。</para>
        /// </summary>
        Commander,
        /// <summary>
        /// 単独実施する読み書き。
        /// <para>書き込みは状況により不可。</para>
        /// </summary>
        Barrier,
        /// <summary>
        /// 遅延書き込み。
        /// </summary>
        LazyWriter,
    }

    public abstract class PluginPersistentStorageBase
    {
        /// <summary>
        /// プラグイン用DB操作処理構築。
        /// <para>読み込み専用・書き込み可能に分かれる。書き込み専用は基本的に設定画面くらい。</para>
        /// </summary>
        /// <param name="pluginIdentifiers"></param>
        /// <param name="pluginVersions"></param>
        /// <param name="databaseCommands"></param>
        /// <param name="databaseStatementLoader"></param>
        /// <param name="isReadOnly">読み込み専用か。</param>
        /// <param name="loggerFactory"></param>
        protected PluginPersistentStorageBase(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseCommands databaseCommands, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            DatabaseCommander = databaseCommands.Commander;
            DatabaseImplementation = databaseCommands.Implementation;
            IsReadOnly = isReadOnly;
            DatabaseStatementLoader = databaseStatementLoader;
            Mode = PluginPersistentMode.Commander;
        }

        /// <summary>
        /// プラグイン用DB操作処理構築。
        /// <para>読み込み専用・書き込み可能に分かれる。逐次処理される。</para>
        /// </summary>
        /// <param name="pluginIdentifiers"></param>
        /// <param name="pluginVersions"></param>
        /// <param name="databaseBarrier"></param>
        /// <param name="databaseStatementLoader"></param>
        /// <param name="isReadOnly">読み込み専用か。</param>
        /// <param name="loggerFactory"></param>
        protected PluginPersistentStorageBase(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            DatabaseBarrier = databaseBarrier;
            IsReadOnly = isReadOnly;
            DatabaseStatementLoader = databaseStatementLoader;
            Mode = PluginPersistentMode.Barrier;
        }

        /// <summary>
        /// プラグイン用DB操作処理構築。
        /// <para>通常実行時の書き込み処理で、遅延処理される。</para>
        /// </summary>
        /// <param name="pluginIdentifiers"></param>
        /// <param name="pluginVersions"></param>
        /// <param name="databaseBarrier"></param>
        /// <param name="databaseLazyWriter"></param>
        /// <param name="databaseStatementLoader"></param>
        /// <param name="loggerFactory"></param>
        protected PluginPersistentStorageBase(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseLazyWriter databaseLazyWriter, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            DatabaseBarrier = databaseBarrier;
            DatabaseLazyWriter = databaseLazyWriter;
            DatabaseStatementLoader = databaseStatementLoader;
            IsReadOnly = false;
            Mode = PluginPersistentMode.LazyWriter;
        }

        #region property

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        IPluginIdentifiers PluginIdentifiers { get; }
        public string PluginName => PluginIdentifiers.PluginName;
        protected IPluginVersions PluginVersions { get; }

        protected PluginPersistentMode Mode { get; }
        protected IDatabaseImplementation? DatabaseImplementation { get; }
        protected IDatabaseCommander? DatabaseCommander { get; }
        protected IDatabaseBarrier? DatabaseBarrier { get; }
        protected IDatabaseLazyWriter? DatabaseLazyWriter { get; }
        protected IDatabaseStatementLoader DatabaseStatementLoader { get; }

        public bool IsReadOnly { get; }
        #endregion
    }

    /// <inheritdoc cref="IPluginPersistentStorage"/>
    public sealed class PluginPersistentStorage: IPluginPersistentStorage, IPluginId
    {
        /// <summary>
        /// プラグイン用DB操作処理構築。
        /// <para>読み込み専用・書き込み可能に分かれる。書き込み専用は基本的に設定画面くらい。</para>
        /// </summary>
        /// <param name="pluginIdentifiers"></param>
        /// <param name="pluginVersions"></param>
        /// <param name="databaseCommands"></param>
        /// <param name="databaseStatementLoader"></param>
        /// <param name="isReadOnly">読み込み専用か。</param>
        /// <param name="loggerFactory"></param>
        public PluginPersistentStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseCommands databaseCommands, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            DatabaseCommander = databaseCommands.Commander;
            DatabaseImplementation = databaseCommands.Implementation;
            IsReadOnly = isReadOnly;
            DatabaseStatementLoader = databaseStatementLoader;
            Mode = PluginPersistentMode.Commander;
        }

        /// <summary>
        /// プラグイン用DB操作処理構築。
        /// <para>読み込み専用・書き込み可能に分かれる。逐次処理される。</para>
        /// </summary>
        /// <param name="pluginIdentifiers"></param>
        /// <param name="pluginVersions"></param>
        /// <param name="databaseBarrier"></param>
        /// <param name="databaseStatementLoader"></param>
        /// <param name="isReadOnly">読み込み専用か。</param>
        /// <param name="loggerFactory"></param>
        public PluginPersistentStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            DatabaseBarrier = databaseBarrier;
            IsReadOnly = isReadOnly;
            DatabaseStatementLoader = databaseStatementLoader;
            Mode = PluginPersistentMode.Barrier;
        }

        /// <summary>
        /// プラグイン用DB操作処理構築。
        /// <para>通常実行時の書き込み処理で、遅延処理される。</para>
        /// </summary>
        /// <param name="pluginIdentifiers"></param>
        /// <param name="pluginVersions"></param>
        /// <param name="databaseBarrier"></param>
        /// <param name="databaseLazyWriter"></param>
        /// <param name="databaseStatementLoader"></param>
        /// <param name="loggerFactory"></param>
        public PluginPersistentStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseLazyWriter databaseLazyWriter, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            DatabaseBarrier = databaseBarrier;
            DatabaseLazyWriter = databaseLazyWriter;
            DatabaseStatementLoader = databaseStatementLoader;
            IsReadOnly = false;
            Mode = PluginPersistentMode.LazyWriter;
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

        IPluginIdentifiers PluginIdentifiers { get; }
        public string PluginName => PluginIdentifiers.PluginName;
        IPluginVersions PluginVersions { get; }

        PluginPersistentMode Mode { get; }
        IDatabaseImplementation? DatabaseImplementation { get; }
        IDatabaseCommander? DatabaseCommander { get; }
        IDatabaseBarrier? DatabaseBarrier { get; }
        IDatabaseLazyWriter? DatabaseLazyWriter { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }
        #endregion

        #region function

        string NormalizeKey(string key)
        {
            return string.IsNullOrWhiteSpace(key)
                ? string.Empty
                : key.Trim()
            ;
        }

        #endregion

        #region IPluginPersistentStorage

        /// <inheritdoc cref="IPluginPersistentStorage.IsReadOnly"/>
        public bool IsReadOnly { get; }

        /// <inheritdoc cref="IPluginPersistentStorage.Exists(string)"/>
        public bool Exists(string key)
        {
            switch(Mode) {
                case PluginPersistentMode.Commander: {
                        Debug.Assert(DatabaseCommander != null);
                        Debug.Assert(DatabaseImplementation != null);

                        var pluginSettingsEntityDao = new PluginSettingsEntityDao(DatabaseCommander, DatabaseStatementLoader, DatabaseImplementation, LoggerFactory);
                        return pluginSettingsEntityDao.SelecteExistsPluginSetting(PluginId, NormalizeKey(key));
                    }

                case PluginPersistentMode.Barrier:
                case PluginPersistentMode.LazyWriter: {
                        Debug.Assert(DatabaseBarrier != null);

                        if(Mode == PluginPersistentMode.LazyWriter) {
                            Debug.Assert(DatabaseLazyWriter != null);
                            DatabaseLazyWriter.Flush();
                        }

                        return DatabaseBarrier.ReadData(c => {
                            var pluginSettingsEntityDao = new PluginSettingsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                            return pluginSettingsEntityDao.SelecteExistsPluginSetting(PluginId, NormalizeKey(key));
                        });
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        /// <inheritdoc cref="IPluginPersistentStorage.TryGet{TValue}(string, out TValue)"/>
        public bool TryGet<TValue>(string key, [MaybeNullWhen(returnValue: false)] out TValue value)
        {
            if(Mode == PluginPersistentMode.LazyWriter) {
                // 遅延書き込み待機を終了
                Debug.Assert(DatabaseLazyWriter != null);
                DatabaseLazyWriter.Flush();
            }

            PluginSettingRawValue? data;
            switch(Mode) {
                case PluginPersistentMode.Commander: {
                        Debug.Assert(DatabaseCommander != null);
                        Debug.Assert(DatabaseImplementation != null);

                        var pluginSettingsEntityDao = new PluginSettingsEntityDao(DatabaseCommander, DatabaseStatementLoader, DatabaseImplementation, LoggerFactory);
                        data = pluginSettingsEntityDao.SelectPluginSettingValue(PluginId, NormalizeKey(key));
                    }
                    break;

                case PluginPersistentMode.Barrier:
                case PluginPersistentMode.LazyWriter: {
                        Debug.Assert(DatabaseBarrier != null);

                        if(Mode == PluginPersistentMode.LazyWriter) {
                            Debug.Assert(DatabaseLazyWriter != null);
                            DatabaseLazyWriter.Flush();
                        }

                        data = DatabaseBarrier.ReadData(c => {
                            var pluginSettingsEntityDao = new PluginSettingsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                            return pluginSettingsEntityDao.SelectPluginSettingValue(PluginId, NormalizeKey(key));
                        });
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            if(data == null) {
                value = default;
                return false;
            }

            switch(data.Format) {
                case PluginPersistentFormat.SimpleXml:
                case PluginPersistentFormat.DataXml: {
                        SerializerBase serializer = data.Format switch
                        {
                            PluginPersistentFormat.SimpleXml => new XmlSerializer(),
                            PluginPersistentFormat.DataXml => new XmlDataContractSerializer(),
                            _ => throw new NotImplementedException(),
                        };
                        try {
                            var binary = Encoding.UTF8.GetBytes(data.Value);
                            using(var stream = new MemoryStream(binary)) {
                                value = serializer.Load<TValue>(stream);
                                return true;
                            }
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                            value = default;
                            return false;
                        }
                    }

                case PluginPersistentFormat.Json: {
                        try {
                            value = JsonSerializer.Deserialize<TValue>(data.Value);
                            return true;
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                            value = default;
                            return false;
                        }
                    }

                case PluginPersistentFormat.Text: {
                        if(typeof(TValue) != typeof(string)) {
                            Logger.LogWarning("文字列であるべきデータ: {0} -> {1}", nameof(value), typeof(TValue));
                            value = default;
                            return false;
                        }

                        value = (TValue)(object)data.Value;
                        return true;
                    }

                default:
                    throw new NotImplementedException();
            }

        }

        /// <inheritdoc cref="IPluginPersistentStorage.Set{TValue}(string, TValue, PluginPersistentFormat)"/>
        public bool Set<TValue>(string key, TValue value, PluginPersistentFormat format)
        {
            if(IsReadOnly) {
                throw new InvalidOperationException(nameof(IsReadOnly));
            }

            if(value == null) {
                Logger.LogWarning("value は null のため処理終了");
                return false;
            }

            string textValue;

            switch(format) {
                case PluginPersistentFormat.SimpleXml:
                case PluginPersistentFormat.DataXml: {
                        SerializerBase serializer = format switch
                        {
                            PluginPersistentFormat.SimpleXml => new XmlSerializer(),
                            PluginPersistentFormat.DataXml => new XmlDataContractSerializer(),
                            _ => throw new NotImplementedException(),
                        };
                        try {
                            using(var stream = new MemoryStream()) {
                                serializer.Save(value, stream);
                                textValue = serializer.Encoding.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                            }
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                            return false;
                        }
                    }
                    break;

                case PluginPersistentFormat.Json: {
                        try {
                            textValue = JsonSerializer.Serialize(value);
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                            return false;
                        }
                    }
                    break;

                case PluginPersistentFormat.Text: {
                        if(typeof(TValue) != typeof(string)) {
                            Logger.LogWarning("文字列であるべきデータ: {0} -> {1}", nameof(value), typeof(TValue));
                            textValue = value.ToString()! ?? string.Empty;
                        } else {
                            textValue = (string)(object)value;
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            var data = new PluginSettingRawValue(format, textValue);

            switch(Mode) {
                case PluginPersistentMode.Commander: {
                        Debug.Assert(DatabaseCommander != null);
                        Debug.Assert(DatabaseImplementation != null);

                        var pluginSettingsEntityDao = new PluginSettingsEntityDao(DatabaseCommander, DatabaseStatementLoader, DatabaseImplementation, LoggerFactory);

                        var normalizedKey = NormalizeKey(key);
                        if(pluginSettingsEntityDao.SelecteExistsPluginSetting(PluginId, normalizedKey)) {
                            pluginSettingsEntityDao.UpdatePluginSetting(PluginId, normalizedKey, data, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                        } else {
                            pluginSettingsEntityDao.InsertPluginSetting(PluginId, normalizedKey, data, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                        }
                    }
                    break;

                case PluginPersistentMode.Barrier: {
                        Debug.Assert(DatabaseBarrier != null);

                        using(var commander = DatabaseBarrier.WaitWrite()) {
                            var pluginSettingsEntityDao = new PluginSettingsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                            var normalizedKey = NormalizeKey(key);
                            if(pluginSettingsEntityDao.SelecteExistsPluginSetting(PluginId, normalizedKey)) {
                                pluginSettingsEntityDao.UpdatePluginSetting(PluginId, normalizedKey, data, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                            } else {
                                pluginSettingsEntityDao.InsertPluginSetting(PluginId, normalizedKey, data, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                            }
                            commander.Commit();
                        }
                    }
                    break;

                case PluginPersistentMode.LazyWriter: {
                        Debug.Assert(DatabaseLazyWriter != null);

                        DatabaseLazyWriter.Stock(c => {
                            var pluginSettingsEntityDao = new PluginSettingsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                            var normalizedKey = NormalizeKey(key);
                            if(pluginSettingsEntityDao.SelecteExistsPluginSetting(PluginId, normalizedKey)) {
                                pluginSettingsEntityDao.UpdatePluginSetting(PluginId, normalizedKey, data, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                            } else {
                                pluginSettingsEntityDao.InsertPluginSetting(PluginId, normalizedKey, data, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                            }
                        });
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            return true;
        }
        /// <inheritdoc cref="IPluginPersistentStorage.Set{TValue}(string, TValue)"/>
        public bool Set<TValue>(string key, TValue value) => Set(key, value, PluginPersistentFormat.Json);

        /// <inheritdoc cref="IPluginPersistentStorage.Delete(string)"/>
        public bool Delete(string key)
        {
            if(IsReadOnly) {
                throw new InvalidOperationException(nameof(IsReadOnly));
            }

            if(Mode == PluginPersistentMode.LazyWriter) {
                // 遅延書き込み待機を終了
                Debug.Assert(DatabaseLazyWriter != null);
                DatabaseLazyWriter.Flush();
            }

            switch(Mode) {
                case PluginPersistentMode.Commander: {
                        Debug.Assert(DatabaseCommander != null);
                        Debug.Assert(DatabaseImplementation != null);

                        var pluginSettingsEntityDao = new PluginSettingsEntityDao(DatabaseCommander, DatabaseStatementLoader, DatabaseImplementation, LoggerFactory);
                        return pluginSettingsEntityDao.DeletePluginSetting(PluginId, NormalizeKey(key));
                    }

                case PluginPersistentMode.Barrier: {
                        Debug.Assert(DatabaseBarrier != null);

                        using(var commander = DatabaseBarrier.WaitWrite()) {
                            var pluginSettingsEntityDao = new PluginSettingsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                            var result = pluginSettingsEntityDao.DeletePluginSetting(PluginId, NormalizeKey(key));
                            commander.Commit();
                            return result;
                        }
                    }

                case PluginPersistentMode.LazyWriter: {
                        Debug.Assert(DatabaseLazyWriter != null);

                        DatabaseLazyWriter.Stock(c => {
                            var pluginSettingsEntityDao = new PluginSettingsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                            pluginSettingsEntityDao.DeletePluginSetting(PluginId, NormalizeKey(key));
                        });
                        // 成功したかどうか不明
                        return false;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region IPluginId

        public Guid PluginId => PluginIdentifiers.PluginId;

        #endregion
    }

    /// <inheritdoc cref="IPluginFiles"/>
    public class PluginFiles: IPluginFiles
    {
        public PluginFiles(PluginFileStorage user, PluginFileStorage machine, PluginFileStorage temporary)
        {
            User = user;
            Machine = machine;
            Temporary = temporary;
        }

        #region IPluginFile

        /// <inheritdoc cref="IPluginFiles.User"/>
        public PluginFileStorage User { get; }
        IPluginFileStorage IPluginFiles.User => User;
        /// <inheritdoc cref="IPluginFiles.Machine"/>
        public PluginFileStorage Machine { get; }
        IPluginFileStorage IPluginFiles.Machine => Machine;
        /// <inheritdoc cref="IPluginFiles.Temporary"/>
        public PluginFileStorage Temporary { get; }
        IPluginFileStorage IPluginFiles.Temporary => Temporary;

        #endregion
    }

    /// <inheritdoc cref="IPluginPersistents"/>
    public class PluginPersistents: IPluginPersistents
    {
        public PluginPersistents(PluginPersistentStorage normal, PluginPersistentStorage large, PluginPersistentStorage temporary)
        {
            Normal = normal;
            Large = large;
            Temporary = temporary;
        }

        #region IPluginPersistent

        /// <inheritdoc cref="IPluginPersistents.Normal"/>
        public PluginPersistentStorage Normal { get; }
        IPluginPersistentStorage IPluginPersistents.Normal => Normal;
        /// <inheritdoc cref="IPluginPersistents.Large"/>
        public PluginPersistentStorage Large { get; }
        IPluginPersistentStorage IPluginPersistents.Large => Large;
        /// <inheritdoc cref="IPluginPersistents.Temporary"/>
        public PluginPersistentStorage Temporary { get; }
        IPluginPersistentStorage IPluginPersistents.Temporary => Temporary;

        #endregion
    }

    /// <inheritdoc cref="IPluginStorage"/>
    public class PluginStorage: IPluginStorage
    {
        public PluginStorage(PluginFiles file, PluginPersistents persistent)
        {
            File = file;
            Persistent = persistent;
        }

        #region IPluginStorage

        /// <inheritdoc cref="IPluginStorage.File"/>
        public PluginFiles File { get; }
        IPluginFiles IPluginStorage.File => File;
        /// <inheritdoc cref="IPluginStorage.Persistent"/>
        public PluginPersistents Persistent { get; }
        IPluginPersistents IPluginStorage.Persistent => Persistent;

        #endregion
    }
}
