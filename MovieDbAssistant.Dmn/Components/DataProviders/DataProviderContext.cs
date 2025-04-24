namespace MovieDbAssistant.Dmn.Components.DataProviders;

/// <summary>
/// data provider context
/// </summary>
public sealed class DataProviderContext
{
    /// <summary>
    /// creation date
    /// </summary>
    public DateTime CreationDate { get; }

    public DataProviderContext()
    {
        CreationDate = DateTime.Now;
    }
}
