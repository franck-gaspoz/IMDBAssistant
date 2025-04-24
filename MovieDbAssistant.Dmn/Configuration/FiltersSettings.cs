using MovieDbAssistant.Dmn.Models.Queries;

namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// filters settings
/// </summary>
public sealed class FiltersSettings
{
    /// <summary>
    /// Gets or sets the countries.
    /// </summary>
    /// <value>An array of strings</value>
    public required string[] Countries { get; set; }

    /// <summary>
    /// Gets or sets the languages.
    /// </summary>
    /// <value>An array of strings</value>
    public required string[]? Languages { get; set; }

    /// <summary>
    /// Gets or sets the count.
    /// </summary>
    /// <value>An <see cref="int"/></value>
    public int Count { get; set; }

    /// <summary>
    /// type of title (movie,game,..)
    /// </summary>
    public TitleTypes[]? Types { get; set; }

    /// <summary>
    /// min rating
    /// </summary>
    public string? RatingMin { get; set; }

    /// <summary>
    /// max rating
    /// </summary>
    public string? RatingMax { get; set; }
}
