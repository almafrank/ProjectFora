using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectFora.Server.Data;
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
        private readonly AuthDbContext _context;

        private UserAccount User { get; set; } = new();
        
        public AccountsController(SignInManager<ApplicationUser> signInManager, AuthDbContext context)
        {
            _signInManager = signInManager;
            _context = context;
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
                    _context.SaveChanges();

                    user.Token = token;

                    return Ok(token);
                }
            }

            return BadRequest("User not found");
        }
        //updateasync

        [HttpGet]
        [Route("check")]
        public async Task<ActionResult<UserStatusDto>> CheckUserLogin([FromQuery] string accessToken)
        {
            // Returnera någon slags flagga som säger isLoggedIn och isAdmin

            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if(user != null)
            {
                UserStatusDto userStatus = new();

                userStatus.IsLoggedIn = true;

                userStatus.IsAdmin = await _signInManager.UserManager.IsInRoleAsync(user, "Admin");

                return Ok(userStatus);
            }

            return BadRequest("User not found");
        }

        [HttpGet]
        [Route("logout")]
        public async Task Logout()
        {

          await _signInManager.SignOutAsync();
        }

        //[HttpPost]
        //[Route("editUser")]
        //public async Task<ActionResult> EditUser(LoginModel user)
        //{
        //    var userDb = await _signInManager.UserManager.FindByEmailAsync(user.Email);

        //    if(userDb != null)
        //    {
                
        //    }

        //}

        //mAP minimal api
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
