using FaceAPI.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace FaceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;

       public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }
        [HttpPost]
       public async Task<ApiResponse> Login([FromBody] UserDto userDto)=>await loginService.LoginAsync(userDto.Username,userDto.Password);
        [HttpPost]
        public async Task<ApiResponse> Register([FromBody] UserDto userDto)=>await loginService.RegisterAsync(userDto);
    }
}
