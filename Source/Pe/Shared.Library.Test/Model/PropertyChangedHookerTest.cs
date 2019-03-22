using System;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Library.Test.Model
{
    [TestClass]
    public class PropertyChangedHookerTest
    {
        IDispatcherWapper DispatcherWapper => new DispatcherWapper(Dispatcher.CurrentDispatcher, TestLogger.Create(GetType()));

        [TestMethod]
        public void AddHook_1_Test()
        {
            var pch = new PropertyChangedHooker(DispatcherWapper, TestLogger.Create(GetType()));
            Assert.ThrowsException<ArgumentNullException>(() => pch.AddHook(default(HookItem)));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(new HookItem(null,null,null,null)));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(new HookItem("",null,null,null)));
        }

        [TestMethod]
        public void AddHook_2_Test()
        {
            var pch = new PropertyChangedHooker(DispatcherWapper, TestLogger.Create(GetType()));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(default(string)));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(""));
            var result = pch.AddHook("A");
            Assert.AreEqual("A", result.NotifyPropertyName);
            Assert.AreEqual("A", result.RaisePropertyNames[0]);
        }

        [TestMethod]
        public void AddHook_3_Test()
        {
            var pch = new PropertyChangedHooker(DispatcherWapper, TestLogger.Create(GetType()));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(default(string), default(string)));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook("A", default(string)));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(default(string), "B"));
            var result = pch.AddHook("a", "b");
            Assert.AreEqual("a", result.NotifyPropertyName);
            Assert.AreEqual("b", result.RaisePropertyNames[0]);
        }
    }
}
