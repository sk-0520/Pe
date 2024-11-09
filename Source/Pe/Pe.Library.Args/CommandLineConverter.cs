using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Args
{
    /// <summary>
    /// コマンドラインをデータ構造にマッピング。
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class CommandLineConverter<TData>
        where TData : class
    {
        public CommandLineConverter(CommandLine commandLine, TData data)
        {
            CommandLine = commandLine;
            Data = data;
        }

        #region property

        public CommandLine CommandLine { get; }
        public TData Data { get; }
        public Exception? Exception { get; private set; }

        #endregion

        #region function

        private IReadOnlyDictionary<PropertyInfo, CommandLineAttribute> GetPropertyAttributeMapping(Type type)
        {
            var properties = type.GetProperties();

            var map = new Dictionary<PropertyInfo, CommandLineAttribute>(properties.Length);
            foreach(var property in properties) {
                var attributes = property.GetCustomAttributes(typeof(CommandLineAttribute), true);
                if(attributes != null && attributes.Any()) {
                    map.Add(property, attributes.OfType<CommandLineAttribute>().First());
                }
            }

            return map;
        }

        private IReadOnlyDictionary<PropertyInfo, CommandLineKey> SetPropertyKeyMapping(CommandLine commandLine, IReadOnlyDictionary<PropertyInfo, CommandLineAttribute> propertyAttributeMap)
        {
            var map = new Dictionary<PropertyInfo, CommandLineKey>();
            foreach(var pair in propertyAttributeMap) {
                var attribute = pair.Value;
                var key = commandLine.Add(attribute.LongKey, attribute.HasValue, attribute.Description);
                map.Add(pair.Key, key);
            }

            return map;
        }

        object? ConvertValue(Type type, ICommandLineValue value)
        {
            if(type == typeof(float)) {
                return float.Parse(value.First, CultureInfo.InvariantCulture);
            }
            if(type == typeof(double)) {
                return double.Parse(value.First, CultureInfo.InvariantCulture);
            }

            if(1 < value.Items.Count) {
                if(type.IsArray) {
                    return value.Items.ToArray();
                }
                if(typeof(System.Collections.IList).IsAssignableFrom(type)) {
                    return value.Items.ToList();
                }
                if(typeof(System.Collections.IEnumerable).IsAssignableFrom(type)) {
                    return value.Items;
                }
            }

            return Convert.ChangeType(value.First, type, CultureInfo.InvariantCulture);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        private object GetTrueSwitch(Type type)
        {
            if(type == typeof(bool)) {
                return true;
            }

            if(type == typeof(char)) {
                return 'Y';
            }

            if(type == typeof(string)) {
                return true.ToString();
            }

            var numTypes = new[] {
                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
            };

            if(Array.IndexOf(numTypes, type) != -1) {
                return 1;
            }

            if(type == typeof(decimal)) {
                return 1m;
            }

            throw new NotImplementedException();
        }

        protected bool MappingCore()
        {
            var type = Data.GetType();
            var attributeMap = GetPropertyAttributeMapping(type);

            var keyMap = SetPropertyKeyMapping(CommandLine, attributeMap);
            try {
                if(CommandLine.Parse()) {
                    foreach(var pair in keyMap) {
                        if(pair.Value.HasValue) {
                            // 値取得
                            if(CommandLine.Values.TryGetValue(pair.Value, out var value)) {
                                var convertedValue = ConvertValue(pair.Key.PropertyType, value);
                                pair.Key.SetValue(Data, convertedValue);
                            }
                        } else {
                            // スイッチ
                            if(CommandLine.Switches.Contains(pair.Value)) {
                                var switchValue = GetTrueSwitch(pair.Key.PropertyType);
                                pair.Key.SetValue(Data, switchValue);
                            }
                        }
                    }
                }

                return true;
            } catch(Exception ex) {
                Exception = ex;
                return false;
            }
        }

        /// <summary>
        /// <see cref="Data"/>へマッピング。
        /// </summary>
        /// <returns>成功か。</returns>
        public virtual bool Mapping()
        {
            return MappingCore();
        }

        #endregion
    }
}
