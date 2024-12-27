using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserLogin
    {
        /// <summary>
        /// contains the username of the User
        /// </summary>
        [Required]
        public string username { get; set; }

        /// <summary>
        /// contains the password of the User
        /// </summary>
        [Required]
        public string password { get; set; }
    }
}