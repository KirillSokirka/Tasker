using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasker.Application.DTOs;
using Tasker.Application.DTOs.Auth;
using Tasker.Application.Interfaces;

namespace Tasker.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserAuthService _userAuthService;

    public AuthController(IUserAuthService userAuthService)
    {
        _userAuthService = userAuthService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userAuthService.RegisterUserAsync(model);

        if (result.Succeeded)
        {
            return Ok();
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var operationResult = await _userAuthService.LoginUserAsync(model);

        if (operationResult.IsSuccess)
        {
            return Ok(new { operationResult.Token });
        }

        foreach (var error in operationResult.Errors)
        {
            ModelState.AddModelError("", error);
        }

        return BadRequest(ModelState);
    }

    [Authorize]
    [HttpPost("update-password")]
    public async Task<IActionResult> UpdatePassword([FromBody] PasswordUpdateModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var operationResult = await _userAuthService.UpdatePasswordAsync(model);

        if (operationResult.IsSuccess)
        {
            return Ok(new { operationResult.Token });
        }

        foreach (var error in operationResult.Errors)
        {
            ModelState.AddModelError("", error);
        }

        return BadRequest(ModelState);
    }
}