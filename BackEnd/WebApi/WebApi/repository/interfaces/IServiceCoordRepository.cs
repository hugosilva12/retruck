using WebApplication1.DTOS;
using WebApplication1.Models;

namespace WebApplication1.Repository.Interfaces;

/// <summary>
/// This interface represents a ServiceCoor Repository that contains all the necessary methods to save and list the coordinates of a service.
/// </summary>
public interface IServiceCoordRepository
{
    /// <summary>
    /// This method adds a <see cref="ServiceCoord"/> in the database.
    /// </summary>
    /// <param name="transportWrite">to be added</param>
    /// <returns>The added <see cref="ServiceCoord"/>, null if the coordinates already exists </returns>
    Task<ServiceCoord> addServiceCoord(ServiceCoordWriteDto transportWrite);


    /// <summary>
    /// This method returns all coordinates <see cref="ServiceCoord"/> associated with a service.
    /// </summary>
    /// <param name="idService"> id of the service for which the coordinates are to be obtained</param>
    /// <returns><see cref="List{T}" /> of <see cref="ServiceCoord" /></returns>
    Task<List<ServiceCoord>> getAllServiceCoordByServiceId(Guid idService);
}