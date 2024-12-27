using WebApplication1.DTOS;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;

namespace WebApplication1.Repository.Implementations;

/// <summary>
/// This interface represents a Absence Repository that contains all the necessary methods for Absence management.
/// </summary>
public interface IAbsenceRepository
{
    /// <summary>
    /// This method adds a <see cref="Absence"/> to the database.
    /// </summary>
    /// <param name="absence">to be added</param>
    /// <param name="idUser"> >The <see cref="Guid"/> of user </param>
    /// <returns>Added <see cref="Absence"/> if the driver does not already have one on that day</returns>
    Task<Absence> addAbsence(Absence absence, Guid idUser);

    /// <summary>
    /// This method updates the data of a <see cref="Absence"/> in the database.
    /// </summary>
    /// <param name="status"><see cref="Status"/> to be updated</param>
    /// <param name="id"> Absence <see cref="Guid"/>to edit</param>
    Task<Absence> updateAbsence(Guid idAbsence, Status status);

    /// <summary>
    /// This method adds a <see cref="Absence"/> to the database an absence that was present in the firebase database.
    /// </summary>
    /// <param name="absenceWriteDto">to be added</param>
    /// <returns>Added <see cref="Absence"/>, null if the driver already has an absence on that day</returns>
    Task<Absence> addAbsenceFromFirebase(AbsenceWriteDto absenceWriteDto);

    /// <summary>
    ///  This method returns all the existing <see cref="Absence"/>.
    /// </summary>
    /// <returns><see cref="IEnumerable{T}" /> of <see cref="Absence" /></returns>
    Task<IEnumerable<Absence>> getAllAbsences();
}