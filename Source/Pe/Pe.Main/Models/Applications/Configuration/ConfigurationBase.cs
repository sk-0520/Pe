using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    internal readonly struct ConfigurationSetting
    {
        public ConfigurationSetting(FieldInfo field, PropertyInfo property, ConfigurationAttribute configuration)
        {
            Field = field;
            Property = property;
            Configuration = configuration;
        }

        #region property

        public FieldInfo Field { get; }
        public PropertyInfo Property { get; }
        public ConfigurationAttribute Configuration { get; }

        #endregion
    }

    /// <summary>
    /// アプリケーション構成ファイルの読み込み基底処理。
    /// <para>すべてをコンストラクタで処理し、失敗時は例外を投げて先に進ませないようにする。</para>
    /// <para>バッキングフィールド名を信じてえんやこらさっさ。</para>
    /// </summary>
    public abstract class ConfigurationBase
    {
        /// <summary>
        /// 規約として get-only プロパティ を対象とする。
        /// </summary>
        /// <param name="conf"></param>
        protected ConfigurationBase(IConfiguration conf)
        {
            var type = GetType();
            var properties = type.GetProperties()
                .ToDictionary(i => i.Name, i => i)
            ;
            var items =
                type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField)
                .Where(i => i.MemberType == MemberTypes.Field)
                .Cast<FieldInfo>()
                .Select(i => (field: i, attribute: i.GetCustomAttribute<CompilerGeneratedAttribute>()))
                .Where(i => i.attribute != null)
                .Select(i => (i.field, propertyName: i.field.Name.Substring(1, i.field.Name.IndexOf('>') - 1)))
                .Where(i => properties.ContainsKey(i.propertyName))
                .Select(i => (i.field, property: properties[i.propertyName]))
                .Select(i => new ConfigurationSetting(i.field, i.property, i.property.GetCustomAttribute<ConfigurationAttribute>()!))
                .Where(i => i.Configuration != null)
            ;

            var nameConverter = new NameConveter();

            foreach(var item in items) {
                var memberKey = item.Configuration.MemberName.Length == 0
                    ? nameConverter.PascalToSnake(item.Property.Name)
                    : item.Configuration.MemberName
                ;
                if(memberKey == null) {
                    memberKey = nameConverter.PascalToSnake(item.Property.Name);
                }

                var result = GetValue(conf, memberKey, item.Field.FieldType);
                item.Field.SetValue(this, result);

                Debug.WriteLine("[{2}] {0}:{1} - `{3}' -> `{4}'", item.Field.Name, item.Property.Name, item.Field.FieldType, item.Configuration.MemberName, memberKey);
            }

#if DEBUG
            var checkHasConfiguration = false;
            foreach(var debugProperty in type.GetProperties().Where(i => i.GetCustomAttribute<ConfigurationAttribute>() != null)) {
                if(debugProperty.GetValue(this) == null) {
                    checkHasConfiguration = true;
                    Debug.WriteLine("default -> {0}", debugProperty);
                }
            }

            if(checkHasConfiguration) {
                throw new Exception();
            }
#endif
        }

        #region function

        private static object? GetValue(IConfiguration conf, string memberKey, Type valueType)
        {
            if(valueType.IsSubclassOf(typeof(ConfigurationBase))) {
                var childSection = conf.GetSection(memberKey);
                var result = Activator.CreateInstance(valueType, new[] { childSection });
                return result;
            } else if(valueType == typeof(string)) {
                if(string.IsNullOrEmpty(memberKey)) {
                    var getResult = conf.Get(typeof(string));
                    return getResult;
                }
                var result = conf.GetValue(valueType, memberKey);
                return result;
            } else {
                var result = conf.GetValue(valueType, memberKey);
                if(result == null) {
                    var childSection = conf.GetSection(memberKey);
                    if(childSection.Value == null) {
                        if(valueType.IsGenericType) {
                            // ReadOnlyList のみサポートする
                            var genArgs = valueType.GetGenericArguments();
                            Debug.Assert(genArgs != null);
                            var genIndex = valueType.Name.IndexOf('`');
                            var genName = valueType.Name.Substring(0, genIndex);
                            switch(genName) {
                                case "IReadOnlyList": {
                                        var childrenRaws = childSection.GetChildren().ToList();
                                        var childrenValues = Array.CreateInstance(genArgs[0], childrenRaws.Count);
                                        foreach(var child in childrenRaws.Counting()) {
                                            var childValue = GetValue(child.Value, string.Empty, genArgs[0]);
                                            childrenValues.SetValue(childValue, child.Number);
                                        }
                                        return childrenValues;
                                    }

                                default:
                                    break;
                            }
                        }
                    }
                    throw new Exception($"{childSection.Path}: {valueType}");
                } else {
                    return result;
                }
            }
        }

        protected static IReadOnlyList<T> GetList<T>(IConfigurationSection section, string key)
        {
            return section.GetSection(key).Get<T[]>();
        }

        protected static Size GetSize(IConfigurationSection section, string key)
        {
            var size = section.GetSection(key);
            return new Size(size.GetValue<double>("width"), size.GetValue<double>("height"));
        }

        protected static MinMax<T> GetMinMax<T>(IConfigurationSection section, string key)
            where T : IComparable<T>
        {
            var size = section.GetSection(key);
            return new MinMax<T>(size.GetValue<T>("minimum"), size.GetValue<T>("maximum"));
        }
        protected static MinMaxDefault<T> GetMinMaxDefault<T>(IConfigurationSection section, string key)
            where T : IComparable<T>
        {
            var size = section.GetSection(key);
            return new MinMaxDefault<T>(size.GetValue<T>("minimum"), size.GetValue<T>("maximum"), size.GetValue<T>("default"));
        }

        #endregion
    }

}
