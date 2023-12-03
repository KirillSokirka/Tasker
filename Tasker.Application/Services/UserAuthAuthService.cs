using Microsoft.AspNetCore.Identity;
using Tasker.Application.DTOs;
using Tasker.Application.Interfaces;
using Tasker.Domain.Entities.Identity;

namespace Tasker.Application.Services;

public class UserAuthAuthService : IUserAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public UserAuthAuthService(ITokenService tokenService,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
    {
        var user = new ApplicationUser { UserName = model.Username, Email = model.Email };

        return await _userManager.CreateAsync(user, model.Password);
    }

    public async Task<LoginOperationResult> LoginUserAsync(LoginModel model)
    {
        var result = new LoginOperationResult();

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
        {
            result.AddError($"The user with {model.Email} doesn't exist");

            return result;
        }

        var signInResult = await _signInManager.PasswordSignInAsync(
            user: user, 
            password: model.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        if (signInResult.Succeeded)
        {
            result.Token = await _tokenService.GenerateTokensPairAsync(user);
        }
        else
        {
            result.AddError("Invalid login attempt.");
        }

        return result;
    }
}