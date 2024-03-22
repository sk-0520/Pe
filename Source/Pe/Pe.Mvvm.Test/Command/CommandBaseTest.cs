using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Command;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Command
{
    [TestClass]
    public class CommandBaseTest
    {
        private class TestCommandA: CommandBase
        {
            public int ExecuteCount { get; private set; }

            private bool _isEnabled = true;
            public bool IsEnabled
            {
                get => this._isEnabled;
                set
                {
                    this._isEnabled = value;
                    OnCanExecuteChanged();
                }
            }

            public override bool CanExecute(object? parameter)
            {
                return IsEnabled;
            }

            public override void Execute(object? parameter)
            {
                ExecuteCount += 1;
            }
        }

        #region function

        [TestMethod]
        public void ExecuteTest()
        {
            var command = new TestCommandA();

            Assert.IsTrue(command.CanExecute(null));
            Assert.AreEqual(0, command.ExecuteCount);

            command.Execute(null);
            Assert.AreEqual(1, command.ExecuteCount);

            command.IsEnabled = false;
            Assert.IsFalse(command.CanExecute(null));
            // ICommand 直接実行は CanExecute がどうとかは制御されない
            command.Execute(null);
            Assert.AreEqual(2, command.ExecuteCount);
        }

        [TestMethod]
        public async Task CanExecuteTest()
        {
            var command = new TestCommandA();

            using(var ev = new AutoResetEvent(false)) {
                var task = Task.Run(() => {
                    ev.WaitOne();
                    command.IsEnabled = false;
                }).ConfigureAwait(false);

                Assert.IsTrue(command.CanExecute(null));

                ev.Set();
                await task;

                Assert.IsFalse(command.CanExecute(null));
            }
        }

        #endregion
    }

}
