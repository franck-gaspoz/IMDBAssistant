using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using MovieDbAssistant.Dmn.Components.Builders.Document;
using MovieDbAssistant.Dmn.Models.Extensions;
using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// template properties model
/// </summary>
public sealed class TemplateModel
{
    public TemplateModel(
        string name,
        string version,
        string versionDate,
        string description,
        string id,
        List<string> authors,
        ThemeModel theme,
        List<PageModel> pages,
        JsonElement? props,
        List<string> files,
        List<string> resources,
        string? path,
        PathsModel paths,
        List<TransformModel> transforms)
    {
        Name = name;
        Authors = authors;
        Version = version;
        VersionDate = versionDate;
        Description = description;
        Id = id;
        Theme = theme;
        Pages = pages;
        Props = props;
        Files = files;
        Resources = resources;
        Path = path;
        Paths = paths;
        Transforms = transforms;
    }

    /// <summary>
    /// name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// author
    /// </summary>
    public List<string> Authors { get; set; }

    /// <summary>
    /// version number
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// version date
    /// </summary>
    public string VersionDate { get; set; }

    /// <summary>
    /// description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the theme.
    /// </summary>
    /// <value>A <see cref="ThemeModel"/></value>
    public ThemeModel Theme { get; set; }

    /// <summary>
    /// pages
    /// </summary>
    public List<PageModel> Pages { get; set; }

    /// <summary>
    /// Gets or sets the template dynamic json properties. (fits js needs)
    /// </summary>
    /// <value>A <see cref="JsonElement? "/></value>
    public JsonElement? Props { get; set; }

    /// <summary>
    /// dynamic props
    /// </summary>
    [JsonIgnore]
    [IgnoreDataMember]
    public dynamic DProps
        => Props.ToDynamic();

    /// <summary>
    /// Gets or sets the files.
    /// </summary>
    /// <value>A list of strings.</value>
    public List<string> Files { get; set; } = [];

    /// <summary>
    /// gets or sets the resources
    /// </summary>
    public List<string> Resources { get; set; } = [];

    #region working properties

    /// <summary>
    /// path on disk of the template folder
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// paths
    /// </summary>
    public PathsModel Paths { get; set; }

    /// <summary>
    /// transforms
    /// </summary>
    public List<TransformModel> Transforms { get; set; } = [];

    #endregion

    /// <summary>
    /// Pages index path.
    /// </summary>
    /// <param name="docBuilderContext">doc builder context</param>
    /// <param name="extension">extension</param>
    /// <returns>A <see cref="string"/></returns>
    public string PageIndexPath(DocumentBuilderContext docBuilderContext,
        string extension)
            => docBuilderContext.TplFilePath(
                this.PageList()!.Filename!, extension);
}
