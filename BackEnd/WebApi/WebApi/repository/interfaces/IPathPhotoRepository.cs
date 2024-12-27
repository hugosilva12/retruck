using WebApplication1.Models;

namespace WebApplication1.Repository;

/// <summary>
/// This interface represents a Image Repository that contains all the necessary methods for Image management.
/// </summary>
public interface IPathPhotoRepository
{
    /// <summary>
    /// This method adds a <see cref="PathPhoto"/> to the database.
    /// </summary>
    /// <param name="photo">to be added</param>
    /// <returns>Added <see cref="PathPhoto"/></returns>
    Task<PathPhoto> addImage(PathPhoto photo);
}