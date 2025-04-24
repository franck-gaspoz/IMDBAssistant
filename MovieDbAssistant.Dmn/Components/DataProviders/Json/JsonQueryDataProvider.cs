using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Components.Builders.Models.Extensions;
using MovieDbAssistant.Dmn.Components.DataProviders.Json.SourceModelAdapters;
using MovieDbAssistant.Dmn.Components.Scrapper;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Extensions;
using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json;

/// <summary>
/// json from query file data provider
/// <para>call query data provider</para>
/// </summary>
public sealed class JsonQueryDataProvider : JsonDataProvider
{
    const string SEPARATOR_TEMP_FILENAME_ID = "-";
    const string File_Extension_Json = ".json";

    readonly IConfiguration _config;
    readonly SourceModelAdapterFactory _sourceModelAdapterFactory;
    readonly IServiceProvider _serviceProvider;
    readonly IOptions<DmnSettings> _settings;

    public JsonQueryDataProvider(
        ILogger<JsonQueryDataProvider> logger,
        IConfiguration config,
        IOptions<DmnSettings> settings,
        SourceModelAdapterFactory sourceModelAdapterFactory,
        IServiceProvider serviceProvider)
        : base(logger)
    {
        _config = config;
        _settings = settings;
        _sourceModelAdapterFactory = sourceModelAdapterFactory;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// get from query model
    /// </summary>
    /// <param name="source">query model</param>
    /// <param name="context">context</param>
    /// <returns>movies model or null</returns>
    public override MoviesModel? Get(object? source, DataProviderContext context)
    {
        if (source == null) return null;
        if (source is not QueryModel query) return null;

        var qid = query.Metadata!.InstanceId.Value + "";
        var outputFile = query.HashKey + File_Extension_Json;

        Logger.LogInformation(
            this,
            $"handle search query #{qid}: {query}");

        var aggregateModel = new MoviesModel();

        query.Spiders
            .ToList()
            .ForEach(spiderId =>
            {
                var output = Path.Combine(
                     Directory.GetCurrentDirectory(),
                     _settings.Value.Paths.Temp,
                     spiderId + SEPARATOR_TEMP_FILENAME_ID + outputFile
                );

                MoviesModel? models = null;

                var scraper = _serviceProvider
                    .GetRequiredService<MovieDbScrapper>();

                if (_settings.Value.Scrap.SkipIfTempOutputFileAlreadyExists
                    && File.Exists(output))
                {
                    var outputFile = Path.GetFileName(output);

                    Logger.LogInformation(
                        this,
                        $"skip search query #{qid}: file '{outputFile}' already exists");

                    models = scraper.GetAndBuildOutput(output);
                }
                else
                {
                    var filters = _sourceModelAdapterFactory
                        .Create(spiderId)
                        .CreateFilter(query);

                    models = scraper.Scrap(
                        spiderId,
                        output,
                        filters,
                        query);

                    if (models != null)
                        Logger.LogInformation(this, $"scrap #{query.InstanceId()} completed");
                }

                if (models != null)
                    query.SetupPostQuery(
                        models,
                        _settings.Value,
                        spiderId,
                        [output]);

                // merge spider models in catalog
                aggregateModel.Merge(models);

            });

        return aggregateModel;
    }
}
