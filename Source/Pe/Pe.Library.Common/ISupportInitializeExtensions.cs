using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Common
{
    /// <summary>
    /// <see cref="ISupportInitialize"/>の初期化から初期化終了までを<see langword="using" />で実施できるようにする。
    /// </summary>
    public sealed class SupportInitializer<TSupportInitialize>: IDisposable, IDisposed
        where TSupportInitialize : ISupportInitialize
    {
        public SupportInitializer(TSupportInitialize target)
        {
            Target = target;
        }

        #region property

        public TSupportInitialize Target { get; }

        #endregion

        #region IDisposed

        public bool IsDisposed { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S2953:Methods named \"Dispose\" should implement \"IDisposable.Dispose\"", Justification = "<保留中>")]
        private void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Target.EndInit();
                }

                IsDisposed = true;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose メソッドは、SuppressFinalize を呼び出す必要があります", Justification = "<保留中>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3971:\"GC.SuppressFinalize\" should not be called", Justification = "<保留中>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S2953:Methods named \"Dispose\" should implement \"IDisposable.Dispose\"", Justification = "<保留中>")]
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
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
