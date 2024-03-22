using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Command;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Command
{
    [TestClass]
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

        [TestMethod]
        public void Invoke_command_Test()
        {
            var command = new TestCommand();

            command.Invoke();
            Assert.AreEqual(1, command.ExecuteCount);

            command.IsEnabled = false;
            command.Invoke();
            Assert.AreEqual(1, command.ExecuteCount);

            command.IsEnabled = true;
            command.Invoke();
            Assert.AreEqual(2, command.ExecuteCount);
        }


        [TestMethod]
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
            Assert.AreEqual(1, executeCount);
            Assert.AreEqual("exec", stockValue);

            command.Invoke("exec");
            Assert.AreEqual(1, executeCount);
            Assert.AreEqual("exec", stockValue);

            command.Invoke("run");
            Assert.AreEqual(2, executeCount);
            Assert.AreEqual("run", stockValue);

            Assert.ThrowsException<InvalidCastException>(() => command.Invoke(123));
        }

        [TestMethod]
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
            Assert.AreEqual(1, executeCount);
            Assert.AreEqual("exec", stockValue);

            await command.Invoke("exec");
            Assert.AreEqual(1, executeCount);
            Assert.AreEqual("exec", stockValue);

            await command.Invoke("run");
            Assert.AreEqual(2, executeCount);
            Assert.AreEqual("run", stockValue);
        }

        #endregion
    }
}
