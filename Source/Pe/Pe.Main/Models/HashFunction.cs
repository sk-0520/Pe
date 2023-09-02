using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Main.Models
{
    internal class HashFunction: IHashFunction
    {
        #region IHashFunction

        public HashAlgorithm Create(HashAlgorithmKind hashAlgorithmKind)
        {
            return hashAlgorithmKind switch {
                HashAlgorithmKind.SHA1 => SHA1.Create(),
                HashAlgorithmKind.SHA256 => SHA256.Create(),
                HashAlgorithmKind.SHA384 => SHA384.Create(),
                HashAlgorithmKind.SHA512 => SHA512.Create(),
                HashAlgorithmKind.MD5 => MD5.Create(),
                _ => throw new NotImplementedException()
            };
        }

        #endregion
    }
}
