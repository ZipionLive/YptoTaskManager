using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YptoTaskManager.BE.IBusiness;
using YptoTaskManager.BE.IBusiness.Dtos;

namespace YptoTaskManager.BE.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authService.LoginAsync(request, cancellationToken);
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid email or password.");
        }
    }

    [Authorize]
    [HttpGet("whoami")]
    public ActionResult<string> WhoAmI()
    {
        return User.Identity?.Name ?? "Unknown";
    }
}
