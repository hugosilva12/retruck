using WebApplication1.Global.Enumerations;

namespace WebApplication1.DTOS;

/// <summary>
///  The Absence class stores information to update an absence
/// </summary>
public class AbsenceUpdateWriteDto
{
    /// <summary>
    /// contains the state of the abscence
    /// </summary> 
    public Status status { get; set; }
}