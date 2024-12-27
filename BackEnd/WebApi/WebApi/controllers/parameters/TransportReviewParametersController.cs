using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOS;
using WebApplication1.Models;
using WebApplication1.Repository.Interfaces;

namespace WebApplication1.Controllers.Config;

/// <summary>
/// This controller manage the algorithm settings.
/// </summary>
[Route("api/v1/config")]
[ApiController]
public class TransportReviewParametersController : Controller
{
    private IMapper mapper;

    private readonly ITransportReviewParametersRepository transportReviewParametersRepository;

    public TransportReviewParametersController(ITransportReviewParametersRepository transportReviewParametersRepository,
        IMapper mapper)
    {
        this.transportReviewParametersRepository = transportReviewParametersRepository;
        this.mapper = mapper;
    }

    /// <summary>
    /// This endpoint adds a <see cref="TransportReviewParameters"/> to the database for transport review.
    /// </summary>
    /// <param name="transportReviewParameters">to be added</param>
    /// <returns> <see cref="TransportReviewParametersDto"/> with the parameters</returns>
    [HttpPost]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<TransportReviewParametersDto>> addTransportReviewParameters(
        [FromBody] TransportReviewParametersDto transportReviewParameters)
    {
        if (transportReviewParameters.id == null)
        {
            transportReviewParameters.id = new Guid();
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var transportReviewParametersForAdd = mapper.Map<TransportReviewParameters>(transportReviewParameters);

            var transportReviewParametersForAdded =
                await transportReviewParametersRepository.addTransportReviewParameters(transportReviewParametersForAdd);

            var transportReviewParametersForReturn =
                mapper.Map<TransportReviewParametersDto>(transportReviewParametersForAdded);

            return Ok(transportReviewParametersForReturn);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }


    /// <summary>
    /// This endpoint gets the parameters for transport review.
    /// </summary>
    /// <returns> <see cref="TransportReviewParametersDto"/> with the parameters</returns>
    [HttpGet]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult<TransportReviewParametersDto>> getTransportReviewParameters()
    {
        try
        {
            var transportReviewParametersForAdded =
                await transportReviewParametersRepository.getTransportReviewParameters();
            if (transportReviewParametersForAdded == null)
                NotFound();

            var transportReviewParametersForReturn =
                mapper.Map<TransportReviewParametersDto>(transportReviewParametersForAdded);

            return Ok(transportReviewParametersForReturn);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// This endpoint updates the parameters for transport review.
    /// </summary>
    /// <param name="transportReviewParameters">to be updated</param>
    /// <param name="id">The <see cref="Guid"/> of the <see cref="TransportReviewParameters"/> to update</param>
    /// <returns> Ok if the parameters have been updated </returns>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "MANAGER")]
    public async Task<ActionResult> updateTransportReviewParameters(Guid id,
        [FromBody] TransportReviewParametersDto transportReviewParameters)
    {
        // not return exception
        if (transportReviewParameters.id == null)
        {
            transportReviewParameters.id = new Guid();
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var transportReviewParametersForAdd = mapper.Map<TransportReviewParameters>(transportReviewParameters);
            await transportReviewParametersRepository.updateTransportReviewParameters(transportReviewParametersForAdd,
                id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}