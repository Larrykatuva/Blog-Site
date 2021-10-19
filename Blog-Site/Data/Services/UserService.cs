using Blog_Site.Data.Models;
using Blog_Site.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog_Site.Data.Services
{
    public class UserService: IUserService
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public object ValidIssuer { get; private set; }
        public object ValidAudience { get; private set; }

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._configuration = configuration;
        }


        public async Task<RegisterResponse> RegisterUserAsync(RegisterVM registerVM)
        {
            var userExists = await userManager.FindByNameAsync(registerVM.Username);
            if(userExists != null)
            {
                return isError("User already exists", true, null);
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerVM.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerVM.Username
            };
            var result = await userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                return isError("An error has occured", true, null);
            }
            RegisterResponse res = isError("User created successfully", false, null);
            return res;

        }

        public async Task<LoginResponse> LoginUserAsync(LoginVM loginVM) {
            var user = await userManager.FindByNameAsync(loginVM.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, loginVM.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                LoginResponse res = new LoginResponse();
                res.Message = "Login successful";
                res.Error = false;
                res.Username = user.UserName;
                res.Email = user.Email;
                res.Token = new JwtSecurityTokenHandler().WriteToken(token);
                res.expiry = token.ValidTo;

                return res;
            }
            return null;
        }
        public RegisterResponse isError(string message, Boolean error, RegisterVM registerVM)
        {
            RegisterResponse res = new RegisterResponse();
            res.Message = message;
            res.Error = error;
            res.registerVM = registerVM;
            return res;
        }
    }
}
