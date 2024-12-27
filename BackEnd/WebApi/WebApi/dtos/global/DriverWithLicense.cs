using WebApplication1.Global.Enumerations;

namespace WebApplication1.DTOS;

/// <summary>
///  The DriverWithLicense class stores information about driver
/// </summary>
public class DriverWithLicense
{
    /// <summary>
    /// contains the driver id
    /// </summary>
    public Guid id_driver { get; set; }

    /// <summary>
    /// contains the name of driver
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// contains the category of license
    /// </summary>
    public TruckCategory truckCategory { get; set; }
}