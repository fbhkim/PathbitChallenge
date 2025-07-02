using Microsoft.AspNetCore.Mvc;
using PathbitChallenge.Application.DTOs;
using PathbitChallenge.Application.Interfaces;

namespace PathbitChallenge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }

  [HttpPost("signup")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> SignUp(SignUpRequest request)
  {
    var result = await _authService.SignUpAsync(request);
    if (result.IsFailure)
    {
      return BadRequest(new { message = result.Error });
    }
    return Ok(new { message = "Usuário criado com sucesso." });
  }

  [HttpPost("login")]
  [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Login(LoginRequest request)
  {
    var result = await _authService.LoginAsync(request);
    if (result.IsFailure)
    {
      return BadRequest(new { message = result.Error });
    }
    return Ok(result.Value);
  }
}
