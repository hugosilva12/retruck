using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Repository.Implementations;

/// <summary>
/// This class represents a Image Repository that contains all the necessary methods for Image management.
/// </summary>
public class PathPhotoRepository : IPathPhotoRepository
{
    /// <summary>
    ///The DbContext instance that represents a session with the database that can be used to query, insert, update and drop
    /// </summary>
    private readonly AppDbContext context;

    /// <summary>
    /// Constructor method for the PathPhoto repository.
    /// </summary>
    /// <param name="databaseContext"> <see cref="DatabaseContext"/> of database </param>
    public PathPhotoRepository(AppDbContext databaseContext)
    {
        context = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<PathPhoto> addImage(PathPhoto photo)
    {
        var image = await context.PathPhoto.AddAsync(photo);
        context.SaveChanges();
        return image.Entity;
    }
}