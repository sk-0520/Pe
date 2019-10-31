using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Core.Models.Data
{
    public interface IReadOnlyData
    { }

    public interface IData : IReadOnlyData
    { }

    [Serializable, DataContract]
    public abstract class DataBase : IData
    { }



    public interface IRawModel : IReadOnlyData
    {
        #region property

        object BaseRawObject { get; }

        #endregion
    }

    public interface IRawModel<T> : IData, IRawModel
    {
        #region property

        T Raw { get; }

        #endregion
    }

    public class RawModel : DisposerBase, IRawModel
    {
        public RawModel(object rawObject)
        {
            if(rawObject == null) {
                throw new ArgumentNullException(nameof(rawObject));
            }

            BaseRawObject = rawObject;
        }

        #region IRawModel

        public object BaseRawObject { get; }

        #endregion
    }

    public class RawModel<T> : RawModel, IRawModel<T>
    {
        public RawModel(T rawObject)
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            : base(rawObject)
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
        {
            Raw = rawObject;
        }

        #region IRawModel

        public T Raw { get; }

        #endregion
    }

}
