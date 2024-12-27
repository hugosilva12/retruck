namespace WebApplication1.Global.Utils;

/// <summary>
/// This interface contains the methods that allows get the information  between two locations
/// </summary>
public interface IDistanceService
{
    /// <summary>
    /// This method gets the information between two locations
    /// </summary>
    Task<ModelResponseDistanceMatrix> getDistanceAndEstimatedTimeWhitParams(string origin, string destiny);
}