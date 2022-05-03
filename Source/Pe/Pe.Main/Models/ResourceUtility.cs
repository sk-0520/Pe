using System;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models
{
    public static class ResourceUtility
    {
        #region function

        public static Stream OpenSyntaxStreamByName(string syntaxResourceName)
        {
            var resourceValue = Properties.Resources.ResourceManager.GetString(syntaxResourceName);
            if(resourceValue == null) {
                throw new InvalidProgramException($"{nameof(syntaxResourceName)}: {syntaxResourceName}");
            }

            return OpenSyntaxStream(resourceValue);
        }

        public static Stream OpenSyntaxStream(string syntax)
        {
            var binary = Encoding.UTF8.GetBytes(syntax);
            return new MemoryReleaseStream(binary);
        }

        #endregion
    }
}
