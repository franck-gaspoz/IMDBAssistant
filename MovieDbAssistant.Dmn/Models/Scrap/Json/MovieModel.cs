using System.Diagnostics;

namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

#pragma warning disable CD1606 // The property must have a documentation header.

/// <summary>
/// The movie model
/// </summary>
[DebuggerDisplay("{Title} | {_score}")]
public sealed partial class MovieModel
{
    /// <summary>
    /// dynamic after query score
    /// </summary>
    string _score => $"aff={MetaData?.SearchScore?.Affinity} dco={MetaData?.SearchScore?.DataCompletion} v={MetaData?.SearchScore?.Value}";

    /// <summary>
    /// url of scraped page: details
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// model meta data
    /// </summary>
    public MovieModelMetaData MetaData { get; set; } = new();

    /// <summary>
    /// movie sources
    /// </summary>
    public MediaSources Sources { get; set; } = new();

    /// <summary>
    /// provider movie id
    /// </summary>
    public string? Id { get; set; }

    public string? Title { get; set; }

    /// <summary>
    /// scraped title by opposite to queried title
    /// </summary>
    public string? OriginalTitle { get; set; }

    public string? Summary { get; set; }

    public List<string> Interests { get; set; } = [];

    public string? Rating { get; set; }

    public string? RatingCount { get; set; }

    public string? Duration { get; set; }

    public string? ReleaseDate { get; set; }

    /// <summary>
    /// scraped year
    /// </summary>
    public string? Year { get; set; }

    /// <summary>
    /// scraped year
    /// </summary>
    public string? OriginalYear { get; set; }

    public string? Vote { get; set; }

    public string? Director { get; set; }

    public List<string> Writers { get; set; } = [];

    public List<string> Stars { get; set; } = [];

    public List<ActorModel> Actors { get; set; } = [];

    public string? Anecdotes { get; set; }

    public string? MinPicUrl { get; set; }

    public string? MinPicWidth { get; set; }

    public string? MinPicAlt { get; set; }

    public List<string>? PicsUrls { get; set; } = [];

    public string? MedPicUrl => PicsUrls != null ? PicsUrls.Count > 2
        ? PicsUrls[2] : MinPicUrl
        : null;

    public string? PicFullUrl { get; set; }

    public List<string> PicsSizes { get; set; } = [];
}
