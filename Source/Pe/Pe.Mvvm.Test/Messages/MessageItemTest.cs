using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Messages;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Messages
{
    public class MessageItemTest
    {
        [Fact]
        public void Constructor_messageId_throw_Test()
        {
            var actual = Assert.Throws<ArgumentNullException>(() => new MessageItem(null!, GetType(), this, GetType().GetMethod(nameof(ToString))!));
            Assert.Equal("messageId", actual.ParamName);
        }

        [Fact]
        public void Constructor_messageType_throw_Test()
        {
            var actual = Assert.Throws<ArgumentNullException>(() => new MessageItem("", null!, this, GetType().GetMethod(nameof(ToString))!));
            Assert.Equal("messageType", actual.ParamName);
        }

        [Fact]
        public void Constructor_callbackMethodInfo_throw_Test()
        {
            var actual = Assert.Throws<ArgumentNullException>(() => new MessageItem("", GetType(), this, null!));
            Assert.Equal("callbackMethodInfo", actual.ParamName);
        }

        [Fact]
        public void ConstructorTest()
        {
            var actual = new MessageItem("", GetType(), this, GetType().GetMethod(nameof(ToString))!);
            Assert.Equal("", actual.MessageId);
            Assert.Equal(GetType(), actual.MessageType);
            Assert.Equal(this, actual.CallbackInstance);
            Assert.Equal(GetType().GetMethod(nameof(ToString)), actual.CallbackMethodInfo);
        }

        [Fact]
        public void DisposeTest()
        {
            var actual = new MessageItem("", GetType(), this, GetType().GetMethod(nameof(ToString))!);
            actual.Dispose();
            Assert.Equal("", actual.MessageId);
            Assert.Equal(GetType(), actual.MessageType);
            Assert.Null(actual.CallbackInstance);
            Assert.Equal(GetType().GetMethod(nameof(ToString)), actual.CallbackMethodInfo);
            actual.Dispose();
        }
    }
}
