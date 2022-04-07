using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectFora.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountsController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }
        [HttpPost("Registration")]
        public async Task RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {

            var user = new IdentityUser { UserName = userForRegistration.Email, Email = userForRegistration.Email };

            await _signInManager.UserManager.CreateAsync(user, userForRegistration.Password);

        }

        //[HttpGet("currentUser")]
        //public async Task<UserForLoginDto> GetCurrentUser()
        //{
        //   var currentUser = await _signInManager.UserManager.FindByLoginAsync
        //}

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
        public string GenerateToken()
        {
            string token = Guid.NewGuid().ToString();
            return token;
        }
    }
}
