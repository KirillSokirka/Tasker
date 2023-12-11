using Microsoft.AspNetCore.Identity;
using Tasker.Application.DTOs;
using Tasker.Application.Interfaces;
using Tasker.Application.Interfaces.Repositories;
using Tasker.Application.Repositories;
using Tasker.Domain.Entities.Identity;

namespace Tasker.Application.Services;

public class UserAuthService : IUserAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    
    public UserAuthService(ITokenService tokenService,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager, IUserRepository userRepository)
    {
        _signInManager = signInManager;
        _tokenService = tokenService;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
    {
        var user = new ApplicationUser { UserName = model.Username, Email = model.Email };

        var identityResult = await _userManager.CreateAsync(user, model.Password);

        if (identityResult.Succeeded)
        {
            await HandleApplicationUserCreation(user);
        }
        
        return identityResult;
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
            
            await HandleApplicationUserCreation(user);
        }
        else
        {
            result.AddError("Invalid login attempt.");
        }

        return result;
    }
    
    private async Task HandleApplicationUserCreation(ApplicationUser user)
    {
        if (await _userRepository.GetAsync(user.Id) is null)
        {
            await _userRepository.CreateAsync(new()
            {
                Id = user.Id,
                Title = user.UserName!
            });
        }
    }
}