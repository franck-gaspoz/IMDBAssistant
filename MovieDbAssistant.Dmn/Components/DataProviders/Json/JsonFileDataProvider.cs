using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json;

/// <summary>
/// json from file data provider
/// <para>handle MovieDbScraper output</para>
/// </summary>
[Transient]
public class JsonFileDataProvider : JsonDataProvider
{
    public JsonFileDataProvider(ILogger<JsonDataProvider> logger)
        : base(logger) { }

    /// <summary>
    /// get from path
    /// </summary>
    /// <param name="source">The path.</param>
    /// <param name="context">context</param>
    /// <returns>A <see cref="MoviesModel"/></returns>
    public override MoviesModel? Get(object? source, DataProviderContext context)
    {
        if (source == null) return null;
        var src = (string)source;

        Logger.LogInformation(
            this,
            "parse json: "
            + Path.GetFileName(src));

        return base.Get(File.ReadAllText(src), new DataProviderContext());
    }
}
