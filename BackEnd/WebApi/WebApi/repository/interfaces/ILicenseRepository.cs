using WebApplication1.Models;

namespace WebApplication1.Repository;

/// <summary>
/// This interface represents a License Repository that contains all the necessary methods for License management.
/// </summary>
public interface ILicenseRepository
{
    /// <summary>
    /// Returns a specific <see cref="License"/> by its id.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of the <see cref="License"/> to find</param>
    /// <returns>The chosen <see cref="License"/>, null if it doesn't exist</returns>
    public Task<License> getLicence(Guid id);
}