using WebApplication1.Context.Global;
using WebApplication1.Global.Enumerations;

namespace WebApplication1.Models;

/// <summary>
///  The User class stores information about the User
/// </summary>
public class User
{
    /// <summary>
    ///  identifier of the user
    /// </summary>
    public Guid id { get; set; }

    /// <summary>
    ///  contains the user organization
    /// </summary>
    public virtual Organization organization { get; set; }

    /// <summary>
    /// contains the username of the User
    /// </summary>
    public string username { get; set; }

    /// <summary>
    /// contains the name of the User
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// contains the email of the User
    /// </summary>
    public string email { get; set; }

    /// <summary>
    /// contains the password of the User
    /// </summary>
    public string password { get; set; }

    /// <summary>
    /// contains the role of the User
    /// </summary>
    public Profile role { get; set; }

    /// <summary>
    /// contains the user's photo path
    /// </summary>
    public string photofilename { get; set; }

    /// <summary>
    /// contains the user state
    /// </summary>
    public State userState { get; set; }
}