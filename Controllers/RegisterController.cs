using Microsoft.AspNetCore.Mvc;
using MSSQLTEST.Models;
using MSSQLTEST.Models.Request;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MSSQLTEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ModelContext _context;
        public RegisterController(ModelContext context)
        {
            _context = context;
        }
        // GET: api/<RegisterController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<RegisterController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RegisterController>
        [HttpPost]
        public IActionResult Post([FromBody] RegisterRequest register)
        {
            bool IsRegistered = (from a in _context.Users
                                where a.UserId == register.User_id
                                select a).Any();
            if (IsRegistered)
            {
                return StatusCode(409);
            }
            else 
            { 
                _context.Users.Add(new User
                {
                    UserId = register.User_id,
                    Password = register.Password,
                    Email = register.Email,
                    Btime = DateTime.Now,
                    Status = 1
                });
                _context.SaveChanges();
                return Ok();
            }
        }

        // PUT api/<RegisterController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RegisterController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
