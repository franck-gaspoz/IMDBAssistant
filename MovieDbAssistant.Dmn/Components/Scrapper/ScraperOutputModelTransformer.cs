using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Components.DataProviders;
using MovieDbAssistant.Dmn.Components.DataProviders.Json;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Dmn.Components.Scrapper;

/// <summary>
/// The scraper output model transformer.
/// </summary>
[Transient]
public sealed class ScraperOutputModelTransformer
{
    readonly JsonDataProvider _jsonDataProvider;
    readonly IOptions<DmnSettings> _dmnSettings;

    public ScraperOutputModelTransformer(
        JsonDataProvider jsonDataProvider,
        IOptions<DmnSettings> dmnSettings)
    {
        _jsonDataProvider = jsonDataProvider;
        _dmnSettings = dmnSettings;
    }

    /// <summary>
    /// transforms the json output of a scraper to a movies model
    /// </summary>
    /// <param name="output">scraper output</param>
    /// <returns></returns>
    public MoviesModel Transform(string output)
    {
        output = Wrap(output);

        var models = _jsonDataProvider.Get(output, new DataProviderContext())!;

        return models;
    }

    /// <summary>
    /// fix the json wraping it inside OutputModelPostfix / OutputModelPrefix
    /// </summary>
    /// <param name="output"></param>
    /// <returns>string</returns>
    public string Wrap(string output)
    {
        output =
            _dmnSettings.Value.Scrap.OutputModelPrefix
            + output
            + _dmnSettings.Value.Scrap.OutputModelPostfix;
        return output;
    }
}
