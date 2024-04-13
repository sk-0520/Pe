using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base;

namespace ContentTypeTextNet.Pe.Mvvm.Message
{
    /// <summary>
    /// 受信用メッセンジャー。
    /// </summary>
    public interface IReceivableMessenger
    {
        /// <summary>
        /// 対象となる TMessage(と<paramref name="messageId"/>)に対するメッセージ受信時の処理を登録。
        /// </summary>
        /// <typeparam name="TMessage">メッセージ。</typeparam>
        /// <param name="action">受信時の処理。</param>
        /// <param name="messageId">メッセージを特定するID。</param>
        /// <returns>登録されたメッセージ処理。</returns>
        MessageItem Register<TMessage>(Action<TMessage> action, string messageId = "")
            where TMessage : IMessage
        ;
        /// <summary>
        /// 対象となる TMessage(と<paramref name="messageId"/>)に対するメッセージ受信時の処理を登録。
        /// </summary>
        /// <remarks>
        /// <para>非同期版。</para>
        /// </remarks>
        /// <typeparam name="TMessage">メッセージ。</typeparam>
        /// <param name="callback">受信時の処理。</param>
        /// <param name="messageId">メッセージを特定するID。</param>
        /// <returns>登録されたメッセージ処理。</returns>
        MessageItem Register<TMessage>(Func<TMessage, Task> callback, string messageId = "")
            where TMessage : IMessage
        ;
        /// <summary>
        /// 登録されたメッセージ処理の破棄。
        /// </summary>
        /// <param name="messageItem">メッセージ処理。</param>
        void Unregister(MessageItem messageItem);
    }

    /// <summary>
    /// 送信用メッセンジャー。
    /// </summary>
    public interface ISendableMessenger
    {
        /// <summary>
        /// メッセージの送信。
        /// </summary>
        /// <typeparam name="TMessage">メッセージ。</typeparam>
        /// <param name="message">送信メッセージ。</param>
        void Send<TMessage>(TMessage message)
            where TMessage : IMessage
        ;
        /// <summary>
        /// メッセージの送信。
        /// </summary>
        /// <remarks>
        /// <para>非同期版。</para>
        /// </remarks>
        /// <typeparam name="TMessage">メッセージ。</typeparam>
        /// <param name="message">送信メッセージ。</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : IMessage
        ;
    }

    /// <summary>
    /// 主に使用するメッセンジャー。
    /// </summary>
    public interface IMessenger: IReceivableMessenger, ISendableMessenger
    { }

    /// <summary>
    /// メッセンジャー。
    /// </summary>
    public class Messenger: DisposerBase, IMessenger
    {
        public Messenger()
        { }

        #region property

        private Dictionary<Type, List<MessageItem>> RegisteredSyncTypes { get; } = new Dictionary<Type, List<MessageItem>>();
        private Dictionary<Type, List<MessageItem>> RegisteredAsyncTypes { get; } = new Dictionary<Type, List<MessageItem>>();

        #endregion

        #region function

        private bool UnregisterCore(MessageItem messageItem, Dictionary<Type, List<MessageItem>> registeredTypes)
        {
            if(registeredTypes.TryGetValue(messageItem.MessageType, out var messages)) {
                if(messages.Remove(messageItem)) {
                    messageItem.Dispose();

                    return true;
                }
            }

            return false;
        }

        private MessageItem? SearchMessageItem(IMessage needle, IReadOnlyList<MessageItem> haystack)
        {
            return haystack.FirstOrDefault(a => a.MessageId == needle.MessageId);
        }

        #endregion

        #region IMessenger.IReceivableMessenger

        public MessageItem Register<TMessage>(Action<TMessage> action, string messageId = "")
            where TMessage : IMessage
        {
            ThrowIfDisposed();

            var messageType = typeof(TMessage);

            if(!RegisteredSyncTypes.TryGetValue(messageType, out var messages)) {
                messages = new List<MessageItem>();
                RegisteredSyncTypes.Add(messageType, messages);
            }

            var item = new MessageItem(messageId, messageType, action.Target, action.Method);
            messages.Add(item);
            return item;
        }

        public MessageItem Register<TMessage>(Func<TMessage, Task> callback, string messageId = "")
            where TMessage : IMessage
        {
            ThrowIfDisposed();

            var messageType = typeof(TMessage);

            if(!RegisteredAsyncTypes.TryGetValue(messageType, out var messages)) {
                messages = new List<MessageItem>();
                RegisteredAsyncTypes.Add(messageType, messages);
            }

            var item = new MessageItem(messageId, messageType, callback.Target, callback.Method);
            messages.Add(item);
            return item;
        }

        public void Unregister(MessageItem messageItem)
        {
            ThrowIfDisposed();

            if(!UnregisterCore(messageItem, RegisteredSyncTypes)) {
                UnregisterCore(messageItem, RegisteredAsyncTypes);
            }
        }

        #endregion

        #region IMessenger.ISendableMessenger

        public void Send<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            ThrowIfDisposed();

            if(!RegisteredSyncTypes.TryGetValue(typeof(TMessage), out var messages)) {
                return;
            }

            var messageItem = SearchMessageItem(message, messages);
            if(messageItem == null) {
                return;
            }

            messageItem.CallbackMethodInfo.Invoke(messageItem.CallbackInstance, new object[] { message });
        }

        public Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : IMessage
        {
            ThrowIfDisposed();

            if(!RegisteredAsyncTypes.TryGetValue(typeof(TMessage), out var messages)) {
                return Task.CompletedTask;
            }

            var messageItem = SearchMessageItem(message, messages);
            if(messageItem == null) {
                return Task.CompletedTask;
            }

            var result = messageItem.CallbackMethodInfo.Invoke(messageItem.CallbackInstance, new object[] { message });

#pragma warning disable CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
#pragma warning disable CS8603 // Null 参照戻り値である可能性があります。
            return (Task)result;
#pragma warning restore CS8603 // Null 参照戻り値である可能性があります。
#pragma warning restore CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
        }

        #endregion

        #region DisposerBase

        private static void DisposeTypes(Dictionary<Type, List<MessageItem>> registeredTypes)
        {
            foreach(var pair in registeredTypes) {
                foreach(var item in pair.Value) {
                    item.Dispose();
                }
                pair.Value.Clear();
            }
            registeredTypes.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                DisposeTypes(RegisteredSyncTypes);
                DisposeTypes(RegisteredAsyncTypes);
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
