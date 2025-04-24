using System.Text.RegularExpressions;

using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Dmn.Models.Queries;

/// <summary>
/// query model (merge all specific models)
/// </summary>
public sealed record class QueryModel
{
    /// <summary>
    /// spiders. default: SpidersIds.imdb
    /// </summary>
    public SpidersIds[] Spiders { get; set; }

    /// <summary>
    /// Gets or sets the metadata.
    /// </summary>
    /// <value>A <see cref="QueryMetadata? "/></value>
    public QueryMetadata? Metadata { get; set; }

    /// <summary>
    /// quasi/pseudo unique key
    /// </summary>
    public string HashKey =>
        _title.ToHexLettersAndDigitsString()
        + Count?.ToString()
        + Year?.ToString()
        + Languages.IfNullElse(() => "", x => string.Join("",
            x.Select(x => x.ToHexLettersAndDigitsString())))
        + Countries.IfNullElse(() => "", x => string.Join("",
            x.Select(x => x.ToHexLettersAndDigitsString())))
        + RatingMin?.ToHexLettersAndDigitsString()
        + RatingMax?.ToHexLettersAndDigitsString()
        + TitleTypes.IfNullElse(() => "", x => string.Join("",
            x.Select(x => x.ToString().ToHexLettersAndDigitsString())))
        + Genres.IfNullElse(() => "", x => string.Join("",
            x.Select(x => x.ToString().ToHexLettersAndDigitsString())))
        ;

    string? _title;
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Title
    {
        get => _title!;
        set
        {
            _title = value;
            var pattern = @"\b\d{4}\b";
            var matches = Regex.Matches(value, pattern);
            if (matches.Count != 0)
            {
                _year = matches.Last().Value;
                if (_title.EndsWith(Year!))
                    _title = _title[..^Year!.Length].Trim();
            }
        }
    }

    /// <summary>
    /// count
    /// </summary>
    public int? Count { get; set; }

    string? _year;
    /// <summary>
    /// Gets or sets the year.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? Year
    {
        get => _year;
        set
        {
            if (value != null)
                _year = value;
        }
    }

    /// <summary>
    /// Gets or sets the languages
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string[]? Languages { get; set; }

    /// <summary>
    /// movie countries
    /// </summary>
    public string[]? Countries { get; set; }

    /// <summary>
    /// titles types
    /// </summary>
    public TitleTypes[]? TitleTypes { get; set; }

    /// <summary>
    /// genres
    /// </summary>
    public Genres[]? Genres { get; set; }

    /// <summary>
    /// min rating
    /// </summary>
    public string? RatingMin { get; set; }

    /// <summary>
    /// max rating
    /// </summary>
    public string? RatingMax { get; set; }

    public QueryModel(
        string title,
        string[]? languages = null,
        string? year = null,
        string[]? countries = null,
        string? ratingMin = null,
        string? ratingMax = null
        )
    {
        Title = title;
        Languages = languages;
        Year = year;
        Countries = countries;
        Metadata = new();
        RatingMin = ratingMin;
        RatingMax = ratingMax;
        Spiders = [SpidersIds.imdb];
    }
}
