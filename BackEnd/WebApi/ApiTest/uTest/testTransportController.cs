using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Context.Global;
using WebApplication1.Controllers;
using WebApplication1.DTOS;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace ApiTest.UTest;

public class testTransportController
{
    private readonly TransportController transportController;
    private readonly Mock<ITransportRepository> mock = new Mock<ITransportRepository>();

    public testTransportController()
    {
        transportController = new TransportController(mock.Object, null);
    }

    public async Task<Transport> instanceTransport()
    {
        var userInput = new User()
        {
            username = "test1", password = "0000hug1", role = Profile.CLIENT, name = "Hugo",
            email = "hugsaf2132@gmail.com", photofilename = "olamundo", userState = State.ACTIVE
        };

        Transport transport = new Transport();
        transport.origin = "Amarante";
        transport.destiny = "Amarante";
        transport.user_client = userInput;
        transport.date = DateTime.Now;
        transport.liters = 2000;
        transport.status = Status.WAIT_APROVE;
        transport.truck_category = TruckCategory.CISTERN_TRUCK;
        return transport;
    }

    public async Task<Transport> instanceTransportApproved()
    {
        var userInput = new User()
        {
            username = "test1", password = "0000hug1", role = Profile.CLIENT, name = "Hugo",
            email = "hugsaf2132@gmail.com", photofilename = "olamundo", userState = State.ACTIVE
        };

        Transport transport = new Transport();
        transport.origin = "Amarante";
        transport.destiny = "Amarante";
        transport.user_client = userInput;
        transport.date = DateTime.Now;
        transport.liters = 2000;
        transport.status = Status.ACCEPT;
        transport.truck_category = TruckCategory.CISTERN_TRUCK;
        return transport;
    }

    [Test]
    public async Task addTransportInvalidTest()
    {
        ActionResult<Transport> truckResult = await transportController.addTransport(null);

        //Assert
        Assert.IsNull(truckResult.Value);
    }

    [Test]
    public async Task getAllTransportsPendingBVA01Test()
    {
        var list = new List<Transport>();
        list.Add(await instanceTransportApproved());
        IEnumerable<Transport> enumerable = list;

        mock.Setup(x => x.getAllTransports())
            .ReturnsAsync(() => enumerable);

        ActionResult<List<TransportReadDto>> truckResult = await transportController.getAllTransportsPending();
        Assert.AreEqual(0, truckResult.Value.Count);
    }

    [Test]
    public async Task getAllTransportsPendingTest()
    {
        var list = new List<Transport>();
        list.Add(await instanceTransport());
        IEnumerable<Transport> enumerable = list;

        mock.Setup(x => x.getAllTransports())
            .ReturnsAsync(() => enumerable);

        ActionResult<List<TransportReadDto>> truckResult = await transportController.getAllTransportsPending();
        Assert.AreEqual(1, truckResult.Value.Count);
        Assert.AreEqual("Amarante", truckResult.Value[0].origin);
        Assert.AreEqual("Amarante", truckResult.Value[0].destiny);
        Assert.AreEqual(2000, truckResult.Value[0].liters);
    }

    [Test]
    public async Task getAllTransportsTest()
    {
        var list = new List<Transport>();
        list.Add(await instanceTransportApproved());
        list.Add(await instanceTransport());
        IEnumerable<Transport> enumerable = list;

        mock.Setup(x => x.getAllTransports())
            .ReturnsAsync(() => enumerable);

        ActionResult<List<TransportReadDto>> truckResult = await transportController.getAllTransports();
        Assert.AreEqual(2, truckResult.Value.Count);
    }
}