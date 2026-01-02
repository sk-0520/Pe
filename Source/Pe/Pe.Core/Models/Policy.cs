using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class Policy: IPolicy
    {
        #region IPolicy

        public PolicyBuilder CreateBuilder() => new PolicyBuilder();
        IPolicyBuilder IPolicy.CreateBuilder() => CreateBuilder();

        #endregion
    }

    public class PolicyBuilder: IPolicyBuilder
    {
        #region property

        private HashSet<Type> Exceptions { get; } = new();

        #endregion

        #region function

        private void AddException<TException>()
            where TException : Exception
        {
            Exceptions.Add(typeof(TException));
        }

        #endregion

        #region IPolicyBuilder

        public PolicyBuilder Handle<TException>()
            where TException : Exception
        {
            AddException<TException>();
            return this;
        }

        #endregion
    }
}
