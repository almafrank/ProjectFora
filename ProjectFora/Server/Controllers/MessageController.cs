using ProjectFora.Shared;
using Microsoft.AspNetCore.Mvc;
using ProjectFora.Server.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MessageController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // GET: 
        [HttpGet("GetMessages")]
        public async Task<List<MessageModel>> GetAllMessages()
        {
            return _context.Messages.ToList();
        }

        

        // POST 
        [HttpPost]
        public void Post(MessageModel postMessage)
        {
             _context.Messages.Add(postMessage);
             _context.SaveChanges();
        }

        

        // DELETE 
        [HttpDelete("DeleteMessage:{id}")]
        public async Task DeleteMessage(int id)
        {
            var usermessage = _context.Messages.FirstOrDefault(Message => Message.Id == id);
            if (usermessage != null)
            {
                _context.Messages.Remove(usermessage);
                _context.SaveChanges();
            }
        }
    }
}
