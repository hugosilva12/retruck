using WebApplication1.DTOS;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;

namespace WebApplication1.Repository.Interfaces;

/// <summary>
/// This interface represents a Service Repository that contains all the necessary methods for Service management.
/// </summary>
public interface IServiceRepository
{
    /// <summary>
    /// This method adds a <see cref="ServiceTransport"/> to the database.
    /// </summary>
    /// <param name="serviceWriteDto">to be added</param>
    /// <returns>The added <see cref="ServiceTransport"/>, null if the data corresponds to a service already created</returns>
    Task<ServiceTransport> addServiceTransport(ServiceWriteDto serviceWriteDto);

    /// <summary>
    /// Returns all the existing <see cref="ServiceTransport"/>.
    /// </summary>
    /// <returns><see cref="List{T}" /> of <see cref="ServiceTransportReadDto" /></returns>
    Task<List<ServiceTransportReadDto>> getAllServiceTransports();

    /// <summary>
    /// Returns a specific <see cref="ServiceTransport"/> by its id.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/> of the <see cref="ServiceTransport"/> to find</param>
    /// <returns>The chosen <see cref="ServiceTransport"/>, null if it doesn't exist</returns>
    Task<ServiceTransportReadDto> getServiceTransport(Guid guid);

    /// <summary>
    /// This method updates the state of service
    /// </summary>
    /// <param name="serviceStatus"><see cref="ServiceStatus"/> to be updated</param>
    /// <param name="guid"> Service <see cref="Guid"/>to edit</param>
    Task updateStateService(Guid guid, ServiceStatus serviceStatus);

    /// <summary>
    /// This method updates the profit of service
    /// </summary>
    /// <param name="value"><see cref="double"/> value to add</param>
    /// <param name="guid"> Service <see cref="Guid"/>to edit</param>
    Task updateProfitService(Guid guid, double value);
}