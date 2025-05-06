using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Setupper
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseNoSetupAttribute: Attribute
    {
        public DatabaseNoSetupAttribute(string description)
        {
            Description = description;
        }

        #region property
        public string Description { get; }
        #endregion
    }
}
