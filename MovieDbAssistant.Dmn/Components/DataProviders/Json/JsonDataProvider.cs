using System.Text.Json;

using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.InstanceCounter;

using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json;

/// <summary>
/// json data provider == MovieDbScraper output
/// </summary>
[Transient]
public class JsonDataProvider : IDataProvider, IIdentifiable
{
    protected ILogger<JsonDataProvider> Logger { get; set; }

    /// <summary>
    /// Gets the instance id.
    /// </summary>
    /// <value>A <see cref="SharedCounter"/></value>
    public SharedCounter InstanceId { get; }

    public JsonDataProvider(ILogger<JsonDataProvider> logger)
        => (InstanceId, Logger) = (new(this), logger);

    /// <summary>
    /// get from text
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="context">context</param>
    /// <returns>A <see cref="MoviesModel"/></returns>
    public virtual MoviesModel? Get(object? source, DataProviderContext context)
    {
        if (source == null) return null;
        var r = JsonSerializer.Deserialize<MoviesModel>(
            (string)source,
            JsonDeserializerProperties.Value);

        return r!;
    }
}
