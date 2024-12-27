using WebApplication1.DTOS.Read;
using WebApplication1.Global.Utils;

namespace WebApplication1.Services;

/// <summary>
/// This interface contains the transport analysis and truck selection algorithm
/// </summary>
public interface ISelectionAlgorithmService
{
    /// <summary>
    ///  This method analyzes a transport, checks if it is profitable and selects the best truck to carry it out
    /// </summary>
    /// <param name="idTransport">The <see cref="Guid"/> of transport to be analyzed</param>
    ///  <returns><see cref="ReviewTransportDto" /> with the results of the analysis</returns>
    Task<ReviewTransportDto> selectionAndAnalysisAlgorithm(Guid idTransport);

    /// <summary>
    ///  This method sorts a list of trucks by tank volume
    /// </summary>
    /// <param name="listToSort">sorting list</param>
    /// <returns>Sorted<see cref="IList{T}" /> of <see cref="TruckForReviewReadDto" /></returns>
    Task<List<TruckForReviewReadDto>> sortListTrucksByTankVolume(
        List<TruckForReviewReadDto> listToSort);


    /// <summary>
    ///  This method sorts a list of trucks by occupied volume
    /// </summary>
    /// <returns>Sorted<see cref="IList{T}" /> of <see cref="TruckForReviewReadDto" /></returns>
    Task<List<TruckForReviewReadDto>> sortListTrucksByOccupiedVolume(
        List<TruckForReviewReadDto> listToOrder);

    /// <summary>
    ///  This method sorts a list of trucks by score
    /// </summary>
    /// <param name="listToSort">sorting list</param>
    /// <returns>Sorted<see cref="IList{T}" /> of <see cref="TruckForReviewReadDto" /></returns>
    Task<List<TruckForReviewReadDto>> sortListTrucksByFitness(
        List<TruckForReviewReadDto> listToSort);

    /// <summary>
    ///  This method order by descending a list of trucks by estimated cost of transport
    /// </summary>
    /// <param name="listToSort">sorting list</param>
    /// <returns>Sorted<see cref="IList{T}" /> of <see cref="TruckForReviewReadDto" /></returns>
    Task<List<TruckForReviewReadDto>> sortListTrucksByEstimatedCostOfTransport(
        List<TruckForReviewReadDto> listToSort);

    /// <summary>
    ///  This method removes score according to the number of breakdowns
    /// </summary>
    /// <returns><see cref="IList{T}" /> of <see cref="TruckForReviewReadDto" />with the new values of each truck</returns>
    Task<List<TruckForReviewReadDto>> analyzeTruckBreakDowns(
        List<TruckForReviewReadDto> listTrucks);

    /// <summary>
    ///   This method makes all trucks unavailable for a service
    /// </summary>
    /// <returns> List of trucks with updated value </returns>
    Task<List<TruckForReviewReadDto>> disableTrucks(List<TruckForReviewReadDto> listToUpdate);


    /// <summary>
    ///  This method gets the information stored in the XML file about the truck
    /// </summary>
    /// <param name="matricula">truck registration to find </param>
    /// <returns><see cref="TruckDetails" /> with information, null if registration does not match to a truck</returns>
    Task<TruckDetailsXml> getAllTruckInformation(String matricula);


    /// <summary>
    ///  This method calculates the percentage between what is needed for transport and the capacity of the truck
    /// </summary>
    /// <param name="truckVolume">transport volume</param>
    /// <param name="transportVolume">truck maximum volume</param>
    /// <returns> Double with the percentage </returns>  
    Double getOccupiedVolumePercentage(Double transportVolume, Double truckVolume);


    /// <summary>
    /// This method gets the kms the total kilometers of the route
    /// </summary>
    /// <returns> Double with the total kms of the route</returns>
    Task<double> getTotalNumberKmsOfRoute(String origin, String destiny, String organizationAddress);


    /// <summary>
    ///  This method calculates the fuel spent for transport
    /// </summary>
    /// <returns> Double with fuel spent</returns>
    double getLitresConsumption(double kms, double consumption);


    /// <summary>
    ///  This method compares two boolean variables
    /// </summary>
    /// <returns> True if both are false, false otherwise</returns>
    Task<Boolean> isAvailableToService(Boolean isAbsent, Boolean truckIsAbsent, Boolean truckHasOnService);

    /// <summary>
    ///  This method checks if the truck is already associated with a service on that day
    /// </summary>
    /// <param name="dateTime">date</param>
    ///  <param name="idDriver">id of truck</param>
    /// <returns> True if yes, False otherwise</returns>
    Task<bool> truckAlreadyHasService(DateTime dateTime, Guid idTruck);


    /// <summary>
    ///  This method checks that the truck has absence on a date
    /// </summary>
    /// <param name="dateTime">date</param>
    ///  <param name="idDriver">id of truck</param>
    /// <returns> True if yes, False otherwise</returns>
    Task<bool> truckIsAbsent(DateTime dateTime, Guid idTruck);


    /// <summary>
    ///  This method checks that the driver has absence on a date
    /// </summary>
    /// <param name="dateTime">date</param>
    ///  <param name="idDriver">id of driver</param>
    /// <returns> True if yes, False otherwise</returns>
    Task<bool> isAbsent(DateTime dateTime, Guid idDriver);


    /// <summary>
    ///  This method calculates the amount spent on fuel and tolls
    /// </summary>
    /// <returns> Double with Amount spent </returns>
    Task<Double> calculeValueSpend(Double valueFuel, Double litresSpend, Double toll, Double kms);


    /// <summary>
    ///  This method gets the number of breakdowns of a truck in the current year
    /// </summary>
    /// <returns>Number of breakdowns of truck in the current year </returns>
    Task<int> getNumberTruckBreakDownsByTruckInTheCurrentYear(Guid idTruck);

    /// <summary>
    ///  This method analyzes the trucks for a transport (with Cistern category) and assigns a score
    /// </summary>
    /// <param name="listToAnalyze">List of trucks to analyze</param>
    /// <returns><see cref="IList{T}" /> of <see cref="TruckForReviewReadDto" />with the result of the analysis and the score assigned</returns>
    Task<List<TruckForReviewReadDto>> analyzeTrucksWithCategoryCistern(
        List<TruckForReviewReadDto> listToAnalyze,
        Boolean isCistern);


    /// <summary>
    ///  This method analyzes the trucks for a transport (with Dump, Refrigerator or Container category) and assigns a score
    /// </summary>
    /// <param name="listToAnalyze">List of trucks to analyze</param>
    /// <returns><see cref="IList{T}" /> of <see cref="TruckForReviewReadDto" />with the result of the analysis and the score assigned</returns>
    Task<List<TruckForReviewReadDto>> analyzeTrucks(List<TruckForReviewReadDto> listToAnalyze,
        Boolean isContainerOrRefrigerator);

    /// <summary>
    ///  This method checks if a date is a holiday in Portugal
    /// </summary>
    /// <returns> true if it's a holiday, false otherwise</returns>
    Task<bool> dateIsHoliday(int day, int mouth, int year);


    /// <summary>
    ///  This calculates the final value of the service by the truck
    /// </summary>
    /// <returns> <see cref="IList{T}" /> of <see cref="TruckForReviewReadDto" /> with the final values of the cost of the service by truck</returns>
    Task<List<TruckForReviewReadDto>> calculatesFinalValueByTruck(List<TruckForReviewReadDto> listTrucks,
        DateTime dateTime);

    /// <summary>
    ///  This method adjusts the volume of the tanker trucks
    /// </summary>
    /// <returns> List of trucks with updated value </returns>
    Task<ReviewTransportDto> adjustOccupiedThankVolume(List<TruckReadDto> listTruckSelect,
        List<TruckForReviewReadDto> listToUpdate, ReviewTransportDto reviewTransportDto, Double capacity);

    /// <summary>
    ///  This method adjusts the occupancy of the truck
    /// </summary>
    /// <returns> List of trucks with updated value </returns>
    Task<List<TruckForReviewReadDto>> adjustOccupiedVolume(List<TruckReadDto> listTruckSelect,
        List<TruckForReviewReadDto> listToUpdate, Double capacity);

    /// <summary>
    /// This method checks if the customer has a positive history
    /// </summary>
    /// <param name="reviewTransportDto"><see cref="ReviewTransportDto"/> to be verified</param>
    /// <returns>Boolean <see name="alertManagerForGoodHistory"/> with value True if it has a good history, variable <see name="alertManagerForGoodHistory"/> with value False if the history is negative or null </returns>
    Task<ReviewTransportDto> methodThinksTwice(ReviewTransportDto reviewTransportDto);
}