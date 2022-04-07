﻿using ProjectFora.Shared;
using Microsoft.AspNetCore.Mvc;
using ProjectFora.Server.Data;

namespace ProjectFora.Server.Controllers
{
    [Route("profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        // GET all users: api/<ProfileController>
        [HttpGet("Users")]
        public async Task<List<UserModel>> GetAllUsers()
        {
            return _context.Users.ToList();
          
        }

        // GET singel user: api/<ProfileController>/5
        [HttpGet("User:{id}")]
        public async Task<UserModel?> GetUser(int id)
        {
           var user= _context.Users.Where(u => u.Id == id);
            return user.FirstOrDefault();
        }

        // POST a new user: api/<ProfileController>
        [HttpPost("newUser")]
        public async Task PostANewUser([FromBody] UserModel userToAdd)
        {
            _context.Users.Add(userToAdd);
            _context.SaveChanges();
        }

        // PUT,update a user:  api/<ProfileController>/5
        [HttpPut("updateUser:{id}")]
        public async Task UpdateUser(int id, [FromBody]UserModel user)
        {
            var updateUser = _context.Users.Where(user => user.Id == id);
            _context.Update(updateUser);
            _context.SaveChanges();
        }

        // DELETE a user: api/<ProfileController>/5
        [HttpDelete("deleteUser:{id}")]
        public async Task DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}