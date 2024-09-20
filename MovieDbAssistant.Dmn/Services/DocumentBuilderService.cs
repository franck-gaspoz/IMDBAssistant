﻿using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Components.DataProviders;
using MovieDbAssistant.Lib.Components;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Dmn.Services;

/// <summary>
/// The document builder service.
/// </summary>
[Scoped]
public sealed class DocumentBuilderService
{
    readonly BackgroundWorkerWrapper _backgroundWorkerWrapper = new();
    readonly ISignalR _signal;
    readonly DataProviderFactory _dataProviderFactory;
    DocumentBuilderContext? _context;

    public DocumentBuilderService(
        ISignalR signal,
        DataProviderFactory _dataProviderFactory)
    {
        _signal = signal;
        this._dataProviderFactory = _dataProviderFactory;
    }

    /// <summary>
    /// build a document
    /// </summary>
    /// <param name="context">The context.</param>
    public void Build(ActionContext actionContext, DocumentBuilderContext context)
    {
        _context = context;
        _backgroundWorkerWrapper.RunAction(
            (o, e) => BuildInternal(actionContext, context));
    }

    void BuildInternal(ActionContext actionContext, DocumentBuilderContext context)
    {
        try
        {
            //TODO: fix sender should be caller of Build(..)
            // this below to a lib part that doesn't listen to action events, but just produces them

            //_signal.Send(actionContext.Sender,new ActionEndedEvent(actionContext));
            if (actionContext.Sender is IActionFeature feature)
                actionContext.For(feature, new ActionEndedEvent(actionContext));
        }
        catch (Exception)
        {
            // send..
        }
        finally
        {

        }
    }
}
