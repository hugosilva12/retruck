using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOS.Read;

/// <summary>
///  The TruckBreakDownsReadDto class stores information about the truck breakdown
/// </summary>
public class TruckBreakDownsReadDto : BasicDto
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
    /// contains the of truckDto that broke down
    /// </summary>
    [Required]
    public TruckReadDto truckReadDto { get; set; }

    /// <summary>
    /// contains the price of the truck breakdown
    /// </summary>
    [Required]
    public double price { get; set; }
}