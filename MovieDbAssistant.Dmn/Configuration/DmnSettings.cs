namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// domain settings
/// </summary>
public sealed class DmnSettings
{
    /// <summary>
    /// scrap settings
    /// </summary>
    public required ScrapSettings Scrap { get; set; }

    /// <summary>
    /// texts settings
    /// </summary>
    public required TextsSettings Texts { get; set; }

    /// <summary>
    /// paths settings
    /// </summary>
    public required PathsSettings Paths { get; set; }

    /// <summary>
    /// build settings
    /// </summary>
    public required BuildSettings Build { get; set; }

    /// <summary>
    /// app settings
    /// </summary>
    public required AppMetadataSettings App { get; set; }

    /// <summary>
    /// media providers
    /// </summary>
    public required MediaProvidersSettings MediaProviders { get; set; }
}
