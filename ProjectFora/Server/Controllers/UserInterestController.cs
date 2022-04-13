
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
        public async Task<UserInterestModel> GetSingelInterest( int InterestId)
        
        {
            var interest = _context.UserInterests.Where(u => u.InterestId == InterestId);
            return interest.FirstOrDefault(); ;   
        }

        // GET : All userinterests
        [HttpGet]
        public List<InterestModel> Get([FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if(user != null)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.Username == user.UserName);
                var userInterests = _context.Interests.Where(i => i.UserInterests.Any(ui => ui.UserId == currentUser.Id)).ToList();

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

            if(user != null)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.Username == user.UserName);
                var uinterest = _context.Interests.FirstOrDefault(i => i.Id == interest.Id);

                if(currentUser != null)
                {
                    UserInterestModel userInterest = new();
                    userInterest.Interest = uinterest;
                    userInterest.User = currentUser;

                    _context.UserInterests.Add(userInterest);
                    await _context.SaveChangesAsync();
                }
            }
        }

        [HttpDelete("DeleteUserInterest:{id}")]
        public async Task DeleteUserInterest(int InterestId)
        {
            var userInterest = _context.UserInterests.FirstOrDefault(x => x.InterestId == InterestId);
            if (userInterest != null)
            {
                _context.UserInterests.Remove(userInterest);
                _context.SaveChanges();
            }
        }
        [HttpPut("UpdateUserInterest")]
        public async Task UpdateUserInterest(int InterestId)
        {
            var updateUserInterest = _context.UserInterests.Where(x => x.InterestId == InterestId);
            if(updateUserInterest != null)
            {
                _context.Update(updateUserInterest);
                _context.SaveChanges();
            }
        }


        //[HttpDelete("removeUserInterest")]

        //public async Task<ActionResult> RemoveUserInterest(UserModel user, InterestModel interest)
        //{
       

     

        //    if (user != null && interest != null)
        //    {
        //        _context.UserInterests.Remove(new UserInterestModel { User = user, Interest = interest });
        //        _context.SaveChanges();

        //        return Ok();
        //    }
        //    return BadRequest("Någonting gick snett");
        //}






    }
}
