using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectFora.Shared;
using ProjectFora.Shared.AccountModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        private UserAccount User { get; set; } = new();
        
        public AccountsController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }
        [HttpPost("Registration")]
        public async Task<ActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (_signInManager.UserManager.Users.Any(x => x.Email == userForRegistration.Email))
            {
                return BadRequest("Username is already taken");

            }
            var user = new IdentityUser { UserName = userForRegistration.Email, Email = userForRegistration.Email };

            await _signInManager.UserManager.CreateAsync(user, userForRegistration.Password);
            return Ok();
        }

   
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginModel user)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);

            if (signInResult.Succeeded)
            {

                string token = GenerateToken();

                user.Token = token;

                return Ok(token);
            }

            return BadRequest("User not found");
        }

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
