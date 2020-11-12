using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public interface IDropable
    {
        /// <summary>
        /// ドラッグ中アイテムが上に存在しているか。
        /// </summary>
        bool IsDragOver { get; set; }
    }

    public interface IDragAndDrop
    {
        #region property

        /// <summary>
        /// ドラッグ開始とみなす距離。
        /// </summary>
        [PixelKind(Px.Device)]
        Size DragStartSize { get; set; }

        /// <summary>
        ///<see cref="UIElement.PreviewMouseMove"/> 的な Preview のイベントを使用する。
        ///<para>基本的には false で <see cref="UIElement.MouseMove"/> を使用する。 なんにせよ<see cref="UIElement.PreviewMouseDown"/> は強制される。</para>
        /// </summary>
        bool UsingPreviewEvent { get; }

        #endregion

        #region function

        void MouseDown(UIElement sender, MouseButtonEventArgs e);
        void MouseMove(UIElement sender, MouseEventArgs e);
        void DragEnter(UIElement sender, DragEventArgs e);
        void DragOver(UIElement sender, DragEventArgs e);
        void DragLeave(UIElement sender, DragEventArgs e);
        void Drop(UIElement sender, DragEventArgs e);

        #endregion
    }


    public class DragParameter
    {
        public DragParameter(UIElement element, DragDropEffects effects, DataObject data)
        {
            Element = element;
            Effects = effects;
            Data = data;
        }

        #region property

        public UIElement Element { get; set; }
        public DragDropEffects Effects { get; set; }
        public DataObject Data { get; set; }

        #endregion
    }


    public abstract class DragAndDropBase : IDragAndDrop
    {
        /// <summary>
        /// <see cref="UsingPreviewEvent"/> は false を使用する。
        /// </summary>
        /// <param name="logger"></param>
        protected DragAndDropBase(ILogger logger)
        {
            UsingPreviewEvent = false;
            Logger = logger;
        }
        /// <summary>
        /// <see cref="UsingPreviewEvent"/> は false を使用する。
        /// </summary>
        /// <param name="loggerFactory"></param>
        protected DragAndDropBase(ILoggerFactory loggerFactory)
        {
            UsingPreviewEvent = false;
            Logger = loggerFactory.CreateLogger(GetType());
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="usingPreviewEvent"><see cref="UIElement.PreviewMouseMove"/> 的な Preview のイベントを使用するか。</param>
        /// <param name="logger"></param>
        protected DragAndDropBase(bool usingPreviewEvent, ILogger logger)
        {
            UsingPreviewEvent = usingPreviewEvent;
            Logger = logger;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="usingPreviewEvent"><see cref="UIElement.PreviewMouseMove"/> 的な Preview のイベントを使用するか。</param>
        /// <param name="loggerFactory"></param>
        protected DragAndDropBase(bool usingPreviewEvent, ILoggerFactory loggerFactory)
        {
            UsingPreviewEvent = usingPreviewEvent;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILogger Logger { get; }

        [PixelKind(Px.Logical)]
        Point DragStartPosition { get; set; }

        public bool IsDragging { get; private set; }

        /// <summary>
        /// ドラッグ開始とみなす距離。
        /// </summary>
        [PixelKind(Px.Device)]
        public Size DragStartSize { get; set; } = new Size(10, 10);

        #endregion

        #region function

        protected abstract bool CanDragStartImpl(UIElement sender, MouseEventArgs e);
        /// <summary>
        /// ドラッグした<see cref="DragParameter"/>の取得。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected abstract IResultSuccessValue<DragParameter> GetDragParameterImpl(UIElement sender, MouseEventArgs e);

        void MouseDownCore(UIElement sender, MouseEventArgs e)
        {
            DragStartPosition = e.GetPosition(null);
        }

        void MouseMoveCore(UIElement sender, MouseEventArgs e)
        {
            var nowPosition = e.GetPosition(null);

            var isDragX = Math.Abs(nowPosition.X - DragStartPosition.X) > DragStartSize.Width;
            var isDragY = Math.Abs(nowPosition.Y - DragStartPosition.Y) > DragStartSize.Height;
            if(isDragX || isDragY) {
                var parameterResult = GetDragParameterImpl(sender, e);
                if(parameterResult.Success) {
                    var parameter = parameterResult.SuccessValue;
                    if(parameter == null) {
                        Logger.LogWarning(nameof(parameter) + " is null, 後続D&D処理スキップ");
                    } else {
                        IsDragging = true;
                        try {
                            DragDrop.DoDragDrop(parameter.Element, parameter.Data, parameter.Effects);
                        } finally {
                            IsDragging = false;
                        }
                    }
                }
            }
        }

        #endregion

        #region IDragAndDrop

        /// <summary>
        /// <see cref="IDragAndDrop.UsingPreviewEvent"/>
        /// </summary>
        public bool UsingPreviewEvent { get; }

        public void MouseDown(UIElement sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed) {
                MouseDownCore(sender, e);
            }
        }

        public void MouseMove(UIElement sender, MouseEventArgs e)
        {
            if(e.LeftButton != MouseButtonState.Pressed) {
                return;
            }

            if(IsDragging) {
                return;
            }

            if(!CanDragStartImpl(sender, e)) {
                return;
            }

            MouseMoveCore(sender, e);
        }

        public abstract void DragEnter(UIElement sender, DragEventArgs e);

        public abstract void DragOver(UIElement sender, DragEventArgs e);

        public abstract void DragLeave(UIElement sender, DragEventArgs e);

        public abstract void Drop(UIElement sender, DragEventArgs e);


        #endregion
    }

    public delegate bool CanDragStartDelegate(UIElement sender, MouseEventArgs e);
    public delegate IResultSuccessValue<DragParameter> GetDragParameterDelegate(UIElement sender, MouseEventArgs e);
    public delegate void DragAndDropDelegate(UIElement sender, DragEventArgs e);

    public class DelegateDragAndDrop : DragAndDropBase
    {
        public DelegateDragAndDrop(ILogger logger)
            : base(logger)
        { }
        public DelegateDragAndDrop(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        public DelegateDragAndDrop(bool usingPreviewEvent, ILogger logger)
            : base(usingPreviewEvent, logger)
        { }
        public DelegateDragAndDrop(bool usingPreviewEvent, ILoggerFactory loggerFactory)
            : base(usingPreviewEvent, loggerFactory)
        { }

        #region property

        public CanDragStartDelegate? CanDragStart { get; set; }
        public GetDragParameterDelegate? GetDragParameter { get; set; }
        public DragAndDropDelegate? DragEnterAction { get; set; }
        public DragAndDropDelegate? DragLeaveAction { get; set; }
        public DragAndDropDelegate? DragOverAction { get; set; }
        public DragAndDropDelegate? DropAction { get; set; }

        #endregion

        #region DragAndDropBase

        protected override bool CanDragStartImpl(UIElement sender, MouseEventArgs e)
        {
            if(CanDragStart != null) {
                return CanDragStart(sender, e);
            }

            return false;
        }

        protected override IResultSuccessValue<DragParameter> GetDragParameterImpl(UIElement sender, MouseEventArgs e)
        {
            if(GetDragParameter != null) {
                return GetDragParameter(sender, e);
            }

            return ResultSuccessValue.Failure<DragParameter>();
        }

        public override void DragEnter(UIElement sender, DragEventArgs e)
        {
            DragEnterAction?.Invoke(sender, e);
        }

        public override void DragLeave(UIElement sender, DragEventArgs e)
        {
            DragLeaveAction?.Invoke(sender, e);
        }

        public override void DragOver(UIElement sender, DragEventArgs e)
        {
            DragOverAction?.Invoke(sender, e);
        }

        public override void Drop(UIElement sender, DragEventArgs e)
        {
            DropAction?.Invoke(sender, e);
        }

        #endregion
    }
}
