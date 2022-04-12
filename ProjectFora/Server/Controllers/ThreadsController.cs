﻿using Microsoft.AspNetCore.Mvc;
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
        // PUT,update a user:
        [HttpPut("updateThread")]
        public async Task UpdateThread(int id,ThreadModel thread)
        {
            var updateThread = _context.Threads.Where(x => x.Id == id);
           _context.Update(updateThread);
            _context.SaveChanges();
        }

        // GET a specifik thread:
        [HttpGet("GetAThread")]
        public async Task<ThreadModel> GetThread(int id)
        {
            var thread = _context.Threads.Where(u => u.Id == id);
            return thread.FirstOrDefault();
        }

        // POST a thead:
        [HttpPost("PostThread")]
        public async Task PostThread(ThreadModel postThread)
        {

            var interest = _context.Interests.FirstOrDefault(i => i.Id == 11);
            var user = _context.Users.FirstOrDefault(u => u.Id == 1);

            postThread.Interest = interest;
            postThread.User = user;

            _context.Threads.Add(postThread);
            _context.SaveChanges();
        }

        // DELETE a thread: 
        [HttpDelete("DeleteThread")]
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
