using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Messages;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Messages
{
    public class MessengerHelperTest
    {
        #region function

        private class TestGetMessengerFromProperty_Empty
        { }

        [Fact]
        public void GetMessengerFromProperty_empty_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_Empty();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.Null(actual);
        }


        private class TestGetMessengerFromProperty_Private
        {
            [Messenger]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
            private Messenger Messenger { get; } = new Messenger();
        }

        [Fact]
        public void GetMessengerFromProperty_private_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_Private();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.Null(actual);
        }

        private class TestGetMessengerFromProperty_Type1
        {
            [Messenger]
            public Uri Messenger { get; } = new Uri("file://NUL");
        }

        [Fact]
        public void GetMessengerFromProperty_type1_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_Type1();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.Null(actual);
        }

        private class TestGetMessengerFromProperty_Type2
        {
            [Messenger]
            public object Messenger { get; } = new object();
        }

        [Fact]
        public void GetMessengerFromProperty_type2_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_Type2();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.Null(actual);
        }

        private class TestGetMessengerFromProperty_Null
        {
            [Messenger]
            public Messenger? Messenger { get; } = null;
        }

        [Fact]
        public void GetMessengerFromProperty_null_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_Null();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.Null(actual);
        }

        private class TestGetMessengerFromProperty_Success
        {
            [Messenger]
            public Messenger Messenger { get; } = new Messenger();
        }

        [Fact]
        public void TestGetMessengerFromProperty_success_scoped_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_Success();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.NotNull(actual);
            Assert.IsType<ScopedMessenger>(actual);
        }

        [Fact]
        public void TestGetMessengerFromProperty_success_raw_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_Success();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext, true);
            Assert.NotNull(actual);
            Assert.IsType<Messenger>(actual);
        }

        #endregion
    }

}
