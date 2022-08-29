namespace MSSQLTEST.Models.Response
{
    public class LoginResponse
    {
        public string User_id { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; } 
        public string Token { get; set; } = null!;
    }
    public class LoginUser
    {
        public LoginResponse User { get; set; } = null !;
    }
}
