using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Common
{
    public static class TaskExtensions
    {
        public static void ThrowIfHasException(this Task task)
        {
            if(task.Exception is Exception exception) {
                throw exception;
            }
        }
    }
}
