using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

/// <summary>
///  The ServiceCoord class stores information about the location of the truck in a service
/// </summary>
public class ServiceCoord
{
    /// <summary>
    /// identifier of the ServiceCoord
    /// </summary>
    public Guid id { get; set; }

    /// <summary>
    /// contains the service corresponding to the location
    /// </summary>
    [Required]
    public virtual ServiceTransport serviceTransport { get; set; }

    /// <summary>
    /// contains the latitude of the location
    /// </summary>
    public double latitude { get; set; }

    /// <summary>
    /// contains the longitude of the location
    /// </summary>
    public double longitude { get; set; }
}