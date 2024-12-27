using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using WebApplication1.Context;
using WebApplication1.DTOS;
using WebApplication1.Global.Enumerations;
using WebApplication1.Models;

namespace ApiTest
{
    internal class InsertInformation
    {
      
        private static string urlTest = "api/v1/usertest";
        public static string organizationID = "";
        public static async Task<string> InsertData(Application application)
        {
            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var dataBase = provider.GetRequiredService<AppDbContext>())
                {
                    await dataBase.Database.EnsureCreatedAsync();
                    //Save Organization
                    var organization = new Organization()
                    {
                        id = Guid.Parse("e3394944-f38b-49e6-14a6-08da7e9f14c7"),enable = true, vatin = 1, addresses = "Amarante", name = "Organization1",
                    };
                    var organization_create = await dataBase.Organization.AddAsync(organization);
                    dataBase.SaveChanges(); 

                    organizationID = organization_create.Entity.id.ToString();
                    
                    //SAVE USER - DRIVER
                    var userInput = new RegisterUserDto { username = "manuel", password = "0000hug1", role = Profile.MANAGER,name= "Hugo",email = "hugsaf2132@gmail.com",photofilename="olamundo" , organizationId =  Guid.Parse(organization_create.Entity.id.ToString())};
                    
                    var client = application.CreateClient();
                    var result = await client.PostAsJsonAsync(urlTest, userInput);
                    string returnValue = result.Content.ReadAsStringAsync().Result;

                    JObject json = JObject.Parse(returnValue);
                    
                    return json.GetValue("id").ToString();
                  
                }
            }
        }
        
        public static async Task<string> InsertDataAdmin(Application application)
        {
            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider; 
                using (var database = provider.GetRequiredService<AppDbContext>())
                {
                    await database.Database.EnsureCreatedAsync();
                    //Save Organization
                    var organization = new Organization()
                    {
                        enable = true, vatin = 1, addresses = "Amarante", name = "Organization1",
                    };
                    var organization_create = await database.Organization.AddAsync(organization);
                    database.SaveChanges();

                    organizationID = organization_create.Entity.id.ToString();
                    
                    //SAVE USER - ADMIN
                    var userInput = new RegisterUserDto { username = "hugoSilva", password = "0000hug1", role = Profile.SUPER_ADMIN, name= "Hugo",email = "hugsaf2132@gmail.com",photofilename="olamundo" ,organizationId =  Guid.Parse(organization_create.Entity.id.ToString())};
                    
                    var client = application.CreateClient();
                    var result = await client.PostAsJsonAsync(urlTest, userInput);
                    string returnValue = result.Content.ReadAsStringAsync().Result;
    
                    JObject json = JObject.Parse(returnValue);
                   
                    return json.GetValue("id").ToString();
                }
            }
        }
        public static async Task<string> InsertDataOrganization(Application application)
        {
            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var catalogoDbContext = provider.GetRequiredService<AppDbContext>())
                {
                    await catalogoDbContext.Database.EnsureCreatedAsync();
                    //Save Organization
                    var organization = new Organization()
                    {
                        enable = true, vatin = 1, addresses = "Amarante", name = "Organization1",
                    };
                    var organization_create = await catalogoDbContext.Organization.AddAsync(organization);
                    catalogoDbContext.SaveChanges();

                    organizationID = organization_create.Entity.id.ToString();
                    return "ok";
                }
            }
        }
        public static async Task<string> InsertDataAbsence(Application application)
        {
            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var catalogoDbContext = provider.GetRequiredService<AppDbContext>())
                {
                    await catalogoDbContext.Database.EnsureCreatedAsync();
                    //Save Organization
                    var organization = new Organization()
                    {
                        enable = true, vatin = 1, addresses = "Amarante", name = "Organization1",
                    };
                    var organization_create = await catalogoDbContext.Organization.AddAsync(organization);
                    catalogoDbContext.SaveChanges();

                    organizationID = organization_create.Entity.id.ToString();
                    
                    //SAVE USER - ADMIN
                    var userInput = new RegisterUserDto { username = "hugoSilva", password = "0000hug1", role = Profile.SUPER_ADMIN, name= "Hugo",email = "hugsaf2132@gmail.com",photofilename="olamundo" ,organizationId =  Guid.Parse(organization_create.Entity.id.ToString())};
                     
                    var client = application.CreateClient();
                    var result = await client.PostAsJsonAsync(urlTest, userInput);
                    string returnValue = result.Content.ReadAsStringAsync().Result;
                    JObject json = JObject.Parse(returnValue);
                    return json.GetValue("username").ToString();
                }
            }
        }

    }
}
