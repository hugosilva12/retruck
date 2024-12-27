using WebApplication1.DTOS;
using WebApplication1.DTOS.Read;
using WebApplication1.Global.Enumerations;
using WebApplication1.Global.Utils;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    /// <summary>
    /// This interface represents a Truck Repository that contains all the necessary methods for Truck management.
    /// </summary>
    public interface ITruckRepository
    {
        /// <summary>
        /// This method adds a <see cref="Truck"/> to the database.
        /// </summary>
        /// <param name="truckWriteDto">to be added</param>
        /// <returns>The added <see cref="Truck"/>, exception if registration already exists or if the driver is already associated with another truck </returns>
        Task<Truck> addTruck(TruckWriteDto truckWriteDto);

        /// <summary>
        /// This method removes a specific <see cref="Truck"/> by its id.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> of truck to remove </param>
        Task<Truck> removeTruck(Guid id);

        /// <summary>
        /// This method returns a specific <see cref="Truck"/> by its id.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> of the <see cref="Truck"/> to find</param>
        /// <returns>The chosen <see cref="Truck"/>, null if it doesn't exist</returns>
        Task<Truck> getTruck(Guid id);

        /// <summary>
        /// This method returns all active <see cref="Truck"/>.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="Truck" /></returns>
        Task<IEnumerable<Truck>> getAllTrucks();

        /// <summary>
        /// This method returns all trucks from the XML file that are not registered
        /// </summary>
        /// <returns><see cref="List{T}" /> of <see cref="TruckDetailsXml" /></returns>    
        Task<List<TruckDetailsXml>> getAllTrucksWithoutRegistrationInSystem();

        /// <summary>
        /// This method returns all the existing <see cref="Truck"/> by category.
        /// </summary>
        /// <returns><see cref="List{T}" /> of <see cref="TruckForReviewReadDto" /></returns>
        Task<List<TruckForReviewReadDto>> getTrucksByCategory(TruckCategory truckCategory);


        /// <summary>
        ///  This method gets the capacity of truck stored in the XML file
        /// </summary>
        /// <param name="matricula">truck registration to find </param>
        /// <returns><see cref="Double" /> with the capacity, -1  if registration does not match to a truck</returns>
        public Task<double> getCapacityOfTruck(String matricula);
    }
}