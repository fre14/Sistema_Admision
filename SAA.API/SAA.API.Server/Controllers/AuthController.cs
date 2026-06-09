using Microsoft.AspNetCore.Mvc;
using SAA.Application.DTOs;
using SAA.Application.Services;

namespace SAA.API.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(request);

        if (!result.Exito)
            return Unauthorized(result);

        return Ok(result);
    }
}
