using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFora.Server.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // GET: 
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserModel>> GetCurrentUser(string email)
        {
            //var currentUser = _context.Users.FirstOrDefault(x => x.Username == email);
            // Skickar tillbaka nuvarande användare
            if(email != null)
            {
                var user = _context.Users
                    .Include(u => u.UserInterests)
                    .Include(ui=> ui.Interests)
                    .Where(x => x.Username == email).FirstOrDefault();

                if(user != null)
                {
                    return Ok(user);
                }
            }
            return BadRequest("Fel här");
        }

        // GET :
        [HttpGet("GetInterest:{id}")]
        public async Task<InterestModel> GetInterest(int id)
        {
            var interest = _context.Interests.Where(u => u.Id == id).FirstOrDefault();
            if(interest != null)
            {
                return interest;
            }
            return null;
        }

        // POST
        [HttpPost]
        [Route("createUser")]
        public async Task CreateUser(UserModel user)
        {
            // Lägger till användare i AppDbContext
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // GET: get users all interest
        [HttpGet("GetInterests")]

        public async Task<List<UserInterestModel>> GetUserInterest()
        {
            var result = _context.UserInterests.Where(x => x.User.Username == "alma@gmail.com").ToList();

            return result;

        }

        // PUT 
        //[HttpPut("UpdateInterest")]
        //public async Task UpdateInterest(List<InterestModel> interests, AccountUserModel user)
        //{
        //    var userToAddInterest = _context.Users.Where(x => x.Username == user.Username).FirstOrDefault();

        //    if(userToAddInterest != null)
        //    {
        //        userToAddInterest = user;
        //        _context.Update(userToAddInterest);
        //        _context.SaveChanges();
        //    }
        //}

        // DELETE 
        [HttpDelete("DeleteInterest:{id}")]
        public async Task DeleteInterest(int id)
        {
            var interest = _context.Interests.FirstOrDefault(x => x.Id == id);
            if (interest != null)
            {
                _context.Interests.Remove(interest);
                _context.SaveChanges();
            }
        }
    }
}
