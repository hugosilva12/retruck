using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOS;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// This controller manage login.
    /// </summary>
    [Route("api/v1/")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// This constructor inject the user repository to be use by the login controller.
        /// </summary>
        /// <param name="userRepository"> user repository</param>
        public LoginController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// This endpoint allows the user to start session.
        /// </summary>
        /// <param name="login"></param>
        /// <returns><see cref="LoggedUser"/> with user data and token, null if login is invalid</returns>
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] UserLogin login)

        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userReturned = await userRepository.getUserLogin(login);

                if (userReturned == null)
                    return NotFound(new { message = "Invalid Username or Password" });

                var token = TokenService.generateToken(userReturned);

                LoggedUser log = new LoggedUser();
                log.role = userReturned.role;
                log.token = token;
                log.username = userReturned.username;
                log.id = userReturned.id;
                log.photoPath = userReturned.photofilename;
                log.name = userReturned.name;
                return Ok(log);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}