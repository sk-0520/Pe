namespace ContentTypeTextNet.Pe.Core.Models.Shell.Value
{
    public class Text: ValueBase
    {
        public Text(string data)
        {
            Data = data;
        }

        #region property

        public string Data { get; }

        #endregion

        #region ValueBase

        public override string Expression => Data;

        #endregion
    }
}
