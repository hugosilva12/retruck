using WebApplication1.Global.Enumerations;

namespace WebApplication1.DTOS.Read;

/// <summary>
///  The TruckReadDto class stores information about the truck which will be used to determine your score
/// </summary>
public class TruckForReviewReadDto : BasicDto
{
    /// <summary>
    /// contains the driver of the truck
    /// </summary>
    public virtual UserReadDto driver { get; set; }

    /// <summary>
    /// contains the id of organization of the truck
    /// </summary>
    public string organizationId { get; set; }

    /// <summary>
    /// contains the decision whether or not the truck is available for service
    /// </summary>
    public bool isAvailable { get; set; }

    /// <summary>
    /// contains the spent fuel to perform the service
    /// </summary>
    public double litresSpend { get; set; }

    /// <summary>
    /// contains the spent value to perform the service
    /// </summary>
    public double valueSpend { get; set; }

    /// <summary>
    /// contains the maximum of liters that the truck transports (if it is a tanker truck)
    /// </summary>
    public double maxLitres { get; set; }

    /// <summary>
    /// contains the maximum of volume that the truck transports (if it is a Refrigerator or Container truck)
    /// </summary>
    public double maxVolum { get; set; }

    /// <summary>
    /// contains the maximum of weigth that the truck transports (if it is a Refrigerator or Container truck)
    /// </summary>
    public double maxWeight { get; set; }

    /// <summary>
    /// contains the occupancy percentage
    /// </summary>
    public double occupiedVolumePercentage { get; set; }

    /// <summary>
    /// contains the score
    /// </summary>
    public double score { get; set; }

    /// <summary>
    /// contains the description of review
    /// </summary>
    public string summaryReview { get; set; }

    /// <summary>
    /// contains the decision if the truck is not available for lack of space
    /// </summary>
    public bool noSpace { get; set; }

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
}