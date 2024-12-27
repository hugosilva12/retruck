using System.ComponentModel.DataAnnotations;
using WebApplication1.Global.Enumerations;

namespace WebApplication1.DTOS
{
    /// <summary>
    ///  The RegisterUserDto class stores information to save or update a user
    /// </summary>
    public class RegisterUserDto
    {
        /// <summary>
        /// contains the username of the User
        /// </summary>
        [Required]
        public string username { get; set; }

        /// <summary>
        /// contains the password of the User, has at least 5 characters
        /// </summary>
        [Required]
        [MinLength(5, ErrorMessage = "The field password must be a string type with a minimum length of 5.")]
        public string password { get; set; }

        /// <summary>
        ///  the id of the organization to which the user belongs
        /// </summary>
        [Required]
        public Guid organizationId { get; set; }

        /// <summary>
        /// contains the role of the User
        /// </summary>
        [Required]
        public Profile role { get; set; }

        /// <summary>
        /// contains the user's photo path
        /// </summary>
        public string photofilename { get; set; }

        /// <summary>
        /// contains the name of the User
        /// </summary>4
        [Required]
        public string name { get; set; }


        /// <summary>
        /// contains the driver's driving license category
        /// </summary>
        public int category { get; set; }


        /// <summary>
        /// contains the email of the User
        /// </summary>
        [Required]
        public string email { get; set; }
    }
}