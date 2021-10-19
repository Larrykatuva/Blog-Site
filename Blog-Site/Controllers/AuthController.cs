using Blog_Site.Data.Models;
using Blog_Site.Data.Services;
using Blog_Site.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Site.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userSerice;

        public AuthController(IUserService userService)
        {
            _userSerice = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterVM registerVM)
        {
            if(registerVM == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Error = true, Message = "All fields are required" });
            }
            if (registerVM.Email == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Error = true, Message = "Email is required" });
            }
            if (registerVM.Username == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Error = true, Message = "Username is required" });
            }
            if (registerVM.Password == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Error = true, Message = "Password is required" });
            }
            var response = await _userSerice.RegisterUserAsync(registerVM);
            return Ok(response);
        }



        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginVM loginVM)
        {
            if (loginVM == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Error = true, Message = "All fields are required" });
            }
            if (loginVM.Username == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Error = true, Message = "Username is required" });
            }
            if (loginVM.Password == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Error = true, Message = "Password is required" });
            }

            var response = await _userSerice.LoginUserAsync(loginVM);
            if (!response.Error)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return Ok(response);
        }

    }
}
