using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using WebApplication1.Context;
using WebApplication1.DTOS;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;

namespace ApiTest;

public class testUserController : GlobalTestUtils

{
    private static String testUrl = "api/v1/usertest";


    [Test]
    public async Task createInvalidUserTest()
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
    public async Task createUserTest()
    {
        await using var application = new Application();
        using (var scope = application.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var dataBase = provider.GetRequiredService<AppDbContext>())
            {
                //Insert Organization (For Use in User)
                await dataBase.Database.EnsureCreatedAsync();
                var organization = new Organization()
                {
                    id = Guid.Parse("e3394944-f38b-49e6-14a6-08da7e9f14c7"), enable = true, vatin = 1,
                    addresses = "Amarante", name = "Organization1",
                };
                await dataBase.Organization.AddAsync(organization);
                dataBase.SaveChanges();

                var client = application.CreateClient();
                var jsonResponseLogin = await GetTokenAdmin();

                // TEST USER CONTROLLER
                var userInput = new RegisterUserDto
                {
                    username = "test1", password = "0000hug1", role = Profile.MANAGER, name = "Hugo",
                    email = "hugsaf2132@gmail.com", photofilename = "olamundo",
                    organizationId = Guid.Parse("e3394944-f38b-49e6-14a6-08da7e9f14c7")
                };

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());

                //Request Post to Insert User
                var result = await client.PostAsJsonAsync(testUrl, userInput);
                string returnValue = result.Content.ReadAsStringAsync().Result;

                JObject json = JObject.Parse(returnValue);

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                Assert.IsNotNull(json.GetValue("id"));
                Assert.IsTrue("test1".Equals(json.GetValue("username").ToString()));
                Assert.IsTrue("Hugo".Equals(json.GetValue("name").ToString()));
                Assert.IsTrue("hugsaf2132@gmail.com".Equals(json.GetValue("email").ToString()));
                Assert.IsTrue("olamundo".Equals(json.GetValue("photofilename").ToString()));
                Assert.IsTrue("".Equals(json.GetValue("password").ToString()));
            }
        }
    }

    [Test]
    public async Task createUserDriverTest()
    {
        await using var application = new Application();
        using (var scope = application.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var dataBase = provider.GetRequiredService<AppDbContext>())
            {
                //Insert Organization (For Use in User)
                await dataBase.Database.EnsureCreatedAsync();
                var organization = new Organization()
                {
                    id = Guid.Parse("e3394944-f38b-49e6-14a6-08da7e9f14c7"), enable = true, vatin = 1,
                    addresses = "Amarante", name = "Organization1",
                };
                await dataBase.Organization.AddAsync(organization);
                dataBase.SaveChanges();

                var client = application.CreateClient();
                var jsonResponseLogin = await GetTokenAdmin();

                // TEST USER CONTROLLER
                var userInput = new RegisterUserDto
                {
                    username = "test1", password = "0000hug1", role = Profile.MANAGER, name = "Hugo",
                    email = "hugsaf2132@gmail.com", photofilename = "olamundo", category = 2,
                    organizationId = Guid.Parse("e3394944-f38b-49e6-14a6-08da7e9f14c7")
                };

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());

                //Request Post to Insert User
                var result = await client.PostAsJsonAsync(testUrl, userInput);
                string returnValue = result.Content.ReadAsStringAsync().Result;
                JObject json = JObject.Parse(returnValue);
                Assert.IsNotNull(json.GetValue("id"));
                Assert.IsTrue("test1".Equals(json.GetValue("username").ToString()));
                Assert.IsTrue("Hugo".Equals(json.GetValue("name").ToString()));
                Assert.IsTrue("hugsaf2132@gmail.com".Equals(json.GetValue("email").ToString()));
                Assert.IsTrue("olamundo".Equals(json.GetValue("photofilename").ToString()));
                Assert.IsTrue("".Equals(json.GetValue("password").ToString()));
            }
        }
    }

    [Test]
    public async Task getUsersBVA01Test()
    {
        await using var application = new Application();
        var client = application.CreateClient();
        var jsonResponseLogin = await GetTokenAdmin();


        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jsonResponseLogin.GetValue("token").ToString());

        //Request
        var result = await client.GetAsync(testUrl);
        string returnValue = result.Content.ReadAsStringAsync().Result;

        Assert.AreEqual("", returnValue);
    }
}