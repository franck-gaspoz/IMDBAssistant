namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// media sources
/// </summary>
public sealed partial class MediaSources
{
    /// <summary>
    /// gets a clone
    /// </summary>
    /// <returns>A <see cref="MediaSource"/></returns>
    public MediaSources Clone()
        => new()
        {
            Download = Download.Select(x => x.Clone()).ToList(),
            Play = Play.Select(x => x.Clone()).ToList()
        };
}
