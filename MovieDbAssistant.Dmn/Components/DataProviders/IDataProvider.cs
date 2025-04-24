using MovieDbAssistant.Dmn.Models.Scrap.Json;

namespace MovieDbAssistant.Dmn.Components.DataProviders;

public interface IDataProvider
{
    /// <summary>
    /// return a model from source properties
    /// </summary>
    /// <param name="source">source properties</param>
    /// <param name="context">operation context</param>
    /// <returns>movies model or null</returns>
    MoviesModel? Get(object? source, DataProviderContext context);
}
