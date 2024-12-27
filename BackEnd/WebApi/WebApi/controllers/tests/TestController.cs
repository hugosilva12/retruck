using AutoMapper;
using FireSharp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOS;
using WebApplication1.Global.Utils;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.Security;

namespace WebApplication1.Controllers;

/// <summary>
/// This controller is specifically used for integration tests
/// </summary>
[Route("api/v1/usertest")]
[ApiController]
public class TestController : Controller
{
    private readonly IUserRepository userRepository;

    private IFirebaseClient client;

    private IMapper mapper;

    private IDistanceService service;


    /// <summary>
    /// This constructor inject the user repository repository to be use by the user controller.
    /// </summary>
    /// <param name="userRepository">user repository</param>
    /// <param name="mapper"></param>
    public TestController(IUserRepository userRepository, IMapper mapper, IDistanceService service)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
        this.service = service;
    }

    /// <summary>
    /// This endpoint returns adds a <see cref="User"/> to the database, is only used in integration tests.
    /// </summary>
    /// <param name="user">to be added</param>
    /// <param name="user">organization id</param>
    /// <returns>The added <see cref="User"/> </returns>
    [HttpPost]
    public async Task<ActionResult<User>> addUser([FromBody] RegisterUserDto createUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (createUserDto == null)
            return BadRequest();

        //Save to DataBase
        string password = createUserDto.password;
        createUserDto.password = PasswordEncrypter.encryptPassword(createUserDto.password);
        var userForAdd = mapper.Map<User>(createUserDto);

        User user = null;
        {
            user = await userRepository.addUserTest(userForAdd, createUserDto.organizationId);
        }

        //Null Response
        user.organization = null;
        user.password = "";
        return Ok(user);
    }
}