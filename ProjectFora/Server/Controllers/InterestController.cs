using Microsoft.AspNetCore.Mvc;
using ProjectFora.Server.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("Interest")]
    [ApiController]
    public class InterestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InterestController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // GET: 
        [HttpGet("currentuser")]
        public async Task<ActionResult<AccountUserModel>> CurrentUser([FromQuery]string email)
        {

            var result = _context.Users.FirstOrDefault(x => x.Username == email);


            if (result != null)
            {
            

                return Ok(result);
            }

            return BadRequest("User not found");
        }
        // GET: 
        [HttpGet("GetAllInterest")]
        public async Task<List<InterestModel>> GetAllInterest()
        {
            return _context.Interests.ToList();

        }

        // GET :
        [HttpGet("GetAInterest:{id}")]
        public async Task<InterestModel> GetAInterest(int id)
        {
            var interest = _context.Interests.Where(u => u.Id == id);
            return interest.FirstOrDefault();
        }

        
        // POST
        [HttpPost]
        public async Task CreateUser(AccountUserModel user)
        {
            // Lägger till användare i AppDbContext
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // PUT 
        [HttpPut("UpdateAInterest:{id}")]
        public async Task UpdateInterest(int id,InterestModel interest)
        {
            var updateInterest = _context.Interests.Where(interest =>interest.Id == id);
            _context.Update(updateInterest);
            _context.SaveChanges();
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
