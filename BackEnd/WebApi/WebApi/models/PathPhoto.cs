using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public class PathPhoto
{
    /// <summary>
    /// identifier of the photo
    /// </summary>
    public Guid id { get; set; }

    /// <summary>
    /// contains the photo number (auto increment in database)
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(Order = 0)]
    public int number { get; set; }


    /// <summary>
    /// contains the name of the photo
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// contains the type of the photo User, Truck or Other
    /// </summary>
    public string type { get; set; }
}