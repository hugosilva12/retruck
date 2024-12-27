using WebApplication1.DTOS.Read;
using WebApplication1.Models;

namespace WebApplication1.Services;

/// <summary>
/// This interface contains all the methods needed to score trucks
/// </summary>
public interface IScoreService
{
    /// <summary>
    ///  This method assigns the score value to each truck (by tank volume)
    /// </summary>
    /// <param name="listTrucksByTankVolume"> truck list</param>
    /// <returns><see cref="IList{T}" /> of <see cref="TruckForReviewReadDto" />with the new values of each truck</returns>
    Task<List<TruckForReviewReadDto>> assignsScoreByTankVolume(
        List<TruckForReviewReadDto> listTrucksByTankVolume);

    /// <summary>
    ///  This method assigns the score value to each truck (by occupied volume)
    /// </summary>
    ///  <param name="listTrucksByOccupiedVolume"> truck list</param>
    /// <returns><see cref="IList{T}" /> of <see cref="TruckForReviewReadDto" />with the new values of each truck</returns>
    Task<List<TruckForReviewReadDto>> assignsScoreOccupiedVolume(
        List<TruckForReviewReadDto> listTrucksByOccupiedVolume);

    /// <summary>
    ///  This method assigns the score value to each truck (by amount spent on transport)
    /// </summary>
    /// <param name="listTrucksByEstimatedCostOfCisternTruck"> truck list</param>
    /// <param name="firstAvaliation"> first Avalaition </param>
    /// <returns><see cref="IList{T}" /> of <see cref="TruckForReviewReadDto" /> with the new values of each truck</returns>
    Task<List<TruckForReviewReadDto>> assignsScoreValueSpendForTruckCistern(
        List<TruckForReviewReadDto> listTrucksByEstimatedCostOfCisternTruck, Boolean isContainerOrRefrigerator,
        Boolean firstAvaliation);


    /// <summary>
    ///  This method assigns the score value to each truck (by amount spent on transport)
    /// </summary>
    /// <param name="listTrucksByEstimatedCostOfTransport"> truck list</param>
    /// <returns><see cref="IList{T}" /> of <see cref="TruckForReviewReadDto" /> with the new values of each truck</returns>
    Task<List<TruckForReviewReadDto>> assignsScoreValueSpend(
        List<TruckForReviewReadDto> listTrucksByEstimatedCostOfTransport, Boolean isContainerOrRefrigerator,
        Boolean firstAvaliation, TransportReviewParameters transportReviewParameters);
}