using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using Microsoft.Extensions.Logging;
using NLog.Fluent;

namespace ContentTypeTextNet.Pe.Main.Models.Command
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    internal class CommandDescriptionAttribute: Attribute
    {
        public CommandDescriptionAttribute(string resourceName)
            : this(resourceName, false)
        { }


        public CommandDescriptionAttribute(string resourceName, bool isExtend)
        {
            ResourceName = resourceName;
            IsExtend = isExtend;
        }

        #region property

        public string ResourceName { get; }
        public bool IsExtend { get; }

        #endregion
    }

    internal enum ApplicationCommand
    {
        /// <summary>
        /// コマンドウィンドウを閉じる。
        /// </summary>
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Close))]
        Close,
        /// <summary>
        /// アプリケーションの終了。
        /// <para>通常はアップデートが可能であればアップデート行う</para>
        /// <para>拡張機能: アップデートがあっても終了する。</para>
        /// </summary>
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Exit), false)]
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Exit_Extend), true)]
        Exit,
        /// <summary>
        /// 再起動。
        /// <para>アップデートがあっても再起動。</para>
        /// </summary>
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Reboot))]
        Reboot,
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_About))]
        About,
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Setting))]
        Setting,
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_GarbageCollection), false)]
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_GarbageCollection_Extend), true)]
        GarbageCollection,
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_CopyInformation), false)]
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_CopyInformation_Extend), true)]
        CopyInformation,
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Help))]
        Help,
    }

    internal class ApplicationCommandParameterFactory
    {
        public ApplicationCommandParameterFactory(CommandConfiguration commandConfiguration)
        {
            CommandConfiguration = commandConfiguration;
        }

        #region property

        CommandConfiguration CommandConfiguration { get; }

        #endregion

        #region function

        string ToHeader(ApplicationCommand applicationCommand)
        {
            var rawValue = CommandConfiguration.ApplicationMapping[applicationCommand];
            var joinedValue = string.Join(CommandConfiguration.ApplicationSeparator, rawValue.Split());
            return CommandConfiguration.ApplicationPrefix + joinedValue;
        }

        (string narmal, string extend) ToDescriptions(ApplicationCommand applicationCommand)
        {
            // テスト側でもろもろ担保
            var descriptionAttributes = applicationCommand.GetType().GetField(applicationCommand.ToString())!.GetCustomAttributes<CommandDescriptionAttribute>().ToList();
            if(descriptionAttributes.Count == 1) {
                var singleValue = Properties.Resources.ResourceManager.GetString(descriptionAttributes[0].ResourceName)!;
                return (singleValue, singleValue);
            } else {
                var a = Properties.Resources.ResourceManager.GetString(descriptionAttributes[0].ResourceName)!;
                var b = Properties.Resources.ResourceManager.GetString(descriptionAttributes[1].ResourceName)!;
                if(descriptionAttributes[1].IsExtend) {
                    return (a, b);
                } else {
                    return (b, a);
                }
            }
        }

        public ApplicationCommandParameter CreateParameter(ApplicationCommand applicationCommand, Action<ICommandExecuteParameter> executor)
        {
            var descriptions = ToDescriptions(applicationCommand);
            return new ApplicationCommandParameter(ToHeader(applicationCommand), descriptions.narmal, descriptions.extend, iconBox => {
                var control = new Control();
                using(Initializer.Begin(control)) {
                    control.Template = (ControlTemplate)Application.Current.Resources["App-Image-Command"];
                    control.Style = iconBox switch
                    {
                        IconBox.Small => (Style)Application.Current.Resources["Image-Small"],
                        IconBox.Normal => (Style)Application.Current.Resources["Image-Normal"],
                        IconBox.Big => (Style)Application.Current.Resources["Image-Big"],
                        IconBox.Large => (Style)Application.Current.Resources["Image-Large"],
                        _ => throw new NotImplementedException()
                    };
                }
                return control;

            }, executor);
        }

        #endregion
    }

    public class ApplicationCommandParameter
    {
        public ApplicationCommandParameter(string header, string description, string extendDescription, Func<IconBox, object> iconGetter, Action<ICommandExecuteParameter> executor)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            ExtendDescription = extendDescription ?? throw new ArgumentNullException(nameof(extendDescription));
            IconGetter = iconGetter ?? throw new ArgumentNullException(nameof(iconGetter));
            Executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        #region property

        public string Header { get; }
        public string Description { get; }
        public string ExtendDescription { get; }
        public Func<IconBox, object> IconGetter { get; }
        public Action<ICommandExecuteParameter> Executor { get; }

        #endregion
    }

    public class ApplicationCommandFinder: DisposerBase, ICommandFinder
    {
        #region variable

        bool _isInitialize;

        #endregion

        public ApplicationCommandFinder(IReadOnlyList<ApplicationCommandParameter> parameters, CommandConfiguration commandConfiguration, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            DispatcherWrapper = dispatcherWrapper;

            Parameters = parameters;
            CommandConfiguration = commandConfiguration;
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        CommandConfiguration CommandConfiguration { get; }
        IReadOnlyList<ApplicationCommandParameter> Parameters { get; }

        #endregion

        #region function

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {

            }

            base.Dispose(disposing);
        }
        #endregion

        #region ICommandFinder

        public bool IsInitialize
        {
            get => this._isInitialize;
            private set => this._isInitialize = value;
        }

        public void Initialize()
        {
            if(IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            IsInitialize = true;
        }

        public void Refresh()
        {
            if(!IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }
        }


        public IEnumerable<ICommandItem> ListupCommandItems(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            if(!IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            if(string.IsNullOrWhiteSpace(inputValue)) {
                foreach(var parameter in Parameters) {
                    var element = new ApplicationCommandItemElement(parameter, DispatcherWrapper, LoggerFactory);
                    element.Initialize();
                    element.EditableScore = hitValuesCreator.GetScore(ScoreKind.Initial, hitValuesCreator.NoBonus) - 1;
                    yield return element;
                }
                yield break;
            }


            if(!inputValue.StartsWith(CommandConfiguration.ApplicationPrefix, StringComparison.Ordinal)) {
                yield break;
            }

            foreach(var parameter in Parameters) {
                var parameterMatches = hitValuesCreator.GetMatches(parameter.Header, inputRegex);
                if(parameterMatches.Any()) {
                    var ranges = hitValuesCreator.ConvertRanges(parameterMatches);
                    var hitValue = hitValuesCreator.ConvertHitValues(parameter.Header, ranges);

                    var element = new ApplicationCommandItemElement(parameter, DispatcherWrapper, LoggerFactory);
                    element.Initialize();

                    element.EditableHeaderValues.SetRange(hitValue);
                    element.EditableScore = hitValuesCreator.CalcScore(parameter.Header, element.EditableHeaderValues);
                    yield return element;
                }
            }
        }

        #endregion
    }
}
