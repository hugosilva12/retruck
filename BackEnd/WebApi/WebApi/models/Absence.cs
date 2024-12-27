using System.ComponentModel.DataAnnotations;
using WebApplication1.Global.Enumerations;

namespace WebApplication1.Models;

/// <summary>
///  The Absence class stores information about the Absence
/// </summary>
public class Absence
{
    /// <summary>
    /// identifier of the Absence
    /// </summary>
    public Guid id { get; set; }

    /// <summary>
    /// contains the driver who registered the absence
    /// </summary>
    [Required]
    public virtual User driver { get; set; }

    /// <summary>
    /// contains the date and time of the absence
    /// </summary>
    public DateTime date { get; set; }

    /// <summary>
    /// contains the description of the absence
    /// </summary>
    public string description { get; set; }

    /// <summary>
    /// contains the type of the absence
    /// </summary> 
    public AbsenceType absence { get; set; }

    /// <summary>
    /// contains the state of the absence
    /// </summary> 
    public Status status { get; set; }
}