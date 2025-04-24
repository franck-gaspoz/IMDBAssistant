namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// The media provider settings.
/// </summary>
public sealed class MediaProviderSettings
{
    /// <summary>
    /// media provider id
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// paths masks
    /// </summary>
    public required string[] Paths { get; set; }

    /// <summary>
    /// require VPN
    /// </summary>
    public bool RequireVPN { get; set; }

    /// <summary>
    /// indicates if the provider insert adds in media (free with adds)s
    /// </summary>
    public bool WithAds { get; set; }

    /// <summary>
    /// geo restricted (2 digits country codes list)
    /// </summary>
    public List<string> GeoRestricted { get; set; } = [];

    /// <summary>
    /// ui embeded player name/ref if any
    /// </summary>
    public string? Embed { get; set; }
}
