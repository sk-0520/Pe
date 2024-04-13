using System;
using ContentTypeTextNet.Pe.Mvvm.Message;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Message
{
    public class MessengerHelperTest
    {
        #region function

        private class TestGetMessengerFromProperty_empty
        { }

        [Fact]
        public void GetMessengerFromProperty_empty_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_empty();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.Null(actual);
        }


        private class TestGetMessengerFromProperty_private
        {
            [Messenger]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
            private Messenger Messenger { get; } = new Messenger();
        }

        [Fact]
        public void GetMessengerFromProperty_private_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_private();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.Null(actual);
        }

        private class TestGetMessengerFromProperty_type1
        {
            [Messenger]
            public Uri Messenger { get; } = new Uri("file://NUL");
        }

        [Fact]
        public void GetMessengerFromProperty_type1_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_type1();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.Null(actual);
        }

        private class TestGetMessengerFromProperty_type2
        {
            [Messenger]
            public object Messenger { get; } = new object();
        }

        [Fact]
        public void GetMessengerFromProperty_type2_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_type2();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.Null(actual);
        }

        private class TestGetMessengerFromProperty_null
        {
            [Messenger]
            public Messenger Messenger { get; } = null!;
        }

        [Fact]
        public void GetMessengerFromProperty_null_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_null();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.Null(actual);
        }

        private class TestGetMessengerFromProperty_success
        {
            [Messenger]
            public Messenger Messenger { get; } = new Messenger();
        }

        [Fact]
        public void TestGetMessengerFromProperty_success_scoped_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_success();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.NotNull(actual);
            Assert.IsType<ScopedMessenger>(actual);
        }

        [Fact]
        public void TestGetMessengerFromProperty_success_raw_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_success();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext, true);
            Assert.NotNull(actual);
            Assert.IsType<Messenger>(actual);
        }

        #endregion
    }
}
