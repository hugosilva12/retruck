namespace WebApplication1.Global.UtilsModels;

public class ModelResponseHolidaysApi
{
    /// <summary>
    /// This class stores information about response of Holidays WebApi
    /// </summary>
    public class ResponseElement
    {
        /// <summary>
        /// contains the description of the response
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// contains the date of the response
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// contains the date_year of the response
        /// </summary>
        public string date_year { get; set; }

        /// <summary>
        /// contains the date_month of the response
        /// </summary>
        public string date_month { get; set; }

        /// <summary>
        /// contains the week_day of the response
        /// </summary>
        public string week_day { get; set; }
    }
}