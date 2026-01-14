using ContentTypeTextNet.Pe.Library.Common;

namespace ContentTypeTextNet.Pe.Mvvm.Bindings
{
    /// <summary>
    /// MVVM で使用するモデル基底。
    /// </summary>
    public abstract class BindModelBase: NotifyPropertyBase
    {
        protected BindModelBase(EventReference propertyChangedEventType)
            : base(propertyChangedEventType)
        { }

        protected BindModelBase()
            : this(EventReference.Weak)
        { }
    }
}
