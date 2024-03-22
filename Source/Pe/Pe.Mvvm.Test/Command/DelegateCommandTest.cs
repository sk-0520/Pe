using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Command;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Command
{
    [TestClass]
    public class DelegateCommandTest
    {
        #region function

        [TestMethod]
        public void ExecuteTest()
        {
            DelegateCommand command = null!;
            command = new DelegateCommand(
                o => {
                    Assert.AreEqual(1, command.ExecutingCount);
                    Assert.IsFalse(command.CanExecute(null));
                }
            );
            command.Execute(null);
        }

        [TestMethod]
        public void SuppressCommandWhileExecutingTest()
        {
            DelegateCommand command = null!;
            command = new DelegateCommand(
                o => {
                    Assert.AreEqual(1, command.ExecutingCount);
                    Assert.IsTrue(command.CanExecute(null));
                }
            ) {
                SuppressCommandWhileExecuting = false,
            };
            command.Execute(null);
        }

        #endregion
    }
}
