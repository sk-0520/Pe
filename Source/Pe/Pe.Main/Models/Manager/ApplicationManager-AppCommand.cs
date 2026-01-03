using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Main.Models.Data;
#if !DOC_FX
using ContentTypeTextNet.Pe.Generator.Exception;
#else
// docfx 用ダミー
[System.AttributeUsage(System.AttributeTargets.Class)]
file sealed class GenerateExceptionAttribute: System.Attribute
{
    public GenerateExceptionAttribute()
    { }
}
#endif

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    [GenerateException]
    public partial class ApplicationExceptionCommandException: Exception
    { }

    partial class ApplicationManager
    {
        #region function

        private IEnumerable<ApplicationCommandParameter> CreateApplicationCommandParameters()
        {
            var factory = ApplicationDiContainer.Build<ApplicationCommandParameterFactory>();

            var result = new List<ApplicationCommandParameter> {
                factory.CreateParameter(ApplicationCommand.Close, p => {
                    Debug.Assert(CommandElement != null);
                    CommandElement.HideView(false);
                }),
                factory.CreateParameter(ApplicationCommand.Exit, p => {
                    // 拡張機能としてアップデート無視
                    Exit(p.IsExtend);
                }),
                factory.CreateParameter(ApplicationCommand.Reboot, p => {
                    Reboot();
                }),
                factory.CreateParameter(ApplicationCommand.About, p => {
                    Debug.Assert(CommandElement != null);
                    CommandElement.HideView(false);
                    _ = ShowAboutViewAsync(CancellationToken.None);
                }),
                factory.CreateParameter(ApplicationCommand.Setting, p => {
                    Debug.Assert(CommandElement != null);
                    CommandElement.HideView(false);
                     _ = ShowSettingViewAsync(CancellationToken.None);
                }),
                factory.CreateParameter(ApplicationCommand.GarbageCollection, p => {
                    GarbageCollection(p.IsExtend);
                }),
                factory.CreateParameter(ApplicationCommand.CopyInformation, p => {
                    var infoCollector = ApplicationDiContainer.Build<ApplicationInformationCollector>();
                    var s = p.IsExtend
                        ? infoCollector.GetLongInformation()
                        : infoCollector.GetShortInformation()
                    ;
                    ClipboardManager.CopyText(s, ClipboardNotify.User);
                }),
                factory.CreateParameter(ApplicationCommand.Proxy, p => {
                    ToggleProxyIsEnabled();
                    var isEnabledProxy = GetProxyIsEnabled();
                    var log = new NotifyMessage(
                        NotifyLogKind.Normal,
                        Properties.Resources.String_Proxy_Toggle_Header,
                        new NotifyLogContent(isEnabledProxy ? Properties.Resources.String_Proxy_Toggle_Content_IsEnabled: Properties.Resources.String_Proxy_Toggle_Content_IsDisabled)
                    );
                    NotifyManager.AppendLogAsync(log, CancellationToken.None);
                }),
                factory.CreateParameter(ApplicationCommand.Help, p => {
                    _ = ShowHelpAsync(p.IsExtend, CancellationToken.None);
                }),
            };

            var commandConfiguration = ApplicationDiContainer.Build<CommandConfiguration>();
            if(commandConfiguration.Application.IsEnabledException) {
                result.Add(factory.CreateParameter(ApplicationCommand.Exception, p => {
                    var contextDispatcher = ApplicationDiContainer.Build<IContextDispatcher>();
                    contextDispatcher.BeginAsync(() => {
#if DEBUG
                        // デバッグ時に例外ぶん投げるとVSが死ぬけどブレークポイント設定しとくと死なないのでこれで濁している
                        if(Debugger.IsAttached) {
                            Debugger.Break();
                        }
#endif
                        throw new ApplicationExceptionCommandException($"{nameof(ApplicationCommand)}.{nameof(ApplicationCommand.Exception)}");
                    }, System.Windows.Threading.DispatcherPriority.SystemIdle);
                }));
            }

            return result;
        }

        #endregion
    }
}
