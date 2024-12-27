using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.DTOS;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;
using WebApplication1.Repository.Interfaces;

namespace WebApplication1.Repository.Implementations;

/// <summary>
/// This class represents a Service Repository that contains all the necessary methods for service transport management.
/// </summary>
public class ServiceRepository : IServiceRepository
{
    /// <summary>
    ///The DbContext instance that represents a session with the database that can be used to query, insert, update, and drop
    /// </summary>
    private readonly AppDbContext context;

    /// <summary>
    /// Constructor method for the Service repository.
    /// </summary>
    /// <param name="databaseContext"> <see cref="DatabaseContext"/> of database </param>
    public ServiceRepository(AppDbContext databaseContext)
    {
        context = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<ServiceTransport> addServiceTransport(ServiceWriteDto serviceWriteDto)
    {
        var service = await context.Transport.FirstOrDefaultAsync(user => user.id == serviceWriteDto.transportId);

        if (service is null)
            throw new InvalidOperationException($"Transport with id \"{serviceWriteDto.transportId}\" does not exist");

        var truck = await context.Truck.FirstOrDefaultAsync(user => user.id == serviceWriteDto.truckId);

        if (truck is null)
            throw new InvalidOperationException($"Truck with id \"{serviceWriteDto.truckId}\" does not exist");

        var alReadyExists = await verifyIfServiceAlreadyExists(serviceWriteDto);
        if (!alReadyExists)
        {
            ServiceTransport serviceTransport = new ServiceTransport();
            serviceTransport.kms = serviceWriteDto.kms;
            serviceTransport.idTransport = serviceWriteDto.transportId.ToString();
            serviceTransport.idTruck = serviceWriteDto.truckId.ToString();
            serviceTransport.profit = serviceWriteDto.profit;
            serviceTransport.status = ServiceStatus.TO_START;
            serviceTransport.capacityAvailable = serviceWriteDto.capacityAvailable;
            if (serviceTransport.capacityAvailable == 0)
                serviceTransport.capacityAvailable = 500;

            var serviceAdded = await context.Service.AddAsync(serviceTransport);

            await context.SaveChangesAsync();
            return serviceAdded.Entity;
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<List<ServiceTransportReadDto>> getAllServiceTransports()
    {
        var listServiceReadDto = new List<ServiceTransportReadDto>();
        var list = await context.Service.ToListAsync();
        foreach (var item in list)
        {
            var service = await entityToDtoMapper(item);
            if (service != null)
                listServiceReadDto.Add(service);
        }

        return listServiceReadDto;
    }

    /// <inheritdoc/>
    public async Task<ServiceTransportReadDto> getServiceTransport(Guid guid)
    {
        var service = await context.Service.FirstOrDefaultAsync(service => service.id == guid);
        
        return service == null ? null : await entityToDtoMapper(service);
    }

    /// <inheritdoc/>
    public async Task updateStateService(Guid guid, ServiceStatus serviceStatus)
    {
        var service = await context.Service.FirstOrDefaultAsync(service => service.id == guid);
        if (service != null)
        {
            service.status = serviceStatus;
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// This method checks if the service is already created.
    /// </summary>
    /// <param name="serviceWriteDto"><see cref="ServiceWriteDto"/> to be found</param>
    /// <returns>True if exists, False otherwise</returns>
    private async Task<Boolean> verifyIfServiceAlreadyExists(ServiceWriteDto serviceWriteDto)
    {
        var list = await context.Service.ToListAsync();
        foreach (var item in list)
        {
            if (Guid.Parse(item.idTruck).Equals(serviceWriteDto.truckId) &&
                Guid.Parse(item.idTransport).Equals(serviceWriteDto.transportId))
                return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public async Task updateProfitService(Guid guid, double value)
    {
        var service = await context.Service.FirstOrDefaultAsync(service => service.id == guid);
        if (service != null)
        {
            service.profit += value;
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    ///  This method converts an entity (<see cref="ServiceTransport"/>)  into a dto (<see cref="ServiceTransportReadDto"/>)
    /// </summary>
    /// <returns> of <see cref="ServiceTransportReadDto"/> created </returns>
    private async Task<ServiceTransportReadDto> entityToDtoMapper(ServiceTransport item)
    {
        ServiceTransportReadDto serviceTransportRead = new ServiceTransportReadDto();
        serviceTransportRead.transportReadDto = new TransportReadDto();
        serviceTransportRead.truckReadDto = new TruckReadDto();

        serviceTransportRead.status = item.status;
        serviceTransportRead.kms = item.kms;
        serviceTransportRead.id = item.id;
        serviceTransportRead.profit = item.profit;
        serviceTransportRead.capacityAvailable = item.capacityAvailable;

        //Get Information of FKS
        var transport =
            await context.Transport.FirstOrDefaultAsync(trans =>
                trans.id == Guid.Parse(item.idTransport));
        if (transport == null)
            return null;

        serviceTransportRead.transportReadDto.origin = transport.origin;
        serviceTransportRead.transportReadDto.truckCategory = transport.truck_category;
        serviceTransportRead.transportReadDto.destiny = transport.destiny;
        serviceTransportRead.transportReadDto.date = transport.date.ToString("dd/MM/yyyy");
        serviceTransportRead.transportReadDto.client_id = new UserReadDto();
        serviceTransportRead.transportReadDto.client_id.id = transport.user_client.id;


        var truck = await context.Truck.FirstOrDefaultAsync(truckSelect =>
            truckSelect.id == Guid.Parse(item.idTruck));

        if (truck == null)
            return null;

        serviceTransportRead.truckReadDto.matricula = truck.matricula;
        serviceTransportRead.truckReadDto.id = truck.id;
        serviceTransportRead.truckReadDto.driver = new UserReadDto();
        serviceTransportRead.truckReadDto.driver.username = truck.driver.username;

        var organization = await context.Organization.FirstOrDefaultAsync(organization =>
            organization.id == Guid.Parse(truck.organization_id));
        serviceTransportRead.organizationAddress = organization.addresses;

        return serviceTransportRead;
    }
}