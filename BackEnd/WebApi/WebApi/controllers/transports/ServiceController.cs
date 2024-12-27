using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication1.DTOS;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Global.Utils;
using WebApplication1.Global.UtilsModels;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.Repository.Interfaces;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

/// <summary>
/// This controller manage services.
/// </summary>
[Route("api/v1/service")]
[ApiController]
public class ServiceController : Controller
{
    private readonly IServiceRepository serviceTransportRepository;

    private IFirebaseClient client;

    private readonly ITransportRepository transportRepository;

    private readonly ITruckRepository truckRepository;

    private readonly IServiceCoordRepository serviceCoordRepository;

    private IPositionStackService positionStackService;

    private IDistanceService distance;


    /// <summary>
    /// This constructor inject all the necessary repositories to be use by the service controller.
    /// </summary>
    /// <param name="serviceTransportRepository">service repository</param>
    /// <param name="truckRepository"> truck repository</param>
    /// <param name="transportRepository"> transport repository</param>
    /// <param name="serviceCoordRepository"> service coordinates repository</param>
    /// <param name="positionStackService"> position stack service </param>
    public ServiceController(IServiceRepository serviceTransportRepository, ITransportRepository transportRepository,
        ITruckRepository truckRepository, IServiceCoordRepository serviceCoordRepository,
        IPositionStackService positionStackService, IDistanceService distance)
    {
        this.serviceTransportRepository = serviceTransportRepository;
        client = new FirebaseClient(Utils.config);
        this.transportRepository = transportRepository;
        this.truckRepository = truckRepository;
        this.serviceCoordRepository = serviceCoordRepository;
        this.positionStackService = positionStackService;
        this.distance = distance;
    }

    /// <summary>
    /// This endpoint adds a new service in sql database and firebase.
    /// </summary>
    /// <param name="serviceWriteDto">to be added</param>
    /// <returns>The added <see cref="Service"/>, null if the data matches an already existing service</returns>
    [HttpPost]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<ServiceWriteDto>> addService([FromBody] ServiceWriteDto serviceWriteDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var service = await serviceTransportRepository.addServiceTransport(serviceWriteDto);
            if (service != null)
            {
                await transportRepository.update(Guid.Parse(service.idTransport), Status.ACCEPT);
                var isUpload = await uploadToFirebase(service);
            }

            return Ok(service);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// This method gets all services
    /// </summary>
    /// <returns> List with all <see cref="ServiceTransportReadDto"/></returns>
    [HttpGet]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<List<ServiceTransportReadDto>>> getAllServices()
    {
        return await serviceTransportRepository.getAllServiceTransports();
    }

    /// <summary>
    /// This method inserts a new transport service into the firebase database
    /// </summary>
    /// <returns>True if the service have been inserted, False otherwise</returns>
    private async Task<bool> uploadToFirebase(ServiceTransport serviceTransport)
    {
        var transport = await transportRepository.getTransport(Guid.Parse(serviceTransport.idTransport));
        var truck = await truckRepository.getTruck(Guid.Parse(serviceTransport.idTruck));
        if (transport != null && truck != null)
        {
            ServiceModelFireBase serviceModelFire = new ServiceModelFireBase();
            serviceModelFire.idService = serviceTransport.id;
            serviceModelFire.date = transport.date.ToString("dd-MM-yyyy");
            serviceModelFire.userNameDriver = truck.driver.username;
            serviceModelFire.origin = "Origem";
            serviceModelFire.destiny = "Destino";
            serviceModelFire.status = ServiceStatus.TO_START;
            client.Push("services/", serviceModelFire);

            return true;
        }

        return false;
    }

    /// <summary>
    /// This endpoint updates the status of all services in progress.
    /// </summary>
    /// <returns>List of all updated services</returns>
    [Route("updateStateService")]
    [HttpGet]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<List<ServiceModelFireBase>>> updateStateService()
    {
        client = new FirebaseClient(Utils.config);
        FirebaseResponse response = client.Get("services");
        dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
        var list = new List<ServiceModelFireBase>();

        if (data != null)
        {
            foreach (var item in data)
            {
                var serviceModelFireBase =
                    JsonConvert.DeserializeObject<ServiceModelFireBase>(((JProperty)item).Value.ToString());

                if (serviceModelFireBase != null)
                {
                    await serviceTransportRepository.updateStateService(serviceModelFireBase.idService,
                        serviceModelFireBase.status);
                    list.Add(serviceModelFireBase);
                }
            }
        }

        return list;
    }

    /// <summary>
    /// This endpoint gets all coordinates from firebase database and insert them into the sql database.
    /// </summary>
    /// <returns>List with all coordinates inserted</returns>
    [Route("/location_services")]
    [HttpGet]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<List<ServiceCoordWriteDto>>> getAllServicesCoordFromFirebase()
    {
        client = new FirebaseClient(Utils.config);
        FirebaseResponse response = client.Get("location_services");
        dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
        var list = new List<ServiceCoord>();

        if (data != null)
        {
            foreach (var item in data)
            {
                var serviceCoordWrite =
                    JsonConvert.DeserializeObject<ServiceCoordWriteDto>(((JProperty)item).Value.ToString());
                var serviceCoordAdded = await serviceCoordRepository.addServiceCoord(serviceCoordWrite);
                if (serviceCoordAdded != null)
                {
                    list.Add(serviceCoordAdded);
                }
            }
        }

        return Ok(list);
    }

    /// <summary>
    /// This endpoint returns the specific service of the given id.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of the <see cref="ServiceTransport"/> to find</param>
    /// <returns>Returns the service <see cref="ServiceTransportReadDto"/>, null if the id does not match a registered service</returns>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<ServiceTransportReadDto>> getService(Guid id)
    {
        var serviceTransportReadDto = await serviceTransportRepository.getServiceTransport(id);
        if (serviceTransportReadDto == null)
            return null;
        
        var listToReturn = await getAllCoordinatesStaticPoints(serviceTransportReadDto);

        serviceTransportReadDto.listCoord = await getAllCoordinatesByServiceId(id, listToReturn);

        if (listToReturn[0] == null || listToReturn[1] == null || listToReturn[1] == null ||
            serviceTransportReadDto.listCoord == null)
        {
            serviceTransportReadDto.listCoord = null;
            return Ok(serviceTransportReadDto);
        }

        //Load lats
        serviceTransportReadDto.organizationAddressCoord = listToReturn[0];
        serviceTransportReadDto.initServiceAddress = listToReturn[1];
        serviceTransportReadDto.finishService = listToReturn[2];
        serviceTransportReadDto.nowLocationTruck =
            serviceTransportReadDto.listCoord[serviceTransportReadDto.listCoord.Count - 1];
        serviceTransportReadDto.currentLocation =
            await positionStackService.getAddressByCoordinates(serviceTransportReadDto.nowLocationTruck);

        var responseDistanceMatrix = await
            distance.getDistanceAndEstimatedTimeWhitParams(serviceTransportReadDto.currentLocation,
                serviceTransportReadDto.transportReadDto.destiny);
        var value = responseDistanceMatrix.rows[0].elements[0].duration.value / 60;
        var hours = value / 60;
        var minutes = value - (hours * 60);
        serviceTransportReadDto.durationToFinish = hours + "h:" + minutes + " minutos!";
        return Ok(serviceTransportReadDto);
    }

    /// <summary>
    /// This method returns all coordinates associated with a specific service.
    /// </summary>
    /// <param name="id"> id of service</param>
    /// <returns>List of coordinates </returns>
    private async Task<List<CoordReadDto>> getAllCoordinatesByServiceId(Guid id, List<CoordReadDto> listStatic)
    {
        var listOfCoordinates = await serviceCoordRepository.getAllServiceCoordByServiceId(id);
        if (listOfCoordinates.Count == 0)
            return null;

        var listOfCoordinatesToReturn = new List<CoordReadDto>();
        if (listStatic[0] != null)
            listOfCoordinatesToReturn.Add(listStatic[0]);

        if (listStatic[1] != null)
            listOfCoordinatesToReturn.Add(listStatic[1]);

        foreach (var item in listOfCoordinates)
        {
            CoordReadDto serviceCoord = new CoordReadDto();
            serviceCoord.lat = item.latitude;
            serviceCoord.lng = item.longitude;
            listOfCoordinatesToReturn.Add(serviceCoord);
        }

        return listOfCoordinatesToReturn;
    }

    /// <summary>
    /// This method gets all coordinates associated with a service.
    /// </summary>
    /// <param name="serviceTransportRead"> service for which all coordinates will be obtained</param>
    /// <returns>List of coordinates, <see cref="List"/> of  <see cref="CoordReadDto"/> </returns>
    private async Task<List<CoordReadDto>> getAllCoordinatesStaticPoints(ServiceTransportReadDto serviceTransportRead)
    {
        var coordOrganization =
            await positionStackService.getCoordinatesByAddress(serviceTransportRead.organizationAddress);

        var coordInitService =
            await positionStackService.getCoordinatesByAddress(serviceTransportRead.transportReadDto.origin);

        var coordFinishService =
            await positionStackService.getCoordinatesByAddress(serviceTransportRead.transportReadDto.destiny);

        var listToReturn = new List<CoordReadDto>();
        listToReturn.Add(coordOrganization);
        listToReturn.Add(coordInitService);
        listToReturn.Add(coordFinishService);
        return listToReturn;
    }

    /// <summary>
    /// This endpoint returns the total profit of all transports performed.
    /// </summary>
    /// <returns>Double with value</returns>
    [Route("/api/v1/service/total")]
    [HttpGet]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<Double>> getAllProfitServices()
    {
        double value = 0.0;
        var allServiceTransports = await serviceTransportRepository.getAllServiceTransports();
        foreach (var item in allServiceTransports)
        {
            value += item.profit;
        }

        return value;
    }

    /// <summary>
    /// This endpoint returns all services in progress that are close to a certain location of another transport
    /// </summary>
    /// <param name="id">The <see cref="BasicDto"/> with the ID of <see cref="Transport"/> to find</param>
    /// <returns><see cref="List"/> of  <see cref="ServiceTransportReadDto"/> with the trucks found</returns>
    [Route("/api/v1/serviceinprogress")]
    [HttpPost]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<List<ServiceTransportReadDto>>> getServiceInProgressAvailable(BasicDto basicDto)
    {
        var transport = await transportRepository.getTransport(basicDto.id);
        var allTransports = await serviceTransportRepository.getAllServiceTransports();
        var listTransportAvailable = new List<ServiceTransportReadDto>();

        if (transport != null)
        {
            foreach (var item in allTransports)
            {
                if (item.transportReadDto.truckCategory == transport.truck_category &&
                    item.status == ServiceStatus.IN_PROGRESS && transport.capacity < item.capacityAvailable)
                {
                    item.listCoord = new List<CoordReadDto>();
                    var listOfCoordinates = await serviceCoordRepository.getAllServiceCoordByServiceId(item.id);
                    foreach (var coord in listOfCoordinates)
                    {
                        CoordReadDto serviceCoord = new CoordReadDto();
                        serviceCoord.lat = coord.latitude;
                        serviceCoord.lng = coord.longitude;
                        item.listCoord.Add(serviceCoord);
                    }

                    item.nowLocationTruck =
                        item.listCoord[item.listCoord.Count - 1];

                    item.currentLocation =
                        await positionStackService.getPostalCode(item.nowLocationTruck);

                    var response =
                        await distance.getDistanceAndEstimatedTimeWhitParams(transport.origin, item.currentLocation);
                    var kms = (response.rows[0].elements[0].distance.value) / 1000;

                    item.kms = kms;

                    if (kms < 30)
                    {
                        listTransportAvailable.Add(item);
                    }
                }
            }
        }

        return listTransportAvailable;
    }

    /// <summary>
    /// This endpoint updates the state of service in progress
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of Service to Update</param>
    /// <param name="serviceWriteDto">The <see cref="ServiceWriteDto"/> with the new profit of services</param>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "MANAGER")]
    public async Task updateService(Guid id, [FromBody] ServiceWriteDto serviceWriteDto)
    {
        await serviceTransportRepository.updateProfitService(id, serviceWriteDto.profit);
        await transportRepository.update(serviceWriteDto.transportId, Status.ACCEPT);
    }
}