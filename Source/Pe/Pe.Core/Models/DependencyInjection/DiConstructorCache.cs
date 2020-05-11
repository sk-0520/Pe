using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
{
    /// <summary>
    /// コンストラクタ情報キャッシュ。
    /// </summary>
    public sealed partial class DiConstructorCache
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="constructorInfo">コンストラクタ情報。</param>
        /// <param name="parameterInfos">コンストラクタのパラメータ。</param>
        public DiConstructorCache(ConstructorInfo constructorInfo, IReadOnlyList<ParameterInfo> parameterInfos)
        {
            ConstructorInfo = constructorInfo;
            ParameterInfos = parameterInfos;

            var map = new Dictionary<ParameterInfo, InjectAttribute>();
            foreach(var parameterInfo in ParameterInfos) {
                var attr = parameterInfo.GetCustomAttribute<InjectAttribute>();
                if(attr != null) {
                    map.Add(parameterInfo, attr);
                }
            }
            ParameterInjections = map;
        }

        #region proeprty

        public ConstructorInfo ConstructorInfo { get; }
        public IReadOnlyList<ParameterInfo> ParameterInfos { get; }
        public IReadOnlyDictionary<ParameterInfo, InjectAttribute> ParameterInjections { get; }
        Func<object[], object>? Creator { get; set; }

        #endregion

        #region function

        IEnumerable<ParameterExpression> CreateParameterExpressions()
        {
            return ParameterInfos
                .Select((p, i) => Expression.Parameter(typeof(object), "wrapperArg_" + i.ToString()))
            ;
        }

        IEnumerable<UnaryExpression> CreateConvertExpressions(IEnumerable<ParameterExpression> parameterExpressions)
        {
            return ParameterInfos
                .Zip(parameterExpressions, (pi, pe) => Expression.Convert(pe, pi.ParameterType))
            ;
        }

        FuncN CreateFunction<FuncN>()
        {
            var parameterExpressions = CreateParameterExpressions().ToArray();
            var convertExpressions = CreateConvertExpressions(parameterExpressions).ToArray();
            Debug.Assert(parameterExpressions.Length == convertExpressions.Length);

            var constructorNewParams = Expression.Lambda<FuncN>(
                Expression.Convert(
                    Expression.New(
                        ConstructorInfo,
                        convertExpressions
                    ),
                    typeof(object)
                ),
                "constructorNewParams_" + ParameterInfos.Count.ToString(),
                parameterExpressions
            );
            var creator = constructorNewParams.Compile();

            return creator;
        }

        /// <summary>
        /// コンストラクタの呼び出し。
        /// <para>内部実装は DiConstructorCacheImpl.tt にて機械生成。</para>
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Create(object[] parameters)
        {
            if(Creator == null) {
                if(ParameterInfos.Count == 0) {
                    var newExp = Expression.New(ConstructorInfo);
                    var lambda = Expression.Lambda<Func<object>>(newExp);
                    var creator = lambda.Compile();
                    Creator = p => creator();
                } else {
                    Creator = CreateCore(parameters);
                }
            }

            return Creator(parameters);
        }

        #endregion
    }
}
