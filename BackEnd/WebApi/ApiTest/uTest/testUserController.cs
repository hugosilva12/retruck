using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Context.Global;
using WebApplication1.Controllers;
using WebApplication1.DTOS.Read;
using WebApplication1.Models;
using WebApplication1.Repository;
using Profile = WebApplication1.Global.Enumerations.Profile;

namespace ApiTest.UTest;

public class TestUserController
{
    private readonly UsersController userController;
    private readonly Mock<IUserRepository> mock = new Mock<IUserRepository>();

    private readonly IMapper mapper;

    public TestUserController()
    {
        userController = new UsersController(mock.Object, mapper, null);
    }

    [Test]
    public async Task getNotFoundTest()
    {
        mock.Setup(x => x.getUser(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        ActionResult<UserReadDto> userResult = await userController.getUser(Guid.NewGuid());

        //Assert
        Assert.AreEqual(typeof(NotFoundResult), userResult.Result.GetType());
    }

    [Test]
    public async Task addUserWithInvalidDtoTest()
    {
        ActionResult<User> userResult = await userController.addUser(null);

        //Assert
        Assert.AreEqual(typeof(BadRequestResult), userResult.Result.GetType());
    }

    [Test]
    public async Task getAllManagersTest()
    {
        var organization = new Organization()
            { enable = true, vatin = 1, addresses = "Amarante", name = "Organization2" };
        organization.users = new List<User>();

        var userInput = new User()
        {
            username = "test1", password = "0000hug1", role = Profile.DRIVER, name = "Hugo",
            email = "hugsaf2132@gmail.com", photofilename = "olamundo", userState = State.ACTIVE
        };
        organization.users.Add(userInput);

        var userInput2 = new User()
        {
            username = "test1", password = "0000hug1", role = Profile.DRIVER, name = "Hugo",
            email = "hugsaf2132@gmail.com", photofilename = "olamundo", userState = State.ACTIVE,
            organization = organization
        };

        mock.Setup(x => x.getAllUsers())
            .ReturnsAsync(() => userInput2);


        ActionResult<List<UserReadDto>> userResult = await userController.getAllManagers();


        Assert.IsNull(userResult.Value);
    }
}