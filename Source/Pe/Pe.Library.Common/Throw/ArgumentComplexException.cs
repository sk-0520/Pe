using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Common.Throw
{
    /// <summary>
    /// <see cref="ArgumentComplexException"/> のパラメータ名が不正な場合に投げられる例外。
    /// </summary>
    /// <remarks>
    /// <para><see langword="public"/> だけど原則 <see cref="ArgumentComplexException"/> 内部でしか使用しないし <see langword="catch"/> することも想定していない。</para>
    /// </remarks>
    public sealed class ArgumentComplexParameterNameException: Exception
    {
        public ArgumentComplexParameterNameException(int index)
            : base($"Parameter name[{index}] is null or empty.")
        {
            Index = index;
        }

        #region property

        public int Index { get; }

        #endregion
    }


    /// <summary>
    /// 複数の引数が互いに不正な場合に投げられる例外。
    /// </summary>
    public class ArgumentComplexException: ArgumentException
    {
        public ArgumentComplexException(string message, string paramName1, string paramName2)
            : base(message, ToParameterName([paramName1, paramName2]))
        {
            ParamNames = [paramName1, paramName2];
        }

        public ArgumentComplexException(string message, string paramName1, string paramName2, string paramName3, params string[] paramNames)
            : base(message, ToParameterName([paramName1, paramName2, paramName3, .. paramNames]))
        {
            ParamNames = [paramName1, paramName2, paramName3, .. paramNames];
        }

        #region property

        public IReadOnlyList<string> ParamNames { get; }

        #endregion

        #region function

        private static string ToParameterName(string[] paramNames)
        {
            for(var i = 0; i < paramNames.Length; i++) {
                var paramName = paramNames[i];
                if(string.IsNullOrWhiteSpace(paramName)) {
                    throw new ArgumentComplexParameterNameException(i);
                }
            }

            return string.Join(", ", paramNames);
        }

        #endregion
    }
}
