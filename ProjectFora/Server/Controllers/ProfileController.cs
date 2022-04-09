using ProjectFora.Shared;
using Microsoft.AspNetCore.Mvc;
using ProjectFora.Server.Data;

namespace ProjectFora.Server.Controllers
{
    [Route("profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        // GET all users: 
        [HttpGet("Users")]
        public async Task<List<AccountUserModel>> GetAllUsers()
        {
            return _context.Users.ToList();
          
        }

        // GET singel user: 
        [HttpGet("User:{id}")]
        public async Task<AccountUserModel?> GetUser(int id)
        {
           var user= _context.Users.Where(u => u.Id == id);
            return user.FirstOrDefault();
        }

        // POST a new user: 
        [HttpPost("newUser")]
        public async Task PostANewUser([FromBody] AccountUserModel userToAdd)
        {
            _context.Users.Add(userToAdd);
            _context.SaveChanges();
        }

        // PUT,update a user:  
        [HttpPut("updateUser:{id}")]
        public async Task UpdateUser(int id, [FromBody]AccountUserModel user)
        {
            var updateUser = _context.Users.Where(user => user.Id == id);
            _context.Update(updateUser);
            _context.SaveChanges();
        }

        // DELETE a user:
        [HttpDelete("deleteUser:{id}")]
        public async Task DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
