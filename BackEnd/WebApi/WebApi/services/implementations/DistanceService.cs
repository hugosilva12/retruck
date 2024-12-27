using WebApplication1.Global;
using WebApplication1.Global.Utils;

namespace WebApplication1.Services
{
    /// <summary>
    /// This class contains the methods that allows get the information between two locations
    /// </summary>
    public class DistanceService : IDistanceService
    {
        /// <summary>
        /// To create <see cref="HttpClient"/>  instances
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;


        public DistanceService(IHttpClientFactory httpClientFactoryParam)
        {
            httpClientFactory = httpClientFactoryParam;
        }

        /// <inheritdoc />
        public async Task<ModelResponseDistanceMatrix> getDistanceAndEstimatedTimeWhitParams(string origin,
            string destiny)
        {
            var client = httpClientFactory.CreateClient();

            var uri = new Uri(
                $"{Utils.MATRIZ_API}?region=pt&origins={origin}&destinations={destiny}&key={Utils.GOOGLE_KEY}");

            var response = await client.GetFromJsonAsync<ModelResponseDistanceMatrix>(uri);

            return response;
        }
    }
}