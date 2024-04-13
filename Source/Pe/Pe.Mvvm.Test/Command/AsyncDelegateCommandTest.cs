using System;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Command
{
    public class AsyncDelegateCommandTest
    {
        #region function

        [Fact]
        public void Constructor_throw_Test()
        {
            var actual1 = Assert.Throws<ArgumentNullException>(() => new AsyncDelegateCommand(null!, null!));
            Assert.Equal("executeAction", actual1.ParamName);

            var actual2 = Assert.Throws<ArgumentNullException>(() => new AsyncDelegateCommand(null!));
            Assert.Equal("executeAction", actual2.ParamName);

            var actual3 = Assert.Throws<ArgumentNullException>(() => new AsyncDelegateCommand(o => Task.CompletedTask, null!));
            Assert.Equal("canExecuteFunc", actual3.ParamName);
        }

        [Fact]
        public void ExecuteTest()
        {
            AsyncDelegateCommand? command = null;
            command = new AsyncDelegateCommand(
                o => {
                    Assert.Equal(1, command!.ExecutingCount);
                    Assert.False(command.CanExecute(null));
                    return Task.CompletedTask;
                }
            );
            command.Execute(null);
        }

        [Fact]
        public void SuppressCommandWhileExecutingTest()
        {
            AsyncDelegateCommand? command = null;
            command = new AsyncDelegateCommand(
                o => {
                    Assert.Equal(1, command!.ExecutingCount);
                    Assert.True(command.CanExecute(null));
                    return Task.CompletedTask;
                }
            ) {
                SuppressCommandWhileExecuting = false,
            };
            command.Execute(null);
        }

        [Fact]
        public void CanExecuteTest()
        {
            var command = new AsyncDelegateCommand(
                o => Task.CompletedTask,
                o => o != null
            );
            Assert.False(command.CanExecute(null));
            Assert.True(command.CanExecute(new object()));
        }
        #endregion
    }

    public class AsyncDelegateCommand_T_Test
    {
        #region function

        [Fact]
        public void ExecuteTest()
        {
            AsyncDelegateCommand<int>? command = null;
            command = new AsyncDelegateCommand<int>(
                o => {
                    Assert.Equal(1, command!.ExecutingCount);
                    Assert.Equal(100, o);
                    Assert.False(command.CanExecute(o));
                    return Task.CompletedTask;
                }
            );
            command.Execute(100);
        }

        [Fact]
        public void SuppressCommandWhileExecutingTest()
        {
            AsyncDelegateCommand<int>? command = null;
            command = new AsyncDelegateCommand<int>(
                o => {
                    Assert.Equal(1, command!.ExecutingCount);
                    Assert.Equal(100, o);
                    Assert.True(command.CanExecute(o));
                    return Task.CompletedTask;
                }
            ) {
                SuppressCommandWhileExecuting = false,
            };
            command.Execute(100);
        }

        [Fact]
        public void CanExecuteTest()
        {
            var command = new AsyncDelegateCommand<int>(
                o => Task.CompletedTask,
                o => o != 0
            );
            Assert.False(command.CanExecute(0));
            Assert.True(command.CanExecute(1));
        }

        #endregion
    }
}
