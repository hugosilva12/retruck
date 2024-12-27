namespace WebApplication1.DTOS.Global;

/// <summary>
/// The UsersRegisteredInSystemDto stores information about the number of users on the system
/// </summary>
public class UsersRegisteredInSystemDto
{
    /// <summary>
    /// contains the number of registered drivers
    /// </summary>
    public int drivers { get; set; }


    /// <summary>
    /// contains the number of registered managers
    /// </summary>
    public int managers { get; set; }

    /// <summary>
    /// contains the number of registered clients
    /// </summary>
    public int clients { get; set; }

    /// <summary>
    /// contains the total of registered users
    /// </summary>
    public int total { get; set; }
}