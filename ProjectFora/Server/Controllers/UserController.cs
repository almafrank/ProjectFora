using Microsoft.AspNetCore.Mvc;
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
        public async Task<AccountUserModel> GetCurrentUser(AccountUserModel userModel)
        {
            // Skickar tillbaka nuvarande användare
            if(userModel != null)
            {
                var user = _context.Users.Where(x => x.Username == userModel.Username).FirstOrDefault();
                if(user != null)
                {
                    return user;
                }
            }
            return null; ;
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
        public async Task CreateUser(AccountUserModel user)
        {
            // Lägger till användare i AppDbContext
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // PUT 
        [HttpPut("UpdateInterest:{id}")]
        public async Task UpdateInterest(InterestModel interest)
        {
            var updateInterest = _context.Interests.Where(x => x.Id == interest.Id).FirstOrDefault();
            if(updateInterest != null)
            {
                updateInterest = interest;
                _context.Update(updateInterest);
                _context.SaveChanges();
            }
        }

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
