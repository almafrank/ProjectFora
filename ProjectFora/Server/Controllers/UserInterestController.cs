using Microsoft.AspNetCore.Identity;
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
        // GET: 
        [HttpGet("AllUserInterest")]

        //public  List<UserInterestModel> GetUserInterest()
        //{
           
             
        //}


        ////// GET
        //[HttpGet("GetSingelInterest:{id}")]
        //public async Task GetSingelInterest( UserInterestModel user)
        //{
        //   var userInterest = _context.Interests.Where(x => x. == user);
        //    await _context.SaveChangesAsync();  
        //}



        //// POST ,user post a new interest:
        //[HttpPost("PostUsernewInterest")]
        //public Task<UserInterestModel>UserPostNewInterest([FromBody] InterestModel postnewInterest)
        //{
        //    _context.Interests.Add(postnewInterest);
        //    _context.SaveChanges();
        //}

        // PUT ,user update a interest:
        [HttpPut("ChangeUserInterest:{id}")]
        public async Task UserUpdateInterest(int id, [FromBody] UserInterestModel interest)
        {
            var updateUserInterest = _context.Interests.Where(user => user.Id == id);
            _context.Update(updateUserInterest);
            _context.SaveChanges();

        }

        // DELETE , user delete a interest:
        [HttpDelete("DeleteUserinterest:{id}")]
        public async Task UserDeleteInterest(UserInterestModel interest,int id)
        {

            {
                var deleteUserInterest = _context.Interests.FirstOrDefault(x => x.Id == id);
                if (deleteUserInterest != null)
                {
                    _context.Interests.Remove(deleteUserInterest);
                    _context.SaveChanges();
                }

            }
        }
    }
}
