namespace WebApplication1.DTOS.Read;

/// <summary>
///  The OrganizationReadDto class stores information about the Organization
/// </summary>
public class OrganizationReadDto : BasicDto
{
    /// <summary>
    /// contains the name of the Organization
    /// </summary>
    public string name { get; set; }

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