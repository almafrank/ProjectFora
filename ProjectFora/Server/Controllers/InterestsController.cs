using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFora.Server.Data;
using ProjectFora.Server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public InterestsController(AppDbContext appDbContext, SignInManager<ApplicationUser> signInManager)
        {
            _context = appDbContext;
            _signInManager = signInManager;
        }

        // GET: Interests
        [HttpGet]
        public List<InterestModel> Get()
        {
            return _context.Interests.ToList();
        }

        // GET : Specific interest
        [HttpGet("{id}")]
        public InterestModel Get([FromRoute] int id)
        {
            return _context.Interests.Include(i => i.Threads).Select(i => new InterestModel()
            {
                Id = i.Id,
                Name = i.Name,
                Threads = i.Threads.Select(t => new ThreadModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList()
            }).FirstOrDefault(x => x.Id == id);
        }

        // POST : New interest
        [HttpPost("PostInterest")]
        public async Task Post([FromBody] InterestModel interest, [FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.Username == user.UserName);

                if (currentUser != null)
                {
                    interest.User = currentUser;

                    _context.Interests.Add(interest);
                    await _context.SaveChangesAsync();
                }
            }
        }

        // PUT : Edit Interest
        [HttpPut("{id}")]
        public async Task UpdateInterest(int id, InterestModel interest)
        {
            var updateInterest = _context.Interests.Where(interest => interest.Id == id);
            _context.Update(updateInterest);
            await _context.SaveChangesAsync();
        }

        // DELETE : Interest
        [HttpDelete("{id}")]
        public void Delete([FromRoute] int id, [FromQuery] string token)
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
