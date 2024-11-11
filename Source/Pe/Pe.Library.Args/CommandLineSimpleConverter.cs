using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Args
{
    sealed public class CommandLineSimpleConverter<TData>: CommandLineConverter<TData>
        where TData : class, new()
    {
        public CommandLineSimpleConverter(CommandLine commandLine)
            : base(commandLine, new TData())
        { }

        #region function

        public TData? GetMappingData()
        {
            var result = MappingCore();
            if(!result) {
                return default;
            }

            return Data;
        }


        #endregion

        #region CommandLineConverter

        public override bool Mapping()
        {
            throw new NotSupportedException();
        }

        #endregion
    }

}
