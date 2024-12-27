using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOS;
using WebApplication1.DTOS.Read;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers;

/// <summary>
/// This controller manage organizations.
/// </summary>
[Route("api/v1/organization")]
[ApiController]
public class OrganizationControLler : Controller
{
    private readonly IOrganizationRepository organizationRepository;

    private IMapper mapper;

    /// <summary>
    /// This constructor inject the organization repository to be use by the organization controller.
    /// </summary>
    /// <param name="organizationRepository"> organization repository</param>
    /// <param name="mapper"></param>
    public OrganizationControLler(IOrganizationRepository organizationRepository, IMapper mapper)
    {
        this.organizationRepository = organizationRepository;
        this.mapper = mapper;
    }

    /// <summary>
    /// This endpoint adds a new organization.
    /// </summary>
    /// <param name="organizationWriteDto"></param>
    /// <returns>Created <see cref="OrganizationReadDto"/>, exception if the data matches an existing organization</returns>
    [HttpPost]
    [Authorize(Roles = "SUPER_ADMIN")]
    public async Task<ActionResult<OrganizationReadDto>> addOrganization(
        [FromBody] OrganizationWriteDto organizationWriteDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (organizationWriteDto == null)
                return BadRequest();

            var organization = await organizationRepository.addOrganization(organizationWriteDto);

            if (organization == null)
                return BadRequest();

            //Convert DTO
            var organizationReadDto = mapper.Map<Organization>(organization);

            return Ok(organizationReadDto);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// This endpoint returns all organizations.
    /// </summary>
    /// <returns> List with <see cref="OrganizationReadDto"/></returns>
    [HttpGet]
    [Authorize(Roles = "SUPER_ADMIN,MANAGER")]
    public async Task<ActionResult<List<OrganizationReadDto>>> getAllOrganizations()
    {
        try
        {
            var listOrganization = new List<OrganizationReadDto>();

            var allOrganizations = await organizationRepository.getAllOrganizations();

            foreach (var item in allOrganizations)
            {
                OrganizationReadDto organizationReadDto = new OrganizationReadDto();
                organizationReadDto.name = item.name;
                organizationReadDto.id = item.id;
                organizationReadDto.vatin = item.vatin;
                organizationReadDto.addresses = item.addresses;
                organizationReadDto.enable = item.enable;
                listOrganization.Add(organizationReadDto);
            }

            return Ok(listOrganization);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// This endpoint returns the specific organization of the given id.
    /// </summary>
    /// <returns><see cref="Organization"/>, null if the <see cref="Guid"/> does not belong to an organization</returns>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "SUPER_ADMIN,MANAGER")]
    public async Task<ActionResult<Organization>> getOrganization(Guid id)
    {
        return await organizationRepository.getOrganization(id);
    }

    /// <summary>
    /// This endpoint updates the specific organization of the given id.
    /// </summary>
    /// <param name="organization"><see cref="OrganizationWriteDto"/> to be updated</param>
    /// <param name="id"> Organization <see cref="Guid"/> to edit</param>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "SUPER_ADMIN")]
    public async Task<ActionResult<Boolean>> updateOrganization(Guid id,
        [FromBody] OrganizationWriteDto organizationWriteDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(false);

        await organizationRepository.updateOrganization(id, organizationWriteDto);
        return Ok(true);
    }

    /// <summary>
    /// This endpoint disables the specific organization of the given id.
    /// </summary>
    /// <param name="id"> Organization <see cref="Guid"/> to disable</param>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "SUPER_ADMIN")]
    public async Task removeOrganization(Guid id)
    {
        await organizationRepository.removeOrganization(id);
    }
}