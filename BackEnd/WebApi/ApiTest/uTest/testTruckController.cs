using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.DTOS;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace ApiTest.UTest;

public class testTruckController
{
    private readonly TruckController truckController;
    private readonly Mock<ITruckRepository> mock = new Mock<ITruckRepository>();

    public testTruckController()
    {
        truckController = new TruckController(mock.Object);
    }


    [Test]
    public async Task getTruckNotFoundTest()
    {
        mock.Setup(x => x.getTruck(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        ActionResult<TruckReadDto> userResult = await truckController.getTruck(Guid.NewGuid());

        //Assert
        Assert.AreEqual(typeof(NotFoundResult), userResult.Result.GetType());
    }

    [Test]
    public async Task getTruckTest()
    {
        Truck truck = await instanceTruck();

        mock.Setup(x => x.getTruck(It.IsAny<Guid>()))
            .ReturnsAsync(() => truck);

        ActionResult<TruckReadDto> truckResult = await truckController.getTruck(Guid.NewGuid());

        //Assert
        Assert.AreEqual("23-XW-12", truckResult.Value.matricula);
        Assert.AreEqual(2000, truckResult.Value.year);
        Assert.AreEqual(20, truckResult.Value.fuelConsumption);
        Assert.AreEqual(200000, truckResult.Value.kms);
        Assert.IsNull(truckResult.Value.organization_id);
    }

    [Test]
    public async Task addTruckInvalidTest()
    {
        ActionResult<TruckReadDto> truckResult = await truckController.addTruck(null);

        //Assert
        Assert.IsNull(truckResult.Value);
    }

    public async Task<TruckWriteDto> instanceTruckWriteDto()
    {
        TruckWriteDto truck = new TruckWriteDto();
        truck.matricula = "23-XW-12";
        truck.organizationId = "olaorganization";
        truck.truckCategory = TruckCategory.DUMP_TRUCK;
        truck.driverId = Guid.NewGuid();
        return truck;
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

    [Test]
    public async Task addTruckTest()
    {
        Truck truck = await instanceTruck();

        TruckWriteDto truckWriteDto = await instanceTruckWriteDto();

        mock.Setup(x => x.addTruck(truckWriteDto))
            .ReturnsAsync(() => truck);

        ActionResult<TruckReadDto> truckResult = await truckController.addTruck(truckWriteDto);

        //Assert
        Assert.AreEqual("23-XW-12", truckResult.Value.matricula);
        Assert.AreEqual(2000, truckResult.Value.year);
        Assert.AreEqual(20, truckResult.Value.fuelConsumption);
        Assert.AreEqual(200000, truckResult.Value.kms);
        Assert.IsNull(truckResult.Value.organization_id);
    }

    [Test]
    public async Task getAllTrucksTestBVA01()
    {
        var list = new List<Truck>();
        IEnumerable<Truck> enumerable = list;
        mock.Setup(x => x.getAllTrucks())
            .ReturnsAsync(() => enumerable);

        ActionResult<List<TruckReadDto>> truckResult = await truckController.getAllTrucks();
        Assert.AreEqual(0, truckResult.Value.Count);
    }

    [Test]
    public async Task getAllTrucksTest()
    {
        var list = new List<Truck>();
        list.Add(await instanceTruck());
        IEnumerable<Truck> enumerable = list;
        mock.Setup(x => x.getAllTrucks())
            .ReturnsAsync(() => enumerable);

        ActionResult<List<TruckReadDto>> truckResult = await truckController.getAllTrucks();
        Assert.AreEqual(1, truckResult.Value.Count);
    }
}