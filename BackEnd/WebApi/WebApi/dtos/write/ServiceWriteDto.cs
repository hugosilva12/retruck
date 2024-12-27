namespace WebApplication1.DTOS;

/// <summary>
///  The ServiceWriteDto class stores information to save a service
/// </summary>
public class ServiceWriteDto
{
    /// <summary>
    /// contains the id of truck 
    /// </summary>
    public Guid truckId { get; set; }

    /// <summary>
    /// contains the id of transport
    /// </summary>
    public Guid transportId { get; set; }

    /// <summary>
    /// contains the kms of service
    /// </summary>
    public double kms { get; set; }

    /// <summary>
    /// contains the service profit
    /// </summary>
    public double profit { get; set; }

    /// <summary>
    /// contains the capacity still available on the truck
    /// </summary>
    public double capacityAvailable { get; set; }
}