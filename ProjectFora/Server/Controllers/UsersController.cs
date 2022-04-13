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

        // GET : Users
        [HttpGet]
        public List<UserModel> Get([FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                return _context.Users.ToList();
            }

            return null;
        }

        // GET : Specific user
        [HttpGet("{id}")]
        public UserModel Get([FromRoute] int id, [FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);


            if (user != null)
            {
                var currentUser = _context.Users
                      .Include(u => u.UserInterests)
                     .Include(ui => ui.Interests)
                     .Include(t => t.Threads)
                     .Include(m => m.Messages)
                     .FirstOrDefault(u => u.Username == user.UserName);

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

        // POST : User
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RegisterModel userToRegister)
        {
            if (_signInManager.UserManager.Users.Any(x => x.UserName == userToRegister.Email))
            {
                return BadRequest("Username is already taken");
            }

            ApplicationUser newUser = new() { UserName = userToRegister.Email};

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

        // PUT : Edit user
        [HttpPut("{id}")]
        public async Task Put([FromRoute] int id, [FromBody] EditPasswordModel editPassword, [FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.Id == id);
                await _signInManager.UserManager.ChangePasswordAsync(user, user.PasswordHash, editPassword.NewPassword);
            }
        }
        // PUT : De/Activate user
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] UserModel userUpdated, [FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                var userToEdit = _context.Users.FirstOrDefault(x => x.Username == user.UserName);

                if (userToEdit != null)
                {
                    if (userToEdit.Deleted)
                    {
                        userToEdit.Deleted = true;
                    }
                    else if (userToEdit.Deleted = true)
                    {
                        userToEdit.Deleted = false;
                    }

                    _context.Update(userToEdit);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }

            return BadRequest("User not found");
        }
        // DELETE : user
        [HttpDelete("{id}")]
        public void Delete([FromRoute] int id, [FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.Id == id);
                _context.Remove(id);
                _context.SaveChangesAsync();
            }
        }
        public string GenerateToken()
        {
            string token = Guid.NewGuid().ToString();
            return token;
        }

    }
}
