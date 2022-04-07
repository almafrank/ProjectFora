using ProjectFora.Shared;
using Microsoft.AspNetCore.Mvc;
using ProjectFora.Server.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("Message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MessageController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // GET: api/<MessageController>
        [HttpGet("GetMessages")]
        public async Task<List<MessageModel>>GetAllMessages()
        {
            return _context.Messages.ToList();
        }

        //// GET api/<MessageController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<MessageController>
        [HttpPost("PostMessage")]
        public async Task Post([FromBody] string value)
        {

        }

        

        // DELETE api/<MessageController>/5
        [HttpDelete("DeleteMessage:{id}")]
        public async Task DeleteMessage(int id)
        {

        }
    }
}
