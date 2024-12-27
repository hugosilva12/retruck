using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

/// <summary>
///  The TruckBreakDowns class stores information about the breakdown of a truck
/// </summary>
public class TruckBreakDowns
{
    /// <summary>
    /// identifier of the truck breakdown
    /// </summary>
    [Required]
    public Guid id { get; set; }

    /// <summary>
    /// contains the description of the truck breakdown
    /// </summary>
    [Required]
    public string description { get; set; }

    /// <summary>
    /// contains the date and time of the truck breakdown
    /// </summary>
    [Required]
    public DateTime date { get; set; }

    /// <summary>
    /// contains the truck that broke down
    /// </summary>
    [Required]
    public virtual Truck truck { get; set; }

    /// <summary>
    /// contains the price of the truck breakdown
    /// </summary>
    [Required]
    public double price { get; set; }
}