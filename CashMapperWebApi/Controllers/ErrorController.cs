using Microsoft.AspNetCore.Mvc;
namespace CashMapperWebApi.Controllers;

/// <summary>
/// A controller for global exceptions. Conceals internal exceptions for security.
/// </summary>
[ApiController]
[Route("error")]
public class ErrorController : ControllerBase
{

    [HttpGet]
    [HttpPost]
    public IActionResult HandleError()
    {
        return Problem(
            detail: "An unexpected server error occurred.",
            statusCode: 500
        );
    }
}
