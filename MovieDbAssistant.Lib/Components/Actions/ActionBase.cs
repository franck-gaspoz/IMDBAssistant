using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.Actions.EventsArgs;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Logger;
using MovieDbAssistant.Lib.Components.Signal;
using MovieDbAssistant.Lib.Components.Sys;
using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Lib.Components.Actions;

///
/// <summary>
/// action base
/// </summary>
#if DEBUG || TRACE
[DebuggerDisplay("{DbgId()}")]
#endif
public abstract class ActionBase<TCommand> :
    IActionFeature,
    ISignalMethodHandler<ActionEndedEvent>,
    ISignalMethodHandler<ActionErroredEvent>
    where TCommand : ActionCommandBase
{
    #region fields & properties

#if DEBUG || TRACE
    /// <summary>
    /// Dbgs the id.
    /// </summary>
    /// <returns>A <see cref="string"/></returns>
    public string DbgId() => this.Id();
#endif

    /// <inheritdoc/>
    public string Id => this.Id();

    /// <summary>
    /// instance id
    /// </summary>
    public SharedCounter InstanceId { get; }

    /// <summary>
    /// true if buzy
    /// </summary>
    public bool Buzy { get; protected set; } = false;

    /// <inheritdoc/>
    public bool RunInBackground { get; protected set; } = true;

    protected ILogger<ActionBase<TCommand>> Logger;
    protected readonly IConfiguration Config;
    protected readonly ISignalR Signal;
    protected readonly IServiceProvider ServiceProvider;
    protected TCommand? Com;
    protected string ActionOnGoingMessage { get; set; }
    protected string MessageErrorUnhandled { get; set; }
    protected string MessageFeatureIsBuzy { get; set; }

    readonly BackgroundWorkerWrapper? _backgroundWorker;

    protected event EventHandler<ActionBaseEventContextArgs>? StartRunningAction;
    protected event EventHandler<ActionBaseEventContextArgs>? StoppingAnimInfo;
    protected event EventHandler<ActionBaseEventContextArgs>? StartingAnimWorkInfo;
    protected event EventHandler<ActionBaseEventContextArgs>? MessageWarningOpening_IsBuzy;
    protected event EventHandler<ActionBaseEventTextArgs>? MessagesErrorOpening_ErrorUnHandled;

    #endregion

    #region build & init

    public ActionBase(
        ILogger<ActionBase<TCommand>> logger,
        IConfiguration config,
        ISignalR signal,
        IServiceProvider serviceProvider,
        string actionOnGoingMessage,
        bool runInBackground,
        string messageErrorUnhandled,
        string featureIsBuzy)
    {
        InstanceId = new(this);
        ServiceProvider = serviceProvider;
        Signal = signal;
        Logger = logger;
        Config = config;
        ActionOnGoingMessage = actionOnGoingMessage;
        RunInBackground = runInBackground;
        MessageErrorUnhandled = messageErrorUnhandled;
        MessageFeatureIsBuzy = featureIsBuzy;
        _backgroundWorker = new(
            logger,
            signal,
            config,
            this);
        _backgroundWorker.AddListener(this, this, signal);
        this.AddListener(this, this, signal);
    }

    #endregion

    #region action feature prototype

    /// <summary>
    /// action
    /// </summary>
    /// <param name="context">action context</param>
    protected abstract void Action(ActionContext context);

    #endregion

    #region /**----- signals handlers -----*/

    /// <summary>
    /// setup feature state ended
    /// </summary>
    /// <param name="context">action context</param>
    /// <param name="error">is end due to error</param>
    public virtual void Handle(
        object sender,
        ActionEndedEvent @event)
    {
        Logger.LogTrace(this, "END");
        if (Com!.HandleUI)
            StoppingAnimInfo?.Invoke(this,
                new(@event, sender, @event.Context));

        Signal.Send(this, new ActionEndingEvent(@event.Context));

        if (!@event.Context.IsErrored)
        {
            Signal.Send(this, new ActionSuccessfullyEnded(@event.Context));

            Buzy = false;

            Signal.Send(this, new ActionFinalisedEvent(@event.Context));
        }

        Buzy = false;
    }

    /// <summary>
    /// setup feature state error
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="event">action errored event</param>
    public virtual void Handle(object sender, ActionErroredEvent @event)
    {
        var message = @event.ToString();

        if (@event.Exception != null)
            Logger.LogError(this, message, @event.Exception);
        else
            Logger.LogError(this, message);

        @event.Context.LogError(@event);

        Signal.Send(this, new ActionEndedEvent(@event.Context));

        Signal.Send(this, new ActionBeforePromptEvent(@event.Context));

        if (Com!.HandleUI)
            MessagesErrorOpening_ErrorUnHandled?.Invoke(this,
                new(@event, this, @event.Context, message));

        AppLoggerExtensions.LogError(
            Logger,
            this,
            Config[MessageErrorUnhandled]
            + '\n'
            + message);

        Signal.Send(this, new ActionAfterPromptEvent(@event.Context));

        Signal.Send(this, new ActionFinalisedEvent(@event.Context));
    }

    #endregion /**----  -----*/

    #region operations

    /// <summary>
    /// run the feature in or not in background worker
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="com">command</param>
    protected void Run(object sender, TCommand com)
    {
        ActionContext? context = null;
        try
        {
            context = ServiceProvider
                .GetRequiredService<ActionContext>()
                .Setup(this, com, [sender]);

            if (com.ActionContext != null)
            {
                context.Merge(
                    this,
                    com.ActionContext);
                com.ActionContext.Setup(context);
            }
            else
                com.Setup(context);

            // -- single thread barrier
            if (Buzy)
            {
                if (com.HandleUI)
                    MessageWarningOpening_IsBuzy?.Invoke(
                        this,
                        new(com, sender, null));

                var msg = MessageFeatureIsBuzy;

                Signal.Send(this, new ActionErroredEvent(context!,
                    new InvalidOperationException(msg + ": " + GetType().Name)));

                return;
            }

            Com = com;
            Buzy = true;

            StartRunningAction?.Invoke(this, new(com, this, context));

            if (RunInBackground)
            {
                // run action in background thread
                _backgroundWorker!.RunAction(
                    this,
                    context,
                    (ctx, @from, @event)
                        => DoWork(context, sender));
            }
            else
            {
                DoWork(context, sender);
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(this, "exception: " + ex.Message);
            if (context != null)
                Signal.Send(this, new ActionErroredEvent(context!, ex));
            else
                Logger.LogError(this, "context is not defined");
        }
    }

    void DoWork(ActionContext context, object sender)
    {
        Logger.LogTrace(this, $"DoWork: background={RunInBackground} handleUI={Com!.HandleUI}");
        try
        {
            if (Com!.HandleUI)
                StartingAnimWorkInfo?.Invoke(this,
                    new(context.Command, sender, context));

            Logger.LogTrace(this, "action");

            Action(context);

            if (!RunInBackground)
            {
                Logger.LogTrace(this, $"action ended");
                Signal.Send(this, new ActionEndedEvent(context));
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(this, "exception: " + ex.Message);
            Signal.Send(this, new ActionErroredEvent(context, ex));
        }
        finally
        {
            _backgroundWorker.RemoveListener(this, this, Signal);
        }
    }

    #endregion
}
