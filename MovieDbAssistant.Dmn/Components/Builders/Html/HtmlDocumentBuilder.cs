using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Components.Builders.Document;
using MovieDbAssistant.Dmn.Components.Builders.Templates;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Logger;

using static MovieDbAssistant.Dmn.Components.Builders.Html.HtmDocumentBuilderSettings;

namespace MovieDbAssistant.Dmn.Components.Builders.Html;

/// <summary>
/// html document builder.
/// </summary>
[Transient]
public sealed class HtmlDocumentBuilder : IDocumentBuilder
{
    const string Folder_Back = "../";
    readonly ILogger<HtmlDocumentBuilder> _logger;
    readonly HtmlMovieDocumentBuilder _htmlMovieDocumentBuilder;
    readonly TemplateBuilder _templateBuilder;
    readonly IOptions<DmnSettings> _dmnSettings;

    public HtmlDocumentBuilder(
        ILogger<HtmlDocumentBuilder> logger,
        HtmlMovieDocumentBuilder htmlMovieDocumentBuilder,
        TemplateBuilder templateBuilder,
        IOptions<DmnSettings> dmnSettings)
    {
        _logger = logger;
        _htmlMovieDocumentBuilder = htmlMovieDocumentBuilder;
        _templateBuilder = templateBuilder;
        _dmnSettings = dmnSettings;
    }

    /// <summary>
    /// build the movies documents
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="data">The data.</param>
    public void Build(
        DocumentBuilderContext context,
        MoviesModel data)
    {
        _logger.LogInformation(this, $"process json: {data.Movies.Count} movies");

        data
            .Filter()
            .Distinct()
            .Sort()
            .Index();

        var folderName = Path.GetFileNameWithoutExtension(context.Source);
        var folder = Path.Combine(context.OutputPath, folderName);
        context.Target = folder;

        _logger.LogInformation(this,
            _dmnSettings.Value.Texts.ProcMovieList
            + Path.GetFileName(context.Source));

        // get template & prepare output

        context.MakeOutputDirs();

        var htmlContext = new HtmlDocumentBuilderContext(
            0, data.Movies.Count, string.Empty, null, null)
        {
            Folder = folderName
        };

        _templateBuilder.LoadTemplate(
            context,
            context
                .BuilderOptions[Template_Id]
                .ToString()!,
            context
                .BuilderOptions[Template_Version]
                .ToString()!
            )

        // build list pages

            .BuildPageList(htmlContext, data);

        // build detail pages

        var index = 0;
        var builders = new List<HtmlDocumentBuilderContext?>();
        foreach (var movie in data.Movies)
            _htmlMovieDocumentBuilder.SetupModel(movie);

        foreach (var movie in data.Movies)
        {
            var queryCacheFile = movie.MetaData.Query?.Metadata?.QueryCacheFiles?.FirstOrDefault();
            var newData = queryCacheFile != null
                && File.GetLastWriteTime(queryCacheFile) >= context.CreationDate;
            var skipBuild = _dmnSettings.Value
                .Build.LimitToNewData && !newData;

            if (!skipBuild)
            {
                var builder = new HtmlDocumentBuilderContext(
                    index,
                    data.Movies.Count,
                    Folder_Back + _templateBuilder.TemplateModel
                        .PageIndexPath(
                            context,
                            _dmnSettings.Value.Build.Html.Extension),
                    index > 0 ?
                        Folder_Back + context.PageFilePath(
                            data.Movies[index - 1].Key!,
                            _dmnSettings.Value.Build.Html.Extension)
                        : null,
                    index < data.Movies.Count - 1 ?
                        Folder_Back + context.PageFilePath(
                            data.Movies[index + 1].Key!,
                            _dmnSettings.Value.Build.Html.Extension)
                        : null
                    )
                {
                    Folder = folderName
                };
                builders.Add(builder);
            }
            else
            {
                builders.Add(null);
                _logger.LogInformation(this, "skip build for item: " + movie.Title);
            }

            index++;
        }

        index = 0;
        foreach (var movie in data.Movies)
        {
            if (builders[index]!=null)
                _htmlMovieDocumentBuilder.BuildMovie(
                    context,
                    builders[index]!,
                    movie);
            index++;
        }

        // copy resources

        _templateBuilder.CopyTplRsc();  // max. priority files
        _templateBuilder.CopyRsc();     // don't replace same files
    }
}
