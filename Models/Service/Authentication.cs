using Microsoft.IdentityModel.Tokens;
using MSSQLTEST.Models;
using MSSQLTEST.Models.Request;
using MSSQLTEST.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

/// <summary>
/// 登入狀態驗證
/// </summary>
public class Authentication
{
    private readonly ModelContext _context;

    private IConfiguration _configuration;
    public Authentication(ModelContext context,IConfiguration configuration)
    {
        _context = context; 

        _configuration = configuration;
    }

    //刷新使用者資訊
    public LoginUser? GetUser(string token)
    {
        token = token.Replace("Bearer ",String.Empty);
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var Email = jwtSecurityToken.Claims.First(claim => claim.Type == "email").Value;
        var Password = jwtSecurityToken.Claims.First(claim => claim.Type == "prn").Value;
        var UserId = jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value;

        return new LoginUser { User = new LoginResponse { Email = Email, Password = Password, User_id = UserId, Token = token } };


        
    }

    //登入
    public LoginUser? GetUser(LoginRequest loginRequest)
    {
        var loginResult = (from a in _context.Users
                           where a.UserId == loginRequest.User_id
                           && a.Password == loginRequest.Password
                           select a).SingleOrDefault();

        if (loginResult==null)
        {
            return null;
        }
        else { 
              //設定使用者資訊
              var claims = new List<Claim>
              {
                  new Claim(JwtRegisteredClaimNames.NameId,loginResult.UserId),
                  new Claim(JwtRegisteredClaimNames.Email,loginResult.Email),
                  new Claim(JwtRegisteredClaimNames.Prn,loginResult.Password)
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
              
              var user = new LoginUser { User = new LoginResponse { User_id = loginResult.UserId, Password = loginResult.Password, Email = loginResult.Email, Token = token } };
              return user;
        }
    }
}
