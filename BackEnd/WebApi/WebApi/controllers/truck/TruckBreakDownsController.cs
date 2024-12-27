using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOS;
using WebApplication1.DTOS.Read;
using WebApplication1.Models;
using WebApplication1.Repository.Interfaces;

namespace WebApplication1.Controllers;

/// <summary>
/// This controller manage trucks.
/// </summary>
[Route("api/v1/truckBreakDown")]
[ApiController]
public class TruckBreakDownsController : Controller
{
    private readonly ITruckBreakDownsRepository truckBreakDownsRepository;


    /// <summary>
    /// This constructor inject the Truckbreakdowns repository to be use by the user controller.
    /// </summary>
    /// <param name="truckRepository"></param>
    /// <param name="mapper"></param>
    public TruckBreakDownsController(ITruckBreakDownsRepository truckBreakDownsRepository)
    {
        this.truckBreakDownsRepository = truckBreakDownsRepository;
    }

    /// <summary>
    /// This endpoint adds a truck breakdown to the database.
    /// </summary>
    /// <param name="truckBreakDownsWriteDto">to be added</param>
    /// <returns>The added <see cref="TruckBreakDownsReadDto"/>, exception if the truck doesn't exist or already have a truck breakdown that day </returns>
    [HttpPost]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<TruckBreakDownsReadDto>> addTruckBreakDowns(
        [FromBody] TruckBreakDownsWriteDto truckBreakDownsWriteDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var truck = await mapperDtoToEntity(truckBreakDownsWriteDto);
            var truckAdded = await truckBreakDownsRepository.addTruckBreakDown(truck, truckBreakDownsWriteDto.truckId);
            var truckReadDto = await mapperEntityToDto(truckAdded);
            return Ok(truckReadDto);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// This endpoint returns all truck breakdowns.
    /// </summary>
    /// <returns><see cref="List{T}" /> of truck breakdowns</returns>
    [HttpGet]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<List<TruckBreakDownsReadDto>>> getAllTruckBreakDowns()
    {
        var list = new List<TruckBreakDownsReadDto>();
        var listTrucks = await truckBreakDownsRepository.getAllTruckBreakDowns();
        foreach (var item in listTrucks)
        {
            var truckRead = await mapperEntityToDto(item);
            list.Add(truckRead);
        }

        return list;
    }

    /// <summary>
    /// This endpoint updates the specific truck breakdown of the given id.
    /// </summary>
    /// <param name="truckBreakDownsWriteDto"><see cref="TruckBreakDownsWriteDto"/> to be updated</param>
    /// <param name="id"> TruckBreakDowns <see cref="Guid"/> to edit</param>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "MANAGER")]
    public async Task updateTruckBreakDown(Guid id, [FromBody] TruckBreakDownsWriteDto truckBreakDownsWriteDto)
    {
        var date = Utils.transformStringToData(truckBreakDownsWriteDto.date);
        await truckBreakDownsRepository.updateTruckBreakDown(id, truckBreakDownsWriteDto, date);
    }


    /// <summary>
    /// This endpoint removes the specific truck breakdown of the given id.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of truck breakdown to remove </param>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "MANAGER")]
    public async Task deleteTruckBreakDown(Guid id)
    {
        await truckBreakDownsRepository.deleteTruckBreakDown(id);
    }

    /// <summary>
    /// This endpoint returns the specific truck breakdown of the given id.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of the truck breakdown to find</param>
    /// <returns>The chosen <see cref="TruckBreakDownsReadDto"/>, NotFound if the id doesn't exist</returns>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<TruckBreakDownsReadDto>> getById(Guid id)
    {
        var item = await truckBreakDownsRepository.getTruckBreakDowns(id);
        if (item == null)
            return NotFound();

        TruckBreakDownsReadDto truckRead = await mapperEntityToDto(item);

        return truckRead;
    }

    /// <summary>
    ///  This method converts an entity (TruckBreakDowns) into a dto (TruckBreakDownsReadDto).
    /// </summary>
    /// <returns> of <see cref="TruckReadDto"/> created </returns>
    private async Task<TruckBreakDownsReadDto> mapperEntityToDto(TruckBreakDowns item)
    {
        TruckBreakDownsReadDto truckBreakDownsReadDto = new TruckBreakDownsReadDto();
        truckBreakDownsReadDto.date = item.date.ToString("dd/MM/yyyy");
        truckBreakDownsReadDto.description = item.description;
        truckBreakDownsReadDto.price = item.price;
        truckBreakDownsReadDto.id = item.id;
        truckBreakDownsReadDto.truckReadDto = new TruckReadDto();
        truckBreakDownsReadDto.truckReadDto.matricula = item.truck.matricula;
        return truckBreakDownsReadDto;
    }

    /// <summary>
    ///  This endpoint returns the total spent on breakdowns and maintenance with trucks.
    /// </summary>
    /// <returns> of <see cref="value"/> double with the total amount </returns>
    [Route("/api/v1/truckBreakDown/total")]
    [HttpGet]
    public async Task<ActionResult<Double>> getTotalSpend()
    {
        double value = 0.0;
        var list = await truckBreakDownsRepository.getAllTruckBreakDowns();
        foreach (var item in list)
        {
            value += item.price;
        }

        return value;
    }

    /// <summary>
    ///  This method converts a dto (TruckBreakDownsReadDto) into an entity (TruckBreakDowns).
    /// </summary>
    /// <returns> of <see cref="TruckReadDto"/> created </returns>
    private async Task<TruckBreakDowns> mapperDtoToEntity(TruckBreakDownsWriteDto truckBreakDownsWriteDto)
    {
        var truckBreakDowns = new TruckBreakDowns();
        truckBreakDowns.date = Utils.transformStringToData(truckBreakDownsWriteDto.date);
        truckBreakDowns.description = truckBreakDownsWriteDto.description;
        truckBreakDowns.price = truckBreakDownsWriteDto.price;

        return truckBreakDowns;
    }
}