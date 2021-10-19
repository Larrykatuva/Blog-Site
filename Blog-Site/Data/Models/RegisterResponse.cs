using Blog_Site.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Site.Data.Models
{
    public class RegisterResponse
    {
        public string Message { get; set; }
        public Boolean Error { get; set; }
        public RegisterVM? registerVM {get; set;}
    }
}
