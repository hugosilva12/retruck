using System.ComponentModel.DataAnnotations;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;

namespace WebApplication1.DTOS;

/// <summary>
///  The TransportReadDto class stores information about a transport
/// </summary>
public class TransportReadDto : BasicDto
{
    /// <summary>
    /// contains the date of the transport
    /// </summary>
    public string date { get; set; }

    /// <summary>
    /// contains the category of truck that is to be used for the service
    /// </summary>
    public TruckCategory truckCategory { get; set; }

    /// <summary>
    /// contains the client who created the transport
    /// </summary>
    [Required]
    public virtual UserReadDto client_id { get; set; }

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
    /// contains the value offered by the transport
    /// </summary>
    public double value_offered { get; set; }

    /// <summary>
    /// contains the status of the transport (accepted, rejected, for analyzing)
    /// </summary>
    public Status status { get; set; }
}