using WebApplication1.DTOS;
using WebApplication1.DTOS.Read;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    /// <summary>
    /// This interface represents a User Repository that contains all the necessary methods for User management.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Adds a <see cref="User"/> to the database.
        /// </summary>
        /// <param name="user">to be added</param>
        /// <param name="idOrganization"> >The <see cref="Guid"/> of organization </param>
        /// <returns>Added <see cref="User"/>, exception if username already exists</returns>
        Task<User> addUser(User user, Guid idOrganization);

        /// <summary>
        /// Removes a specific <see cref="User"/> by its id.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> of user to remove </param>
        Task removeUser(Guid id);

        /// <summary>
        /// Returns a specific <see cref="User"/> by its id.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> of the <see cref="User"/> to find</param>
        /// <returns>The chosen <see cref="User"/>, null if it doesn't exist</returns>
        Task<User> getUser(Guid id);

        /// <summary>
        /// Returns all the existing <see cref="User"/>.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="User" /></returns>
        Task<User> getAllUsers();

        /// <summary>
        /// Adds a <see cref="User"/> of the type driver to the database. Only used in integration tests
        /// </summary>
        Task<User> addUserDriver(User user, Guid idOrganization, int license);

        /// <summary>
        /// Adds a <see cref="User"/> to the database. Only used in integration tests
        /// </summary>
        /// <param name="user">to be added</param>
        /// <param name="idOrganization">organization id</param>
        Task<User> addUserTest(User user, Guid idOrganization);

        /// <summary>
        /// This method updates the data of a <see cref="User"/> in the database.
        /// </summary>
        /// <param name="user"><see cref="RegisterUserDto"/> to be updated</param>
        /// <param name="id"> User <see cref="Guid"/>to edit</param>
        Task updateUser(Guid id, RegisterUserDto registerUserDto);


        /// <summary>
        /// Gets an specific <see cref="User"/> given an specific <see cref="Login"/>
        /// </summary>
        /// <param name="login">The login to find the <see cref="User"/></param>
        /// <returns>The <see cref="User"/> for the Login </returns>
        Task<User> getUserLogin(UserLogin login);

        /// <summary>
        /// Returns all existing drivers that do not have an associated truck.
        /// </summary>
        /// <returns><see cref="List{T}" /> of <see cref="UserReadDto" /></returns>
        public Task<List<UserReadDto>> getAllAvailableDrivers();
    }
}