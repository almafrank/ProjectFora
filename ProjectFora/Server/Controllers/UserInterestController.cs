
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFora.Server.Data;
using ProjectFora.Server.Models;
using ProjectFora.Shared;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInterestController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserInterestController(AppDbContext appDbContext, SignInManager<ApplicationUser> signInManager)

        {
            _context = appDbContext;
            _signInManager = signInManager;
        }

        // GET a singel interest
        [HttpGet("GetSingelInterest:{id}")]
        public async Task<UserInterestModel> GetSingelInterest(int InterestId)

        {
            var interest = _context.UserInterests.Where(u => u.InterestId == InterestId);
            return interest.FirstOrDefault(); ;
        }

        // GET : All userinterests
        [HttpGet]
        public async Task<List<InterestModel>> Get([FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.Username == user.UserName);
                var userInterests = _context.Interests
                    .Where(i => i.UserInterests
                    .Any(ui => ui.UserId == currentUser.Id))
                    .Include(i => i.Threads)
                    .Select(i => new InterestModel()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        UserId = i.UserId,
                        Threads = i.Threads.Select(i => new ThreadModel()
                        {
                            Id = i.Id,
                            InterestId = i.InterestId,
                            Name = i.Name,
                            UserId = i.UserId
                        }).ToList()
                    }).ToList();

                if (userInterests != null)
                {
                    return userInterests;
                }
            }
            return null;
        }


        // POST : New interest to User
        [HttpPost]
        public async Task Post([FromBody] InterestModel interest, [FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.Username == user.UserName);
                var interestToAdd = _context.Interests.FirstOrDefault(i => i.Id == interest.Id);

                if (currentUser != null)
                {
                    UserInterestModel userInterest = new();
                    userInterest.Interest = interestToAdd;
                    userInterest.User = currentUser;

                    _context.UserInterests.Add(userInterest);
                    await _context.SaveChangesAsync();
                }
            }
        }

        // DELETE : Userinterest
        [HttpDelete("{id}")]
        public void Delete([FromRoute] int Id, [FromQuery] string accessToken)
        {

            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var userToDeleteFrom = _context.Users.FirstOrDefault(x => x.Username == user.UserName);
                var deleteInterest = _context.Interests.FirstOrDefault(x => x.Id == Id);

                UserInterestModel userInterest = new();
                userInterest.Interest = deleteInterest;
                userInterest.User = userToDeleteFrom;

                if (userToDeleteFrom != null && deleteInterest != null)
                {
                    _context.UserInterests.Remove(userInterest);
                    _context.SaveChanges();
                }
            }
        }

        //[HttpPut("{id}")]
        //public async Task UpdateUserInterest([FromRoute] int InterestId, string editedName, string accessToken)
        //{
        //    var updateUserInterest = _context.UserInterests.Where(x => x.InterestId == InterestId);
        //    if (updateUserInterest != null)
        //    {
        //        _context.Update(updateUserInterest);
        //        _context.SaveChanges();
        //    }
        //}
    }
}
