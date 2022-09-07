using Microsoft.AspNetCore.Mvc;
using MSSQLTEST.Models.Response;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MSSQLTEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageReflashController : ControllerBase
    {
        private Authentication _authentication;
        public PageReflashController(Authentication authentication)
        {
            _authentication = authentication;
        }

        // GET api/<PageReflashController>/5
        [HttpGet]
        public LoginUser? Get()
        {
            string user = HttpContext.Request.Headers["Authorization"];
            LoginUser? res = _authentication.GetUser(user);
            return res;
        }

    }
}
