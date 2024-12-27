using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.DTOS;
using WebApplication1.Models;
using WebApplication1.Repository.Interfaces;

namespace WebApplication1.Repository.Implementations;

/// <summary>
/// This class represents a TruckBreakDowns that contains all the necessary methods for truck breakDowns management.
/// </summary>
public class TruckBreakDownsRepository : ITruckBreakDownsRepository
{
    /// <summary>
    /// The DbContext instance that represents a session with the database that can be used to query, insert, update and drop
    /// </summary>
    private readonly AppDbContext context;

    /// <summary>
    /// Constructor method for the TruckBreakDowns repository.
    /// </summary>
    /// <param name="databaseContext"> <see cref="DatabaseContext"/> of database </param>
    public TruckBreakDownsRepository(AppDbContext databaseContext)
    {
        context = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<TruckBreakDowns> addTruckBreakDown(TruckBreakDowns truckBreakDowns, Guid idTruck)
    {
        truckBreakDowns.truck = await context.Truck.FirstOrDefaultAsync(truck => truck.id == idTruck);
        if (truckBreakDowns.truck is null)
            throw new InvalidOperationException($"Truck with id \"{idTruck}\" does not exist");


        var truckBreakDownsToAdd = context.TruckBreakDowns.Add(truckBreakDowns);

        await context.SaveChangesAsync();

        return truckBreakDownsToAdd.Entity;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TruckBreakDowns>> getAllTruckBreakDowns()
    {
        return await context.TruckBreakDowns.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task deleteTruckBreakDown(Guid id)
    {
        var truckBreakDownsToRemove = await context.TruckBreakDowns.FirstOrDefaultAsync(route => route.id == id);
        if (truckBreakDownsToRemove is null)
            throw new InvalidOperationException($"The truckBreakDowns with id \"{id}\" does not exist");

        context.TruckBreakDowns.RemoveRange(truckBreakDownsToRemove);

        await context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<TruckBreakDowns> getTruckBreakDowns(Guid id)
    {
        var truckBreackDown = await context.TruckBreakDowns.FirstOrDefaultAsync(truck => truck.id == id);
        return truckBreackDown != null ? truckBreackDown : null;
    }

    /// <inheritdoc/>
    public async Task updateTruckBreakDown(Guid id, TruckBreakDownsWriteDto truckBreakDownsWriteDto, DateTime dateTime)
    {
        var truckBreakDowns = await getTruckBreakDowns(id);
        truckBreakDowns.date = dateTime;
        truckBreakDowns.description = truckBreakDownsWriteDto.description;
        truckBreakDowns.price = truckBreakDownsWriteDto.price;
        await context.SaveChangesAsync();
    }
}