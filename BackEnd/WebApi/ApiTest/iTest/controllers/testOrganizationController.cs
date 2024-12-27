using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using WebApplication1.DTOS;

namespace ApiTest;

public class TestControllerOrganization : GlobalTestUtils
{
    private static String testUrl = "api/v1/organization";

    [Test]
    public async Task createOrganizationTest()
    {
        await using var application = new Application();

        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenAdmin();

        var organizationWrite = new OrganizationWriteDto()
            { enable = true, vatin = 1, addresses = "Amarante", name = "Organization2" };

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());

        //Request
        var result = await client.PostAsJsonAsync(testUrl, organizationWrite);
        string returnValue = result.Content.ReadAsStringAsync().Result;

        JObject json = JObject.Parse(returnValue);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(json.GetValue("id"));
        Assert.IsTrue("Organization2".Equals(json.GetValue("name").ToString()));
        Assert.IsTrue("Amarante".Equals(json.GetValue("addresses").ToString()));
        Assert.IsTrue("1".Equals(json.GetValue("vatin").ToString()));
    }

    [Test]
    public async Task createOrganizationWithSameNameTest()
    {
        await using var application = new Application();

        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenAdmin();

        var organizationWrite = new OrganizationWriteDto()
            { enable = true, vatin = 1, addresses = "Amarante", name = "Organization2" };

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());

        //Request
        var result = await client.PostAsJsonAsync(testUrl, organizationWrite);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        result = await client.PostAsJsonAsync(testUrl, organizationWrite);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
    }

    [Test]
    public async Task createOrganizationInvalidTest()
    {
        await using var application = new Application();

        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenAdmin();


        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());

        //Request
        var result = await client.PostAsJsonAsync(testUrl, " ");

        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Test]
    public async Task getOrganizationTest()
    {
        await using var application = new Application();
        await InsertInformation.InsertDataOrganization(application);
        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenAdmin();


        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());

        //Request
        var result = await client.GetAsync(testUrl);
        string returnValue = result.Content.ReadAsStringAsync().Result;

        JArray array = JArray.Parse(returnValue);
        JObject json = JObject.Parse(array[0].ToString());


        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(1, array.Count);
        Assert.AreEqual("Amarante", json.GetValue("addresses").ToString());
        Assert.AreEqual("Organization1", json.GetValue("name").ToString());
    }

    [Test]
    public async Task getOrganizationBVA01Test()
    {
        await using var application = new Application();
        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenAdmin();


        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());

        //Request
        var result = await client.GetAsync(testUrl);
        string returnValue = result.Content.ReadAsStringAsync().Result;

        JArray array = JArray.Parse(returnValue);
        Assert.AreEqual(0, array.Count);
    }
}