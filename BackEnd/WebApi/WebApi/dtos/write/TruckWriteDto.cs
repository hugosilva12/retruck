using System.ComponentModel.DataAnnotations;
using WebApplication1.Global.Enumerations;

namespace WebApplication1.DTOS
{
    /// <summary>
    ///  The Truck class stores information to save a truck
    /// </summary>
    public class TruckWriteDto
    {
        /// <summary>
        /// contains the registration of the truck
        /// </summary>
        [Required]
        public string matricula { get; set; }

        /// <summary>
        /// contains the truck's photo path
        /// </summary>
        public string photofilename { get; set; }

        /// <summary>
        /// contains the organization of the truck
        /// </summary>
        public string organizationId { get; set; }

        /// <summary>
        /// contains the identifier of the driver
        /// </summary>
        [Required]
        public Guid driverId { get; set; }

        /// <summary>
        /// contains the category of the truck
        /// </summary>
        [Required]
        public TruckCategory truckCategory { get; set; }
    }
}