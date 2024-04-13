using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Mvvm.Binding;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Command
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3897:Classes that provide \"Equals(<T>)\" should implement \"IEquatable<T>\"")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4035:Classes implementing \"IEquatable<T>\" should be sealed", Justification = "<保留中>")]
    public class CommandItemViewModel: ViewModelWithLoggerBase
    {
        public CommandItemViewModel(ICommandItem item, IconScale iconScale, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Item = item;
            IconScale = iconScale;
            DispatcherWrapper = dispatcherWrapper;

            HeaderValues = Item.HeaderValues.Select(i => new HitValueItemViewModel(i, LoggerFactory)).ToList();
            DescriptionValues = Item.DescriptionValues.Select(i => new HitValueItemViewModel(i, LoggerFactory)).ToList();
            ExtendDescriptionValues = Item.ExtendDescriptionValues.Select(i => new HitValueItemViewModel(i, LoggerFactory)).ToList();
        }

        #region property

        private ICommandItem Item { get; }
        private IconScale IconScale { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }
        public CommandItemKind Kind => Item.Kind;
        public double Score => Item.Score;

        public string FullMatchValue => Item.FullMatchValue;

        public IReadOnlyList<HitValueItemViewModel> HeaderValues { get; }
        public IReadOnlyList<HitValueItemViewModel> DescriptionValues { get; }
        public IReadOnlyList<HitValueItemViewModel> ExtendDescriptionValues { get; }

        public bool ShowDescription
        {
            get
            {
                switch(Kind) {
                    case CommandItemKind.LauncherItem:
                    case CommandItemKind.LauncherItemName:
                        return false;

                    default:
                        return true;
                }
            }
        }

        public object Icon
        {
            get
            {
                return DispatcherWrapper.Get(i => i.item.GetIcon(i.iconScale), (item: Item, iconScale: IconScale));
            }
        }

        public string Header => string.Join(string.Empty, HeaderValues.Select(i => i.Value));

        #endregion

        #region function

        public void Execute(IScreen screen)
        {
            var isExtend = Keyboard.Modifiers == ModifierKeys.Shift;
            var parameter = new CommandExecuteParameter(screen, isExtend);
            Item.Execute(parameter);
        }

        public bool IsEquals(CommandItemViewModel commandItemViewModel)
        {
            return Item.IsEquals(commandItemViewModel.Item);
        }

        #endregion
    }
}
