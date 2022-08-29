namespace MSSQLTEST.Models.Request
{
    public class LoginRequest
    {
        public string User_id { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
