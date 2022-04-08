using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectFora.Server.Models;
using ProjectFora.Shared;
using ProjectFora.Shared.AccountModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        private UserAccount User { get; set; } = new();
        
        public AccountsController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        [HttpPost("Registration")]
        public async Task<ActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            //måste kolla så att inte användaren redan finns på databasen

            if (_signInManager.UserManager.Users.Any(x => x.Email == userForRegistration.Email))
            {
                return BadRequest("Username is already taken");

            }
            var user = new ApplicationUser { UserName = userForRegistration.Email, Email = userForRegistration.Email };

            await _signInManager.UserManager.CreateAsync(user, userForRegistration.Password);
            return Ok();
        }

   
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginModel user)
        {
            var userDb = await _signInManager.UserManager.FindByEmailAsync(user.Email);

            if(userDb != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(userDb, user.Password, false);
               
                if (signInResult.Succeeded)
                {

                    string token = GenerateToken();

                    userDb.Token = token;

                    user.Token = token;

                    return Ok(token);
                }
            }

            return BadRequest("User not found");
        }
        //updateasync

        [HttpGet]
        [Route("logout")]
        public async Task Logout()
        {

          await _signInManager.SignOutAsync();
        }



        //[HttpGet("currentUser")]
        //public async Task<UserForLoginDto> GetCurrentUser()
        //{

        //}

        public string GenerateToken()
        {
            string token = Guid.NewGuid().ToString();
            return token;
        }
    }
}
