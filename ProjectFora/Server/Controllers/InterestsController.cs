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
        public List<InterestModel> Get([FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                return _context.Interests.ToList();
            }

            return null;
        }

        // GET : Specific interest
        [HttpGet("{id}")]
        public InterestModel Get([FromRoute] int id, [FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var result= _context.Interests.Include(i => i.Threads).Select(i => new InterestModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Threads = i.Threads.Select(t => new ThreadModel()
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList()
                }).FirstOrDefault(x => x.Id == id);

                return result;
            }
            return null;
        }

        // POST : Create interest
        [HttpPost]
        public async Task Post([FromBody] InterestModel interest, [FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.Username == user.UserName);

                if (currentUser != null)
                {
                    InterestModel newInterest = new();

                    newInterest.User = currentUser;
                    newInterest.Name = interest.Name;

                    _context.Interests.Add(newInterest);
                    await _context.SaveChangesAsync();
                }
            }
        }

        // PUT : Edit Interest
        [HttpPut("{id}")]
        public async Task UpdateInterest(InterestModel interest, [FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var updateInterest = _context.Interests.Where(i => i.Id == interest.Id);

                _context.Update(updateInterest);
                await _context.SaveChangesAsync();
            }
        }

       
        // DELETE : Interest 
        [HttpDelete("{id}")]
        public async Task Delete([FromRoute] int id, [FromQuery] string accessToken)
        {
            // Måste kolla så att användaren har skapat intresset!

            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);
           
            if(user != null)
            {
                var interest = _context.Interests.FirstOrDefault(x => x.Id == id);

                if (interest != null)
                {
                    _context.Interests.Remove(interest);
                    await _context.SaveChangesAsync();
                }
            }
      
        }


    }
}
