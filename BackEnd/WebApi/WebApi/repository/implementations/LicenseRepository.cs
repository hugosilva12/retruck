using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Repository.Implementations;

/// <summary>
/// This class represents a License Repository that contains all the necessary methods for License management.
/// </summary>
public class LicenseRepository : ILicenseRepository
{
    /// <summary>
    /// The DbContext instance that represents a session with the database that can be used to query, insert, update and drop
    /// </summary>
    private readonly AppDbContext context;

    /// <summary>
    /// Constructor method for the License repository.
    /// </summary>
    /// <param name="databaseContext"> <see cref="DatabaseContext"/> of database </param>
    public LicenseRepository(AppDbContext databaseContext)
    {
        context = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<License> getLicence(Guid id)
    {
        var license = await context.License.FromSqlInterpolated($"select * from license l where l.driverid = {id}")
            .FirstOrDefaultAsync();
        return license;
    }
}