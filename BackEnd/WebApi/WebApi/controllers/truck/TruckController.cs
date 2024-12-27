using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOS;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Global.Utils;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// This controller manage trucks.
    /// </summary>
    [Route("api/v1/truck")]
    [ApiController]
    public class TruckController : Controller
    {
        private readonly ITruckRepository truckRepository;


        /// <summary>
        /// This constructor inject the truck repository to be use by the truck controller.
        /// </summary>
        /// <param name="truckRepository"> truck repository </param>
        public TruckController(ITruckRepository truckRepository)
        {
            this.truckRepository = truckRepository;
        }

        /// <summary>
        /// This endpoint adds a truck to the database.
        /// </summary>
        /// <param name="truckWriteDto">to be added</param>
        /// <returns>The added <see cref="TruckReadDto"/>, exception if registration already exists or if the driver is already associated with another truck </returns>
        [HttpPost]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<TruckReadDto>> addTruck([FromBody] TruckWriteDto createTruckDto)
        {
            if (!ModelState.IsValid || createTruckDto == null)
                return BadRequest(ModelState);

            try
            {
                var truck = await truckRepository.addTruck(createTruckDto);
                var truckReadDto = await mapperEntityToDto(truck);
                return truckReadDto;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// This endpoint returns all trucks with the active state.
        /// </summary>
        /// <returns><see cref="List{T}" /> of trucks</returns>
        [HttpGet]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<List<TruckReadDto>>> getAllTrucks()
        {
            var list = new List<TruckReadDto>();
            var listTrucks = await truckRepository.getAllTrucks();
            foreach (var item in listTrucks)
            {
                var capacity = await truckRepository.getCapacityOfTruck(item.matricula);
                TruckReadDto truckReadDto = new TruckReadDto();
                truckReadDto.year = item.year;
                truckReadDto.kms = item.kms;
                truckReadDto.matricula = item.matricula;
                truckReadDto.photoPath = item.photoPath;
                truckReadDto.truckCategory = item.truckCategory;
                truckReadDto.id = item.id;
                truckReadDto.capacity = capacity;
                list.Add(truckReadDto);
            }

            return list;
        }

        /// <summary>
        /// This endpoint returns the specific truck of the given id.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> of the truck to find</param>
        /// <returns>The chosen <see cref="TruckReadDto"/>, NotFound if the id doesn't exist</returns>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<TruckReadDto>> getTruck(Guid id)
        {
            try
            {
                var truck = await truckRepository.getTruck(id);
                if (truck == null)
                    return NotFound();

                //Convert Entity to DTO
                var truckReadDto = await mapperEntityToDto(truck);
                return truckReadDto;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// This endpoint disables the specific truck of the given id.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> of truck to disable </param>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "MANAGER")]
        public async Task removeTruck(Guid id)
        {
            await truckRepository.removeTruck(id);
        }

        /// <summary>
        /// This endpoint returns all trucks from the XML file that are not registered.
        /// </summary>
        [HttpGet]
        [Route("trucksnotregistration")]
        [Authorize(Roles = "MANAGER")]
        public async Task<List<TruckDetailsXml>> getAllTrucksWithoutRegistrationInSystem()
        {
            return await truckRepository.getAllTrucksWithoutRegistrationInSystem();
        }


        /// <summary>
        ///  This method converts an entity into a dto.
        /// </summary>
        /// <returns> of <see cref="TruckReadDto"/> created </returns>
        private async Task<TruckReadDto> mapperEntityToDto(Truck truck)
        {
            var capacity = await truckRepository.getCapacityOfTruck(truck.matricula);
            TruckReadDto truckReadDto = new TruckReadDto();
            truckReadDto.kms = truck.kms;
            truckReadDto.matricula = truck.matricula;
            truckReadDto.photoPath = truck.photoPath;
            truckReadDto.truckCategory = truck.truckCategory;
            truckReadDto.nextRevision = truck.nextRevision;
            truckReadDto.year = truck.year;
            truckReadDto.fuelConsumption = truck.fuelConsumption;
            truckReadDto.id = truck.id;
            truckReadDto.driver = new UserReadDto();
            truckReadDto.driver.name = truck.driver.name;
            truckReadDto.capacity = capacity;
            return truckReadDto;
        }

        /// <summary>
        /// This endpoint returns the number of truck by category.
        /// </summary>
        /// <returns><see cref="TypesOfTrucksDto"/> with the values</returns>
        [Route("categories")]
        [HttpGet]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<TypesOfTrucksDto>> getCategoryTypes()
        {
            var dtoToReturn = new TypesOfTrucksDto();
            dtoToReturn.concrete_truck = 0;
            dtoToReturn.just_tractor = 0;
            dtoToReturn.refrigerator = 0;
            dtoToReturn.dump_truck = 0;
            var listTrucks = await truckRepository.getAllTrucks();
            foreach (var item in listTrucks)
            {
                if (item.truckCategory == TruckCategory.DUMP_TRUCK)
                {
                    dtoToReturn.dump_truck += 1;
                }
                else if (item.truckCategory == TruckCategory.CONTAINER_TRUCK)
                {
                    dtoToReturn.just_tractor += 1;
                }
                else if (item.truckCategory == TruckCategory.CISTERN_TRUCK)
                {
                    dtoToReturn.concrete_truck += 1;
                }
                else if (item.truckCategory == TruckCategory.REFRIGERATOR_TRUCK)
                {
                    dtoToReturn.refrigerator += 1;
                }
            }

            return dtoToReturn;
        }
    }
}