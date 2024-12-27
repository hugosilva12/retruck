using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.DTOS;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;

namespace WebApplication1.Repository.Implementations;

/// <summary>
/// This class represents a Absence Repository that contains all the necessary methods for Absence management.
/// </summary>
public class AbsenceRepository : IAbsenceRepository
{
    /// <summary>
    ///The DbContext instance that represents a session with the database that can be used to query, insert, update and drop
    /// </summary>
    private readonly AppDbContext context;

    /// <summary>
    /// Constructor method for the Absence repository.
    /// </summary>
    /// <param name="databaseContext"> <see cref="DatabaseContext"/> of database </param>
    public AbsenceRepository(AppDbContext databaseContext)
    {
        context = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<Absence> addAbsence(Absence absence, Guid idUser)
    {
        absence.driver = await context.User.FirstOrDefaultAsync(user => user.id == idUser);
        if (absence.driver is null)
            throw new InvalidOperationException($"Driver with id \"{idUser}\" does not exist");


        if (absence.absence == AbsenceType.SICK || absence.absence == AbsenceType.FAMILY)
        {
            absence.status = Status.ACCEPT;
        }
        else
        {
            absence.status = Status.WAIT_APROVE;
        }

        var absenceAdded = await context.Absence.AddAsync(absence);

        context.SaveChanges();
        return absenceAdded.Entity;
    }

    /// <inheritdoc/>
    public async Task<Absence> updateAbsence(Guid idAbsence, Status status)
    {
        var absence = await context.Absence.FirstOrDefaultAsync(absence => absence.id == idAbsence);
        if (absence is null)
            throw new InvalidOperationException($"Absence with id \"{idAbsence}\" does not exist");

        if (absence.status != Status.WAIT_APROVE)
            throw new InvalidOperationException($"Absence with id \"{idAbsence}\" cannot be updated");

        absence.status = status;
        context.SaveChanges();
        return absence;
    }


    /// <inheritdoc/>
    public async Task<Absence> addAbsenceFromFirebase(AbsenceWriteDto absenceWriteDto)
    {
        Absence absence = new Absence();
        absence.description = absenceWriteDto.description;
        absence.absence = absenceWriteDto.absenceType;
        absence.date = Utils.transformStringToData2(absenceWriteDto.date);
        absence.driver = await context.User.FirstOrDefaultAsync(user => user.username == absenceWriteDto.id);

        //Driver exists ?
        if (absence.driver != null)
        {
            if (absence.absence == AbsenceType.SICK || absence.absence == AbsenceType.FAMILY)
            {
                absence.status = Status.ACCEPT;
            }
            else
            {
                absence.status = Status.WAIT_APROVE;
            }

            //Absence Repetition
            var exists = await verifyIfExists(absenceWriteDto);
            if (exists == false)
            {
                var absenceAdded = await context.Absence.AddAsync(absence);
                context.SaveChanges();

                return absenceAdded.Entity;
            }
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Absence>> getAllAbsences()
    {
        return await context.Absence.ToListAsync();
    }

    /// <summary>
    ///  This method checks if the absence already exists
    /// </summary>
    /// <returns> True if it exists, false if not </returns>
    public async Task<Boolean> verifyIfExists(AbsenceWriteDto absenceWriteDto)
    {
        var list = await context.Absence.ToListAsync();
        var dateTime = Utils.transformStringToData2(absenceWriteDto.date);

        foreach (var item in list)
        {
            //user => user.driver == absence.driver && user.date.Equals(absence.date)
            if (item.driver.username == absenceWriteDto.id)
            {
                if (item.date.Equals(dateTime))
                {
                    return true;
                }
            }
        }

        return false;
    }
}