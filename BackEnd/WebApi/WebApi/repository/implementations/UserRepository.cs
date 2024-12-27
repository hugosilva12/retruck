using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Context.Global;
using WebApplication1.DTOS;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;
using WebApplication1.Security;

namespace WebApplication1.Repository
{
    /// <summary>
    /// This class represents a User Repository that contains all the necessary methods for User management.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        ///The DbContext instance that represents a session with the database that can be used to query, insert, update and drop
        /// </summary>
        private readonly AppDbContext context;


        /// <summary>
        /// Constructor method for the User repository.
        /// </summary>
        /// <param name="databaseContext"> <see cref="DatabaseContext"/> of database </param>
        public UserRepository(AppDbContext databaseContext)
        {
            context = databaseContext;
        }

        /// <inheritdoc/>
        public async Task<User> addUser(User user, Guid idOrganization)
        {
            var userExist = await context.User.FirstOrDefaultAsync(userMap => userMap.username == user.username);
            if (userExist != null)
                throw new InvalidOperationException($"UserName already exists");

            user.organization = await context.Organization.FirstOrDefaultAsync(user => user.id == idOrganization);

            if (user.organization == null)
                throw new InvalidOperationException($"Organization with id \"{idOrganization}\" does not exist");

            //Load path photo
            user.photofilename = await getPathPhoto(user.photofilename);

            user.userState = State.ACTIVE;
            var userAdded = await context.User.AddAsync(user);

            await context.SaveChangesAsync();
            return userAdded.Entity;
        }

        /// <inheritdoc/>
        public async Task<User> addUserTest(User user, Guid idOrganization)
        {
            var userExist = await context.User.FirstOrDefaultAsync(userMap => userMap.username == user.username);
            if (userExist != null)
                throw new InvalidOperationException($"UserName already exists");

            user.organization = await context.Organization.FirstOrDefaultAsync(user => user.id == idOrganization);

            if (user.organization == null)
                throw new InvalidOperationException($"Organization with id \"{idOrganization}\" does not exist");

            var userAdded = await context.User.AddAsync(user);

            await context.SaveChangesAsync();
            return userAdded.Entity;
        }

        /// <inheritdoc/>
        public async Task<User> addUserDriver(User user, Guid idOrganization, int license)
        {
            //Verify if username already exists
            var userExist = await context.User.FirstOrDefaultAsync(userMap => userMap.username == user.username);
            if (userExist != null)
                throw new InvalidOperationException($"UserName already exists");

            //Verify if Organization exists
            user.organization = await context.Organization.FirstOrDefaultAsync(user => user.id == idOrganization);
            if (user.organization == null)
                throw new InvalidOperationException($"Organization with id \"{idOrganization}\" does not exist");

            //Load path photo
            user.photofilename = await getPathPhoto(user.photofilename);

            var userAdded = await context.User.AddAsync(user);

            await context.SaveChangesAsync();

            //Insert License
            var id = Guid.NewGuid();
            var aux = context.Database.ExecuteSqlInterpolated(
                $"Insert into License (id,driverid,truck_category) values({id},{userAdded.Entity.id},{license});");
            return userAdded.Entity;
        }

        /// <inheritdoc/>
        public async Task<User> getUser(Guid id)
        {
            var user = await context.User.FirstOrDefaultAsync(user => user.id == id);
            return user != null ? user : null;
        }


        /// <inheritdoc/>
        public async Task removeUser(Guid id)
        {
            var toRemove = await getUser(id);

            toRemove.userState = State.DISABLE;

            await context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<User> getUserLogin(UserLogin login)
        {
            var user = await context.User.FirstOrDefaultAsync(user => user.username == login.username);

            if (user is null)
                return null;

            return PasswordEncrypter.verifyPassword(login.password, user.password) ? user : null;
        }

        /// <inheritdoc/>
        public async Task<User> getAllUsers()
        {
            var allUsers = await context.User.FirstAsync();
            return allUsers;
        }

        /// <inheritdoc/>
        public async Task updateUser(Guid id, RegisterUserDto registerUserDto)
        {
            var user = await getUser(id);
            user.name = registerUserDto.name;
            user.email = registerUserDto.email;
            await context.SaveChangesAsync();
        }

        /// <summary>
        ///  Get the <see cref="PathPhoto"/> of a user.
        /// </summary>
        /// <param name="photo"> basic url</param>
        /// <returns><see cref="string"/> with the photo 
        private async Task<string> getPathPhoto(string photo)
        {
            //Load path photo
            var aux_2 = await context.PathPhoto
                .FromSqlInterpolated($"select TOP 1 * from pathphoto p ORDER BY number DESC").FirstOrDefaultAsync();

            if (aux_2 != null && aux_2.type == "U")
            {
                photo += aux_2.name;
            }
            else
            {
                photo += "anonymous.png";
            }

            return photo;
        }

        /// <inheritdoc/>
        public async Task<List<UserReadDto>> getAllAvailableDrivers()
        {
            var allTrucks = context.Truck.FromSqlInterpolated($"Select * from truck  where status  = 0");

            var allUsers = await getAllUsers();

            var listToReturn = new List<UserReadDto>();
            foreach (var user in allUsers.organization.users)
            {
                if (user.role == Profile.DRIVER)
                {
                    if (Utils.isAvailableDriver(allTrucks, user.id))
                    {
                        UserReadDto toReturn = Utils.entityToDtoUser(user);
                        listToReturn.Add(toReturn);
                    }
                }
            }

            return listToReturn;
        }
    }
}