using WebApplication1.DTOS;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;

namespace WebApplication1.Repository;

/// <summary>
/// This interface represents a Transport Repository that contains all the necessary methods for Transport management.
/// </summary>
public interface ITransportRepository
{
    /// <summary>
    /// Adds a <see cref="Transport"/> to the database.
    /// </summary>
    /// <param name="transportWrite">to be added</param>
    /// <returns>The added <see cref="Transport"/>, null if the transport already exists </returns>
    Task<Transport> addTransport(TransportWriteDto transportWrite);

    /// <summary>
    /// Remove a specific <see cref="Transport"/> by its id.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of Transport to remove </param>
    Task<Transport> removeTransport(Guid id);

    /// <summary>
    /// Returns a specific <see cref="Transport"/> by its id.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of the <see cref="Transport"/> to find</param>
    /// <returns>The chosen <see cref="User"/>, null if not</returns>
    Task<Transport> getTransport(Guid id);

    /// <summary>
    /// This method updates the state of transport
    /// </summary>
    /// <param name="status"><see cref="Status"/> to be updated</param>
    /// <param name="id"> Transport <see cref="Guid"/>to edit</param>
    Task<Transport> update(Guid id, Status status);

    /// <summary>
    /// Returns all the existing <see cref="Transport"/>.
    /// </summary>
    /// <returns><see cref="IEnumerable{T}" /> of <see cref="Transport" /></returns>
    Task<IEnumerable<Transport>> getAllTransports();
}