using ProjectFora.Shared;
using Microsoft.AspNetCore.Mvc;
using ProjectFora.Server.Data;
using Microsoft.AspNetCore.Identity;
using ProjectFora.Server.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public MessagesController(AppDbContext appDbContext, SignInManager<ApplicationUser> signInManager)
        {
            _context = appDbContext;
            _signInManager = signInManager;
        }

        // GET: All messages
        [HttpGet]
        public List<MessageModel> GetAllMessages([FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                return _context.Messages.ToList();
            }

            return null;
        }

       
        // GET : messages from specific thread
        [HttpGet]
        [Route("threadmessages/{id}")]
        public List<MessageModel> GetThreadMessages([FromRoute] int id, [FromQuery] string token)
        {

            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                var messages = _context.Messages.Include(m => m.User).Where(m => m.ThreadId == id).Select(m => new MessageModel
                {
                    Message = m.Message,
                    User = new UserModel()
                    {
                        Id = m.User.Id,
                        Username = m.User.Username,
                        Banned = m.User.Banned,
                        Deleted = m.User.Deleted,
                    }
                }).ToList();

                return messages;
            }

            return null;
        }

        // POST : message
        [HttpPost]
        public async Task CreateMessage([FromQuery] int threadId, [FromBody] MessageModel message, [FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.Username == user.UserName);

                if (currentUser != null)
                {
                    var thread = _context.Threads.FirstOrDefault(t => t.Id == threadId);

                    var messageToAdd = new MessageModel()
                    {
                        Message = message.Message,
                        User = currentUser,
                        Thread = thread,
                        CreatedBy = currentUser.Username,
                        MessageCreated = DateTime.Now
                    };

                    _context.Messages.Add(message);
                     await _context.SaveChangesAsync();
                }
            }
        }

        // PUT : update message
        [HttpPut("{id}")]
        public async Task UpdateAThreadMessage([FromRoute] int id, [FromBody] MessageModel updatedMessage, [FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                var messageToUpdate = _context.Messages.FirstOrDefault(m => m.Id == id);

                if (messageToUpdate != null)
                {
                    messageToUpdate.Message = updatedMessage.Message;

                    // Todo: Set the a message bool property "Edited" to True

                    _context.Messages.Update(messageToUpdate);
                    await _context.SaveChangesAsync();
                }
            }
        }

        // DELETE : specific message
        [HttpDelete("{id}")]
        public async Task Delete( int id, string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
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
}
