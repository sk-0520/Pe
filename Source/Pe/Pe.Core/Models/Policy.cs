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
        private PolicyRetryOptions RetryOptions { get; set; } = new PolicyRetryOptions();
        private PolicyTimeoutOptions TimeoutOptions { get; set; } = new PolicyTimeoutOptions();

        #endregion

        #region function

        private void AddHandleCore(Type exceptionType)
        {
            if(exceptionType.IsAssignableFrom(typeof(Exception))) {
                throw new ArgumentException($"{nameof(exceptionType)} is not ${nameof(Exception)}", nameof(exceptionType));
            }

            Exceptions.Add(exceptionType);
        }

        #endregion

        #region IPolicyBuilder

        public PolicyBuilder AddHandle<TException>()
            where TException : Exception
        {
            AddHandleCore(typeof(TException));
            return this;
        }
        IPolicyBuilder IPolicyBuilder.AddHandle<TException>() => AddHandle<TException>();

        public PolicyBuilder SetRetry(PolicyRetryOptions options)
        {
            RetryOptions = options;
            return this;
        }
        IPolicyBuilder IPolicyBuilder.SetRetry(PolicyRetryOptions options) => SetRetry(options);

        public PolicyBuilder SetTimeout(PolicyTimeoutOptions options)
        {
            TimeoutOptions = options;
            return this;
        }
        IPolicyBuilder IPolicyBuilder.SetTimeout(PolicyTimeoutOptions options) => SetTimeout(options);

        #endregion
    }
}
