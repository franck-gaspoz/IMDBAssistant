using System.IO.Enumeration;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Scrap.Json;

namespace MovieDbAssistant.Dmn.Components.Builders.Models.Extensions;

/// <summary>
/// model media sources extensions
/// </summary>
public static class MediaSourcesExtensions
{
    /// <summary>
    /// setup media providers ids
    /// </summary>
    /// <param name="mediaSources">media sources</param>
    /// <param name="dmnSettings">domain settings</param>
    public static void SetupMediaProviderId(
        this MediaSources mediaSources,
        DmnSettings dmnSettings)
    {
        foreach (var mediaSource in mediaSources.Download)
            if (mediaSource.MediaProviderId==null)
                Setup(mediaSource,dmnSettings.MediaProviders);
        foreach (var mediaSource in mediaSources.Play)
            if (mediaSource.MediaProviderId == null)
                Setup(mediaSource, dmnSettings.MediaProviders);
    }

    static void Setup(
        MediaSource mediaSource,
        MediaProvidersSettings providersSettings)
    {
        Setup(mediaSource, providersSettings.Urls, providersSettings);
        //Setup(mediaSource, providersSettings.PhysicalTypes, providersSettings);
    }

    static void Setup(
        MediaSource mediaSource,
        List<MediaProviderSettings> providers,
        MediaProvidersSettings mediaProvidersSettings)
    {
        if (mediaSource.Path == null) return;
        foreach (var provider in providers)
            foreach (var path in provider.Paths)
                if (Match(mediaSource.Path, path))
                {
                    mediaSource.MediaProviderId = provider.Id;
                    return;
                }
        mediaSource.MediaProviderId = mediaProvidersSettings.FallBackProvider.Id;
    }

    static bool Match(string? text,string pattern)
        => text != null && FileSystemName.MatchesSimpleExpression(pattern, text );
}
