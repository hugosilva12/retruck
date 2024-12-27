using WebApplication1.Global.Enumerations;

namespace WebApplication1.DTOS
{
    /// <summary>
    ///  The LoggedUser class stores information about the about the authenticated user
    /// </summary>
    public class LoggedUser : BasicDto

    {
        /// <summary>
        /// contain the name of the User
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// contain the username of the User
        /// </summary>
        public string username { get; set; }

        public Profile role { get; set; }

        /// <summary>
        /// contain the token of the User
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// contain the user's photo path
        /// </summary>
        public string photoPath { get; set; }
    }
}