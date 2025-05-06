using System.Threading.Tasks;
using System;
using Xunit;
using ContentTypeTextNet.Pe.Mvvm.Commands;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Commands
{
    public class DelegateCommandTest
    {
        #region function

        [Fact]
        public void Constructor_throw_Test()
        {
            var actual1 = Assert.Throws<ArgumentNullException>(() => new DelegateCommand((Action<object>)null!, null!));
            Assert.Equal("executeAction", actual1.ParamName);

            var actual2 = Assert.Throws<ArgumentNullException>(() => new DelegateCommand((Action<object>)null!));
            Assert.Equal("executeAction", actual2.ParamName);

            var actual3 = Assert.Throws<ArgumentNullException>(() => new DelegateCommand(o => { }, null!));
            Assert.Equal("canExecuteFunc", actual3.ParamName);
        }

        [Fact]
        public void ExecutingTest()
        {
            DelegateCommand? command = null;
            command = new DelegateCommand(
                o => {
                    Assert.True(command!.CanExecute(null));
                }
            );
            command.Execute(null);
        }

        [Fact]
        public void CanExecuteTest()
        {
            var command = new DelegateCommand(
                o => { },
                o => o != null
            );
            Assert.False(command.CanExecute(null));
            Assert.True(command.CanExecute(new object()));
        }

        #endregion
    }

    public class DelegateCommand_T_Test
    {
        #region function

        [Fact]
        public void ExecuteTest()
        {
            DelegateCommand<int>? command = null;
            command = new DelegateCommand<int>(
                o => {
                    Assert.Equal(100, o);
                }
            );
            command.Execute(100);
        }

        [Fact]
        public void ExecutingTest()
        {
            DelegateCommand<int>? command = null;
            command = new DelegateCommand<int>(
                o => {
                    Assert.Equal(100, o);
                    Assert.True(command!.CanExecute(o));
                }
            );
            command.Execute(100);
        }

        [Fact]
        public void CanExecuteTest()
        {
            var command = new DelegateCommand<int>(
                o => { },
                o => o != 0
            );
            Assert.False(command.CanExecute(0));
            Assert.True(command.CanExecute(1));
        }

        #endregion
    }

}
