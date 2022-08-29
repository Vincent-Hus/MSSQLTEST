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
        private ModelContext _context;
        private IConfiguration _configuration;
        public LoginController (ModelContext context ,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST api/<LoginController>
        [HttpPost]
        public IActionResult Post([FromBody] LoginRequest login)
        {
            var loginResult = (from a in _context.Users
                               where a.UserId == login.User_id
                               && a.Password == login.Password
                               select a).SingleOrDefault();
            if (loginResult == null)
            {
                return BadRequest("帳號密碼錯誤");
            }
            else
            {
                //設定使用者資訊
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.NameId, login.User_id)
                };

                // var role = from a in _context.MngUserRoles
                //            where a.UserId == user.UserId
                //            select a;
                // //設定Role
                // foreach (var temp in role)
                // {
                //     claims.Add(new Claim(ClaimTypes.Role, temp.Name));
                // }

                //取出appsettings.json裡的KEY處理
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));

                //設定jwt相關資訊
                var jwt = new JwtSecurityToken
                (
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

                //產生JWT Token
                var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                //回傳JWT Token給認證通過的使用者
                //return Ok(new LoginResponse {User_id=loginResult.UserId,Password=loginResult.Password,Email=loginResult.Email,Token=token});
                return Ok(new LoginUser { User = new LoginResponse { User_id = loginResult.UserId, Password = loginResult.Password, Email =loginResult.Email,Token=token } });

            }
        }
        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
