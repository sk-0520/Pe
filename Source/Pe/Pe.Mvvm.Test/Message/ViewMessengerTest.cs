using System.Windows;
using ContentTypeTextNet.Pe.Mvvm.Message;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Message
{
    public class ViewMessengerTest
    {
        #region define

        private class TestWindow: Window
        {
            public TestWindow()
                : base()
            {
                Opacity = 0;
                WindowStyle = WindowStyle.None;
                AllowsTransparency = true;
                ShowInTaskbar = false;
            }
        }

        #endregion

        #region function

        [WpfFact]
        public void Constructor_none_Test()
        {
            var ui = new Window();
            var messenger = new ViewMessenger<Window>(ui, m => {
                Assert.Fail();
            });
        }

        [WpfFact]
        public void Constructor_DataContextIsNull_Test()
        {
            var ui = new TestWindow();
            ui.Show();
            var messenger = new ViewMessenger<Window>(ui, m => {
                Assert.Fail();
            });
        }

        private class TestConstructor_NotLoaded_DataContextHasMessenger
        {
            [Messenger]
            public IMessenger Messenger { get; } = new Messenger();
        }


        [WpfFact]
        public void Constructor_NotLoaded_Test()
        {
            var callCount = 0;
            var ui = new TestWindow();
            var vm = new TestConstructor_NotLoaded_DataContextHasMessenger();
            ui.DataContext = vm;
            ui.Show();
            var messenger = new ViewMessenger<Window>(ui, m => {
                callCount += 1;
            });

            Assert.Equal(1, callCount);
        }

        [WpfFact]
        public void Constructor_Loaded_Test()
        {
            var callCount = 0;
            var ui = new TestWindow();
            var vm = new TestConstructor_NotLoaded_DataContextHasMessenger();
            ui.Show();

            ui.DataContext = vm;

            var messenger = new ViewMessenger<Window>(ui, m => {
                callCount += 1;
            });

            Assert.Equal(1, callCount);
        }

        [WpfFact]
        public void Constructor_Change_Test()
        {
            var callCount = 0;
            var ui = new TestWindow();
            var vm = new TestConstructor_NotLoaded_DataContextHasMessenger();
            ui.Show();

            ui.DataContext = vm;

            var messenger = new ViewMessenger<Window>(ui, m => {
                callCount += 1;
            });

            var vm2 = new TestConstructor_NotLoaded_DataContextHasMessenger();
            ui.DataContext = vm2;

            Assert.Equal(2, callCount);
        }

        [WpfFact]
        public void Constructor_Unload_Test()
        {
            var callCount = 0;
            var ui = new TestWindow();
            var vm = new TestConstructor_NotLoaded_DataContextHasMessenger();
            ui.DataContext = vm;
            ui.Show();
            var messenger = new ViewMessenger<Window>(ui, m => {
                callCount += 1;
            });

            ui.Close();

            var vm2 = new TestConstructor_NotLoaded_DataContextHasMessenger();
            ui.DataContext = vm2;

            Assert.Equal(1, callCount);
        }
        #endregion
    }
}
