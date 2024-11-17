using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Args
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CommandLineAttribute: Attribute
    {
        public CommandLineAttribute(string longKey, CommandLineKeyKind kind, string description = "")
        {
            Key = new CommandLineKey(longKey, kind, description);
        }

        #region property

        public CommandLineKey Key { get; }

        #endregion
    }

}
