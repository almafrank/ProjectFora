
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFora.Server.Data;
using ProjectFora.Shared;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("UserInterest")]
    [ApiController]
    public class UserInterestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserInterestController(AppDbContext appDbContext)

        {
            _context = appDbContext;
        }



        // GET a singel interest
        [HttpGet("GetSingelInterest:{id}")]
        public async Task<UserInterestModel> GetSingelInterest( int InterestId)
        
        {
            var interest = _context.UserInterests.Where(u => u.InterestId == InterestId);
            return interest.FirstOrDefault(); ;   
        }
        

        // POST ,user post a new interest:
        [HttpPost("UserPostnewInterest")]
        public async Task PostUserInterest(UserInterestModel postinterest)
        {
            _context.UserInterests.Add(postinterest);
            _context.SaveChanges();
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

        [HttpPost("postUserInterest")]

        public async Task<ActionResult> AddUserInterest(UserInterestModel user)
        {

            if (user != null)
            {
                _context.UserInterests.Add(user);
                _context.SaveChanges();

                return Ok();
            }
            return BadRequest("Någonting gick snett");
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
