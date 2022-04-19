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
        public async Task<List<ThreadModel>> Get([FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var threads = _context.Threads
                    .Include(t => t.Messages)
                    .Select(t => new ThreadModel
                    {
                        Name = t.Name,
                        Id = t.Id,
                        UserId = t.UserId,
                        User = t.User,
                        Messages = t.Messages.Select(m => new MessageModel()
                        {
                            Id = m.User.Id,
                            Message = m.Message,
                            User = m.User,
                        }).ToList()
                    }).ToList();

                return threads;
            }

            return null;

        }

        // GET : Specific thread:
        [HttpGet("thread/{id}")]
        public async Task<ThreadModel> GetThread([FromRoute] int id, [FromQuery] string token)
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
        public async Task<ActionResult> Post([FromBody] ThreadDto thread, [FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);
            var currentUser = _context.Users.FirstOrDefault(u => u.Username == user.UserName);

            if (currentUser != null)
            {
                var result = _context.Threads.Where(i => i.Name.ToLower() == thread.Name.ToLower()).ToList();

                if (result.Any())
                {
                    return Ok("Tråd finns redan");
                }
                else
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
            return BadRequest();
        }

        // PUT : Edit thread:
        [HttpPut("{id}")]
        public void Put([FromRoute] int id, [FromBody] ThreadDto threadToUpdate, [FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var thread = _context.Threads.FirstOrDefault(t => t.Id == id);
                thread.Name = threadToUpdate.Name;

                _context.Update(thread);
                _context.SaveChanges();
            }
        }

        // DELETE : Thread 
        [HttpDelete("{id}")]
        public void Delete([FromRoute] int id, [FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

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
