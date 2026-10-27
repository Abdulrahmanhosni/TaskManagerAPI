using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs.Auth;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authservice;
        public AuthController (IAuthService authservice)
        {
            _authservice = authservice;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            await _authservice.RegisterAsync(dto);
            return Ok("User registered successfully");
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginDto dto) 
        {
            var token =  await _authservice.LoginAsync(dto);
            return Ok(new { token = token });
   
        }


    }
}
