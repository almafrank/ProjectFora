using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectFora.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountsController(UserManager<IdentityUser>userManager)
        {
            _userManager = userManager;
        }
       

        // POST api/<AccountsController>
        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterModel registerModel)
        {
            if (registerModel == null || !ModelState.IsValid)
                return BadRequest();

            var newUser = new IdentityUser { UserName = registerModel.Email, Email = registerModel.Email };

            var result = await _userManager.CreateAsync(newUser, registerModel.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();

                return Ok(new RegisterResult { Successful = false, Errors = errors });

            }

            return Ok(new RegisterResult { Successful = true });

           

        }


    }
}
