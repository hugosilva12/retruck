using WebApplication1.Models;

namespace WebApplication1.Repository.Interfaces;

/// <summary>
/// This interface represents a TransportReviewParameters Repository that contains all the necessary methods to save and change parameters to analyze a transport
/// </summary>
public interface ITransportReviewParametersRepository
{
    /// <summary>
    /// This endpoint adds a <see cref="TransportReviewParameters"/> to the database for transport review.
    /// </summary>
    /// <param name="transportReviewParameters">to be added</param>
    /// <returns>The added <see cref="TransportReviewParameters"/></returns>
    Task<TransportReviewParameters> addTransportReviewParameters(TransportReviewParameters transportReviewParameters);

    /// <summary>
    /// This endpoint returns the parameters for transport review
    /// </summary>
    Task<TransportReviewParameters> getTransportReviewParameters();

    /// <summary>
    /// This endpoint updates the parameters for transport review
    /// </summary>
    /// <param name="transportReviewParameters">to be updated</param>
    /// <param name="id">The <see cref="Guid"/> of the <see cref="TransportReviewParameters"/> to update</param>
    Task updateTransportReviewParameters(TransportReviewParameters transportReviewParameters,
        Guid idTransportReviewParameters);
}