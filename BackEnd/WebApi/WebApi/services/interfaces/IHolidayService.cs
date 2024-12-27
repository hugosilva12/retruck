namespace WebApplication1.Services;

/// <summary>
/// This interface contains the methods to know if a date is a holiday
/// </summary>
public interface IHolidayService
{
    /// <summary>
    ///  This method checks if a date is a holiday in Portugal
    /// </summary>
    /// <returns> true if it's a holiday, false otherwise</returns>
    Task<bool> dateIsHoliday(int day, int mouth, int year);
}