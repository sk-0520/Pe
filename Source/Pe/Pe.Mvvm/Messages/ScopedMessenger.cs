using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Common;

namespace ContentTypeTextNet.Pe.Mvvm.Messages
{
    public class ScopedMessenger: DisposerBase, IMessenger
    {
        public ScopedMessenger(IMessenger messenger)
        {
            Messenger = messenger;
        }

        #region property

        private IMessenger Messenger { get; }
        private ISet<MessageItem> ScopeItems { get; } = new HashSet<MessageItem>();

        #endregion

        #region IMessenger

        public MessageItem Register<TMessage>(Action<TMessage> action, string messageId = "")
            where TMessage : IMessage
        {
            ThrowIfDisposed();

            var messageItem = Messenger.Register(action, messageId);
            ScopeItems.Add(messageItem);

            return messageItem;
        }

        public MessageItem Register<TMessage>(Func<TMessage, Task> callback, string messageId = "")
            where TMessage : IMessage
        {
            ThrowIfDisposed();

            var messageItem = Messenger.Register(callback, messageId);
            ScopeItems.Add(messageItem);

            return messageItem;
        }

        public void Unregister(MessageItem messageItem)
        {
            ThrowIfDisposed();

            if(ScopeItems.Contains(messageItem)) {
                Messenger.Unregister(messageItem);
                ScopeItems.Remove(messageItem);
            }
        }

        public void Send<TMessage>(TMessage message)
            where TMessage : IMessage
            => Messenger.Send(message)
        ;

        public Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : IMessage
            => Messenger.SendAsync(message, cancellationToken)
        ;

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                var items = ScopeItems.ToArray();
                foreach(var item in items) {
                    Unregister(item);
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
