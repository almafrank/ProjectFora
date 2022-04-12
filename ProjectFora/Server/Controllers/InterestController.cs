using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFora.Server.Data;
using ProjectFora.Server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("Interest")]
    [ApiController]
    public class InterestController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public InterestController(AppDbContext appDbContext, SignInManager<ApplicationUser> signInManager)
        {
            _context = appDbContext;
            _signInManager = signInManager;
        }

        // GET: All Interests
        [HttpGet]
        public List<InterestModel> Get()
        {
            return _context.Interests.ToList();
        }

        // GET : Specific interest
        [HttpGet("{id}")]
        public InterestModel Get([FromRoute] int id)
        {
            return _context.Interests.FirstOrDefault(x => x.Id == id);
        }

        // POST : new interest
        [HttpPost("PostInterest")]
        public async Task Post([FromBody] InterestModel interest, [FromQuery] string token)
        {
            var user = _signInManager.UserManager(u => u.Token == token)
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == 1);

            interest.User = user;

            if(interest != null)
            {
                _context.Interests.Add(interest);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();        
        }
        //Fungerar!
   

  

        
        // POST
        [HttpPost]
        public async Task CreateUser(UserModel user)
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
