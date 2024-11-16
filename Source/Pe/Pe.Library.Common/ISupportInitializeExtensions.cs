using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Common
{
    /// <summary>
    /// <see cref="ISupportInitialize"/>の初期化から初期化終了までを<see langword="using" />で実施できるようにする。
    /// </summary>
    /// <remarks>持ち運びしない想定。</remarks>
    public readonly ref struct SupportInitializer<TSupportInitialize>
        where TSupportInitialize : ISupportInitialize
    {
        public SupportInitializer(TSupportInitialize target)
        {
            Target = target;
        }

        #region property

        public TSupportInitialize Target { get; }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Target.EndInit();
        }

        #endregion
    }

    public static class ISupportInitializeExtensions
    {
        #region function

        /// <summary>
        /// 初期化用処理を簡略化。
        /// </summary>
        /// <example>
        /// // 基本こちら
        /// using(target.BeginInitialize()) {
        ///     target.Property = xxx;
        /// }
        /// // なんかあれこれするならこちら
        /// using(var obj = target.BeginInitialize()) {
        ///     obj.Target.Property = xxx;
        /// }
        /// </example>
        /// <param name="target"></param>
        /// <returns></returns>
        public static SupportInitializer<TSupportInitialize> BeginInitialize<TSupportInitialize>(this TSupportInitialize target)
            where TSupportInitialize : ISupportInitialize
        {
            var result = new SupportInitializer<TSupportInitialize>(target);

            result.Target.BeginInit();

            return result;
        }

        #endregion
    }
}
