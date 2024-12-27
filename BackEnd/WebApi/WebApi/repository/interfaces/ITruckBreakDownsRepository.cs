using WebApplication1.DTOS;
using WebApplication1.Models;

namespace WebApplication1.Repository.Interfaces;

/// <summary>
/// This interface represents a TruckBreakDowns Repository that contains all the necessary methods for truck breakDowns management.
/// </summary>
public interface ITruckBreakDownsRepository
{
    /// <summary>
    /// Adds a <see cref="TruckBreakDowns"/> to the database.
    /// </summary>
    /// <param name="truckBreakDowns">to be added</param>
    /// <param name="idTruck"> >The <see cref="Guid"/> of truck </param>
    /// <returns>Added <see cref="TruckBreakDowns"/>, exception if the truck doesn't exist or already have a truck breakdown that day</returns>
    Task<TruckBreakDowns> addTruckBreakDown(TruckBreakDowns truckBreakDowns, Guid idTruck);

    /// <summary>
    /// Returns all the existing <see cref="TruckBreakDowns"/>.
    /// </summary>
    /// <returns><see cref="IEnumerable{T}" /> of <see cref="TruckBreakDowns" /></returns>
    Task<IEnumerable<TruckBreakDowns>> getAllTruckBreakDowns();

    /// <summary>
    /// Removes a specific <see cref="TruckBreakDowns"/> by its id.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of truck breakdown to remove </param>
    Task deleteTruckBreakDown(Guid id);

    /// <summary>
    /// This method updates the data of a <see cref="TruckBreakDowns"/> in the database.
    /// </summary>
    /// <param name="truckBreakDownsWriteDto"><see cref="TruckBreakDownsWriteDto"/> to be updated</param>
    /// <param name="dateTime"><see cref="DateTime"/> it occurs</param>
    /// <param name="id"> TruckBreakDowns <see cref="Guid"/>to edit</param>
    Task updateTruckBreakDown(Guid id, TruckBreakDownsWriteDto truckBreakDownsWriteDto, DateTime dateTime);

    /// <summary>
    /// Returns a specific <see cref="TruckBreakDowns"/> by its id.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of the <see cref="TruckBreakDowns"/> to find</param>
    /// <returns>The chosen <see cref="TruckBreakDowns"/>, null if the id does not exist</returns>
    Task<TruckBreakDowns> getTruckBreakDowns(Guid id);
}