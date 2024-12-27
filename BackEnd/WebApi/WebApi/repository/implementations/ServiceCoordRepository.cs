using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.DTOS;
using WebApplication1.Models;
using WebApplication1.Repository.Interfaces;

namespace WebApplication1.Repository.Implementations;

/// <summary>
/// This class represents a ServiceCoord Repository that contains all the necessary methods to save and list the coordinates of a service.
/// </summary>
public class ServiceCoordRepository : IServiceCoordRepository
{
    /// <summary>
    ///The DbContext instance that represents a session with the database that can be used to query, insert, update, and drop
    /// </summary>
    private readonly AppDbContext context;

    /// <summary>
    /// Constructor method for the Transport repository.
    /// </summary>
    /// <param name="context"> <see cref="DatabaseContext"/> of database </param>
    public ServiceCoordRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<ServiceCoord> addServiceCoord(ServiceCoordWriteDto serviceCoordWrite)
    {
        var serviceCoord = dtoToEntity(serviceCoordWrite);
        serviceCoord.serviceTransport =
            await context.Service.FirstOrDefaultAsync(user => user.id == serviceCoordWrite.idService);

        var isNewCoord = await verifyIfCoordinatesAlreadyExists(serviceCoordWrite);
        if (serviceCoord.serviceTransport != null && !isNewCoord)
        {
            var serviceCoordAdded = await context.ServiceCoord.AddAsync(serviceCoord);

            context.SaveChanges();

            return serviceCoordAdded.Entity;
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<List<ServiceCoord>> getAllServiceCoordByServiceId(Guid idService)
    {
        var list = await context.ServiceCoord.ToListAsync();
        var listToReturn = new List<ServiceCoord>();

        foreach (var item in list)
        {
            if (item.serviceTransport.id == idService)
                listToReturn.Add(item);
        }

        return listToReturn;
    }

    /// <summary>
    ///  This method checks if the coordinates already exists
    /// </summary>
    /// <returns> True if it exists, false if not </returns>
    private async Task<Boolean> verifyIfCoordinatesAlreadyExists(ServiceCoordWriteDto serviceCoordWrite)
    {
        var list = await context.ServiceCoord.ToListAsync();

        foreach (var item in list)
        {
            if (item.latitude == serviceCoordWrite.coord.latitude &&
                item.longitude == serviceCoordWrite.coord.longitude &&
                item.serviceTransport.id == serviceCoordWrite.idService)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///  This method converts a dto into a database entity
    /// </summary>
    /// <returns> <see cref="Transport"/> created </returns>
    private ServiceCoord dtoToEntity(ServiceCoordWriteDto serviceCoordWrite)
    {
        ServiceCoord service = new ServiceCoord();
        service.latitude = serviceCoordWrite.coord.latitude;
        service.longitude = serviceCoordWrite.coord.longitude;
        return service;
    }
}