using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Extensions;
using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Errors;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Logger;
using MovieDbAssistant.Lib.Components.Signal;
using MovieDbAssistant.Lib.Components.Sys;

namespace MovieDbAssistant.Dmn.Components.Scrapper;

/// <summary>
/// The movie db scrapper.
/// </summary>
[Transient]
public sealed class MovieDbScrapper : IIdentifiable
{
    readonly ILogger<MovieDbScrapper> _logger;
    readonly ISignalR _signal;
    readonly IConfiguration _config;
    readonly IOptions<DmnSettings> _dmnSettings;
    readonly ProcessWrapper _processWrapper;
    readonly ScraperOutputModelTransformer _scraperOutputModelTransformer;
    readonly BackgroundWorkerWrapper _worker;
    bool _completed;
    Exception? _exception;

    /// <summary>
    /// Gets the instance id.
    /// </summary>
    /// <value>A <see cref="SharedCounter"/></value>
    public SharedCounter InstanceId { get; }

    public MovieDbScrapper(
        ILogger<MovieDbScrapper> logger,
        ISignalR signal,
        IConfiguration config,
        IOptions<DmnSettings> dmnSettings,
        ProcessWrapper processWrapper,
        ScraperOutputModelTransformer scraperOutputModelTransformer
        )
    {
        InstanceId = new(this);
        _logger = logger;
        _signal = signal;
        _config = config;
        _dmnSettings = dmnSettings;
        _processWrapper = processWrapper;
        _scraperOutputModelTransformer = scraperOutputModelTransformer;
        _worker = new(logger, _signal, this);
        _worker.Setup(
            o => _completed = true);
        _worker.Setup((o, ex) =>
        {
            _exception = ex;
            _completed = true;
        });
    }

    /// <summary>
    /// scrap using the query. block until the end
    /// <para>call MovieDbScrapper</para>
    /// </summary>
    /// <param name="spiderId">spider id</param>
    /// <param name="output">output file path</param>
    /// <param name="filters">eventually filters</param>
    /// <param name="query">The query model.</param>
    /// <returns>A <see cref="MoviesModel"/></returns>
    public MoviesModel? Scrap(
        SpidersIds spiderId,
        string output,
        string? filters,
        QueryModel query)
    {
        MoviesModel? result = null;
        _completed = false;
        _worker.RunAction(
            this
            , new ActionContext(
                _signal,
                new StackErrors())
            , (context, owner, eventArgs) =>
            {
                // call a scrap process

                var toolPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    _dmnSettings.Value.Scrap.ToolPath);

                // args

                /// spiderId outputFile title [filters]
                var args = new List<string>
                {
                    spiderId.ToString(),
                    output,
                    query.Title,                    
                };
                if (filters != null)
                    args.Add("a=b&"+filters);       // TODO: remove the hack, fix the scrapper (remove filters= in from url)

                _logger.LogInformation(this,
                    $"scrap: #{query.InstanceId()} spider={args[0]} title={args[2]} filters={(args.Count > 3 ? args[3] : "")} output={args[1]}");

                _processWrapper.Start(toolPath, args);

                var hasErrors = _processWrapper.HasErrors
                    | !File.Exists(output);
                if (hasErrors)
                {
                    _logger.LogError(this, $"scrap #{query.InstanceId()} has failed");
                    // TODO: throw new ex (task fail)
                    _completed = true;
                    return;
                }

                // get & build the output

                result = GetAndBuildOutput(output);
                _completed = true;

            }
            , _config
            );
        while (!_completed)
        {
            Thread.Yield();
        }
        return result;
    }

    /// <summary>
    /// Get and build output.
    /// </summary>
    /// <param name="output">The output.</param>
    /// <returns>A <see cref="MovieModel? "/></returns>
    public MoviesModel GetAndBuildOutput(string output)
    {
        var src = File.ReadAllText(output);
        return _scraperOutputModelTransformer.Transform(src);
    }
}
