namespace WebApplication1.Models;

/// <summary>
///  The Organization class stores information about the Organization
/// </summary>
public class Organization
{
    /// <summary>
    /// identifier of the Organization
    /// </summary>
    public Guid id { get; set; }

    /// <summary>
    /// contains the name of the Organization
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// contains the list of users of the Organization
    /// </summary>
    public virtual List<User> users { get; set; }

    /// <summary>
    /// contains the state of the Organization
    /// </summary>
    public bool enable { get; set; }

    /// <summary>
    /// contains the addresses of the Organization
    /// </summary>
    public string addresses { get; set; }

    /// <summary>
    /// contains the vatin of the Organization
    /// </summary>
    public int vatin { get; set; }
}