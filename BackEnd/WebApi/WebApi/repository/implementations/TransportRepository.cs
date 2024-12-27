using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.DTOS;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;

//using WebApi.Migrations;

namespace WebApplication1.Repository.Implementations;

/// <summary>
/// This class represents a Transport Repository that contains all the necessary methods for Transport management.
/// </summary>
public class TransportRepository : ITransportRepository
{
    /// <summary>
    ///The DbContext instance that represents a session with the database that can be used to query, insert, update, and drop
    /// </summary>
    private readonly AppDbContext context;

    /// <summary>
    /// Constructor method for the Transport repository.
    /// </summary>
    /// <param name="databaseContext"> <see cref="DatabaseContext"/> of database </param>
    public TransportRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<Transport> addTransport(TransportWriteDto transportWrite)
    {
        var transport = dtoToEntity(transportWrite);
        transport.user_client =
            await context.User.FirstOrDefaultAsync(user => user.username == transportWrite.userName);

        //User Exists
        if (transport.user_client != null)
        {
            if (transport.user_client.role == Profile.CLIENT)
            {
                var exists = await verifyIfExists(transportWrite);

                if (exists == false)
                {
                    transport.status = Status.WAIT_APROVE;

                    var truckToAdded = await context.Transport.AddAsync(transport);

                    context.SaveChanges();

                    return truckToAdded.Entity;
                }
            }
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<Transport> removeTransport(Guid id)
    {
        var toRemove = await context.Transport.FirstOrDefaultAsync(user => user.id == id);

        var removeObject = context.Transport.Remove(toRemove);

        await context.SaveChangesAsync();

        return removeObject.Entity;
    }

    /// <inheritdoc/>
    public async Task<Transport> getTransport(Guid id)
    {
        return await context.Transport.FirstOrDefaultAsync(user => user.id == id);
    }

    /// <inheritdoc/>
    public async Task<Transport> update(Guid id, Status status)
    {
        var updateObjet = await context.Transport.FirstOrDefaultAsync(user => user.id == id);
        updateObjet.status = status;
        await context.SaveChangesAsync();
        return updateObjet;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Transport>> getAllTransports()
    {
        return await context.Transport.ToListAsync();
    }

    /// <summary>
    ///  This method checks if the transport already exists
    /// </summary>
    /// <returns> True if it exists, false if not </returns>
    private async Task<Boolean> verifyIfExists(TransportWriteDto transportToCreate)
    {
        var list = await context.Transport.ToListAsync();
        var dateTime = Utils.transformStringToData2(transportToCreate.date);

        foreach (var item in list)
        {
            if (item.user_client.username == transportToCreate.userName)
            {
                if (item.date.Equals(dateTime))
                {
                    if (item.truck_category.Equals(transportToCreate.truckCategory))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    ///  This method converts a dto into a database entity
    /// </summary>
    /// <returns> <see cref="Transport"/> created </returns>
    private Transport dtoToEntity(TransportWriteDto transportWrite)
    {
        Transport transport = new Transport();
        transport.capacity = transportWrite.capacity;
        if (transportWrite.truckCategory == TruckCategory.CISTERN_TRUCK)
            transport.capacity = transportWrite.liters;

        if (transportWrite.truckCategory == TruckCategory.DUMP_TRUCK)
            transport.capacity = transportWrite.weight;

        transport.origin = transportWrite.origin;
        transport.value_offered = transportWrite.value_offered;
        transport.destiny = transportWrite.destiny;
        transport.weight = transportWrite.weight;
        transport.liters = transportWrite.liters;
        transport.truck_category = transportWrite.truckCategory;
        transport.date = Utils.transformStringToData2(transportWrite.date);

        return transport;
    }
}