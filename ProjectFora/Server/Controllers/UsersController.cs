using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFora.Server.Data;
using ProjectFora.Server.Models;

namespace ProjectFora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(AppDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET : Current user
        [HttpGet("user")]
        public UserModel GetUser([FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.Username == user.UserName);

                if (currentUser != null)
                {
                    return currentUser;
                }
            }
            return null;
        }

        // GET : StatusCheck if user is logged in and is admin
        [HttpGet]
        [Route("check")]
        public async Task<ActionResult<UserStatusDto>> CheckUserLogin([FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                UserStatusDto userStatus = new();

                userStatus.IsLoggedIn = true;

                userStatus.IsAdmin = await _signInManager.UserManager.IsInRoleAsync(user, "Admin");

                return Ok(userStatus);
            }

            return BadRequest("User not found");
        }

        // POST : Register user to both database
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RegisterModel userToRegister)
        {
            if (_signInManager.UserManager.Users.Any(x => x.UserName == userToRegister.UserName))
            {
                return BadRequest("Username is already taken");
            }

            ApplicationUser newUser = new() { UserName = userToRegister.UserName, Email = userToRegister.Email};

            var result = await _signInManager.UserManager.CreateAsync(newUser, userToRegister.Password);

            if(result.Succeeded)
            {
                string token = GenerateToken();

                newUser.Token = token;
                await _signInManager.UserManager.UpdateAsync(newUser);

                UserModel user = new();
                user.Username = newUser.UserName;
                user.Banned = false;
                user.Deleted = false;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(token);
            }

            return BadRequest();
        }

        // POST : Login user
        [HttpPost("loginUser")]
        public async Task<ActionResult> Login(LoginModel user)
        {
            var userDb = await _signInManager.UserManager.FindByNameAsync(user.Email);

            if (userDb != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(userDb, user.Password, false);

                if (signInResult.Succeeded)
                {

                    string token = GenerateToken();

                    userDb.Token = token;
                    await _signInManager.UserManager.UpdateAsync(userDb);

                    user.Token = token;

                    return Ok(token);
                }
            }

            return BadRequest("User not found");
        }

        // PUT : Change password
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] EditPasswordModel editPassword, [FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var result = await _signInManager.UserManager.ChangePasswordAsync(user, editPassword.CurrentPassword, editPassword.NewPassword);
                if(result.Succeeded)
                {
                    return Ok();
                }
            }
            return BadRequest("User not found");
        }

        // PUT : De/Activate user
        [HttpGet("userStatus")]

        public async Task<ActionResult> UpdateUser([FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);
            string message = "User not found";
            if (user != null)
            {
                var userToEdit = _context.Users.FirstOrDefault(x => x.Username == user.UserName);

                if (userToEdit != null)
                {
                    if (userToEdit.Deleted)
                    {
                        userToEdit.Deleted = false;
                        message = "Account is not active";
                    }
                    else if (!userToEdit.Deleted)
                    {
                        userToEdit.Deleted = true;
                        message = "Account is now active";
                    }

                    _context.Update(userToEdit);
                    await _context.SaveChangesAsync();
                    return Ok(message);
                }
            }

            return BadRequest(message);
        }
        // DELETE : user
        [HttpDelete]
        public async Task Delete([FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.Username == user.UserName);
               await  _signInManager.UserManager.DeleteAsync(user);
                _context.Remove(currentUser);
               await _context.SaveChangesAsync();
            }
        }
        public string GenerateToken()
        {
            string token = Guid.NewGuid().ToString();
            return token;
        }

    }
}
