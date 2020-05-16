using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public abstract class NotifyPropertyBase : BindableBase, IDisposable, IDisposer
    {
        public NotifyPropertyBase()
        { }

        ~NotifyPropertyBase()
        {
            Dispose(false);
        }

        #region property
        #endregion

        #region IDisposable

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// <para>呼び出し時点では<see cref="IsDisposed"/>は偽のまま。</para>
        /// </summary>
        [field: NonSerialized]
        public event EventHandler? Disposing;

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed { get; private set; }

        protected void ThrowIfDisposed([CallerMemberName] string _callerMemberName = "")
        {
            if(IsDisposed) {
                throw new ObjectDisposedException(_callerMemberName);
            }
        }


        /// <summary>
        /// <see cref="IDisposable.Dispose"/>の内部処理。
        /// <para>継承先クラスでは本メソッドを呼び出す必要がある。</para>
        /// </summary>
        /// <param name="disposing">CLRの管理下か。</param>
        protected virtual void Dispose(bool disposing)
        {
            if(IsDisposed) {
                return;
            }

            if(Disposing != null) {
                Disposing(this, EventArgs.Empty);
            }

            if(disposing) {
#pragma warning disable S3971 // "GC.SuppressFinalize" should not be called
                GC.SuppressFinalize(this);
#pragma warning restore S3971 // "GC.SuppressFinalize" should not be called
            }

            IsDisposed = true;
        }

        /// <summary>
        /// 解放。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }

    public abstract class BindModelBase : NotifyPropertyBase
    {
        public BindModelBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            LoggerFactory = loggerFactory;
        }

        ~BindModelBase()
        {
            Dispose(false);
        }

        #region property

        protected ILogger Logger { get; private set; }
        protected ILoggerFactory LoggerFactory { get; private set; }

        #endregion

        //#region NotifyPropertyBase

        //protected override void Dispose(bool disposing)
        //{
        //    if(!IsDisposed) {
        //        if(disposing) {
        //            if(Logger is IDisposable disposer) {
        //                disposer.Dispose();
        //            }
        //        }
        //    }

        //    base.Dispose(disposing);
        //}

        //#endregion

    }

    public sealed class WrapModel<TData> : BindModelBase
    {
        public WrapModel(TData data, ILoggerFactory loggerFactory)
            : this(data, true, loggerFactory)
        { }
        public WrapModel(TData data, bool useDispose, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Data = data;
            UseDispose = useDispose;
        }

        #region property

        public TData Data { get; private set; }
        bool UseDispose { get; }
        #endregion

        #region BindModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(UseDispose && Data is IDisposable diposer) {
                    diposer.Dispose();
                }
            }
            base.Dispose(disposing);
            // 参照があれば切っておく
            Data = default!;
        }

        #endregion
    }

    public static class WrapModel
    {
        public static WrapModel<TData> Create<TData>(TData data, ILoggerFactory loggerFactory)
        {
            return new WrapModel<TData>(data, loggerFactory);
        }

        public static WrapModel<TData> Create<TData>(TData data, bool useDispose, ILoggerFactory loggerFactory)
        {
            return new WrapModel<TData>(data, useDispose, loggerFactory);
        }

    }

}
