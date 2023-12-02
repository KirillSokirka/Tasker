using Microsoft.AspNetCore.Mvc;
using Tasker.Application.DTOs;
using Tasker.Application.Interfaces;

namespace Tasker.Controllers;

[ApiController]
[Route("[controller]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel)
        => new ObjectResult(await _tokenService.RefreshToken(refreshTokenModel));
}