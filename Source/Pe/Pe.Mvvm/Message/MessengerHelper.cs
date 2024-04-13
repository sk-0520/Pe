using System;
using System.Linq;
using System.Reflection;

namespace ContentTypeTextNet.Pe.Mvvm.Message
{
    public static class MessengerHelper
    {
        #region function

        /// <summary>
        /// <see cref="MessengerAttribute"/>で指定された<see cref="IMessenger"/>型のプロパティを抜き出す。
        /// </summary>
        /// <remarks>
        /// <para>TODO: 本処理は直接使用せずさらにヘルパーを噛ませる想定。</para>
        /// </remarks>
        /// <param name="dataContext">対象オブジェクト。</param>
        /// <param name="rawMessenger">プロパティの値を直接取得するか。<see langword="true" />の場合直接取得し、<see langword="false" />(もしくは未指定)の場合は<see cref="ScopedMessenger"/>として取得する。</param>
        /// <returns>取得した<see cref="IMessenger"/>。取得できなかった場合は<see langword="null"/>を返す。</returns>
        public static IMessenger? GetMessengerFromProperty(object dataContext, bool rawMessenger = false)
        {
            if(dataContext == null) {
                throw new InvalidOperationException(nameof(dataContext));
            }

            var type = dataContext.GetType();
            var properties = type.GetProperties();
            var messengerAttribute = properties
                .Select(a => (property: a, messenger: a.GetCustomAttribute<MessengerAttribute>()))
                .FirstOrDefault(a => a.messenger != null)
            ;

            if(messengerAttribute.messenger == null) {
                return null;
            }

            var value = messengerAttribute.property.GetValue(dataContext);
            if(value is IMessenger messenger) {
                if(rawMessenger) {
                    return messenger;
                }

                return new ScopedMessenger(messenger);
            }

            return null;
        }

        #endregion
    }
}
