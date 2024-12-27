using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOS;

/// <summary>
///  The OrganizationWriteDto class stores information to save or update an organization
/// </summary>
public class OrganizationWriteDto
{
    /// <summary>
    /// contains the name of the Organization
    /// </summary>
    [Required]
    public string name { get; set; }

    /// <summary>
    /// contains the state of the Organization
    /// </summary>
    [Required]
    public bool enable { get; set; }

    /// <summary>
    /// contains the addresses of the Organization
    /// </summary>
    [Required]
    public string addresses { get; set; }

    /// <summary>
    /// contains the vatin of the Organization
    /// </summary>
    [Required]
    public int vatin { get; set; }
}