
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
        // GET: get users all interest
        [HttpGet("AllUserInterest")]

        public async Task<List<UserInterestModel>> GetUserInterest()
        {

            return await _context.UserInterests.ToListAsync();

        }

        //// GET a singel interest
        //[HttpGet("GetSingelInterest:{id}")]
        //public async Task<UserInterestModel> GetSingelInterest( int InterestId)
        
        //{
        //    return await _context.UserInterests.FirstOrDefaultAsync(x => x.InterestId == InterestId);   
        //}
        

        // POST ,user post a new interest:
        [HttpPost("PostUsernewInterest")]
        public async Task PostUserInterest(UserInterestModel postinterest)
        {
            _context.UserInterests.Add(postinterest);
            _context.SaveChanges();
        }



        // DELETE , user delete a interest:
        [HttpDelete("DeleteUserinterest:{id}")]
        public async Task<UserInterestModel> UserDeleteInterest(int InterestId)
        {

            {
                var deleteUserInterest = _context.UserInterests.FirstOrDefault(x => x.InterestId == InterestId);
                if (deleteUserInterest != null)
                {
                    _context.UserInterests.Remove(deleteUserInterest);
                    _context.SaveChanges();
                }

            }
        }
    }
}
