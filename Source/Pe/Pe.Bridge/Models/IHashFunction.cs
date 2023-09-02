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
    /// ハッシュ関数の簡易的なやつ。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IHashFunction
    {
        #region function

        HashAlgorithm Create(HashAlgorithmKind hashAlgorithmKind);

        #endregion
    }

    public static class IHashFunctionExtensions
    {

        #region function

        /// <summary>
        /// .NET7 で使えなくなった <see cref="HashAlgorithm.Create(string)"/> のラッパー。
        /// </summary>
        /// <param name="hashFunction"></param>
        /// <param name="algorithmName"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"><c>System.Security.*</c>系を指定。</exception>
        /// <exception cref="NotImplementedException"></exception>
        public static HashAlgorithm Create(this IHashFunction hashFunction, string algorithmName)
        {
            var kind = algorithmName.ToUpperInvariant() switch {
                "SHA" or "SHA1" or "SHA-1" => HashAlgorithmKind.SHA1,
                "SHA256" or "SHA-256" => HashAlgorithmKind.SHA256,
                "SHA384" or "SHA-384" => HashAlgorithmKind.SHA384,
                "SHA512" or "SHA-512" => HashAlgorithmKind.SHA512,
                "MD5" => HashAlgorithmKind.MD5,

                "SYSTEM.SECURITY.CRYPTOGRAPHY.SHA1"
                or "SYSTEM.SECURITY.CRYPTOGRAPHY.SHA256"
                or "SYSTEM.SECURITY.CRYPTOGRAPHY.SHA384"
                or "SYSTEM.SECURITY.CRYPTOGRAPHY.SHA512"
                or "SYSTEM.SECURITY.CRYPTOGRAPHY.MD5"
                or "SYSTEM.SECURITY.CRYPTOGRAPHY.HASHALGORITHM"
                => throw new NotSupportedException(algorithmName),

                _ => throw new NotImplementedException(algorithmName),
            };

            return hashFunction.Create(kind);
        }

        #endregion
    }
}
