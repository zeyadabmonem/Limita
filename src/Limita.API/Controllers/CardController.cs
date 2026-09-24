namespace Limita.API.Controllers;

[Authorize]
[Route("api/v1/cards")]
[ApiController]
public class CardController : ControllerBase
{
    private readonly ICardService cardService;

    public CardController(ICardService cardService)
    {
        this.cardService = cardService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CardResponseDTO>>> GetAllCards()
    {
        ServiceResult<List<CardResponseDTO>> result =
            await cardService.GetAll(User.GetUserId());

        return result.ToActionResult(this);
    }

    [HttpGet("{cardId:int}")]
    public async Task<ActionResult<CardResponseDTO>> GetCardById(int cardId)
    {
        ServiceResult<CardResponseDTO> result =
            await cardService.GetById(User.GetUserId(), cardId);

        return result.ToActionResult(this);
    }
}
