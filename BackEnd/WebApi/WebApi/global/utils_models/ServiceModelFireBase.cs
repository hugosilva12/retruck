using WebApplication1.Global.Enumerations;


namespace WebApplication1.Global.UtilsModels;

/// <summary>
/// This class stores the service data that will be stored in the firebase database
/// </summary>
public class ServiceModelFireBase
{
    /// <summary>
    /// identifier of the Service
    /// </summary>
    public Guid idService { get; set; }

    /// <summary>
    /// contains de the username of driver who will do the service
    /// </summary>
    public string userNameDriver { get; set; }

    /// <summary>
    /// contains the origin of the transport
    /// </summary>
    public string origin { get; set; }

    /// <summary>
    /// contains the destiny of the transport
    /// </summary>
    public string destiny { get; set; }

    /// <summary>
    /// contains the date of the transport
    /// </summary>
    public string date { get; set; }

    /// <summary>
    /// contains the state of service
    /// </summary>
    public ServiceStatus status { get; set; }
}