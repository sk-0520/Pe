using System;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Common.Test.Linq
{
    public class TaskExtensionsTest
    {
        #region function

        [Fact]
        public void Task_ThrowIfHasException_None_Test()
        {
            var task = Task.CompletedTask;
            var exception = Record.Exception(() => task.ThrowIfHasException());
            Assert.Null(exception);
        }

        [Fact]
        public void Task_ThrowIfHasException_Exception_Test()
        {
            var task = Task.FromException(new InvalidOperationException());
            Assert.Throws<InvalidOperationException>(() => task.ThrowIfHasException());
        }

        [Fact]
        public void ValueTask_ThrowIfHasException_None_Test()
        {
            var task = ValueTask.CompletedTask;
            var exception = Record.Exception(() => task.ThrowIfHasException());
            Assert.Null(exception);
        }

        [Fact]
        public void ValueTask_ThrowIfHasException_Exception_Test()
        {
            var task = ValueTask.FromException(new InvalidOperationException());
            Assert.Throws<InvalidOperationException>(() => task.ThrowIfHasException());
        }

        #endregion
    }
}
