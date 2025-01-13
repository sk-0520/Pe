using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Messages;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Messages
{
    public class ScopedMessengerTest
    {
        #region function

        private class ActionMessage: IMessage
        {
            public ActionMessage(string messageId = "")
            {
                MessageId = messageId;
            }

            public string MessageId { get; }
        }

        private class ActionMessage2: ActionMessage
        {
            public ActionMessage2(string messageId = "")
                : base(messageId)
            { }
        }

        [Fact]
        public void Scenario_normal_Test()
        {
            var rootMessenger = new Messenger();
            var scopedMessenger = new ScopedMessenger(rootMessenger);

            var rootCallCount = 0;
            var scopedCallCountNotCall = 0;
            var scopedCallCountCanCall = 0;

            var rootMessageItem = rootMessenger.Register<ActionMessage>(m => rootCallCount += 1);
            // 既に登録されてるので呼び出されない(スコープ云々関係なくID指定が必要なパターン)
            var scopedMessageItemNotCall = scopedMessenger.Register<ActionMessage>(m => scopedCallCountNotCall += 1);
            // 新たに登録した完全別物
            var scopedMessageItemCanCall = scopedMessenger.Register<ActionMessage2>(m => scopedCallCountCanCall += 1);

            rootMessenger.Send(new ActionMessage());
            Assert.Equal(1, rootCallCount);
            Assert.Equal(0, scopedCallCountNotCall);
            Assert.Equal(0, scopedCallCountCanCall);

            // rootMessenger に登録済みのものが呼び出される(既に登録されてるのでIDが必要パターン)
            scopedMessenger.Send(new ActionMessage());
            Assert.Equal(2, rootCallCount);
            Assert.Equal(0, scopedCallCountNotCall);
            Assert.Equal(0, scopedCallCountCanCall);

            // スコープ側での登録処理は最終的に root に伝わる
            rootMessenger.Send(new ActionMessage2());
            Assert.Equal(2, rootCallCount);
            Assert.Equal(0, scopedCallCountNotCall);
            Assert.Equal(1, scopedCallCountCanCall);

            // これはまぁふつう
            scopedMessenger.Send(new ActionMessage2());
            Assert.Equal(2, rootCallCount);
            Assert.Equal(0, scopedCallCountNotCall);
            Assert.Equal(2, scopedCallCountCanCall);

            // 破棄したら root でも呼び出し無効
            scopedMessenger.Dispose();
            Assert.True(scopedMessageItemNotCall.IsDisposed);
            Assert.True(scopedMessageItemCanCall.IsDisposed);

            rootMessenger.Send(new ActionMessage2());
            Assert.Equal(2, rootCallCount);
            Assert.Equal(0, scopedCallCountNotCall);
            Assert.Equal(2, scopedCallCountCanCall);
        }

        #endregion  
    }
}
