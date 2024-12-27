using Moq;
using WebApplication1.Controllers;
using WebApplication1.DTOS;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace ApiTest.UTest;

public class OrganizationControllerTest
{
    private readonly OrganizationControLler organizationController;
    private readonly Mock<IOrganizationRepository> mock = new Mock<IOrganizationRepository>();

    public OrganizationControllerTest()
    {
        organizationController = new OrganizationControLler(mock.Object, null);
    }

    [Test]
    public async Task getOrganizationTest()
    {
        //organizationId
        var organizationId = Guid.NewGuid();
        var organization = new Organization()
            { enable = true, vatin = 1, addresses = "Amarante", name = "Organization2" };
        organization.id = organizationId;

        //Simulation
        mock.Setup(x => x.getOrganization(organizationId))
            .ReturnsAsync(organization);

        var organizationResult = await organizationController.getOrganization(organizationId);

        Assert.AreEqual(organizationId, organizationResult.Value.id);
        Assert.AreEqual(true, organizationResult.Value.enable);
    }

    [Test]
    public async Task createInvalidOrganizationTest()
    {
        var organizationResult = await organizationController.addOrganization(null);

        Assert.IsNull(organizationResult.Value);
    }

    [Test]
    public async Task updateInvalidOrganizationTest()
    {
        var organizationResult = await organizationController.updateOrganization(new Guid(), null);

        //Assert
        Assert.IsFalse(organizationResult.Value);
    }

    [Test]
    public async Task updateOrganizationTest()
    {
        var organization = new OrganizationWriteDto()
            { enable = true, vatin = 1, addresses = "Amarante", name = "Organization2" };


        var organizationResult = await organizationController.updateOrganization(new Guid(), organization);

        //Assert
        Assert.IsFalse(organizationResult.Value);
    }
}