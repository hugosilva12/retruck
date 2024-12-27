using AutoMapper;
using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication1.DTOS;
using WebApplication1.DTOS.Global;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers;

/// <summary>
/// This controller manage transports.
/// </summary>
[Route("api/v1/transport")]
[ApiController]
public class TransportController : Controller
{
    private readonly ITransportRepository repository;
    private IMapper mapper;
    private IFirebaseClient client;

    /// <summary>
    /// This constructor inject the Transport repository to be use by the Transport controller.
    /// </summary>
    /// <param name="repository"> transport repository </param>
    /// <param name="mapper"></param>
    public TransportController(ITransportRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    /// <summary>
    /// This endpoint adds a new transport in sql database.
    /// </summary>
    /// <param name="createTransportWriteDto">to be added</param>
    /// <returns>The added <see cref="Transport"/>, exception if the data matches an already existing transport</returns>
    [HttpPost]
    [Authorize(Roles = "CLIENT")]
    public async Task<ActionResult<Transport>> addTransport([FromBody] TransportWriteDto createTransportWriteDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (createTransportWriteDto == null)
                return BadRequest();

            var transport = await repository.addTransport(createTransportWriteDto);
            var transportReadDto = mapper.Map<TransportReadDto>(transport);
            return Ok(transportReadDto);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// This endpoint gets all transports from firebase database and insert them into the sql database.
    /// </summary>
    /// <returns> List with <see cref="TransportWriteDto"/> inserted into the database</returns>
    [Route("/transportsfirebase")]
    [HttpGet]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<List<TransportWriteDto>>> getAllTransportFromFirebase()
    {
        client = new FirebaseClient(Utils.config);
        FirebaseResponse response = client.Get("transports");
        dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
        var list = new List<TransportReadDto>();

        if (data != null)
        {
            foreach (var item in data)
            {
                var register = JsonConvert.DeserializeObject<TransportWriteDto>(((JProperty)item).Value.ToString());
                var transport = await repository.addTransport(register);
                if (transport != null)
                {
                    var transportReadDto = mapper.Map<TransportReadDto>(transport);
                    list.Add(transportReadDto);
                }
            }
        }

        return Ok(list);
    }

    /// <summary>
    /// This endpoint returns all transports.
    /// </summary>
    /// <returns> List with <see cref="TransportReadDto"/></returns>
    [HttpGet]
    [Authorize(Roles = "MANAGER")]
    public async Task<List<TransportReadDto>> getAllTransports()
    {
        var list = await repository.getAllTransports();
        var listToReturn = new List<TransportReadDto>();
        foreach (var item in list)
        {
            var transportRead = await mapperEntityToDto(item);
            listToReturn.Add(transportRead);
        }

        return listToReturn;
    }

    /// <summary>
    /// This endpoint returns all transports that not analyzed.
    /// </summary>
    /// <returns> List with <see cref="TransportReadDto"/></returns>
    [Route("pending")]
    [HttpGet]
    [Authorize(Roles = "MANAGER")]
    public async Task<List<TransportReadDto>> getAllTransportsPending()
    {
        var list = await repository.getAllTransports();
        var listToReturn = new List<TransportReadDto>();
        foreach (var item in list)
        {
            if (item.status == Status.WAIT_APROVE)
            {
                var transportRead = await mapperEntityToDto(item);
                listToReturn.Add(transportRead);
            }
        }

        return listToReturn;
    }

    /// <summary>
    ///  This method converts an entity (<see cref="Transport"/>)  into a dto (<see cref="TransportReadDto"/>).
    /// </summary>
    /// <returns> of <see cref="TruckReadDto"/> created </returns>
    private async Task<TransportReadDto> mapperEntityToDto(Transport item)
    {
        TransportReadDto transportReadDto = new TransportReadDto();
        transportReadDto.capacity = item.capacity;
        transportReadDto.status = item.status;
        transportReadDto.date = item.date.ToString("dd/MM/yyyy");
        transportReadDto.weight = item.weight;
        transportReadDto.value_offered = item.value_offered;
        transportReadDto.truckCategory = item.truck_category;
        transportReadDto.liters = item.liters;
        transportReadDto.destiny = item.destiny;
        transportReadDto.origin = item.origin;
        transportReadDto.client_id = new UserReadDto();
        transportReadDto.client_id.name = item.user_client.name;
        transportReadDto.client_id.email = item.user_client.email;
        transportReadDto.id = item.id;
        return transportReadDto;
    }

    /// <summary>
    /// This endpoint rejects a transport.
    /// </summary>
    /// <param name="id"> Transport <see cref="Guid"/> to reject</param>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "MANAGER")]
    public async Task updateTransport(Guid id)
    {
        await repository.update(id, Status.REJECTED);
    }

    /// <summary>
    /// This endpoint returns the number of transports by category.
    /// </summary>
    /// <returns><see cref="TypesOfTrucksDto"/> with the values</returns>
    [Route("transportsbycategory")]
    [HttpGet]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<TypesOfTrucksDto>> getNumberTransportsByCategory()
    {
        var transportsByCategory = new TypesOfTrucksDto();
        transportsByCategory.concrete_truck = 0;
        transportsByCategory.just_tractor = 0;
        transportsByCategory.refrigerator = 0;
        transportsByCategory.dump_truck = 0;

        var list = await repository.getAllTransports();
        foreach (var item in list)
        {
            if (item.truck_category == TruckCategory.DUMP_TRUCK)
            {
                transportsByCategory.dump_truck += 1;
            }
            else if (item.truck_category == TruckCategory.CONTAINER_TRUCK)
            {
                transportsByCategory.just_tractor += 1;
            }
            else if (item.truck_category == TruckCategory.CISTERN_TRUCK)
            {
                transportsByCategory.concrete_truck += 1;
            }
            else if (item.truck_category == TruckCategory.REFRIGERATOR_TRUCK)
            {
                transportsByCategory.refrigerator += 1;
            }
        }

        return transportsByCategory;
    }


    /// <summary>
    /// This endpoint returns the number of accepted and rejected transports.
    /// </summary>
    /// <returns><see cref="TransportsAcceptedAndRejected"/> with the values</returns>
    [Route("statistics")]
    [HttpGet]
    [Authorize(Roles = "MANAGER")]
    public async Task<TransportsAcceptedAndRejected> getNumberOfAcceptedandRejectedTransports()
    {
        TransportsAcceptedAndRejected transportsAcceptedAndRejected = new TransportsAcceptedAndRejected();
        transportsAcceptedAndRejected.accepted = 0;
        transportsAcceptedAndRejected.denied = 0;
        var list = await repository.getAllTransports();

        foreach (var item in list)
        {
            if (item.status == Status.REJECTED)
            {
                transportsAcceptedAndRejected.denied++;
            }

            if (item.status == Status.ACCEPT)
            {
                transportsAcceptedAndRejected.accepted++;
            }
        }

        return transportsAcceptedAndRejected;
    }
}