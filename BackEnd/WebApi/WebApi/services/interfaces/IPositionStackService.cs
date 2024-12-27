using WebApplication1.DTOS.Read;

namespace WebApplication1.Services;

/// <summary>
/// This interface contains the methods that interacts with external APIs and gets information about addresses  
/// </summary>
public interface IPositionStackService
{
    /// <summary>
    /// This method gets the coordinates of an address
    /// </summary>
    /// <param name="name"> the name of address</param>
    /// <returns>The address coordinates, null if http request response is empty</returns>
    Task<CoordReadDto> getCoordinatesByAddress(String name);

    /// <summary>
    ///  This method gets the address by gets address by its coordinates
    /// </summary>
    /// <returns> String with the location given by the coordinates</returns>
    Task<String> getAddressByCoordinates(CoordReadDto coord);

    /// <summary>
    ///  This method gets the postal code by gets address by its coordinates
    /// </summary>
    /// <returns> String with the postal code given by the coordinates</returns>
    Task<string> getPostalCode(CoordReadDto coord);
}