using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using WebApplication1.Models;

namespace ApiTest;

public  class GlobalTestUtils
{
    public static String loginURL = "api/v1/login";
    
    public  async Task<JObject> GetTokenManager()
    {
   
        await using var application = new Application();
        var  idDriver  = await  InsertInformation.InsertData(application);
        var client = application.CreateClient();
        var userLogin = new UserLogin() { username = "manuel", password = "0000hug1" };
        var result = await client.PostAsJsonAsync(loginURL, userLogin);
        string returnValue = result.Content.ReadAsStringAsync().Result;
            
        JObject json = JObject.Parse(returnValue);
    
        return json;
    }
    
    public  async Task<JObject> GetTokenAdmin()
    {
        await using var application = new Application();
        var  idDriver  = await  InsertInformation.InsertDataAdmin(application);
        var client = application.CreateClient();
        var userLogin = new UserLogin() { username = "hugoSilva", password = "0000hug1" };
        var result = await client.PostAsJsonAsync(loginURL, userLogin);
        string returnValue = result.Content.ReadAsStringAsync().Result;
            
        JObject json = JObject.Parse(returnValue);
    
        return json;
    }
}