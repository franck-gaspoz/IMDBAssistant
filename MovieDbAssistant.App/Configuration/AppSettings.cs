using MovieDbAssistant.Dmn.Configuration;

namespace MovieDbAssistant.App.Configuration;

/// <summary>
/// The app settings.
/// </summary>
public sealed class AppSettings
{
    /// <summary>
    /// texts settings
    /// </summary>
    public required TextsSettings Texts { get; set; }

    /// <summary>
    /// balloon tips settings
    /// </summary>
    public required BalloonTipsSettings BalloonTips { get; set; }

    /// <summary>
    /// assets
    /// </summary>
    public required AssetsSettings Assets { get; set; }

    /// <summary>
    /// tools
    /// </summary>
    public required ToolsSettings Tools { get; set; }

    /// <summary>
    /// anims
    /// </summary>
    public required AnimsSettings Anims { get; set; }

    /// <summary>
    /// options
    /// </summary>
    public required OptionsSettings Options { get; set; }

    /// <summary>
    /// urls
    /// </summary>
    public required UrlsSettings Urls { get; set; }
}
