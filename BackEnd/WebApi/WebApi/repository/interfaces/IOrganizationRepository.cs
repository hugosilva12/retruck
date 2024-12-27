using WebApplication1.DTOS;
using WebApplication1.Models;

namespace WebApplication1.Repository;

/// <summary>
/// This interface represents an Organization Repository that contains all the necessary methods for Organization management.
/// </summary>
public interface IOrganizationRepository
{
    /// <summary>
    /// Adds a <see cref="Organization"/> to the database.
    /// </summary>
    /// <param name="organization">to be added</param>
    /// <returns>The added <see cref="Organization"/>, exception if the data matches an existing organization</returns>
    Task<Organization> addOrganization(OrganizationWriteDto organization);


    /// <summary>
    /// Remove a specific <see cref="Organization"/> by its id.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of Organization to remove </param>
    Task<Organization> removeOrganization(Guid id);

    /// <summary>
    /// Returns a specific <see cref="Organization"/> by its id.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of the <see cref="Organization"/> to find</param>
    /// <returns>The chosen <see cref="Organization"/>, null if it doesn't exist </returns>
    Task<Organization> getOrganization(Guid id);


    /// <summary>
    /// Returns all the existing <see cref="Organization"/>.
    /// </summary>
    /// <returns><see cref="IEnumerable{T}" /> of <see cref="Organization" /></returns>
    Task<IEnumerable<Organization>> getAllOrganizations();


    /// <summary>
    /// This method updates the data of a <see cref="Organization"/> in the database.
    /// </summary>
    /// <param name="organization"><see cref="OrganizationWriteDto"/> to be updated</param>
    /// <param name="id"> Organization <see cref="Guid"/>to edit</param>
    Task<Organization> updateOrganization(Guid id, OrganizationWriteDto organization);
}