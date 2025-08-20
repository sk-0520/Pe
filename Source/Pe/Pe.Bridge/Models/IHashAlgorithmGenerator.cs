using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    public enum HashAlgorithmKind
    {
        Unknown,
        SHA1,
        SHA256,
        SHA384,
        SHA512,
        MD5,
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IHashAlgorithmGenerator
    {
        #region function

        HashAlgorithm Create(HashAlgorithmKind hashAlgorithmKind);

        /// <summary>
        /// .NET7 で使えなくなった <see cref="HashAlgorithm.Create(string)"/> のラッパー。
        /// </summary>
        /// <param name="algorithmName"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"><c>System.Security.*</c>系を指定。</exception>
        /// <exception cref="NotImplementedException"></exception>
        HashAlgorithm Create(string algorithmName);

        #endregion
    }
}
