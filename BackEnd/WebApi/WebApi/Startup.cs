using System.Text;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApplication1.DTOS;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Utils;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.Repository.Implementations;
using WebApplication1.Repository.Interfaces;
using WebApplication1.Services;

namespace WebApplication1.Context;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc(option => option.EnableEndpointRouting = false);

        //Importante
        services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
        services.AddControllers();

        services.AddHttpClient();

        services.AddEndpointsApiExplorer();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("DBConnection"));
            //options.UseSqlServer(Configuration.GetConnectionString("DBConnection"));
        }, ServiceLifetime.Transient);

        //JSON ENUMS
        services
            .AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        //JWT AUTH
        var key = Encoding.ASCII.GetBytes(Utils.SECRET);

        services.AddAuthentication(configOptions =>
            {
                configOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                configOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.RequireHttpsMetadata = false;
                jwtOptions.SaveToken = true;
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        //Mapping
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<OrganizationWriteDto, Organization>();
            cfg.CreateMap<Organization, OrganizationReadDto>();
            cfg.CreateMap<RegisterUserDto, User>();
            cfg.CreateMap<User, UserReadDto>();
            cfg.CreateMap<TransportWriteDto, Transport>();
            cfg.CreateMap<Transport, TransportReadDto>();
            cfg.CreateMap<TransportReviewParameters, TransportReviewParametersDto>();
            cfg.CreateMap<TransportReviewParametersDto, TransportReviewParameters>();
        });

        IMapper mapper = config.CreateMapper();
        services.AddSingleton(mapper);

        //Services 
        services.AddScoped<IPositionStackService, PositionStackService>();
        services.AddScoped<IHolidayService, HolidayService>();
        services.AddScoped<IDistanceService, DistanceService>();
        services.AddScoped<ISelectionAlgorithmService, SelectionAlgorithmService>();
        services.AddScoped<IScoreService, ScoreService>();

        //Repositories 
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILicenseRepository, LicenseRepository>();
        services.AddScoped<IPathPhotoRepository, PathPhotoRepository>();
        services.AddScoped<IAbsenceRepository, AbsenceRepository>();
        services.AddScoped<ITruckRepository, TruckRepository>();
        services.AddScoped<ITruckBreakDownsRepository, TruckBreakDownsRepository>();
        services.AddScoped<ITransportRepository, TransportRepository>();
        services.AddScoped<ITransportReviewParametersRepository, TransportReviewParametersRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IServiceCoordRepository, ServiceCoordRepository>();

        //Support Cycles Json
        services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        //Swagger Configs With Generation Doc
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "8180378", Version = "v1" });
            //Comment to ApiTest
            var xmlFile = $"Doc.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "8180378"));
        }

        app.UseRouting();

        app.UseCors(x => x
            .SetIsOriginAllowed(origin => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Authorization")
            .AllowCredentials());

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        //Comment to ApiTest, Photos
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "photos")),
            RequestPath = "/photos"
        });
    }
}