namespace Limita.API.Helper;

public static class ServiceResultExtensions
{
    public static IActionResult ToActionResult<T>(
        this ServiceResult<T> result,
        ControllerBase controller)
    {
        if (result.Success)
            return controller.Ok(result.Data);

        return result.ErrorCode switch
        {
            ServiceErrorCode.Validation => controller.BadRequest(result.Message),
            ServiceErrorCode.NotFound => controller.NotFound(result.Message),
            ServiceErrorCode.Conflict => controller.Conflict(result.Message),
            ServiceErrorCode.Unauthorized => controller.Unauthorized(),
            ServiceErrorCode.UnprocessableEntity => controller.UnprocessableEntity(result.Message),
            _ => controller.StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };
    }
}
