using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using WebApplication1.DTOS;
using WebApplication1.Global.Enumerations;

namespace ApiTest;

public class TestAbsence : GlobalTestUtils
{
    private static String testUrl = "api/v1/absence";


    [Test]
    public async Task createAbsenceTest()
    {
        await using var application = new Application();

        var idDriver = await InsertInformation.InsertDataAbsence(application);

        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenManager();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());
        //Create Input
        var absenceWrite = new AbsenceWriteDto()
            { description = "description", id = idDriver, absenceType = AbsenceType.SICK, date = "23/2/2023" };

        //Request
        var result = await client.PostAsJsonAsync(testUrl, absenceWrite);

        string returnValue = result.Content.ReadAsStringAsync().Result;

        JObject json = JObject.Parse(returnValue);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(json.GetValue("id"));
        Assert.AreEqual("description", json.GetValue("description").ToString());
        Assert.AreEqual("SICK", json.GetValue("absence").ToString());
    }

    [Test]
    public async Task createAbsenceInvalidTest()
    {
        await using var application = new Application();

        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenAdmin();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());

        //Request
        var result = await client.PostAsJsonAsync(testUrl, "");

        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
    }


    [Test]
    public async Task createAbsenceWithoutDescriptionTest()
    {
        await using var application = new Application();

        var idDriver = await InsertInformation.InsertDataAbsence(application);

        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenManager();


        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());
        //Create Input
        var absenceWrite = new AbsenceWriteDto()
            { description = "", id = idDriver, absenceType = AbsenceType.SICK, date = "23/2/2023" };

        //Request
        var result = await client.PostAsJsonAsync(testUrl, absenceWrite);

        string returnValue = result.Content.ReadAsStringAsync().Result;

        JObject json = JObject.Parse(returnValue);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(json.GetValue("id"));
        Assert.AreEqual("Sem Descrição", json.GetValue("description").ToString());
        Assert.AreEqual("SICK", json.GetValue("absence").ToString());
    }

    [Test]
    public async Task updateAbsenceInvalidTest()
    {
        await using var application = new Application();

        var idDriver = await InsertInformation.InsertDataAbsence(application);

        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenManager();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());
        //Create Input
        var absenceWrite = new AbsenceWriteDto()
            { description = "description", id = idDriver, absenceType = AbsenceType.SICK, date = "23/2/2023" };

        //Request
        var result = await client.PostAsJsonAsync(testUrl, absenceWrite);

        string returnValue = result.Content.ReadAsStringAsync().Result;
        JObject json = JObject.Parse(returnValue);

        var resultUpdate = await client.PutAsJsonAsync(testUrl + "/" + json.GetValue("id").ToString(), "");
        Assert.AreEqual(HttpStatusCode.BadRequest, resultUpdate.StatusCode);
    }

    [Test]
    public async Task updateAbsenceWithStatusAcceptedTest()
    {
        await using var application = new Application();

        var idDriver = await InsertInformation.InsertDataAbsence(application);

        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenManager();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());
        //Create Input
        var absenceWrite = new AbsenceWriteDto()
            { description = "description", id = idDriver, absenceType = AbsenceType.SICK, date = "23/2/2023" };

        //Request
        var result = await client.PostAsJsonAsync(testUrl, absenceWrite);

        string returnValue = result.Content.ReadAsStringAsync().Result;
        JObject json = JObject.Parse(returnValue);

        var inputUpdate = new AbsenceUpdateWriteDto() { status = Status.ACCEPT };

        var resultUpdate = await client.PutAsJsonAsync(testUrl + "/" + json.GetValue("id").ToString(), inputUpdate);
        returnValue = result.Content.ReadAsStringAsync().Result;
        Assert.AreEqual(HttpStatusCode.InternalServerError, resultUpdate.StatusCode);
    }

    [Test]
    public async Task updateAbsenceTest()
    {
        await using var application = new Application();

        var idDriver = await InsertInformation.InsertDataAbsence(application);

        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenManager();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());
        //Create Input
        var absenceWrite = new AbsenceWriteDto()
            { description = "description", id = idDriver, absenceType = AbsenceType.VACATION, date = "23/2/2023" };

        //Request
        var result = await client.PostAsJsonAsync(testUrl, absenceWrite);

        string returnValue = result.Content.ReadAsStringAsync().Result;
        JObject json = JObject.Parse(returnValue);

        JObject json2 = JObject.Parse("{'status': 'ACCEPT'}");

        var resultUpdate = await client.PutAsJsonAsync(testUrl + "/" + json.GetValue("id").ToString(), json2);
        returnValue = result.Content.ReadAsStringAsync().Result;
        Assert.AreEqual(HttpStatusCode.BadRequest, resultUpdate.StatusCode);
    }
}