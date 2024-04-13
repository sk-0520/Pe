using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Command
{
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

        [Fact]
        public void ExecuteTest()
        {
            var command = new TestCommandA();

            Assert.True(command.CanExecute(null));
            Assert.Equal(0, command.ExecuteCount);

            command.Execute(null);
            Assert.Equal(1, command.ExecuteCount);

            command.IsEnabled = false;
            Assert.False(command.CanExecute(null));
            // ICommand 直接実行は CanExecute がどうとかは制御されない
            command.Execute(null);
            Assert.Equal(2, command.ExecuteCount);
        }

        [Fact]
        public async Task CanExecuteTest()
        {
            var command = new TestCommandA();

            using(var ev = new AutoResetEvent(false)) {
                var task = Task.Run(() => {
                    ev.WaitOne();
                    command.IsEnabled = false;
                });

                Assert.True(command.CanExecute(null));

                ev.Set();
                await task;

                Assert.False(command.CanExecute(null));
            }
        }

        #endregion
    }
}
