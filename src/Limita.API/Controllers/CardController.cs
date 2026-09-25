namespace Limita.API.Controllers;

[Authorize]
[Route("api/v1/cards")]
[ApiController]
public class CardController : ControllerBase
{
    private readonly ICardService cardService;
    public CardController(ICardService cardService) { this.cardService = cardService; }

    [HttpGet]
    public async Task<IActionResult> GetAllCards()
    {
        ServiceResult<List<CardResponseDTO>> result = await cardService.GetAll(User.GetUserId());
        return result.ToActionResult(this);
    }

    [HttpGet("{cardId:int}")]
    public async Task<IActionResult> GetCardById(int cardId)
    {
        ServiceResult<CardResponseDTO> result = await cardService.GetById(User.GetUserId(), cardId);
        return result.ToActionResult(this);
    }

    [HttpPost]
    public async Task<IActionResult> AddCard([FromBody] AddCardRequestDTO requestDTO)
    {
        ServiceResult<CardResponseDTO> result = await cardService.AddAsync(User.GetUserId(), requestDTO);
        return result.ToActionResult(this);
    }

    [HttpPatch("{cardId:int}/status")]
    public async Task<IActionResult> UpdateStatus(int cardId, [FromBody] CardStatus status)
    {
        ServiceResult<CardResponseDTO> result = await cardService.UpdateStatusAsync(User.GetUserId(), cardId, status);
        return result.ToActionResult(this);
    }
}
