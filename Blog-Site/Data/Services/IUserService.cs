using Blog_Site.Data.Models;
using Blog_Site.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Site.Data.Services
{
    public interface IUserService
    {
        Task<RegisterResponse> RegisterUserAsync(RegisterVM registerVM);

        Task<LoginResponse> LoginUserAsync(LoginVM loginVM);
    }
}
