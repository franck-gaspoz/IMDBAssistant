using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.App.Configuration;
using MovieDbAssistant.App.Services;
using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// action feature base
/// </summary>
#if DEBUG || TRACE
[DebuggerDisplay("{DbgId()}")]
#endif
abstract class ActionFeatureBase<TCommand> : ActionBase<TCommand>
    where TCommand : ActionCommandBase
{
    #region fields & properties

    protected readonly Messages Messages;
    readonly IOptions<AppSettings> _appSettings;

    protected TrayMenuService Tray => ServiceProvider
        .GetRequiredService<TrayMenuService>();

    #endregion

    #region build & init

    public ActionFeatureBase(
        ILogger<ActionFeatureBase<TCommand>> logger,
        IConfiguration config,
        ISignalR signal,
        IServiceProvider serviceProvider,
        Messages messages,
        IOptions<AppSettings> appSettings,
        string actionOnGoingMessage,
        bool runInBackground,
        string? messageErrorUnhandled = null,
        string? messageFeatureIsBuzy = null) : base(
            logger,
            config,
            signal,
            serviceProvider,
            actionOnGoingMessage,
            runInBackground,
            messageErrorUnhandled ?? appSettings.Value.Texts.ErrorUnhandled,
            messageFeatureIsBuzy ?? appSettings.Value.Texts.FeatureBusy)
    {
        Messages = messages;
        _appSettings = appSettings;
        StoppingAnimInfo += (o, e) => Tray.StopAnimInfo();

        StartingAnimWorkInfo += (o, e) => Tray
            .AnimWorkInfo(
                Logger,
                e.Context!,
                e.Sender,
                ActionOnGoingMessage!);

        MessagesErrorOpening_ErrorUnHandled += (o, e) =>
            Messages.Err(MessageErrorUnhandled, '\n' + e.Text);

        MessageWarningOpening_IsBuzy += (o, e) =>
            Messages.Warn(MessageFeatureIsBuzy);
    }

    #endregion
}
