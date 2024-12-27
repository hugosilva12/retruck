using Newtonsoft.Json.Linq;
using WebApplication1.DTOS.Read;

namespace WebApplication1.Services;

/// <summary>
/// This class contains the methods that interacts with external APIs and gets information about addresses  
/// </summary>
public class PositionStackService : IPositionStackService
{
    /// <summary>
    /// To create <see cref="HttpClient"/>  instances
    /// </summary>
    private readonly IHttpClientFactory httpClientFactory;

    public PositionStackService(IHttpClientFactory httpClientFactoryParam)
    {
        httpClientFactory = httpClientFactoryParam;
    }

    /// <inheritdoc/>
    public async Task<CoordReadDto> getCoordinatesByAddress(string name)
    {
        var city = name + ",Portugal";

        var client = httpClientFactory.CreateClient();
        var uri = new Uri(
            $"{Utils.POSITIONSTACK_API}={Utils.POSITIONSTACK_KEY}&query={city}&limit=1");

        var response = await client.GetAsync(uri);
        string returnValue = response.Content.ReadAsStringAsync().Result;

        CoordReadDto coordReadDto = new CoordReadDto();

        var jsonObject = getJsonObject(returnValue);
        if (jsonObject != null)
        {
            coordReadDto.lat = Convert.ToDouble(jsonObject.GetValue("latitude").ToString());
            coordReadDto.lng = Convert.ToDouble(jsonObject.GetValue("longitude").ToString());
            return coordReadDto;
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<string> getAddressByCoordinates(CoordReadDto coord)
    {
        var latitude = coord.lat.ToString();
        var latitudeFormated = latitude.Replace(",", ".");

        var longitude = coord.lng.ToString();
        var longitudeFormated = longitude.Replace(",", ".");

        var client = httpClientFactory.CreateClient();
        var uri = new Uri(
            $"{Utils.POSITIONSTACK_API_REVERSE}={Utils.POSITIONSTACK_KEY}&query={latitudeFormated},{longitudeFormated}&limit=1");

        var response = await client.GetAsync(uri);
        string returnValue = response.Content.ReadAsStringAsync().Result;

        var jsonObject = getJsonObject(returnValue);
        if (jsonObject != null)
        {
            var local = jsonObject.GetValue("label").ToString();
            return local;
        }

        return null;
    }

    /// <summary>
    ///  This method processes the PositionStack API response
    /// </summary>
    /// <returns>JObject with response, null if the date is not a holiday </returns>
    public JObject getJsonObject(string response)
    {
        if (response == null || response == "")
        {
            return null;
        }

        string[] split = response.Split("[");

        // Empty Response
        if (split[1] == "]}")
        {
            return null;
        }

        string[] split2 = split[1].Split("]");
        JObject json = JObject.Parse(split2[0]);

        return json;
    }

    /// <inheritdoc/>
    public async Task<string> getPostalCode(CoordReadDto coord)
    {
        var latitude = coord.lat.ToString();
        var latitudeFormated = latitude.Replace(",", ".");

        var longitude = coord.lng.ToString();
        var longitudeFormated = longitude.Replace(",", ".");

        var client = httpClientFactory.CreateClient();
        var uri = new Uri(
            $"{Utils.POSITIONSTACK_API_REVERSE}={Utils.POSITIONSTACK_KEY}&query={latitudeFormated},{longitudeFormated}&limit=1");

        var response = await client.GetAsync(uri);
        string returnValue = response.Content.ReadAsStringAsync().Result;

        var jsonObject = getJsonObject(returnValue);
        if (jsonObject != null)
        {
            return jsonObject.GetValue("postal_code") + "," + jsonObject.GetValue("administrative_area");
        }

        return null;
    }
}