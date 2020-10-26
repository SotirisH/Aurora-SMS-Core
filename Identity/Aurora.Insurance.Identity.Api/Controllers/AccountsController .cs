using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Aurora.Insurance.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController  : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountsController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] string userName, [FromForm] string password)
        {
            var user = new IdentityUser
            {
                UserName = userName,
                Email = ""
            };
            IdentityResult result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result);

        }
    }
}
