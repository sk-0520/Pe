using System;
using System.Reflection;
using ContentTypeTextNet.Pe.Standard.Base;

namespace ContentTypeTextNet.Pe.Mvvm.Message
{
    /// <summary>
    /// 保持しているメッセージ。
    /// </summary>
    public class MessageItem: DisposerBase
    {
        public MessageItem(string messageId, Type messageType, object? callbackInstance, MethodInfo callbackMethodInfo)
        {
            MessageId = messageId ?? throw new ArgumentNullException(nameof(messageId));
            MessageType = messageType ?? throw new ArgumentNullException(nameof(messageType));
            CallbackInstance = callbackInstance;
            CallbackMethodInfo = callbackMethodInfo ?? throw new ArgumentNullException(nameof(callbackMethodInfo));
        }

        #region property

        /// <inheritdoc cref="IMessage.MessageId"/>
        public string MessageId { get; }
        /// <summary>
        /// <see cref="IMessage"/> の型情報。
        /// </summary>
        public Type MessageType { get; }
        /// <summary>
        /// 登録された処理のインスタンス。
        /// </summary>
        public object? CallbackInstance { get; private set; }
        /// <summary>
        /// 登録された処理のメソッド情報。
        /// </summary>
        public MethodInfo CallbackMethodInfo { get; }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                CallbackInstance = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
