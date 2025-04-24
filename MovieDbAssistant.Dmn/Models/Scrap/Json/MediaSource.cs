namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// a source of a media
/// </summary>
public sealed partial class MediaSource
{
    /// <summary>
    /// source path
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// media provider identifier (from MediaProviders.Urls)
    /// </summary>
    public string? MediaProviderId { get; set; }

    /// <summary>
    /// media physical type id (hdd,ssd,...) if any
    /// </summary>
    public string? MediaPhysicalTypeId { get; set; }

    public MediaSource(string? path = null) => Path = path;
}
