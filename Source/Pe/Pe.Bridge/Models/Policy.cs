using System;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    //TODO: Pe 側で Polly を使ったような処理を隠蔽したい想いの実装

    public record class PolicyRetryOptions
    {
        #region property

        /// <summary>
        /// リトライ数。
        /// </summary>
        public int Maximum { get; init; }

        #endregion
    }

    public interface IPolicy
    {
        #region function

        IPolicyBuilder CreateBuilder();

        #endregion
    }

    public interface IPolicyBuilder
    {
        #region function

        IPolicyBuilder AddHandle<TException>()
            where TException : Exception
        ;

        IPolicyBuilder SetRetry(PolicyRetryOptions options);

        #endregion
    }
}
