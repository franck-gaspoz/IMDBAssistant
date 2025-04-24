namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// a set of sources of a media
/// </summary>
public sealed partial class MediaSources
{
    /// <summary>
    /// media providers urls (download)
    /// </summary>
    public List<MediaSource> Download { get; set; } = [];

    /// <summary>
    /// media providers urls (play)
    /// </summary>
    public List<MediaSource> Play { get; set; } = [];

    static readonly MediaSource _emptySource = new();

    /// <summary>
    /// preferred movie source play (choice from Play)
    /// </summary>
    public MediaSource PreferredSourcePlay
        => Play.Count > 0
            ? Play[0] : _emptySource;

    /// <summary>
    /// preferred movie source play (choice from Download)
    /// </summary>
    public MediaSource PreferredSourceDownload
        => Play.Count > 0
            ? Download[0] : _emptySource;

    /// <summary>
    /// clear the media sources
    /// </summary>
    public MediaSources Clear()
    {
        Download.Clear();
        Play.Clear();
        return this;
    }
}
