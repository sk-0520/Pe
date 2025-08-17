using System;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Mvvm.Commands;
using Xunit;

namespace ContentTypeTextNet.Pe.Mvvm.Test.Commands
{
    public class AsyncDelegateCommandTest
    {
        #region function

        [Fact]
        public void Constructor_throw_Test()
        {
            var actual1 = Assert.Throws<ArgumentNullException>(() => new AsyncDelegateCommand((Func<object, Task>)null!, null!));
            Assert.Equal("executeAction", actual1.ParamName);

            var actual2 = Assert.Throws<ArgumentNullException>(() => new AsyncDelegateCommand((Func <Task>)null!));
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
                    Assert.NotNull(command);
                    Assert.False(command.CanExecute(null));
                    return Task.CompletedTask;
                }
            );
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

        [Fact]
        public void CancelTokenTest()
        {
            using var ev1 = new ManualResetEventSlim(false);
            var command = new AsyncDelegateCommand(
                (o, c) => Task.Run(() => {
                    ev1.Wait(c);
                },
                c),
                o => true
            );

            command.Execute(null);
            Assert.True(command.IsExecuting);

            using var ev2 = new ManualResetEventSlim(false);
            command.CanExecuteChanged += (s, e) => {
                ev2.Set();
            };
            ev1.Set();
            ev2.Wait(cancellationToken: TestContext.Current.CancellationToken);
            Assert.True(command.CanExecute(null));
            Assert.False(command.IsExecuting);
        }

        [Fact]
        public async Task CancelTokenAsyncTest()
        {
            using var ev = new ManualResetEventSlim(false);
            var command = new AsyncDelegateCommand(
                (o, c) => Task.Run(() => {
                    ev.Wait(c);
                }, c),
                o => true
            );

            var task = command.ExecuteAsync(null);
            Assert.True(command.IsExecuting);

            ev.Set();
            await task;
            Assert.True(command.CanExecute(null));
            Assert.False(command.IsExecuting);
        }

        [Fact]
        public async Task CancelExecutionTest()
        {
            using var ev = new ManualResetEventSlim(false);
            var command = new AsyncDelegateCommand(
                (o, c) => Task.Run(() => {
                    ev.Wait(c);
                }, c),
                o => true
            );

            Assert.False(command.CancelExecution());

            var task = command.ExecuteAsync(null);
            Assert.True(command.IsExecuting);

            Assert.True(command.CancelExecution());

            await task;

            Assert.True(command.CanExecute(null));
            Assert.False(command.IsExecuting);
        }


        [Fact]
        public async Task CancelExecution_Rethrow_Test()
        {
            using var ev = new ManualResetEventSlim(false);
            var command = new AsyncDelegateCommand(
                (o, c) => Task.Run(() => {
                    ev.Wait(c);
                }, c),
                o => true
            ) {
                RethrowOperationCanceledException = true
            };

            Assert.False(command.CancelExecution());

            var task = command.ExecuteAsync(null);
            Assert.True(command.IsExecuting);

            Assert.True(command.CancelExecution());

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await task);

            Assert.True(command.CanExecute(null));
            Assert.False(command.IsExecuting);
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
                    Assert.NotNull(command);
                    Assert.Equal(100, o);
                    Assert.False(command.CanExecute(o));
                    return Task.CompletedTask;
                },
                o => true
            );
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
