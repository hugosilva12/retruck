using System.ComponentModel.DataAnnotations;
using WebApplication1.Global.Enumerations;

namespace WebApplication1.DTOS.Read;

/// <summary>
///  The TransportReviewParameters class stores the result of the analysis of a transport
/// </summary>
public class ReviewTransportDto : BasicDto
{
    /// <summary>
    /// contains the number of kms that the truck will travel from the transport
    /// </summary>
    public double kms { get; set; }

    /// <summary>
    /// contains the value offered by the transport
    /// </summary>
    public double valueOffered { get; set; }

    /// <summary>
    /// contains the decision whether or not the service is acceptable
    /// </summary>
    public bool available { get; set; }

    /// <summary>
    /// contains the addresses of the Organization
    /// </summary>
    public string addresseOrganization { get; set; }

    /// <summary>
    /// contains the analyzed trucks
    /// </summary>
    public List<TruckForReviewReadDto> listTrucks { get; set; }

    /// <summary>
    /// contains the client who created the transport
    /// </summary>
    [Required]
    public virtual UserReadDto client { get; set; }

    /// <summary>
    /// contains the selected trucks
    /// </summary>
    public List<TruckReadDto> truckSelected { get; set; }

    /// <summary>
    /// contains the expected profit for the service
    /// </summary>
    public double profit { get; set; }

    /// <summary>
    /// contains the decision whether or not the service is acceptable
    /// </summary>
    public bool thinksTwice { get; set; }

    /// <summary>
    /// contains the decision if it is to evaluate the history of the customer
    /// </summary>
    public bool reviewIsClose { get; set; }

    /// <summary>
    /// contains if the service is impossible because of the size of the trucks
    /// </summary>
    public bool serviceNotAvailableBecauseSizeOfTruck { get; set; }

    /// <summary>
    /// contains the decision whether the manager should be alerted to the client's good history
    /// </summary>
    public bool alertManagerForGoodHistory { get; set; }

    /// <summary>
    /// contains the description of review
    /// </summary>
    public string description { get; set; }

    /// <summary>
    /// contains a list of services
    /// </summary>
    public List<ServiceTransportReadDto> listServices { get; set; }

    /// <summary>
    /// contains the date of the transport
    /// </summary>
    public string date { get; set; }

    /// <summary>
    /// contains the category of truck that is to be used for the service
    /// </summary>
    public TruckCategory truckCategory { get; set; }

    /// <summary>
    /// contains the origin of the transport
    /// </summary>
    public string origin { get; set; }

    /// <summary>
    /// contains the destiny of the transport
    /// </summary>
    public string destiny { get; set; }

    /// <summary>
    /// contains the weigth of the transport
    /// </summary>
    public double weight { get; set; }

    /// <summary>
    /// contains the capacity of the transport
    /// </summary>
    public double capacity { get; set; }

    /// <summary>
    /// contains the liters of the transport (if it is a tanker truck)
    /// </summary>
    public int liters { get; set; }

    /// <summary>
    /// contains the status of the transport (accepted, rejected, for analyzing)
    /// </summary>
    public Status status { get; set; }
}