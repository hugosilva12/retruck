using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;
using WebApplication1.Repository.Interfaces;

namespace ApiTest.UTest;

public class testTruckBreakDownController
{
    private readonly TruckBreakDownsController truckController;
    private readonly Mock<ITruckBreakDownsRepository> mock = new Mock<ITruckBreakDownsRepository>();

    public testTruckBreakDownController()
    {
        truckController = new TruckBreakDownsController(mock.Object);
    }

    public async Task<Truck> instanceTruck()
    {
        Truck truck = new Truck();
        truck.matricula = "23-XW-12";
        truck.year = 2000;
        truck.kms = 200000;
        truck.fuelConsumption = 20;
        truck.truckCategory = TruckCategory.DUMP_TRUCK;
        truck.driver = new User();
        truck.driver.password = "0000hug1";
        return truck;
    }

    public async Task<TruckBreakDowns> instanceTruckBreakDowns()
    {
        TruckBreakDowns truckBreakDown = new TruckBreakDowns();
        truckBreakDown.description = "Test1";
        truckBreakDown.price = 3000;
        truckBreakDown.truck = await instanceTruck();

        return truckBreakDown;
    }

    [Test]
    public async Task getTruckBreakDownNotFoundTest()
    {
        mock.Setup(x => x.getTruckBreakDowns(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        ActionResult<TruckBreakDownsReadDto> truckBreakDown = await truckController.getById(Guid.NewGuid());

        //Assert
        Assert.AreEqual(typeof(NotFoundResult), truckBreakDown.Result.GetType());
    }

    [Test]
    public async Task getTruckBreakDownTest()
    {
        var truckBreakDown = await instanceTruckBreakDowns();
        mock.Setup(x => x.getTruckBreakDowns(It.IsAny<Guid>()))
            .ReturnsAsync(() => truckBreakDown);

        ActionResult<TruckBreakDownsReadDto> result = await truckController.getById(Guid.NewGuid());

        //Assert
        Assert.AreEqual("Test1", result.Value.description);
        Assert.AreEqual(3000, result.Value.price);
        Assert.IsNull(result.Value.truckReadDto.driver);
        Assert.IsNull(result.Value.truckReadDto.organization_id);
        Assert.IsNull(result.Value.truckReadDto.photoPath);
    }

    [Test]
    public async Task addTruckBreakDownInvalidTest()
    {
        ActionResult<TruckBreakDownsReadDto> truckResult = await truckController.addTruckBreakDowns(null);

        //Assert
        Assert.IsNull(truckResult.Value);
    }

    [Test]
    public async Task getAllTruckBreakDownsTest()
    {
        var list = new List<TruckBreakDowns>();
        list.Add(await instanceTruckBreakDowns());
        IEnumerable<TruckBreakDowns> enumerable = list;

        mock.Setup(x => x.getAllTruckBreakDowns())
            .ReturnsAsync(() => enumerable);

        ActionResult<List<TruckBreakDownsReadDto>> truckResult = await truckController.getAllTruckBreakDowns();
        Assert.AreEqual(1, truckResult.Value.Count);
    }

    [Test]
    public async Task getAllTruckBreakDownsBVA01Test()
    {
        var list = new List<TruckBreakDowns>();
        IEnumerable<TruckBreakDowns> enumerable = list;

        mock.Setup(x => x.getAllTruckBreakDowns())
            .ReturnsAsync(() => enumerable);

        ActionResult<List<TruckBreakDownsReadDto>> truckResult = await truckController.getAllTruckBreakDowns();
        Assert.AreEqual(0, truckResult.Value.Count);
    }
}