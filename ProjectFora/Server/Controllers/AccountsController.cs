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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountsController(UserManager<IdentityUser>userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("Registration")]
        public async Task RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {

            var user = new IdentityUser { UserName = userForRegistration.Email, Email = userForRegistration.Email };

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);

        }

        //[HttpGet("currentUser")]
        //public async Task<UserForLoginDto> GetCurrentUser()
        //{
        //   var currentUser = await _signInManager.UserManager.FindByLoginAsync
        //}

        [HttpPost]
        [Route("login")]
        public async Task LoginAsync(UserForLoginDto login)
        {
            await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
          
        }

        [HttpPost]
        [Route("logout")]
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();

        }


        // POST api/<AccountsController>
        //[HttpPost("Registration")]
        //public async Task<IActionResult> RegisterUser([FromBody]UserForRegistrationDto registerModel)
        //{
        //    if (registerModel == null || !ModelState.IsValid)
        //        return BadRequest();

        //    var newUser = new IdentityUser { UserName = registerModel.Email, Email = registerModel.Email };

        //    var result = await _userManager.CreateAsync(newUser, registerModel.Password);

        //    if (!result.Succeeded)
        //    {
        //        var errors = result.Errors.Select(x => x.Description).ToList();

        //        return Ok(new RegisterResult { Successful = false, Errors = errors });

        //    }

        //    return Ok(new RegisterResult { Successful = true });



        //}


    }
}
