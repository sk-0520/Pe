using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.ExtendsExecute
{
    public class WorkDirectoryDragAndDropGuideline: DragAndDropGuidelineBase
    {
        public WorkDirectoryDragAndDropGuideline(IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(contextDispatcher, loggerFactory)
        { }

        #region function

        public bool CanDragStart(UIElement sender, MouseEventArgs e) => false;

        #endregion
    }
}
