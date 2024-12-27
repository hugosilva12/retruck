using Newtonsoft.Json.Linq;
using WebApplication1.Global.UtilsModels;

namespace WebApplication1.Services;

/// <summary>
/// Class that lets you know if a date corresponds to a holiday
/// </summary>
public class HolidayService : IHolidayService
{
    /// <summary>
    /// To create <see cref="HttpClient"/>  instances
    /// </summary>
    private readonly IHttpClientFactory httpClientFactory;


    public HolidayService(IHttpClientFactory httpClientFactoryParam)
    {
        httpClientFactory = httpClientFactoryParam;
    }


    /// <inheritdoc/>
    public async Task<bool> dateIsHoliday(int day, int mouth, int year)
    {
        var client = httpClientFactory.CreateClient();
        var uri = new Uri(
            $"{Utils.HOLIDAYS_API}={Utils.HOLIDAYS_KEY}&country=PT&location=Porto&year={year}&month={mouth}&day={day}");

        var response = await client.GetAsync(uri);

        string returnValue = response.Content.ReadAsStringAsync().Result;
        var jsonResponse = getJsonObject(returnValue);

        ModelResponseHolidaysApi.ResponseElement responseElement = new ModelResponseHolidaysApi.ResponseElement();

        if (jsonResponse != null)
        {
            responseElement.description = jsonResponse.GetValue("description").ToString();
            responseElement.date = jsonResponse.GetValue("date").ToString();
            responseElement.date_month = jsonResponse.GetValue("date_month").ToString();
            responseElement.date_year = jsonResponse.GetValue("date_year").ToString();
            responseElement.week_day = jsonResponse.GetValue("week_day").ToString();
            return true;
        }

        return false;
    }

    /// <summary>
    ///  This method processes the Holidays API response
    /// </summary>
    /// <returns>JObject with response, null if the date is not a holiday </returns>
    public JObject getJsonObject(string response)
    {
        if (response == "" || response == null)
        {
            return null;
        }

        string[] split = response.Split("[");

        // Empty Response
        if (split[1] == "]")
        {
            return null;
        }

        string[] split2 = split[1].Split("]");
        JObject json = JObject.Parse(split2[0]);

        return json;
    }
}