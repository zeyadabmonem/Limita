using Limita.Business.Common;
using Limita.Business.DTOs.Auth;
using Limita.Business.Services.Interface;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Limita.API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly IRegisterService registerService;
        private readonly ILoginService loginService;
        
        public AuthController(IRegisterService registerService , ILoginService loginService) { 
        
            this.registerService = registerService;
            this.loginService = loginService;
            
        }
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDTO>> Register( [FromBody] RegisterRequestDTO requestDTO)
        {
            ServiceResult<RegisterResponseDTO> result = await registerService.RegisterAsync(requestDTO);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login( [FromBody] LoginRequestDTO requestDTO)
        {
            ServiceResult<LoginResponseDTO> result = await loginService.LoginAsync(requestDTO);

            if (!result.Success)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }



    }
}
