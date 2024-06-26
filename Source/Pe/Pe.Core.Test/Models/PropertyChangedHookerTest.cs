using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Standard.Base;
using ContentTypeTextNet.Pe.Standard.Base.Throw;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.Commands;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class HookItemTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            Action action = new Action(() => { });
            IReadOnlyHookItem actual = new HookItem(
                "notify",
                ["A", "B", "C"],
                [
                    ApplicationCommands.New,
                    ApplicationCommands.Undo,
                    ApplicationCommands.Cut,
                ],
                action
            );

            Assert.Equal("notify", actual.NotifyPropertyName);
            Assert.Equal(["A", "B", "C"], actual.RaisePropertyNames);
            Assert.Equal([
                ApplicationCommands.New,
                ApplicationCommands.Undo,
                ApplicationCommands.Cut,
            ], actual.RaiseCommands);
            Assert.Equal(action, actual.Callback);
        }

        #endregion
    }

    public class CachedHookItemTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            DelegateCommand dgCommand1 = new DelegateCommand(() => { });
            DelegateCommand dgCommand2 = new DelegateCommand(() => { });

            Action action1 = new Action(() => { });
            Action action2 = new Action(() => { });

            var actual = new CachedHookItem(
                ["raisePropertyName1", "raisePropertyName2"],
                [ApplicationCommands.New, ApplicationCommands.Undo],
                [dgCommand1, dgCommand2],
                [action1, action2]
            );

            Assert.Equal(["raisePropertyName1", "raisePropertyName2"], actual.RaisePropertyNames);
            Assert.Equal([ApplicationCommands.New, ApplicationCommands.Undo], actual.RaiseCommands);
            Assert.Equal([dgCommand1, dgCommand2], actual.RaiseDelegateCommands);
            Assert.Equal([action1, action2], actual.Callbacks);
        }

        #endregion
    }

    public class PropertyChangedHookerTest
    {
        #region define

        private class Command: ICommand
        {
            #region variable

            private bool _canExecute;

            #endregion

            public Command(bool canExecute)
            {
                this._canExecute = canExecute;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }

            #region property

            public bool Called { get; private set; } = false;

            #endregion

            #region ICommand

            public event EventHandler? CanExecuteChanged;

            public bool CanExecute(object? parameter)
            {
                return this._canExecute;
            }

            public void Execute(object? parameter)
            {
                Called = true;
            }

            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void AddHook_HookItem_Test()
        {
            var test = new PropertyChangedHooker(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddHook(default(HookItem)!));
            Assert.Throws<ArgumentException>(() => test.AddHook(new HookItem(null!, null, null, null)));
            Assert.Throws<ArgumentException>(() => test.AddHook(new HookItem("", null, null, null)));
        }

        [Fact]
        public void AddHook_string_Test()
        {
            var test = new PropertyChangedHooker(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddHook(default(string)!));
            Assert.Throws<ArgumentException>(() => test.AddHook(""));
            var result = test.AddHook("A");
            Assert.Equal("A", result.NotifyPropertyName);
            Assert.NotNull(result.RaisePropertyNames);
            Assert.Single(result.RaisePropertyNames);
            Assert.Contains("A", result.RaisePropertyNames);
        }

        [Fact]
        public void AddHook_string_string_Test()
        {
            var test = new PropertyChangedHooker(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddHook(default(string)!, default(string)!));
            Assert.Throws<ArgumentNullException>(() => test.AddHook("A", default(string)!));
            Assert.Throws<ArgumentNullException>(() => test.AddHook(default(string)!, "B"));
            var result = test.AddHook("a", "b");
            Assert.Equal("a", result.NotifyPropertyName);
            Assert.NotNull(result.RaisePropertyNames);
            Assert.Single(result.RaisePropertyNames);
            Assert.Contains("b", result.RaisePropertyNames);
        }

        [Fact]
        public void AddHook_string_IEnumerableXstringX_Test()
        {
            var test = new PropertyChangedHooker(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddHook(default(string)!, Array.Empty<string>()));
            Assert.Throws<ArgumentNullException>(() => test.AddHook("A", (IEnumerable<string>)null!));
            Assert.Throws<ArgumentEmptyCollectionException>(() => test.AddHook("A", Array.Empty<string>()));

            var result = test.AddHook("a", new[] { "A", "B" });
            Assert.Equal("a", result.NotifyPropertyName);
            Assert.NotNull(result.RaisePropertyNames);
            Assert.Equal(2, result.RaisePropertyNames.Count);
            Assert.Contains("A", result.RaisePropertyNames);
            Assert.Contains("B", result.RaisePropertyNames);
        }

        [Fact]
        public void AddHook_string_ICommand_Test()
        {
            var test = new PropertyChangedHooker(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddHook(default(string)!, default(ICommand)!));
            Assert.Throws<ArgumentNullException>(() => test.AddHook(default(string)!, new Command(true)));
            Assert.Throws<ArgumentNullException>(() => test.AddHook("A", default(ICommand)!));

            var command = new Command(true);
            var result = test.AddHook("a", command);
            Assert.Equal("a", result.NotifyPropertyName);
            Assert.Null(result.RaisePropertyNames);
            Assert.NotNull(result.RaiseCommands);
            Assert.Single(result.RaiseCommands);
            Assert.Contains(command, result.RaiseCommands);
        }

        [Fact]
        public void AddHook_string_IEnumerableXICommandX_Test()
        {
            var test = new PropertyChangedHooker(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddHook(default(string)!, default(ICommand[])!));
            Assert.Throws<ArgumentNullException>(() => test.AddHook(default(string)!, new [] { new Command(true)}));
            Assert.Throws<ArgumentNullException>(() => test.AddHook("A", default(ICommand[])!));
            Assert.Throws<ArgumentEmptyCollectionException>(() => test.AddHook("A", Array.Empty<ICommand>()));

            var command1 = new Command(true);
            var command2 = new Command(true);
            var result = test.AddHook("a", new [] { command1 , command2});
            Assert.Equal("a", result.NotifyPropertyName);
            Assert.Null(result.RaisePropertyNames);
            Assert.NotNull(result.RaiseCommands);
            Assert.Equal(2, result.RaiseCommands.Count);
            Assert.Contains(command1, result.RaiseCommands);
            Assert.Contains(command2, result.RaiseCommands);
        }

        [Fact]
        public void AddHook_string_Action_Test()
        {
            var test = new PropertyChangedHooker(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddHook(default(string)!, default(Action)!));
            Assert.Throws<ArgumentNullException>(() => test.AddHook(default(string)!, () => { }));
            Assert.Throws<ArgumentNullException>(() => test.AddHook("A", default(Action)!));

            var action = () => { };
            var result = test.AddHook("a", action);

            Assert.Equal("a", result.NotifyPropertyName);
            Assert.Null(result.RaisePropertyNames);
            Assert.Null(result.RaiseCommands);
            Assert.NotNull(result.Callback);
            Assert.Equal(action, result.Callback);
        }

        #endregion
    }
}
