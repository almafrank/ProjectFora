using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectFora.Server.Data;
using ProjectFora.Server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ThreadsController(AppDbContext appDbContext, SignInManager<ApplicationUser> signInManager)
        {
            _context = appDbContext;
            _signInManager = signInManager;
        }

        // GET : Threads
        [HttpGet]
        public List<ThreadModel> Get([FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                return _context.Threads.ToList();
            }

            return null;
        }

        // GET : Specific thread:
        [HttpGet("{id}")]
        public ThreadModel GetAThread([FromRoute] int id, [FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                return _context.Threads.FirstOrDefault(t => t.Id == id);

            }

            return null;
        }

        // POST : Thread
        [HttpPost]
        public async Task Post([FromBody] ThreadDto thread, [FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);
            var currentUser = _context.Users.FirstOrDefault(u => u.Username == user.UserName);

            if (currentUser != null)
            {
                var interest = _context.Interests.FirstOrDefault(i => i.Id == thread.InterestId);

                if (interest != null)
                {
                    var threadToAdd = new ThreadModel()
                    {
                        Name = thread.Name,
                        Interest = interest,
                        User = currentUser
                    };

                    _context.Threads.Add(threadToAdd);
                    await _context.SaveChangesAsync();
                }
            }
        }

        // PUT : Edit thread:
        [HttpPut("{id}")]
        public void Put([FromRoute] int id, [FromBody] ThreadModel updatedThread, [FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                var thread = _context.Threads.FirstOrDefault(t => t.Id == id);
                thread.Name = updatedThread.Name;

                _context.Update(thread);
                _context.SaveChanges();
            }
        }

        // DELETE : Thread 
        [HttpDelete("{id}")]
        public void Delete([FromRoute] int id, [FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                var deleteThread = _context.Threads.FirstOrDefault(x => x.Id == id);
                if (deleteThread != null)
                {
                    _context.Threads.Remove(deleteThread);
                    _context.SaveChanges();
                }
            }
        }
    }
}
