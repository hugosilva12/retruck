using WebApplication1.Context.Global;
using WebApplication1.Global.Enumerations;

namespace WebApplication1.DTOS.Read;

/// <summary>
///  The TruckReadDto class stores information about the truck
/// </summary>
public class TruckReadDto : BasicDto
{
    /// <summary>
    /// contains the driver of the truck
    /// </summary>
    public virtual UserReadDto driver { get; set; }

    /// <summary>
    /// contains the registration of the truck
    /// </summary>
    public string matricula { get; set; }

    /// <summary>
    /// contains the year of the truck
    /// </summary>
    public int year { get; set; }

    /// <summary>
    /// contains the category of the truck
    /// </summary>
    public TruckCategory truckCategory { get; set; }

    /// <summary>
    /// contains the fuel consumption of the truck
    /// </summary>
    public int fuelConsumption { get; set; }

    /// <summary>
    /// contains the kms of the truck
    /// </summary>
    public int kms { get; set; }

    /// <summary>
    /// contains the kms  when the next review should be done
    /// </summary>
    public int nextRevision { get; set; }

    /// <summary>
    /// contains the truck's photo path
    /// </summary>
    public string photoPath { get; set; }

    /// <summary>
    /// contains the organization of the truck
    /// </summary>
    public virtual String organization_id { get; set; }

    /// <summary>
    /// contains the status of the truck
    /// </summary>
    public State status { get; set; }

    /// <summary>
    /// contains the maximum of capacity that the truck transports
    /// </summary>
    public double capacity { get; set; }

    /// <summary>
    /// contains the estimated fuel consumption
    /// </summary>
    public double litresSpend { get; set; }

    /// <summary>
    /// contains the remaining capacity of the truck
    /// </summary>
    public double availableCapacity { get; set; }

    /// <summary>
    /// contains the score
    /// </summary>
    public double score { get; set; }

    /// <summary>
    /// contains the spent value to perform the service
    /// </summary>
    public double valueSpend { get; set; }
}