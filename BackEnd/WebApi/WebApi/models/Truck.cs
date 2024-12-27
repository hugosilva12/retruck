using System.ComponentModel.DataAnnotations;
using WebApplication1.Context.Global;
using WebApplication1.Global.Enumerations;

namespace WebApplication1.Models
{
    /// <summary>
    ///  The Truck class stores information about the truck
    /// </summary>
    public class Truck
    {
        /// <summary>
        /// identifier of the truck
        /// </summary>
        public Guid id { get; init; }

        /// <summary>
        /// contains the driver of the truck
        /// </summary>
        [Required]
        public virtual User driver { get; set; }

        /// <summary>
        /// contains the registration of the truck
        /// </summary>
        [Required]
        public string matricula { get; set; }

        /// <summary>
        /// contains the year of the truck
        /// </summary>
        [Required]
        public int year { get; set; }

        /// <summary>
        /// contains the category of the truck
        /// </summary>
        [Required]
        public TruckCategory truckCategory { get; set; }

        /// <summary>
        /// contains the fuel consumption of the truck
        /// </summary>
        [Required]
        public int fuelConsumption { get; set; }

        /// <summary>
        /// contains the kms of the truck
        /// </summary>
        [Required]
        public int kms { get; set; }

        /// <summary>
        /// contains the kms when the next review should be done
        /// </summary>
        [Required]
        public int nextRevision { get; set; }

        /// <summary>
        /// contains the truck's photo path
        /// </summary>
        public string photoPath { get; set; }

        /// <summary>
        /// contains the organization of the truck
        /// </summary>
        [Required]
        public virtual String organization_id { get; set; }

        /// <summary>
        /// contains the status of the truck
        /// </summary>
        [Required]
        public State status { get; set; }
    }
}