using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Args
{
    /// <summary>
    /// コマンドライン引数のパース結果からオブジェクトに変換する。
    /// </summary>
    public class CommandLineConverter
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="loggerFactory"></param>
        public CommandLineConverter(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

        /// <summary>
        /// 変換処理一覧。
        /// </summary>
        /// <remarks>
        /// <para>必要に応じて消したり追加したりすること。</para>
        /// <para>実装メモ: 2026-01-06 時点では Pe で使用している型のみ対応(DateTime/TimeSpanは別に使ってないけど)。</para>
        /// </remarks>
        public List<ICommandLineConvertHandler> Handlers { get; } = [
            new CommandLineStringConvertHandler(),
            new CommandLineIntegerConvertHandler(),
            new CommandLineBooleanConvertHandler(),
            new CommandLineDateTimeConvertHandler(),
            new CommandLineTimeSpanConvertHandler(),
        ];

        #endregion

        #region function

        private IReadOnlyDictionary<PropertyInfo, CommandLineOptionAttribute> GetPropertyAttributeMapping(Type type)
        {
            var properties = type.GetProperties();

            var map = new Dictionary<PropertyInfo, CommandLineOptionAttribute>(properties.Length);
            foreach(var property in properties) {
                var attributes = property.GetCustomAttributes(typeof(CommandLineOptionAttribute), true);
                if(attributes != null && attributes.Any()) {
                    map.Add(property, attributes.OfType<CommandLineOptionAttribute>().First());
                }
            }

            return map;
        }

        private IReadOnlyDictionary<PropertyInfo, CommandLineOption> ApplyPropertyKeyMapping(CommandLineParser parser, IReadOnlyDictionary<PropertyInfo, CommandLineOptionAttribute> propertyAttributeMap)
        {
            var map = new Dictionary<PropertyInfo, CommandLineOption>();
            foreach(var pair in propertyAttributeMap) {
                var attribute = pair.Value;
                var option = parser.Add(attribute.Option);
                map.Add(pair.Key, option);
            }

            return map;
        }

        private object? ConvertValue(Type type, ICommandLineValues values)
        {
            foreach(var handler in Handlers) {
                if(handler.IsSupportType(type)) {
                    foreach(var data in values.Data) {
                        try {
                            return handler.Convert(type, data);
                        } catch(Exception ex) {
                            Logger.LogWarning("{Message}: {Exception}", ex.Message, ex);
                        }
                    }
                    return null;
                }
            }

            throw new CommandLineTypeConvertException(type);
        }

        protected virtual T ConvertCore<T>(CommandLineParser parser, IReadOnlyList<string> arguments)
            where T : new()
        {
            var type = typeof(T);
            var data = new T();
            var propAttrMap = GetPropertyAttributeMapping(type);
            var propKeyMap = ApplyPropertyKeyMapping(parser, propAttrMap);

            var parsedResult = parser.Parse(string.Empty, arguments);

            foreach(var pair in propKeyMap) {
                if(pair.Value.Kind == CommandLineOptionKind.Value) {
                    // 値取得
                    if(parsedResult.Values.TryGetValue(pair.Value.Key, out var value)) {
                        var convertedValue = ConvertValue(pair.Key.PropertyType, value);
                        if(convertedValue is not null) {
                            pair.Key.SetValue(data, convertedValue);
                        }
                    }
                } else {
                    // スイッチ
                    if(parsedResult.Switches.Contains(pair.Value.Key)) {
                        pair.Key.SetValue(data, true);
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// 変換。
        /// </summary>
        /// <typeparam name="T">変換先データ型</typeparam>
        /// <param name="parser">パーサー。</param>
        /// <param name="arguments">引数。コマンド・プログラム名は含まない。</param>
        /// <returns>変換結果。</returns>
        /// <remarks>
        /// <para>変換失敗の場合は例外が投げられる。投げられる例外は関連項目も参照のこと。</para>
        /// </remarks>
        /// <exception cref="CommandLineTypeConvertException"></exception>
        /// <exception cref="CommandLineConverterException"></exception>
        public T Convert<T>(CommandLineParser parser, IReadOnlyList<string> arguments)
            where T : new()
        {
            return ConvertCore<T>(parser, arguments.ToArray());
        }

        #endregion
    }
}
