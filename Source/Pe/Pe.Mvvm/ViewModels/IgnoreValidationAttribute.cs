using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Mvvm.ViewModels
{
    /// <summary>
    /// 検証無視。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class IgnoreValidationAttribute: Attribute
    {
        public IgnoreValidationAttribute()
        { }
    }
}
