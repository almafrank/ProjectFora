using Microsoft.AspNetCore.Mvc;
using ProjectFora.Server.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("Threads")]
    [ApiController]
    public class ThreadsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ThreadsController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }



        // GET all threads: api/<ThreadsController>
        [HttpGet("AllThreads")]
        public async Task<List<ThreadModel>> GetAllThreads()
        {
             return _context.Threads.ToList();

        }
        //// PUT,update a user:  api/<ProfileController>/5
        //[HttpPut("updateThread:{id}")]
        //public async Task UpdateThread(int id,ThreadModel thread)
        //{
        //    var updateThread = _context.Threads.Where(user => user.Id == id);
        //    _context.Update(updateThread);
        //    _context.SaveChanges();
        //}

        // GET a specifik thread: api/<ThreadsController>/5
        [HttpGet("GetAThread:{id}")]
        public async Task<ThreadModel> GetThread(int id)
        {
            var thread = _context.Threads.Where(u => u.Id == id);
            return thread.FirstOrDefault();
        }




        // POST a thead:api/<ThreadsController>
        [HttpPost("PostThead")]
        public async Task PostThread( ThreadModel postThread)
        {
            _context.Threads.Add(postThread);
            _context.SaveChanges();
        }


        

        // DELETE a thread: api/<ThreadsController>/5
        [HttpDelete("DeleteThread:{id}")]
        public async Task DeleteThread(int id)
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
