using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;

namespace ApiTest;

public class testTransportReviewParametersController : GlobalTestUtils
{
    private static String testUrl = "api/v1/config";

    [Test]
    public async Task getParametersTest()
    {
        await using var application = new Application();
        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenManager();


        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());

        //Request
        var result = await client.GetAsync(testUrl);
        string returnValue = result.Content.ReadAsStringAsync().Result;

        JObject json = JObject.Parse(returnValue);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(0, Int32.Parse(json.GetValue("valueSaturday").ToString()));
        Assert.AreEqual(0, Int32.Parse(json.GetValue("valueSunday").ToString()));
        Assert.AreEqual(1.98, Convert.ToDouble(json.GetValue("valueFuel").ToString()));
        Assert.AreEqual(0.15, Convert.ToDouble(json.GetValue("valueToll").ToString()));
    }
    
    
    [Test]
    public async Task createParametersInvalidTest()
    {
        await using var application = new Application();
        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenManager();


        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());

        var userInput = "";
        
        var result = await client.PostAsJsonAsync(testUrl, userInput);
        
        //Request
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
    }
}