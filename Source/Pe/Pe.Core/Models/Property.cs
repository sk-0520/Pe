using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public interface IPropertyGetter
    {
        #region function

        object? Get(object owner);

        #endregion
    }

    public interface IPropertyGetter<TOwner, TValue>
    {
        #region function

        TValue Get(TOwner target);

        #endregion
    }

    public interface IPropertySetter
    {
        #region function

        void Set(object owner, object value);

        #endregion
    }

    public interface IPropertySetter<TOwner, TValue>
    {
        #region function

        void Set(TOwner owner, TValue value);

        #endregion
    }


    internal static class PropertyFactory
    {
        #region function

        public static ParameterExpression CreateOwner(object owner) => Expression.Parameter(owner.GetType(), nameof(owner));
        public static ParameterExpression CreateOwner<T>() => Expression.Parameter(typeof(T), typeof(T).Name);

        public static Func<object, object?> CreateGetter(ParameterExpression owner, string propertyName)
        {
            var property = Expression.PropertyOrField(owner, propertyName);

            var lambda = Expression.Lambda(
                Expression.Convert(
                    property,
                    property.Type
                ),
                owner
            );

            var executor = lambda.Compile();
            return new Func<object, object?>(o => executor.DynamicInvoke(o));
        }
        public static Func<TOwner, TValue> CreateGetter<TOwner, TValue>(ParameterExpression owner, string propertyName)
        {
            var property = Expression.PropertyOrField(owner, propertyName);

            var lambda = Expression.Lambda<Func<TOwner, TValue>>(
                Expression.Convert(
                    property,
                    typeof(TValue)
                ),
                owner
            );
            return lambda.Compile();
        }

        public static Action<object, object?> CreateSetter(ParameterExpression owner, string propertyName)
        {
            var property = Expression.PropertyOrField(owner, propertyName);
            var value = Expression.Parameter(typeof(object), "value");

            var lambda = Expression.Lambda(
                Expression.Assign(
                    property,
                    Expression.Convert(value, property.Type)
                ),
                owner, value
            );

            var executor = lambda.Compile();
            return new Action<object, object?>((o, v) => executor.DynamicInvoke(o, v));
        }
        public static Action<TOwner, TValue> CreateSetter<TOwner, TValue>(ParameterExpression owner, string propertyName)
        {
            var property = Expression.PropertyOrField(owner, propertyName);
            var value = Expression.Parameter(typeof(TValue), "value");

            var lambda = Expression.Lambda<Action<TOwner, TValue>>(
                Expression.Assign(
                    property,
                    Expression.Convert(value, property.Type)
                ),
                owner, value
            );
            return lambda.Compile();
        }

        #endregion
    }

    public class PropertyAccesser : IPropertyGetter, IPropertySetter
    {
        public PropertyAccesser(ParameterExpression ownerExpression, PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            if(PropertyInfo.CanWrite) {
                Setter = PropertyFactory.CreateSetter(ownerExpression, propertyInfo.Name);
            } else {
                Setter = ThrowSetter;
            }
            Getter = PropertyFactory.CreateGetter(ownerExpression, propertyInfo.Name);
        }
        public PropertyAccesser(object owner, string propertyName)
        {
            var ownerExpression = PropertyFactory.CreateOwner(owner);
            var propertyInfo = ownerExpression.Type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if(propertyInfo == null) {
                throw new ArgumentException($"{nameof(propertyName)}: {propertyName}");
            }
            PropertyInfo = propertyInfo;
            if(PropertyInfo.CanWrite) {
                Setter = PropertyFactory.CreateSetter(ownerExpression, propertyName);
            } else {
                Setter = ThrowSetter;
            }
            Getter = PropertyFactory.CreateGetter(ownerExpression, propertyName);
        }

        #region property

        public PropertyInfo PropertyInfo { get; }

        Func<object, object?> Getter { get; }
        Action<object, object?> Setter { get; }

        #endregion

        #region function

        private static void ThrowSetter(object owner, object? value) => throw new NotSupportedException();

        #endregion


        #region IPropertyGetter

        public object? Get(object owner) => Getter(owner);

        #endregion

        #region IPropertySetter

        public void Set(object owner, object? value) => Setter(owner, value);

        #endregion
    }

    public class PropertyAccesser<TOwner, TValue> : PropertyAccesser, IPropertyGetter<TOwner, TValue>, IPropertySetter<TOwner, TValue>
    {
        public PropertyAccesser(TOwner owner, string propertyName)
            : base(owner!, propertyName)
        {
            var ownerProperty = PropertyFactory.CreateOwner(owner!);
            if(PropertyInfo.CanWrite) {
                Setter = PropertyFactory.CreateSetter<TOwner, TValue>(ownerProperty, propertyName);
            } else {
                Setter = ThrowSetter;
            }
            Getter = PropertyFactory.CreateGetter<TOwner, TValue>(ownerProperty, propertyName);
        }

        #region property

        Func<TOwner, TValue> Getter { get; }
        Action<TOwner, TValue> Setter { get; }

        #endregion

        #region function

        private static void ThrowSetter(TOwner owner, TValue value) => throw new NotSupportedException();

        #endregion

        #region IPropertyGetter

        public TValue Get(TOwner owner) => Getter(owner);

        #endregion

        #region IPropertySetter

        public void Set(TOwner owner, TValue value) => Setter(owner, value);

        #endregion

    }

    public class PropertyAccesserFactory
    {
        #region function

        public static PropertyAccesser Create(object owner, string propertyName) => new PropertyAccesser(owner, propertyName);
        public static PropertyAccesser<TOwner, TValue> Create<TOwner, TValue>(TOwner owner, string propertyName) => new PropertyAccesser<TOwner, TValue>(owner, propertyName);

        #endregion
    }

    public class PropertyCacher
    {
        public PropertyCacher(object owner)
        {
            Owner = owner;
            OwnerExpression = PropertyFactory.CreateOwner(Owner);
            Properties = OwnerExpression.Type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToDictionary(
                    k => k.Name,
                    v => v
                )
            ;
        }

        #region property

        object Owner { get; }

        ParameterExpression OwnerExpression { get; }

        IReadOnlyDictionary<string, PropertyInfo> Properties { get; }

        ConcurrentDictionary<string, PropertyAccesser> AccesserCache { get; } = new ConcurrentDictionary<string, PropertyAccesser>(); // CPUの数とかこの層で取得するのしんどいのでコンストラクタでキャパ指定せず。

        #endregion

        #region function

        PropertyAccesser GetAccessor(string propertyName) => AccesserCache.GetOrAdd(propertyName, s => {
            var propertyInfo = Properties[s];
            return new PropertyAccesser(OwnerExpression, propertyInfo);
        });

        public object? Get(string propertyName)
        {
#if DEBUG
            if(Properties.TryGetValue(propertyName, out var prop)) {
                if(!prop.CanRead) {
                    throw new ArgumentException(nameof(propertyName));
                }
            } else {
                throw new ArgumentException(nameof(propertyName));
            }
#endif
            var accessor = GetAccessor(propertyName);
            return accessor.Get(Owner);
        }

        public void Set(string propertyName, object? value)
        {
#if DEBUG
            if(Properties.TryGetValue(propertyName, out var prop)) {
                if(!prop.CanWrite) {
                    throw new ArgumentException(nameof(propertyName));
                }
            } else {
                throw new ArgumentException(nameof(propertyName));
            }
#endif
            var accessor = GetAccessor(propertyName);
            accessor.Set(Owner, value);
        }

        #endregion
    }
}
