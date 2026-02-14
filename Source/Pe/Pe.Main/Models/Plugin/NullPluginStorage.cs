using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <summary>
    /// すべてなかったことにする <see cref="IPluginFileStorage"/>。
    /// </summary>
    internal sealed class NullPluginFileStorage: IPluginFileStorage
    {
        public NullPluginFileStorage(IPluginIdentifiers pluginIdentifiers, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
        }

        #region property

        private ILogger Logger { get; }
        private IPluginIdentifiers PluginIdentifiers { get; }

        #endregion

        #region IPluginFileStorage

        public void Copy(string sourceName, string destinationName, bool overwrite)
        {
            Logger.LogTrace(
                "{PluginName}({PluginId}): sourceName = {SourceName}, destinationName = {DestinationName}, overwrite = {Overwrite}",
                PluginIdentifiers.PluginName, PluginIdentifiers.PluginId,
                sourceName,
                destinationName,
                overwrite
            );
        }

        public void Delete(string name)
        {
            Logger.LogTrace("{PluginName}({PluginId}): name = {Name}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, name);
        }

        public bool Exists(string name)
        {
            Logger.LogTrace("{PluginName}({PluginId}): name = {Name}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, name);
            return true;
        }

        public Stream Open(string name, FileMode fileMode)
        {
            Logger.LogTrace("{PluginName}({PluginId}): name = {Name}, fileMode = {FileMode}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, name, fileMode);
            return Stream.Null;
        }

        public void Rename(string sourceName, string destinationName, bool overwrite)
        {
            Logger.LogTrace(
                "{PluginName}({PluginId}): sourceName = {SourceName}, destinationName = {DestinationName}, overwrite = {Overwrite}",
                PluginIdentifiers.PluginName, PluginIdentifiers.PluginId,
                sourceName,
                destinationName,
                overwrite
            );
        }

        #endregion
    }

    internal sealed class NullPluginPersistenceStorage: IPluginPersistenceStorage
    {
        public NullPluginPersistenceStorage(IPluginIdentifiers pluginIdentifiers, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
        }

        #region property

        private ILogger Logger { get; }
        private IPluginIdentifiers PluginIdentifiers { get; }

        #endregion

        #region IPluginPersistenceStorage

        public bool IsReadOnly => true;

        public IEnumerable<string> GetKeys()
        {
            Logger.LogTrace("{PluginName}({PluginId})", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId);
            return Array.Empty<string>();
        }

        public bool Delete(string key)
        {
            Logger.LogTrace("{PluginName}({PluginId}): key = {Key}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, key);
            return true;
        }

        public bool Exists(string key)
        {
            Logger.LogTrace("{PluginName}({PluginId}): key = {Key}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, key);
            return true;
        }

        public bool Set<TValue>(string key, TValue value, PluginPersistenceFormat format)
            where TValue : notnull
        {
            Logger.LogTrace(
                "{PluginName}({PluginId}): key = {Key}, value = {Value}, format = {Format}",
                PluginIdentifiers.PluginName, PluginIdentifiers.PluginId,
                key,
                value,
                format
            );
            return true;
        }

        public bool Set<TValue>(string key, TValue value)
            where TValue : notnull
        {
            return Set(key, value, PluginPersistenceFormat.Text);
        }

        public bool TryGet<TValue>(string key, [MaybeNullWhen(returnValue: false)] out TValue value)
        {
            Logger.LogTrace("{PluginName}({PluginId}): key = {Key}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, key);
            value = default;
            return false;
        }

        #endregion
    }

    internal sealed class NullPluginFiles: IPluginFiles
    {
        public NullPluginFiles(IPluginIdentifiers pluginIdentifiers, ILoggerFactory loggerFactory)
        {
            var nullStorage = new NullPluginFileStorage(pluginIdentifiers, loggerFactory);
            User = nullStorage;
            Machine = nullStorage;
            Temporary = nullStorage;
        }

        #region IPluginFiles

        public IPluginFileStorage User { get; }

        public IPluginFileStorage Machine { get; }

        public IPluginFileStorage Temporary { get; }

        #endregion
    }

    internal sealed class NullPluginPersistence: IPluginPersistence
    {
        public NullPluginPersistence(IPluginIdentifiers pluginIdentifiers, ILoggerFactory loggerFactory)
        {
            var nullStorage = new NullPluginPersistenceStorage(pluginIdentifiers, loggerFactory);
            Normal = nullStorage;
            Large = nullStorage;
            Temporary = nullStorage;
        }

        #region IPluginPersistence

        public IPluginPersistenceStorage Normal { get; }

        public IPluginPersistenceStorage Large { get; }

        public IPluginPersistenceStorage Temporary { get; }

        #endregion
    }

    internal sealed class NullPluginStorage: IPluginStorage
    {
        public NullPluginStorage(IPluginIdentifiers pluginIdentifiers, ILoggerFactory loggerFactory)
        {
            File = new NullPluginFiles(pluginIdentifiers, loggerFactory);
            Persistence = new NullPluginPersistence(pluginIdentifiers, loggerFactory);
        }

        #region IPluginStorage

        public IPluginFiles File { get; }

        public IPluginPersistence Persistence { get; }

        #endregion
    }
}
