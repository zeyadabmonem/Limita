using Limita.API.Helper;
using Limita.Business.DTOs.Cards;
using Limita.Business.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limita.API.Controllers
{
    [Authorize]
    [Route("api/v1/cards")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService service;

        public CardController(ICardService service) 
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CardResponseDTO>>> GetAllCards()
        {
            var result = await service.GetAll(User.GetUserId());
            if(result.Success)
                return Ok(result.Data);
            return BadRequest(result.Message);
        }

        [HttpGet("{cardId}")]
        public async Task<ActionResult<CardResponseDTO>> GetCardById(int cardId)
        {
            var result = await service.GetById(User.GetUserId(),cardId);
            if(result.Success)
                return Ok(result.Data);
            return BadRequest(result.Message);
        }
    }
}
