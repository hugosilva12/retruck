namespace WebApplication1.DTOS.Read;

/// <summary>
/// The Coord class stores the coordinates at a time of a truck
/// </summary>
public class CoordReadDto
{
    /// <summary>
    /// contains the latitude of the location
    /// </summary>
    public double lat { get; set; }

    /// <summary>
    /// contains the longitude of the location
    /// </summary>
    public double lng { get; set; }
}