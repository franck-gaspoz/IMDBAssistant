using System.ComponentModel;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Signal;
using MovieDbAssistant.Lib.Components.Sys;

namespace MovieDbAssistant.App.Components.Tray;

/// <summary>
/// The tray background worker.
/// </summary>
sealed class TrayBackgroundWorker : BackgroundWorkerWrapper
{
    readonly IConfiguration _config;
    readonly TrayMenuService _trayMenuService;
    Action<TrayMenuService>? _action;
    readonly int _interval;
    bool _stopOnBallonTipClosed;
    readonly bool _autoRepeat;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrayBackgroundWorker"/> class.
    /// </summary>
    /// <param name="trayMenuService">The tray menu service.</param>
    public TrayBackgroundWorker(
        ILogger logger,
        ISignalR signal,
        IConfiguration config,
        TrayMenuService trayMenuService,
        int interval,
        bool stopOnBallonTipClosed = true,
        bool autoRepeat = true) : base(logger, signal, string.Empty)
    {
        Owner = this;
        _config = config;
        _trayMenuService = trayMenuService;
        _interval = interval;
        _stopOnBallonTipClosed = stopOnBallonTipClosed;
        _autoRepeat = autoRepeat;
    }

    public TrayBackgroundWorker Run(
        ActionContext context,
        object caller,
        Action<TrayMenuService> action,
        int? interval = null,
        bool? stopOnBallonTipClosed = null,
        bool? autoRepeat = null,
        Action? onStop = null)
    {
        _action = action;
        if (stopOnBallonTipClosed != null)
            _stopOnBallonTipClosed = stopOnBallonTipClosed.Value;

        Setup(
            _config,
            DoWorkAction,
            interval ?? _interval,
            PreDoWork,
            autoRepeat ?? _autoRepeat
            );
        Setup(o => { 
            OnStop(this); 
            onStop?.Invoke(); });

        base.Run(context, caller);
        return this;
    }

    void DoWorkAction(ActionContext context, object? o, DoWorkEventArgs e)
        => _action?.Invoke(_trayMenuService);

    void PreDoWork()
    {
        _trayMenuService.BalloonTipClosed -=
            _trayMenuService.BallonTipCloseBackgroundWorkerHandler;
        if (_stopOnBallonTipClosed)
            _trayMenuService.BalloonTipClosed +=
                _trayMenuService.BallonTipCloseBackgroundWorkerHandler;
    }

    protected override string LogPrefix()
        => LogNativePrefix();
}

