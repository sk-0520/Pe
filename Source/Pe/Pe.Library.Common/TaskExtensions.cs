using System;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Common
{
    public static class TaskExtensions
    {
        public static void ThrowIfHasException(this Task task)
        {
            if(task.IsCompletedSuccessfully) {
                return;
            }

            if(task.Exception is Exception exception) {
                if(exception.InnerException is not null) {
                    throw exception.InnerException;
                }
                throw exception;
            }
        }

        public static void ThrowIfHasException(this ValueTask task)
        {
            if(task.IsCompletedSuccessfully) {
                return;
            }

            if(task.AsTask().Exception is Exception exception) {
                if(exception.InnerException is not null) {
                    throw exception.InnerException;
                }
                throw exception;
            }
        }
    }
}
