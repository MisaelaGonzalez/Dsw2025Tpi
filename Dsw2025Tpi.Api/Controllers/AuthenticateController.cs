using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Dsw2025Tpi.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticateController : ControllerBase
    {
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtTokenService _jwtTokenService;

        public AuthenticateController(JwtTokenService jwtTokenService)
        {
           // _userManager = userManager;
            //_signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel request)
        {
            if(request.Username == "admin" && request.Password == "123456")
            {
                var token = _jwtTokenService.GenerateToken(request.Username);
                return Ok(new { token });
            }
            return Unauthorized();

        }

    }
}
