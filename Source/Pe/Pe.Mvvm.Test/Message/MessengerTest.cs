using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Message;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Message
{
    public class MessengerTest
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

        private class OtherMessage: IMessage
        {
            public OtherMessage(string messageId = "")
            {
                MessageId = messageId;
            }

            public string MessageId { get; }
        }


        [Fact]
        public void Scenario_Action_Test()
        {
            using var messenger = new Messenger();
            var callCount1 = 0;
            var callCount2 = 0;
            var messageItem1 = messenger.Register<ActionMessage>(m => callCount1 += 1);
            var messageItem2 = messenger.Register<ActionMessage>(m => callCount2 += 1, "ID");

            Assert.Equal(0, callCount1);
            messenger.Send(new ActionMessage());
            Assert.Equal(1, callCount1);

            messenger.Send(new ActionMessage("no-hit"));
            Assert.Equal(1, callCount1);

            messenger.Send(new ActionMessage("ID"));
            Assert.Equal(1, callCount2);

            messenger.Send(new OtherMessage());
            Assert.Equal(1, callCount1);

            messenger.Send(new ActionMessage());
            Assert.Equal(2, callCount1);

            messenger.Unregister(messageItem1);
            // 登録解除したメッセージは死ぬ
            Assert.True(messageItem1.IsDisposed);

            messenger.Send(new ActionMessage());
            Assert.Equal(2, callCount1);

            // 登録解除は二重に実施しても問題なし
            messenger.Unregister(messageItem1);
        }

        [Fact]
        public async Task Scenario_Function_Test()
        {
            using var messenger = new Messenger();
            var callCount1 = 0;
            var callCount2 = 0;
            var messageItem1 = messenger.Register<ActionMessage>(m => Task.FromResult(callCount1 += 1));
            var messageItem2 = messenger.Register<ActionMessage>(m => Task.FromResult(callCount2 += 1), "ID");

            Assert.Equal(0, callCount1);
            messenger.Send(new ActionMessage());
            Assert.Equal(0, callCount1);

            await messenger.SendAsync(new ActionMessage());
            Assert.Equal(1, callCount1);

            await messenger.SendAsync(new ActionMessage("no-hit"));
            Assert.Equal(1, callCount1);

            await messenger.SendAsync(new ActionMessage("ID"));
            Assert.Equal(1, callCount2);

            await messenger.SendAsync(new OtherMessage());
            Assert.Equal(1, callCount1);

            await messenger.SendAsync(new ActionMessage());
            Assert.Equal(2, callCount1);

            messenger.Unregister(messageItem1);
            // 登録解除したメッセージは死ぬ
            Assert.True(messageItem1.IsDisposed);

            await messenger.SendAsync(new ActionMessage());
            Assert.Equal(2, callCount1);

            // 登録解除は二重に実施しても問題なし
            messenger.Unregister(messageItem1);
        }

        [Fact]
        public void DisposeTest()
        {
            var messenger = new Messenger();

            var messageItem1 = messenger.Register<ActionMessage>(m => { });
            var messageItem2 = messenger.Register<ActionMessage>(m => { }, "ID");

            messenger.Dispose();

            Assert.True(messageItem1.IsDisposed);
            Assert.True(messageItem2.IsDisposed);

            messenger.Dispose();
        }

        #endregion
    }
}
