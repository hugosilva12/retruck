using WebApplication1.Global.Enumerations;

namespace WebApplication1.Models;

/// <summary>
///  The TransportReviewParameters class stores information about parameters that are used in transport analysis
/// </summary>
/// 
public class TransportReviewParameters
{
    /// <summary>
    /// identifier of the transport review parameters
    /// </summary>
    public Guid id { get; set; }

    /// <summary>
    /// contains the value that the service costs more if it is on a Saturday
    /// </summary>
    public double valueSaturday { get; set; }

    /// <summary>
    /// contains the value that the service costs more if it is on a Sunday
    /// </summary>
    public double valueSunday { get; set; }

    /// <summary>
    /// contains the value that the service costs more if it is on a Holiday
    /// </summary>
    public double valueHoliday { get; set; }

    /// <summary>
    /// contains the value of fuel
    /// </summary>
    public double valueFuel { get; set; }

    /// <summary>
    /// contains the value of toll per km
    /// </summary>
    public double valueToll { get; set; }

    /// <summary>
    /// contains the type of analysis to be performed
    /// </summary>
    public TypeAnalysis typeAnalysis { get; set; }

    /// <summary>
    /// contains the decision if the history of truck break downs influences the choice
    /// </summary>
    public ConsiderTruckBreakDowns considerTruckBreakDowns { get; set; }
}