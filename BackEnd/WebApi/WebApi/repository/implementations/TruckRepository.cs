using System.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Context.Global;
using WebApplication1.DTOS;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Global.Utils;
using WebApplication1.Models;

namespace WebApplication1.Repository.Implementations
{
    /// <summary>
    /// This class represents a Truck Repository that contains all the necessary methods for truck management.
    /// </summary>
    public class TruckRepository : ITruckRepository
    {
        /// <summary>
        /// The DbContext instance that represents a session with the database that can be used to query, insert, update and drop
        /// </summary>
        private readonly AppDbContext context;


        /// <summary>
        /// Constructor method for the Truck repository.
        /// </summary>
        /// <param name="databaseContext"> <see cref="DatabaseContext"/> of database </param>
        public TruckRepository(AppDbContext databaseContext)
        {
            context = databaseContext;
        }

        /// <inheritdoc/>
        public async Task<Truck> addTruck(TruckWriteDto truckWriteDto)
        {
            //Get XML Information with the truck
            var truckDetails = await getTruckInformation(truckWriteDto.matricula);

            if (truckDetails == null)
                throw new InvalidOperationException(
                    $"Truck with registration \"{truckWriteDto.matricula}\" does not exist");

            //Verify if exists (matricula)
            var truckAux =
                await context.Truck.FirstOrDefaultAsync(truckDetails =>
                    truckDetails.matricula == truckWriteDto.matricula);

            if (truckAux != null && truckAux.status != State.DISABLE)
                throw new InvalidOperationException(
                    $"Truck with registration \"{truckWriteDto.matricula}\" already exists");

            var allTrucks = context.Truck.FromSqlInterpolated($"Select * from truck  where status  = 0");
            var driverAlreadyExists = Utils.isAvailableDriver(allTrucks, truckWriteDto.driverId);

            //Dto to Entity
            Truck truck = await mapperDtoToEntity(truckWriteDto, truckDetails);
            truck.driver = await context.User.FirstOrDefaultAsync(user => user.id == truckWriteDto.driverId);

            //Verify FKS
            if (truck.driver is null)
                throw new InvalidOperationException($"Driver with id \"{truckWriteDto.driverId}\" does not exist");

            if (truck.driver.role != Profile.DRIVER)
                throw new InvalidOperationException($"User is not a driver");

            //Verify if exists (driver)
            if (driverAlreadyExists != true)
                throw new InvalidOperationException(
                    $"Driver already has an associated truck");

            //Get Photo and Save Truck
            truck.photoPath = await getPathPhoto(truck.photoPath);
            var truckToAdd = await context.Truck.AddAsync(truck);

            context.SaveChanges();

            return truckToAdd.Entity;
        }

        /// <inheritdoc/>
        public async Task<Truck> removeTruck(Guid id)
        {
            //Get Truck For Delete
            var truck = await getTruck(id);

            if (truck is null)
                throw new InvalidOperationException($"Truck with id \"{id}\" does not exist");

            truck.status = State.DISABLE;

            await context.SaveChangesAsync();
            return truck;
        }

        /// <summary>
        /// This method returns the information about the truck that is present in the XML file.
        /// </summary>
        /// <returns> of <see cref="TruckDetailsXml"/>with truck information, null if the registration number corresponds to a non-existent truck </returns>
        private async Task<TruckDetailsXml> getTruckInformation(String id)
        {
            string xmlfilepath = Path.Combine(Directory.GetCurrentDirectory(), "XMLFiles", "TrucksXML.xml");
            var xmlString = File.ReadAllText(xmlfilepath);
            var stringReader = new StringReader(xmlString);
            var dsSet = new DataSet();
            dsSet.ReadXml(stringReader);

            for (int i = 0; i < dsSet.Tables[0].Rows.Count; i++)
            {
                if (dsSet.Tables[0].Rows[i][0].ToString().Equals(id))
                {
                    return new TruckDetailsXml
                    {
                        fuelConsumption = Convert.ToInt32(dsSet.Tables[0].Rows[i][1]),
                        matricula = dsSet.Tables[0].Rows[i][0].ToString(),
                        kms = Convert.ToInt32(dsSet.Tables[0].Rows[i][2]),
                        nextRevision = Convert.ToInt32(dsSet.Tables[0].Rows[i][3]),
                        year = Convert.ToInt32(dsSet.Tables[0].Rows[i][4]),
                    };
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<Truck> getTruck(Guid id)
        {
            var truck = await context.Truck.FirstOrDefaultAsync(truck => truck.id == id);
            truck.driver.organization = null;
            return truck != null ? truck : throw new InvalidOperationException($"Truck whith \"{id}\" not exists");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Truck>> getAllTrucks()
        {
            var allTrucks = context.Truck.FromSqlInterpolated($"Select * from truck  where status  = 0");
            return allTrucks;
        }

        /// <summary>
        ///  Get the <see cref="PathPhoto"/> of a truck.
        /// </summary>
        /// <param name="photo"> basic url</param>
        /// <returns><see cref="string"/> with the photo 
        private async Task<string> getPathPhoto(string photo)
        {
            //Load path photo
            var lastPhotoInsert = await context.PathPhoto
                .FromSqlInterpolated($"select TOP 1 * from pathphoto p ORDER BY number DESC").FirstOrDefaultAsync();

            if (lastPhotoInsert != null && lastPhotoInsert.type == "T")
            {
                photo += lastPhotoInsert.name;
            }
            else
            {
                photo += "no.jpg";
            }

            return photo;
        }

        /// <summary>
        ///  This method converts a dto into a database entity
        /// </summary>
        /// <returns> <see cref="Truck"/> created </returns>
        public async Task<Truck> mapperDtoToEntity(TruckWriteDto truckWriteDto, TruckDetailsXml truckDetails)
        {
            if (truckWriteDto == null)
                throw new InvalidOperationException($"TruckDto is invalid");

            Truck truck = new Truck();
            truck.year = truckDetails.year;
            truck.matricula = truckWriteDto.matricula;
            truck.photoPath = truckWriteDto.photofilename;
            truck.truckCategory = truckWriteDto.truckCategory;
            truck.kms = truckDetails.kms;
            truck.fuelConsumption = truckDetails.fuelConsumption;
            truck.nextRevision = truck.kms + truckDetails.nextRevision;
            truck.organization_id = truckWriteDto.organizationId;
            truck.status = State.ACTIVE;
            return truck;
        }

        /// <inheritdoc/> 
        public async Task<List<TruckDetailsXml>> getAllTrucksWithoutRegistrationInSystem()
        {
            var list = new List<TruckDetailsXml>();

            string xmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "XMLFiles", "TrucksXML.xml");
            var xmlString = File.ReadAllText(xmlFilePath);
            var stringReader = new StringReader(xmlString);
            var dsSet = new DataSet();
            dsSet.ReadXml(stringReader);

            for (int i = 0; i < dsSet.Tables[0].Rows.Count; i++)
            {
                //Verify if exists (matricula)
                var truckAux =
                    await context.Truck.FirstOrDefaultAsync(truckDetails =>
                        truckDetails.matricula == dsSet.Tables[0].Rows[i][0].ToString());


                //Not Exists
                if (truckAux == null || truckAux.status == State.DISABLE)
                    list.Add(new TruckDetailsXml
                    {
                        fuelConsumption = Convert.ToInt32(dsSet.Tables[0].Rows[i][1]),
                        matricula = dsSet.Tables[0].Rows[i][0].ToString(),
                        kms = Convert.ToInt32(dsSet.Tables[0].Rows[i][2]),
                        nextRevision = Convert.ToInt32(dsSet.Tables[0].Rows[i][3]),
                        year = Convert.ToInt32(dsSet.Tables[0].Rows[i][4]),
                        truckCategory = Utils.parseEnum<TruckCategory>(dsSet.Tables[0].Rows[i][6].ToString())
                    });
            }

            return list;
        }

        /// <inheritdoc/>
        public async Task<List<TruckForReviewReadDto>> getTrucksByCategory(TruckCategory truckCategory)
        {
            var list = new List<TruckForReviewReadDto>();
            var listTrucks = await getAllTrucks();

            foreach (var item in listTrucks)
            {
                if (truckCategory == item.truckCategory)
                {
                    TruckForReviewReadDto truckRead = new TruckForReviewReadDto();
                    truckRead.year = item.year;
                    truckRead.kms = item.kms;
                    truckRead.matricula = item.matricula;
                    truckRead.photoPath = item.photoPath;
                    truckRead.truckCategory = item.truckCategory;
                    truckRead.id = item.id;
                    truckRead.driver = new UserReadDto();
                    truckRead.driver.id = item.driver.id;
                    truckRead.driver.name = item.driver.name;
                    truckRead.fuelConsumption = item.fuelConsumption;
                    list.Add(truckRead);
                }
            }

            return list;
        }

        /// <inheritdoc/>
        public async Task<double> getCapacityOfTruck(String matricula)
        {
            string xmlfilepath = Path.Combine(Directory.GetCurrentDirectory(), "XMLFiles", "TrucksXML.xml");
            var xmlString = File.ReadAllText(xmlfilepath);
            var stringReader = new StringReader(xmlString);
            var dsSet = new DataSet();
            dsSet.ReadXml(stringReader);

            for (int i = 0; i < dsSet.Tables[0].Rows.Count; i++)
            {
                if (dsSet.Tables[0].Rows[i][0].ToString().Equals(matricula))
                {
                    return Convert.ToInt32(dsSet.Tables[0].Rows[i][7]);
                }
            }

            return -1;
        }
    }
}