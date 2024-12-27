using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.DTOS;
using WebApplication1.Models;

namespace WebApplication1.Repository.Implementations;

/// <summary>
/// This class represents an Organization Repository that contains all the necessary methods for Organization management.
/// </summary>
public class OrganizationRepository : IOrganizationRepository

{
    /// <summary>
    /// The DbContext instance that represents a session with the database that can be used to query, insert, update and drop
    /// </summary>
    private readonly AppDbContext context;

    /// <summary>
    /// Mapper to convert dto to entity
    /// </summary>
    private IMapper mapper;

    /// <summary>
    /// Constructor method for the Organization repository.
    /// </summary>
    /// <param name="databaseContext"> <see cref="DatabaseContext"/> of database </param>
    /// <param name="mapper"> <see cref="IMapper"/> </param>
    public OrganizationRepository(AppDbContext databaseContext, IMapper mapper)
    {
        context = databaseContext;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<Organization> addOrganization(OrganizationWriteDto organization)
    {
        var organizationForAdd = mapper.Map<Organization>(organization);
        var exist = await context.Organization.FirstOrDefaultAsync(organization =>
            organization.name == organizationForAdd.name);
        if (exist != null)
        {
            throw new InvalidOperationException($"Name already exists");
        }

        var organizationSaved = await context.Organization.AddAsync(organizationForAdd);
        context.SaveChanges();
        return organizationSaved.Entity;
    }

    /// <inheritdoc/>
    public async Task<Organization> removeOrganization(Guid id)
    {
        var organizationToRemove =
            await context.Organization.FirstOrDefaultAsync(organization => organization.id == id);

        if (!organizationToRemove.name.Equals("Demo"))
        {
            context.Organization.RemoveRange(organizationToRemove);
            await context.SaveChangesAsync();
            return organizationToRemove;
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<Organization> getOrganization(Guid id)
    {
        var organization = await context.Organization.FirstOrDefaultAsync(organization => organization.id == id);
        return organization != null ? organization : null;
    }

    /// <inheritdoc/>
    public async Task<Organization> updateOrganization(Guid id, OrganizationWriteDto organization)
    {
        var organizationToUpdate = await getOrganization(id);
        organizationToUpdate.name = organization.name;
        organizationToUpdate.vatin = organization.vatin;
        organizationToUpdate.addresses = organization.addresses;
        await context.SaveChangesAsync();
        return organizationToUpdate;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Organization>> getAllOrganizations()
    {
        return await context.Organization.ToListAsync();
    }
}