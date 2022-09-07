using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MSSQLTEST.Models;
using MSSQLTEST.Models.Request;
using MSSQLTEST.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MSSQLTEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private Authentication _authentication;
        public LoginController (Authentication authentication)
        {
            _authentication = authentication;
        }

        // POST api/<LoginController>
        [HttpPost]
        public IActionResult Post([FromBody] LoginRequest login)
        {
            LoginUser? user = _authentication.GetUser(login);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }

    }
}
