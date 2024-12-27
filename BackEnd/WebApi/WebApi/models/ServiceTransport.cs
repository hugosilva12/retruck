using System.ComponentModel.DataAnnotations;
using WebApplication1.Global.Enumerations;

namespace WebApplication1.Models;

/// <summary>
/// The ServiceTransport class stores information about an accepted transport service
/// </summary>
public class ServiceTransport
{
    /// <summary>
    /// identifier of the service
    /// </summary>
    public Guid id { get; set; }

    /// <summary>
    /// identifier of the truck
    /// </summary>
    [Required]
    public string idTruck { get; set; }

    /// <summary>
    /// identifier of the transport
    /// </summary>
    [Required]
    public string idTransport { get; set; }

    /// <summary>
    /// contains the state of service
    /// </summary>
    public ServiceStatus status { get; set; }

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