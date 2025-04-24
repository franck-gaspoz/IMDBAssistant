namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// build settings
/// </summary>
public sealed class BuildSettings
{
    /// <summary>
    /// Gets or sets the search pattern json.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string SearchPatternJson { get; set; }

    /// <summary>
    /// Gets or sets the search pattern txt.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string SearchPatternTxt { get; set; }

    /// <summary>
    /// Gets or sets the prefix file disabled.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string PrefixFileDisabled { get; set; }

    /// <summary>
    /// Gets or sets the html settings.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required HtmlSettings Html { get; set; }

    /// <summary>
    /// limit to new data
    /// </summary>
    /// <value>A <see cref="bool"/></value>
    public bool LimitToNewData {  get; set; }
}