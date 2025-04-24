namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// a media source
/// </summary>
public sealed partial class MediaSource
{
    /// <summary>
    /// gets a clone
    /// </summary>
    /// <returns>A <see cref="MediaSource"/></returns>
    public MediaSource Clone()
        => new(Path)
        {
            MediaPhysicalTypeId = MediaPhysicalTypeId,
            MediaProviderId = MediaProviderId
        };
}
