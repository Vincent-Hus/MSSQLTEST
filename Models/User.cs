using System;
using System.Collections.Generic;

namespace MSSQLTEST.Models
{
    public partial class User
    {
        public string UserId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public DateTime? Btime { get; set; }
        public byte Status { get; set; }
    }
}
