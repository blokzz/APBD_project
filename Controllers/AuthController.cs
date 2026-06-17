using APBD_PROJEKT.Dtos.Auth;
using Microsoft.AspNetCore.Authorization;

namespace APBD_PROJEKT.Controllers;
using Microsoft.AspNetCore.Mvc;
using Dtos.Clients;
using Exceptions;
using Services;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.Login(dto);
        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        await _authService.Register(dto);
        return Ok();
    }
}