using MovieDbAssistant.Dmn.Models.Build;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Dmn.Components.Builders.Html;

/// <summary>
/// The html document builder context.
/// </summary>
/// <param name="Index">The index.</param>
/// <param name="Total">The total.</param>
/// <param name="HomeLink">The home link.</param>
/// <param name="PreviousLink">The previous link.</param>
/// <param name="NextLink">The next link.</param>
public sealed class HtmlDocumentBuilderContext
{
    /// <summary>
    /// Gets the index.
    /// </summary>
    /// <value>An <see cref="int"/></value>
    public int Index { get; set; }

    /// <summary>
    /// Gets the total.
    /// </summary>
    /// <value>An <see cref="int"/></value>
    public int Total { get; set; }

    /// <summary>
    /// Gets the home link.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string HomeLink { get; set; }

    /// <summary>
    /// Gets the previous link.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? PreviousLink { get; set; }

    /// <summary>
    /// Gets the next link.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? NextLink { get; set; }

    /// <summary>
    /// source folder
    /// </summary>
    public string? Folder { get; set; }

    /// <summary>
    /// Gets the sub title.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? SubTitle => Folder?.ToLower().ToFirstUpper();

    /// <summary>
    /// context creation date
    /// </summary>
    public DateTime CreationDate { get; }

    public HtmlDocumentBuilderContext(MoviesModel data)
        : this(0, data.Movies.Count, string.Empty, null, null) { }

    public HtmlDocumentBuilderContext(
        int index,
        int total,
        string homeLink,
        string? previousLink,
        string? nextLink)
    {
        Index = index;
        Total = total;
        HomeLink = homeLink;
        PreviousLink = previousLink;
        NextLink = nextLink;
        CreationDate = DateTime.Now;
    }
}
