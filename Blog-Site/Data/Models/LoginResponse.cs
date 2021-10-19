using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Site.Data.Models
{
    public class LoginResponse
    {
        public string Message { get; set; }
        public Boolean Error { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string Token { get; set; }
        public DateTime? expiry { get; set; }
    }
}
