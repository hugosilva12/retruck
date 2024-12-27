using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOS;

/// <summary>
///  The TruckBreakDownsWriteDto class stores information to save a truck breakdown
/// </summary>
public class TruckBreakDownsWriteDto
{
    /// <summary>
    /// contains the description of the truck breakdown
    /// </summary>
    [Required]
    public string description { get; set; }

    /// <summary>
    /// contains the date and time of the truck breakdown
    /// </summary>
    [Required]
    public string date { get; set; }

    /// <summary>
    /// contains the id of truck that broke down
    /// </summary>
    public Guid truckId { get; set; }

    /// <summary>
    /// contains the price of the truck breakdown
    /// </summary>
    [Required]
    public double price { get; set; }
}