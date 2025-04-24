using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Dmn.Components.Builders.Models.Extensions;

/// <summary>
/// The movies model create extensions.
/// </summary>
public static partial class MovieModelBuilderExtensions
{
    /// <summary>
    /// Creates default movie movie model.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <returns>A <see cref="MovieModel"/></returns>
    public static MovieModel CreateDefaultMovieModel(
        this QueryModel query)
    {
        var data = new MovieModel
        {
            Title = query.Title,
            Year = query.Year,
            Id = query.Title.ToHexLettersAndDigitsString()
        };
        return data;
    }

    /// <summary>
    /// Setup a movie model after a query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="model">The model.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="spiderId">The spider id.</param>
    /// <param name="queryCacheFiles">output result file path (ignored if null)</param>
    /// <returns>A <see cref="MovieModel"/></returns>
    public static MovieModel SetupPostQuery(
        this QueryModel query,
        MovieModel model,
        DmnSettings settings,
        SpidersIds? spiderId,
        List<string>? queryCacheFiles
        )
    {
        model.MetaData.ScraperTool = Path.GetFileName(
            settings.Scrap.ToolPath);
        model.MetaData.ScraperToolVersion = settings
            .App.MovieDbScraperToolVersion;
        model.MetaData.SpiderId = spiderId?.ToString();

        model.Sources.Clear();

        var mediaSourcePlay = new MediaSource(query.Metadata?.Source);
        var mediaSourceDownload = new MediaSource(query.Metadata?.Download);
        if (mediaSourcePlay != null)
            model.Sources.Play.Add(mediaSourcePlay);
        if (mediaSourceDownload != null)
            model.Sources.Download.Add(mediaSourceDownload);

        model.Sources.SetupMediaProviderId(settings);

        model.MetaData.Query = query;
        if (queryCacheFiles != null)
            model.MetaData.Query.Metadata!.QueryCacheFiles = queryCacheFiles;
        return model;
    }

    /// <summary>
    /// Setup movies models after a query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="models">The models.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="spiderId">The spider id.</param>
    /// <param name="queryCacheFiles">output result file path</param>
    /// <returns>A <see cref="MovieModel"/></returns>
    public static MoviesModel SetupPostQuery(
        this QueryModel query,
        MoviesModel models,
        DmnSettings settings,
        SpidersIds spiderId,
        List<string>? queryCacheFiles
        )
    {
        if (queryCacheFiles != null)
            models.QueryCacheFiles =
                models.QueryCacheFiles
                    .AddRangeDistinct(queryCacheFiles)!;

        foreach (var model in models.Movies)
            query.SetupPostQuery(
                model,
                settings,
                spiderId,
                queryCacheFiles);

        return models;
    }
}
