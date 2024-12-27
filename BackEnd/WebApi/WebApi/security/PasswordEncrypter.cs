using Scrypt;

namespace WebApplication1.Security
{
    /// <summary>
    /// The PasswordEncrypter class contains the necessary methods for encrypting and compare passwords
    /// </summary>
    public class PasswordEncrypter
    {
        /// <summary>
        /// This method returns a encrypted password.
        /// </summary>
        /// <param name="password">Password to be encrypted</param>
        /// <returns>String with encrypted password</returns>
        public static string encryptPassword(string password)
        {
            var encoder = new ScryptEncoder();
            return encoder.Encode(password);
        }

        /// <summary>
        /// This method compares a password string with an encrypted password.
        /// </summary>
        /// <param name="password">Password not encrypted</param>
        /// <param name="passwordEncrypted">Password encrypted</param>
        /// <returns>"true" when the passwords match, "false" when the passwords do not match</returns>
        public static bool verifyPassword(string password, string passwordEncrypted)
        {
            var encoder = new ScryptEncoder();
            return encoder.Compare(password, passwordEncrypted);
        }
    }
}