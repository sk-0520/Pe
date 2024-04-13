using System;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Command
{
    public class CommandExtensionsTest
    {
        #region function

        private class TestCommand: CommandBase
        {
            public bool IsEnabled { get; set; } = true;
            public int ExecuteCount { get; private set; }

            public override bool CanExecute(object? parameter) => IsEnabled;

            public override void Execute(object? parameter)
            {
                ExecuteCount += 1;
            }
        }

        [Fact]
        public void Invoke_command_Test()
        {
            var command = new TestCommand();

            command.Invoke();
            Assert.Equal(1, command.ExecuteCount);

            command.IsEnabled = false;
            command.Invoke();
            Assert.Equal(1, command.ExecuteCount);

            command.IsEnabled = true;
            command.Invoke();
            Assert.Equal(2, command.ExecuteCount);
        }


        [Fact]
        public void Invoke_delegate_Test()
        {
            var executeCount = 0;
            var stockValue = string.Empty;
            var command = new DelegateCommand<string>(
                o => {
                    stockValue = o;
                    executeCount += 1;
                },
                o => o != stockValue
            );

            command.Invoke("exec");
            Assert.Equal(1, executeCount);
            Assert.Equal("exec", stockValue);

            command.Invoke("exec");
            Assert.Equal(1, executeCount);
            Assert.Equal("exec", stockValue);

            command.Invoke("run");
            Assert.Equal(2, executeCount);
            Assert.Equal("run", stockValue);

            Assert.Throws<InvalidCastException>(() => command.Invoke(123));
        }

        [Fact]
        public async Task Invoke_async_Test()
        {
            var executeCount = 0;
            var stockValue = string.Empty;
            var command = new AsyncDelegateCommand<string>(
                o => {
                    stockValue = o;
                    executeCount += 1;
                    return Task.CompletedTask;
                },
                o => o != stockValue
            );

            await command.Invoke("exec");
            Assert.Equal(1, executeCount);
            Assert.Equal("exec", stockValue);

            await command.Invoke("exec");
            Assert.Equal(1, executeCount);
            Assert.Equal("exec", stockValue);

            await command.Invoke("run");
            Assert.Equal(2, executeCount);
            Assert.Equal("run", stockValue);
        }

        #endregion
    }
}
