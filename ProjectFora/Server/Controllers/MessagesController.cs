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
        [Route("{id}")]
        public List<MessageModel> GetThreadMessages([FromRoute] int id, [FromQuery] string token)
        {

            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                var messages = _context.Messages
                    .Include(m => m.User)
                    .Where(m => m.ThreadId == id)
                    .Select(m => new MessageModel
                {
                    Message = m.Message,
                    Id = m.Id,
                    ThreadId = m.ThreadId,
                    MessageCreated = m.MessageCreated,
                    IsEdited = m.IsEdited,
                    HasDeleted = m.HasDeleted,
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
        public async Task CreateMessage([FromBody] MessageDto newMessage, [FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.Username == user.UserName);

                if (currentUser != null)
                {
                    var thread = _context.Threads.FirstOrDefault(t => t.Id == newMessage.ThreadId);

                    var messageToAdd = new MessageModel()
                    {
                        Message = newMessage.Message,
                        User = currentUser,
                        UserId = currentUser.Id,
                        Thread = thread,
                        MessageCreated = DateTime.Now
                    };

                    _context.Messages.Add(messageToAdd);
                     await _context.SaveChangesAsync();
                }
            }
        }

        // PUT : update message
        [HttpPut("{id}")]
        public async Task Put([FromRoute] int id,[FromBody] MessageDto updatedMessage, [FromQuery] string accessToken)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == accessToken);

            if (user != null)
            {
                var messageToUpdate = _context.Messages.FirstOrDefault(m => m.Id == id);

                if (messageToUpdate != null)
                {
                    messageToUpdate.Message = updatedMessage.Message;
                    messageToUpdate.IsEdited = true;

                    _context.Messages.Update(messageToUpdate);
                    await _context.SaveChangesAsync();
                }
            }
        }

        // DELETE : specific message
        [HttpDelete("{id}")]
        public async Task Delete([FromRoute] int id,[FromQuery] string token)
        {
            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                var messageToDelete = _context.Messages.FirstOrDefault(Message => Message.Id == id);
                if (messageToDelete != null)
                {
                    messageToDelete.HasDeleted = true;
                    messageToDelete.Message = "Meddelandet har raderats";
                    _context.Messages.Update(messageToDelete);
                    _context.SaveChanges();
                }

            }
        }
    }
}
