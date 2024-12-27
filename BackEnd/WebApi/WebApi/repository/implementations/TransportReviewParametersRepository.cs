using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;
using WebApplication1.Repository.Interfaces;

namespace WebApplication1.Repository.Implementations;

/// <summary>
/// This class represents a TransportReviewParameters Repository that contains all the necessary methods to save and change parameters to analyze a transport
/// </summary>
public class TransportReviewParametersRepository : ITransportReviewParametersRepository
{
    /// <summary>
    ///The DbContext instance that represents a session with the database that can be used to query, insert, update, and drop
    /// </summary>
    private readonly AppDbContext context;

    /// <summary>
    /// Constructor method for the repository.
    /// </summary>
    /// <param name="databaseContext"> <see cref="DatabaseContext"/> of database </param>
    public TransportReviewParametersRepository(AppDbContext databaseContext)
    {
        context = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<TransportReviewParameters> addTransportReviewParameters(
        TransportReviewParameters transportReviewParameters)
    {
        var transportReviewParametersInDataBase = context.TransportReviewParameters.FirstOrDefault();

        if (transportReviewParametersInDataBase != null)
            throw new InvalidOperationException($"Already exists");

        var transportReviewParametersAdded =
            await context.TransportReviewParameters.AddAsync(transportReviewParameters);
        await context.SaveChangesAsync();
        return transportReviewParametersAdded.Entity;
    }

    /// <inheritdoc/>
    public async Task<TransportReviewParameters> getTransportReviewParameters()
    {
        var transportReviewParametersInDataBase = context.TransportReviewParameters.FirstOrDefault();
        if (transportReviewParametersInDataBase != null)
            return transportReviewParametersInDataBase;

        TransportReviewParameters transportReviewParameters = new TransportReviewParameters();
        transportReviewParameters.valueFuel = 1.98;
        transportReviewParameters.valueHoliday = 0;
        transportReviewParameters.valueSunday = 0;
        transportReviewParameters.valueSaturday = 0;
        transportReviewParameters.valueToll = 0.15;
        transportReviewParameters.typeAnalysis = TypeAnalysis.BOTH;
        transportReviewParameters.considerTruckBreakDowns = ConsiderTruckBreakDowns.NO;
        return await addTransportReviewParameters(transportReviewParameters);
    }

    /// <inheritdoc/>
    public async Task updateTransportReviewParameters(TransportReviewParameters transportReviewParameters,
        Guid idTransportReviewParameters)
    {
        var transportReviewParametersInDataBase =
            await context.TransportReviewParameters.FirstOrDefaultAsync(user => user.id == idTransportReviewParameters);
        if (transportReviewParametersInDataBase == null)
            throw new InvalidOperationException($"Id Not exists");

        transportReviewParametersInDataBase.valueFuel = transportReviewParameters.valueFuel;
        transportReviewParametersInDataBase.valueHoliday = transportReviewParameters.valueHoliday;
        transportReviewParametersInDataBase.valueSunday = transportReviewParameters.valueSunday;
        transportReviewParametersInDataBase.valueToll = transportReviewParameters.valueToll;
        transportReviewParametersInDataBase.valueSaturday = transportReviewParameters.valueSaturday;
        transportReviewParametersInDataBase.typeAnalysis = transportReviewParameters.typeAnalysis;
        transportReviewParametersInDataBase.considerTruckBreakDowns = transportReviewParameters.considerTruckBreakDowns;
        await context.SaveChangesAsync();
    }
}