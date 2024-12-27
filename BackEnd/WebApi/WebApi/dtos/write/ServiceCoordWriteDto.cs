using Coord = WebApplication1.Global.UtilsModels.Coord;

namespace WebApplication1.DTOS;

/// <summary>
///  The ServiceCoordWriteDto class stores information to save the coordinates of a service
/// </summary>
public class ServiceCoordWriteDto
{
    /// <summary>
    /// contains the id of service
    /// </summary>
    public Guid idService { get; set; }

    /// <summary>
    /// contains the coordinates of service 
    /// </summary>
    public Coord coord { get; set; }
}