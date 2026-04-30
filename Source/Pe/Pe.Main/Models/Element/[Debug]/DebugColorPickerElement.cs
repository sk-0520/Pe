using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

#pragma warning disable CA1707 // 識別子はアンダースコアを含むことはできません
namespace ContentTypeTextNet.Pe.Main.Models.Element._Debug_
#pragma warning restore CA1707 // 識別子はアンダースコアを含むことはできません
{
    public class DebugColorPickerElement: DebugElementBase
    {
        public DebugColorPickerElement(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region property

        #endregion

        #region function
        #endregion

        #region DebugElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion


    }
}
