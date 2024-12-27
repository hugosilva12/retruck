using System.ComponentModel.DataAnnotations;
using WebApplication1.Global.Enumerations;

namespace WebApplication1.Models;

/// <summary>
///  The Organization class stores information about the Organization
/// </summary>
public class License
{
    /// <summary>
    /// identifier of the License
    /// </summary>
    public Guid id { get; set; }

    /// <summary>
    /// contains the driver that has the license
    /// </summary>
    [Required]
    public virtual User driver { get; set; }


    /// <summary>
    ///  contains license truck category
    /// </summary>
    public TruckCategory truck_category { get; set; }
}