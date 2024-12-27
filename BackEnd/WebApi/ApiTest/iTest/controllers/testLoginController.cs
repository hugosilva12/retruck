using System.Net;
using System.Net.Http.Json;
using WebApplication1.DTOS;

namespace ApiTest
{
    public class testLoginController : GlobalTestUtils
    {
        [Test]
        public async Task testLoginValid()
        {
            await using var application = new Application();

            var client = application.CreateClient();

            await InsertInformation.InsertDataAdmin(application);

            var userLogin = new RegisterUserDto { username = "hugoSilva", password = "0000hug1" };

            var result = await client.PostAsJsonAsync("api/v1/login", userLogin);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public async Task testLoginNotFound()
        {
            await using var application = new Application();
            var client = application.CreateClient();

            var userLogin = new RegisterUserDto { username = "bia", password = "0000hug1" };

            var result = await client.PostAsJsonAsync("api/v1/login", userLogin);

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task testLoginNull()
        {
            await using var application = new Application();
            var client = application.CreateClient();

            var userLogin = new RegisterUserDto { };

            var result = await client.PostAsJsonAsync("api/v1/login", userLogin);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}