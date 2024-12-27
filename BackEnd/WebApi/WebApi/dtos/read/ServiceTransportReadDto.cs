using WebApplication1.Global.Enumerations;

namespace WebApplication1.DTOS.Read;

/// <summary>
///  The ServiceTransportReadDto class stores information about a service
/// </summary>
public class ServiceTransportReadDto : BasicDto
{
    /// <summary>
    /// contains the transport associated with the service
    /// </summary>
    public TransportReadDto transportReadDto { get; set; }

    /// <summary>
    /// contains the truck associated with the service
    /// </summary>
    public TruckReadDto truckReadDto { get; set; }


    /// <summary>
    /// contains the state of service
    /// </summary>
    public ServiceStatus status { get; set; }

    /// <summary>
    /// contains the organization's address
    /// </summary>
    public string organizationAddress { get; set; }

    /// <summary>
    /// contains all registered coordinates
    /// </summary>
    public List<CoordReadDto> listCoord { get; set; }

    /// <summary>
    /// contains the organization coordinates
    /// </summary>
    public CoordReadDto organizationAddressCoord { get; set; }

    /// <summary>
    /// contains the coordinates where the service starts
    /// </summary>
    public CoordReadDto initServiceAddress { get; set; }

    /// <summary>
    /// contains the coordinates where the service ends
    /// </summary>
    public CoordReadDto finishService { get; set; }

    /// <summary>
    /// contains the coordinates of current location of the truck
    /// </summary>
    public CoordReadDto nowLocationTruck { get; set; }

    /// <summary>
    /// contains the current location of the truck
    /// </summary>
    public string currentLocation { get; set; }

    /// <summary>
    /// contains the expected duration until the service ends
    /// </summary>
    public string durationToFinish { get; set; }

    /// <summary>
    /// contains the kms of service
    /// </summary>
    public double kms { get; set; }

    /// <summary>
    /// contains the service profit
    /// </summary>
    public double profit { get; set; }

    /// <summary>
    /// contains the remaining capacity of the truck
    /// </summary>
    public double capacityAvailable { get; set; }
}