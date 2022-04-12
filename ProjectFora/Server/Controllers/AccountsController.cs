using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFora.Server.Data;
using ProjectFora.Server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AuthDbContext _context;
        private readonly AppDbContext _appDbContext;

        public AccountsController(SignInManager<ApplicationUser> signInManager, AuthDbContext context,AppDbContext appDbContext)
        {
            _signInManager = signInManager;
            _context = context;
            _appDbContext = appDbContext;
        }

        //Fungerar
        [HttpPost("Registration")]
        public async Task<ActionResult> RegisterUser([FromBody] Shared.RegisterModel userForRegistration)
        {

            if (_signInManager.UserManager.Users.Any(x => x.Email == userForRegistration.Email))
            {
                return BadRequest("Username is already taken");
            }

            var user = new ApplicationUser { UserName = userForRegistration.Email, Email = userForRegistration.Email };

            await _signInManager.UserManager.CreateAsync(user, userForRegistration.Password);
        
            return Ok();
        }

   //Fungerar
        [HttpPost]
        [Route("loginuser")]
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

        //Fungerar
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

        //Fungerar
        [HttpPut]
        [Route("edit")]
        public async Task<ActionResult> ChangePassword([FromQuery] string accessToken, EditPasswordModel editUser)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(editUser.Email);

            if (user != null)
            {
               await _signInManager.UserManager.ChangePasswordAsync(user, editUser.CurrentPassword, editUser.NewPassword  );

                return Ok();
            }

            return BadRequest("User not found");

        }

        // GET: 
        [HttpGet("CurrentUser")]
        public async Task<UserModel> GetCurrentUser(string email)
        {
            // Skickar tillbaka nuvarande användare
            if (email != null)
            {
                var user = _appDbContext.Users
                    .Include(u => u.UserInterests)
                    .Include(ui => ui.Interests)
                    .Include(t => t.Threads)
                    .Include(m=> m.Messages)
                    .Where(x => x.Username == email).FirstOrDefault();

                if (user != null)
                {
                    return user;
                }
            }
            return null; ;
        }

        //Fungerar
        public string GenerateToken()
        {
            string token = Guid.NewGuid().ToString();
            return token;
        }
    }
}
