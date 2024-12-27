using AutoMapper;
using FireSharp;
using FireSharp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context.Global;
using WebApplication1.DTOS;
using WebApplication1.DTOS.Global;
using WebApplication1.DTOS.Read;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.Security;
using Profile = WebApplication1.Global.Enumerations.Profile;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// This controller manage users.
    /// </summary>
    [Route("api/v1/user")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;

        private IFirebaseClient client;

        private ILicenseRepository licenseRepository;

        private IMapper mapper;


        /// <summary>
        /// This constructor inject the user repository and the license repository to be use by the user controller.
        /// </summary>
        /// <param name="userRepository"> user repository</param>
        /// <param name="mapper"></param>
        /// <param name="licenseRepository"> license repository</param>
        public UsersController(IUserRepository userRepository, IMapper mapper, ILicenseRepository licenseRepository)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.licenseRepository = licenseRepository;
            client = new FirebaseClient(Utils.config);
        }

        /// <summary>
        /// This endpoint adds a  new user in sql database and firebase.
        /// </summary>
        /// <param name="user">to be added</param>
        /// <returns>The added <see cref="User"/>, exception if the data matches an already existing user</returns>
        [HttpPost]
        [Authorize(Roles = "SUPER_ADMIN")]
        public async Task<ActionResult<User>> addUser([FromBody] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (registerUserDto == null)
                return BadRequest();

            //Save to DataBase
            string password = registerUserDto.password;
            registerUserDto.password = PasswordEncrypter.encryptPassword(registerUserDto.password);
            var userForAdd = mapper.Map<User>(registerUserDto);

            User user = null;
            if (userForAdd.role == Profile.DRIVER)
            {
                user = await userRepository.addUserDriver(userForAdd, registerUserDto.organizationId,
                    registerUserDto.category);
            }
            else
            {
                user = await userRepository.addUser(userForAdd, registerUserDto.organizationId);
            }

            user.organization = null;

            //Save to Firebase 
            if (user.role == Profile.DRIVER || user.role == Profile.CLIENT)
            {
                user.password = password;
                user.photofilename = "null";
                client.Push("users/", user);
            }

            user.password = "";
            return Ok(user);
        }

        /// <summary>
        /// This endpoint returns the specific user of the given id.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> of the <see cref="User"/> to find</param>
        /// <returns>The chosen <see cref="UserReadDto"/> exception if not</returns>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "SUPER_ADMIN")]
        public async Task<ActionResult<UserReadDto>> getUser(Guid id)
        {
            try
            {
                var user = await userRepository.getUser(id);
                if (user == null)
                    return NotFound();
                //Convert DTO
                var userReadDto = mapper.Map<UserReadDto>(user);
                return userReadDto;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// This endpoint returns all drivers
        /// </summary>
        /// <returns><see cref="List{T}" /> of <see cref="UserReadDto" /></returns>
        [HttpGet]
        [Authorize(Roles = "SUPER_ADMIN")]
        public async Task<ActionResult<List<UserReadDto>>> getAllDrivers()
        {
            var list = userRepository.getAllUsers();
            var listToReturn = new List<UserReadDto>();
            foreach (var manager in list.Result.organization.users)
            {
                if (manager.role == Profile.DRIVER)
                {
                    UserReadDto toReturn = Utils.entityToDtoUser(manager);
                    listToReturn.Add(toReturn);
                }
            }

            return Ok(listToReturn);
        }

        /// <summary>
        /// This endpoint returns all managers
        /// </summary>
        /// <returns><see cref="List{T}" /> of <see cref="UserReadDto" /></returns>
        [Route("managers")]
        [HttpGet]
        [Authorize(Roles = "SUPER_ADMIN")]
        public async Task<ActionResult<List<UserReadDto>>> getAllManagers()
        {
            var listToReturn = new List<UserReadDto>();
            var list = userRepository.getAllUsers();

            foreach (var manager in list.Result.organization.users)
            {
                if (manager.role == Profile.MANAGER && manager.userState == State.ACTIVE)
                {
                    UserReadDto toReturn = Utils.entityToDtoUser(manager);
                    listToReturn.Add(toReturn);
                }
            }

            return Ok(listToReturn);
        }

        /// <summary>
        /// This endpoint returns all clients
        /// </summary>
        /// <returns><see cref="List{T}" /> of <see cref="UserReadDto" /></returns>
        [Route("costumers")]
        [HttpGet]
        [Authorize(Roles = "SUPER_ADMIN")]
        public async Task<ActionResult<List<UserReadDto>>> getAllCostumers()
        {
            var listToReturn = new List<UserReadDto>();
            var list = userRepository.getAllUsers();

            foreach (var manager in list.Result.organization.users)
            {
                if (manager.role == Profile.CLIENT && manager.userState == State.ACTIVE)
                {
                    UserReadDto toReturn = Utils.entityToDtoUser(manager);
                    listToReturn.Add(toReturn);
                }
            }

            return Ok(listToReturn);
        }

        /// <summary>
        /// This endpoint returns all drivers and their licenses
        /// </summary>
        /// <returns><see cref="List{T}" /> of <see cref="DriverWithLicense" /></returns>
        [Route("driverswithlicence")]
        [HttpGet]
        [Authorize(Roles = "SUPER_ADMIN,MANAGER")]
        public async Task<ActionResult<List<DriverWithLicense>>> getAllDriversWithLicense()
        {
            var listToReturn = new List<DriverWithLicense>();
            var list = await userRepository.getAllAvailableDrivers();
            foreach (var driver in list)
            {
                if (driver.role == Profile.DRIVER && driver.userState == State.ACTIVE)
                {
                    var license = await licenseRepository.getLicence(driver.id);
                    listToReturn.Add(new DriverWithLicense
                        { id_driver = driver.id, truckCategory = license.truck_category, name = driver.name });
                }
            }

            return Ok(listToReturn);
        }

        /// <summary>
        /// This endpoint updates the specific user of the given id.
        /// </summary>
        /// <param name="user"><see cref="RegisterUserDto"/> to be updated</param>
        /// <param name="id"> User <see cref="Guid"/> to edit</param>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "SUPER_ADMIN")]
        public async Task updateUser(Guid id, [FromBody] RegisterUserDto registerUserDto)
        {
            await userRepository.updateUser(id, registerUserDto);
        }


        /// <summary>
        /// This endpoint disables the specific user of the given id.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> of user to remove </param>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "SUPER_ADMIN")]
        public async Task removeUser(Guid id)
        {
            await userRepository.removeUser(id);
        }


        /// <summary>
        /// This endpoint returns the number of users per profile
        /// </summary>
        /// <returns><see cref="UsersRegisteredInSystemDto"/> with the values</returns>
        [Route("userstatistics")]
        [Authorize(Roles = "SUPER_ADMIN")]
        [HttpGet]
        public async Task<ActionResult<UsersRegisteredInSystemDto>> getUserStatistics()
        {
            UsersRegisteredInSystemDto usersRegisteredInSystemDto = new UsersRegisteredInSystemDto();
            usersRegisteredInSystemDto.clients = 0;
            usersRegisteredInSystemDto.drivers = 0;
            usersRegisteredInSystemDto.managers = 0;
            usersRegisteredInSystemDto.total = 0;
            var list = await userRepository.getAllUsers();

            foreach (var manager in list.organization.users)
            {
                if (manager.role == Profile.CLIENT)
                    usersRegisteredInSystemDto.clients++;

                if (manager.role == Profile.DRIVER)
                    usersRegisteredInSystemDto.drivers++;

                if (manager.role == Profile.MANAGER)
                    usersRegisteredInSystemDto.managers++;

                usersRegisteredInSystemDto.total++;
            }

            return Ok(usersRegisteredInSystemDto);
        }
    }
}