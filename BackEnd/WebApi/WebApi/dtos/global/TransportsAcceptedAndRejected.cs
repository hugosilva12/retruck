namespace WebApplication1.DTOS.Global;

/// <summary>
/// The TransportsAcceptedAndRejected stores information about the number of denied/accepted transports
/// </summary>
public class TransportsAcceptedAndRejected
{
    /// <summary>
    /// contains the number of accepted transports
    /// </summary>
    public int accepted { get; set; }


    /// <summary>
    /// contains the number of denied transports
    /// </summary>
    public int denied { get; set; }
}