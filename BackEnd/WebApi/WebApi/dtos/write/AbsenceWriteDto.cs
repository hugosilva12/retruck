using WebApplication1.Global.Enumerations;

namespace WebApplication1.DTOS;

/// <summary>
///  The Absence class stores information to save an absence
/// </summary>
public class AbsenceWriteDto
{
    /// <summary>
    /// contains the date and time of the abscence
    /// </summary>
    public String date { get; set; }

    /// <summary>
    /// contains the description of the abscence
    /// </summary>
    public string description { get; set; }

    /// <summary>
    /// contains the type of the abscence
    /// </summary> 
    public AbsenceType absenceType { get; set; }

    /// <summary>
    /// identifier of the driver
    /// </summary> 
    public string id { get; set; }
}