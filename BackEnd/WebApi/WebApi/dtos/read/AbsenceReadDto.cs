using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;

namespace WebApplication1.DTOS;

/// <summary>
///  The AbsenceReadDto class stores information about the absence
/// </summary>
public class AbsenceReadDto : BasicDto
{
    /// <summary>
    /// contains the driver of the abscence
    /// </summary>
    public virtual UserReadDto driver { get; set; }

    /// <summary>
    /// contains the date of the abscence
    /// </summary>
    public String date { get; set; }

    /// <summary>
    /// contains the description of the abscence
    /// </summary>
    public string description { get; set; }

    /// <summary>
    /// contains the type of the abscence
    /// </summary> 
    public AbsenceType abscence { get; set; }

    /// <summary>
    /// contains the state of the abscence
    /// </summary> 
    public Status status { get; set; }
}