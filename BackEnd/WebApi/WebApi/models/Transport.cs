using WebApplication1.Global.Enumerations;

namespace WebApplication1.Models;

/// <summary>
/// The Transport class stores information about a transport created by a customer
/// </summary>
public class Transport
{
    /// <summary>
    /// identifier of the transport
    /// </summary>
    public Guid id { get; set; }

    /// <summary>
    /// contains the date of the transport
    /// </summary>
    public DateTime date { get; set; }

    /// <summary>
    /// contains the category of truck that is to be used for the service
    /// </summary>
    public TruckCategory truck_category { get; set; }

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
    /// contains the customer who created the transport
    /// </summary>
    public virtual User user_client { get; set; }

    /// <summary>
    /// contains the status of the transport (accepted, rejected, for analyzing)
    /// </summary>
    public Status status { get; set; }
}