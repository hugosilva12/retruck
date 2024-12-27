using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOS.Read;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

/// <summary>
/// This controller contains the method that analyzes the transports
/// </summary>
[Route("api/v1/review")]
[ApiController]
public class SelectionAndAnalysisAlgorithmController : Controller
{
    private ISelectionAlgorithmService service;

    /// <summary>
    /// This constructor inject the service to be use by the SelectionAndAnalysisAlgorithm controller.
    /// </summary>
    /// <param name="service"></param>
    public SelectionAndAnalysisAlgorithmController(ISelectionAlgorithmService service)
    {
        this.service = service;
    }

    /// <summary>
    /// This endpoint analyzes a transport and selects the best truck or trucks to do it.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of transport to be analyzed</param>
    /// <returns><see cref="ReviewTransportDto" /> with the results of the analysis</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReviewTransportDto>> getInformationAboutTransport(Guid id)
    {
        return await service.selectionAndAnalysisAlgorithm(id);
    }
}