using Limita.Business.Common;
using Limita.Business.DTOs.Auth;
using Limita.Business.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limita.API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterService registerService;
        public AuthController(IRegisterService registerService) { 
        
            this.registerService = registerService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDTO>> Register( [FromBody] RegisterRequestDTO requestDTO)
        {
            ServiceResult<RegisterResponseDTO> result = await registerService.RegisterAsync(requestDTO);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

    }
}
