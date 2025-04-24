using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Components.Builders.Models;
using MovieDbAssistant.Dmn.Components.Builders.Models.Extensions;
using MovieDbAssistant.Dmn.Components.Query;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Dmn.Models.Scrap.Json.Extensions;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json;

/// <summary>
/// json from query file data provider
/// <para>call query data provider</para>
/// </summary>
public sealed class JsonQueryFileDataProvider : JsonFileDataProvider
{
    readonly QueryBuilder _queryBuilder;
    readonly MoviesModelMergeBuilder _moviesModelMergeBuilder;
    readonly IServiceProvider _serviceProvider;
    readonly IOptions<DmnSettings> _settings;

    public JsonQueryFileDataProvider(
        ILogger<JsonQueryFileDataProvider> logger,
        QueryBuilder queryBuilder,
        MoviesModelMergeBuilder moviesModelMergeBuilder,
        IServiceProvider serviceProvider,
        IOptions<DmnSettings> settings)
        : base(logger)
    {
        _queryBuilder = queryBuilder;
        _moviesModelMergeBuilder = moviesModelMergeBuilder;
        _serviceProvider = serviceProvider;
        _settings = settings;
    }

    /// <inheritdoc/>
    public override MoviesModel? Get(object? source, DataProviderContext context)
    {
        if (source == null) return null;
        var src = (string)source;

        Logger.LogInformation(
            this,
            "parse query file: "
            + Path.GetFileName(src));

        var file = File.ReadAllText(src);

        var queries = _queryBuilder.Build(src, file);

        if (queries == null) return null;

        var movies = new List<MovieModel>();

        var queryCacheFiles = new List<string>();

        queries.ForEach(query =>
        {
            var provider = _serviceProvider
                .GetRequiredService<JsonQueryDataProvider>();
            var moviesModel = provider.Get(query,new DataProviderContext());

            var createDefault = moviesModel == null
                || moviesModel.Movies.Count == 0;

            if (!createDefault)
            {
                // TODO
                //var t = _moviesModelMergeBuilder.Collapse(moviesModel);
                //movies.Add(t);

                // compute search score

                moviesModel!.BuildSearchScore(
                    _serviceProvider,
                    query);

                var best = moviesModel!.HavingBestSearchScore();
                if (best != null)
                {
                    var builder = _serviceProvider
                        .GetRequiredService<MovieModelFromQueryBuilder>()
                            .Setup(query, best);
                    var movie = builder.Build();
                    movies.Add(movie);
                }
                else
                    createDefault = true;

                //all results
                //movies.AddRange(moviesModel!.Movies);
            }

            if (createDefault)
            {
                var defaultModel = query.CreateDefaultMovieModel();
                query.SetupPostQuery(
                        defaultModel,
                        _settings.Value,
                        null,
                        moviesModel?.QueryCacheFiles);
                movies.Add(defaultModel);
            }
        });

        foreach (var movie in movies)
            queryCacheFiles.AddRange(movie
                .MetaData!
                .Query!
                .Metadata!
                .QueryCacheFiles!);

        queryCacheFiles = queryCacheFiles.Distinct().ToList();

        // encapsulate type and add meta data (query,..)
        var data = new MoviesModel()
        {
            Movies = movies,
            QueryCacheFiles = queryCacheFiles
        };

        return data;
    }
}
