using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Command;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Command
{
    [TestClass]
    public class AsyncDelegateCommandTest
    {
        #region function

        [TestMethod]
        public void ExecuteTest()
        {
            AsyncDelegateCommand command = null!;
            command = new AsyncDelegateCommand(
                o => {
                    Assert.AreEqual(1, command.ExecutingCount);
                    Assert.IsFalse(command.CanExecute(null));
                    return Task.CompletedTask;
                }
            );
            command.Execute(null);
        }

        [TestMethod]
        public void SuppressCommandWhileExecutingTest()
        {
            AsyncDelegateCommand command = null!;
            command = new AsyncDelegateCommand(
                o => {
                    Assert.AreEqual(1, command.ExecutingCount);
                    Assert.IsTrue(command.CanExecute(null));
                    return Task.CompletedTask;
                }
            ) {
                SuppressCommandWhileExecuting = false,
            };
            command.Execute(null);
        }

        #endregion
    }
}
