using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Command
{
    public abstract class CommandItemElementBase: ElementBase, ICommandItem
    {
        #region variable

        private object? _icon;

        #endregion
        protected CommandItemElementBase(IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ContextDispatcher = contextDispatcher;
        }

        #region property

        protected IContextDispatcher ContextDispatcher { get; }

        public List<HitValue> EditableHeaderValues { get; } = new List<HitValue>();
        public List<HitValue> EditableDescriptionValues { get; } = new List<HitValue>();
        public List<HitValue> EditableExtendDescriptionValues { get; } = new List<HitValue>();
        public int EditableScore { get; set; }

        public Action? ExecuteAction { get; set; }

        #endregion

        #region function

        protected abstract object GetIconImpl(in IconScale iconScale);
        protected abstract void ExecuteImpl(ICommandExecuteParameter parameter);
        protected abstract bool EqualsImpl(CommandItemElementBase commandItemElement);

        #endregion

        #region ICommandItem

        public abstract CommandItemKind Kind { get; }

        public string FullMatchValue
        {
            get
            {
                switch(Kind) {
                    case CommandItemKind.LauncherItem:
                    case CommandItemKind.LauncherItemName:
                        return this.GetHeaderText();

                    case CommandItemKind.LauncherItemCode:
                    case CommandItemKind.LauncherItemTag:
                        return this.GetDescriptionText();

                    case CommandItemKind.ApplicationCommand:
                        return this.GetHeaderText();

                    default:
                        throw new NotImplementedException();
                }
            }
        }



        public IReadOnlyList<HitValue> HeaderValues => EditableHeaderValues;

        public IReadOnlyList<HitValue> DescriptionValues => EditableDescriptionValues;
        public virtual IReadOnlyList<HitValue> ExtendDescriptionValues
        {
            get
            {
                if(EditableExtendDescriptionValues.Count == 0) {
                    return EditableDescriptionValues;
                }
                return EditableExtendDescriptionValues;
            }
        }
        public int Score => EditableScore;

        public object GetIcon(in IconScale iconScale)
        {
            ContextDispatcher.VerifyAccess();

            return this._icon ??= GetIconImpl(iconScale);
        }
        public void Execute(ICommandExecuteParameter parameter) => ExecuteImpl(parameter);

        public bool IsEquals(ICommandItem? commandItem)
        {
            if(commandItem == null) {
                return false;
            }

            if(Kind != commandItem.Kind) {
                return false;
            }

            if(commandItem is CommandItemElementBase commandItemElement) {
                return EqualsImpl(commandItemElement);
            }

            return false;
        }

        #endregion
    }

    public sealed class LauncherCommandItemElement: CommandItemElementBase
    {
        public LauncherCommandItemElement(LauncherItemElement launcherItemElement, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(contextDispatcher, loggerFactory)
        {
            LauncherItemElement = launcherItemElement;
            EditableHeaderValues.AddRange(new[] { new HitValue(LauncherItemElement.Name, false) });
        }

        #region property

        private LauncherItemElement LauncherItemElement { get; }
        public CommandItemKind EditableKind { get; set; } = CommandItemKind.LauncherItem;

        #endregion

        #region CommandItemElementBase

        public override CommandItemKind Kind => EditableKind;

        protected override object GetIconImpl(in IconScale iconScale)
        {
            var factory = LauncherItemElement.CreateLauncherIconFactory();
            var iconSource = factory.CreateIconSource(ContextDispatcher);
            return factory.CreateView(iconSource, true, true, ContextDispatcher);
        }

        protected override void ExecuteImpl(ICommandExecuteParameter parameter)
        {
            if(parameter.IsExtend) {
                LauncherItemElement.OpenExtendsExecuteViewAsync(parameter.Screen, CancellationToken.None);
            } else {
                LauncherItemElement.ExecuteAsync(parameter.Screen, CancellationToken.None);
            }
        }

        protected override bool EqualsImpl(CommandItemElementBase commandItemElement)
        {
            if(commandItemElement is LauncherCommandItemElement launcherCommandItemElement) {
                return LauncherItemElement == launcherCommandItemElement.LauncherItemElement;
            }

            return false;
        }

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion
    }

    public sealed class ApplicationCommandItemElement: CommandItemElementBase
    {
        public ApplicationCommandItemElement(ApplicationCommandParameter parameter, IContextDispatcher contextDispatcher, ILoggerFactory loggerFactory)
            : base(contextDispatcher, loggerFactory)
        {
            Parameter = parameter;
            EditableHeaderValues.AddRange(new[] { new HitValue(Parameter.Header, false) });
            EditableDescriptionValues.AddRange(new[] { new HitValue(Parameter.Description, false) });
            EditableExtendDescriptionValues.AddRange(new[] { new HitValue(Parameter.ExtendDescription, false) });
        }

        #region property

        private ApplicationCommandParameter Parameter { get; }

        #endregion

        #region CommandItemElementBase

        public override CommandItemKind Kind => CommandItemKind.ApplicationCommand;

        protected override void ExecuteImpl(ICommandExecuteParameter parameter)
        {
            Parameter.Executor(parameter);
        }

        protected override object GetIconImpl(in IconScale iconScale)
        {
            return Parameter.IconGetter(iconScale);
            //return Application.Current.Resources["pack://application:,,,/Pe.Main;component/Resources/Icon/App.ico"];
        }

        protected override bool EqualsImpl(CommandItemElementBase commandItemElement)
        {
            if(commandItemElement is ApplicationCommandItemElement applicationCommandItemElement) {
                return Parameter == applicationCommandItemElement.Parameter;
            }

            return false;
        }

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
