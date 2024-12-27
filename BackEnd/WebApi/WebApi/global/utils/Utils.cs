using FireSharp.Config;
using FireSharp.Interfaces;
using WebApplication1.DTOS.Read;
using WebApplication1.Models;

namespace WebApplication1;

public static class Utils
{
    //JsonWebToken
    public const string SECRET = "sswsasas243fsrz234432019092108298uedjxz11";
    public const int TOKEN_DURATION = 30;

    //GOOGLE API URL
    public static string MATRIZ_API = "https://maps.googleapis.com/maps/api/distancematrix/json";

    //GOOGLE API KEY
    public static string GOOGLE_KEY = "AIzaSyBXd0yjOuUk90C9suz2wQB03g2c5f4qXO8";

    //HOLIDAYS 
    public static string HOLIDAYS_API = "https://holidays.abstractapi.com/v1/?api_key";

    //HOLIDAYS API KEY
    public static string HOLIDAYS_KEY = "4997ade96ab44cc0aaf84a2a736dea24";

    //POSITIONSTACK
    public static string POSITIONSTACK_API = "http://api.positionstack.com/v1/forward?access_key";

    //POSITIONSTACK
    public static string POSITIONSTACK_API_REVERSE = "http://api.positionstack.com/v1/reverse?access_key";

    //POSITIONSTACK KEY
    public static string POSITIONSTACK_KEY = "93484ed375cbacc09380ef8f47de122d";

    public static string MSG_EXCELLENT_OCCUPATION = "Taxa de Ocupação - Excelente";

    public static string MSG_VERY_GOOD_OCCUPATION = "Taxa de Ocupação - Muito boa";

    public static string MSG_GOOD_OCCUPATION = "Taxa de Ocupação - Boa";

    public static string MSG_MEDIUM_OCCUPATION = "Taxa de Ocupação - Razoavel";

    public static string MSG_BAD_OCCUPATION = "Taxa de Ocupação - Má";

    public static string MSG_ERROR = "Taxa de Ocupação - Impossível";

    public static string MSG_1_COST = "Custo - Camião que gasta menos dinheiro!";

    public static string MSG_2_COST = "Custo - Segundo camião que gasta menos dinheiro!";

    public static string MSG_3_COST = "Custo - Terceiro camião que gasta menos dinheiro!";

    public static string MSG_4_COST = "Custo - Quarto camião que gasta menos dinheiro!";

    public static string MSG_5_COST = "Custo - Não é dos mais economicos";

    public static IFirebaseConfig config = new FirebaseConfig
    {
        AuthSecret = "WXijAAS22aEhQic1XGIZhHgKslMtdiEFjD12K3oy",
        BasePath = "https://testapsnet-7b02e-default-rtdb.firebaseio.com"
    };

    /// <summary>
    /// This method transforms a string into a date
    /// </summary>
    /// <returns> <see cref="DateTime"/>, exception if the string is invalid</returns>
    public static DateTime transformStringToData(String date)
    {
        if (date == null || date == "")
            throw new InvalidOperationException($"Date Invalid");

        string[] dates = date.Split('-');

        if (dates.Length != 3)
            throw new InvalidOperationException($"Date Invalid");

        int day, year, month;

        Int32.TryParse(dates[0], out day);
        Int32.TryParse(dates[1], out month);
        Int32.TryParse(dates[2], out year);

        DateTime dateTime = new DateTime(year, month, day);
        return dateTime;
    }

    /// <summary>
    /// This method transforms a string into a date
    /// </summary>
    /// <returns> <see cref="DateTime"/>, exception if the string is invalid</returns>
    public static DateTime transformStringToData2(String date)
    {
        if (date == null || date == "")
            throw new InvalidOperationException($"Date Invalid");

        string[] dates = date.Split('/');

        if (dates.Length != 3)
            throw new InvalidOperationException($"Date Invalid");

        int day, year, month;

        Int32.TryParse(dates[0], out day);
        Int32.TryParse(dates[1], out month);
        Int32.TryParse(dates[2], out year);

        DateTime dateTime = new DateTime(year, month, day);
        return dateTime;
    }

    /// <summary>
    ///  This method converts an entity into a dto
    /// </summary>
    /// <returns> of <see cref="UserReadDto"/> created </returns>
    public static UserReadDto entityToDtoUser(User manager)
    {
        UserReadDto toReturn = new UserReadDto();
        toReturn.name = manager.name;
        toReturn.id = manager.id;
        toReturn.email = manager.email;
        toReturn.username = manager.username;
        toReturn.photofilename = manager.photofilename;
        toReturn.role = manager.role;
        toReturn.userState = manager.userState;
        return toReturn;
    }

    /// <summary>
    /// This method checks if the driver is already associated with a truck
    /// </summary>
    /// <returns> true if not, false otherwise </returns>
    public static Boolean isAvailableDriver(IQueryable<Truck> allTrucks, Guid guid)
    {
        foreach (var truck in allTrucks)
        {
            if (truck.driver.id.Equals(guid))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// This method convert a string to an enumeration value
    /// </summary>
    public static T parseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static int KmToMeters = 1000;

    public static int kmsConsumption = 100;
}