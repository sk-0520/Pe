namespace ContentTypeTextNet.Pe.Bridge.Models
{
    //TODO: Pe 側で Polly を使ったような処理を隠蔽したい想いの実装

    public interface IPolicy
    {
        #region function

        IPolicyBuilder CreateBuilder();

        #endregion
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:空のインターフェイスは使用しません", Justification = "実装方針が決まってない")]
    public interface IPolicyBuilder
    {
        #region function
        #endregion
    }
}
