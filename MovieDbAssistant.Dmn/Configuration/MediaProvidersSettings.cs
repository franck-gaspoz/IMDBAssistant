namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// The media providers settings.
/// </summary>
public sealed class MediaProvidersSettings
{
    /// <summary>
    /// fallback provider
    /// </summary>
    public required MediaProviderSettings FallBackProvider { get; set; }

    /// <summary>
    /// urls.
    /// </summary>
    /// <value>A list of media provider settings.</value>
    public required List<MediaProviderSettings> Urls { get; set; }

    /// <summary>
    /// physicial media providers types
    /// </summary>
    /// <value>A list of media provider settings.</value>
    public required List<MediaProviderSettings> PhysicalTypes { get; set; }

}