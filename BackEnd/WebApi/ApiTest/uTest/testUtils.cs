using WebApplication1;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;
using WebApplication1.Services;

namespace ApiTest.UTest;

public class UtilsTest
{
    private readonly HolidayService holidayService = new HolidayService(null);

    [Test]
    public async Task transformStringToData2Test()
    {
        DateTime dateTime = Utils.transformStringToData2("02/03/2022");

        Assert.AreEqual(02, dateTime.Day);
        Assert.AreEqual(03, dateTime.Month);
        Assert.AreEqual(2022, dateTime.Year);
    }

    [Test]
    public async Task transformStringToData2BVA01Test()
    {
        Assert.Throws<InvalidOperationException>(() => Utils.transformStringToData2(""));
    }

    [Test]
    public async Task transformStringToData2BVA02Test()
    {
        Assert.Throws<InvalidOperationException>(() => Utils.transformStringToData2(null));
    }

    [Test]
    public async Task transformStringToData2InvalidTest()
    {
        Assert.Throws<InvalidOperationException>(() => Utils.transformStringToData2("02/03"));
    }

    public User instanceUser()
    {
        User userInput = new User()
        {
            username = "test1", password = "0000hug1", role = Profile.MANAGER, name = "Hugo",
            email = "hugsaf2132@gmail.com", photofilename = "olamundo",
        };
        return userInput;
    }

    [Test]
    public async Task userMapperTest()
    {
        UserReadDto userReadDto = Utils.entityToDtoUser(instanceUser());

        Assert.AreEqual("test1", userReadDto.username);
        Assert.AreEqual("olamundo", userReadDto.photofilename);
        Assert.AreEqual(Profile.MANAGER, userReadDto.role);
        Assert.AreEqual("hugsaf2132@gmail.com", userReadDto.email);
    }

    [Test]
    public async Task transformStringToDataTest()
    {
        DateTime dateTime = Utils.transformStringToData("02-03-2022");

        Assert.AreEqual(02, dateTime.Day);
        Assert.AreEqual(03, dateTime.Month);
        Assert.AreEqual(2022, dateTime.Year);
    }

    [Test]
    public async Task transformStringToDataInvalidTest()
    {
        Assert.Throws<InvalidOperationException>(() => Utils.transformStringToData("02-03"));
    }


    [Test]
    public async Task mapperResponseHolidayTest()

    {
        HolidayService holidayService = new HolidayService(null);
        string response = "[{'name':'Natal'}]";
        Assert.IsNotNull(holidayService.getJsonObject(response));
    }

    [Test]
    public async Task mapperResponseHolidayBVA01Test()

    {
        string response = "[]";
        Assert.IsNull(holidayService.getJsonObject(response));
    }

    [Test]
    public async Task mapperResponseHolidayBVA02Test()

    {
        string response = "";
        Assert.IsNull(holidayService.getJsonObject(response));
    }

    [Test]
    public async Task mapperResponseHolidayBVA03Test()

    {
        Assert.IsNull(holidayService.getJsonObject(null));
    }
}