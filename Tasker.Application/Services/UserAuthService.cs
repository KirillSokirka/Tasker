using Microsoft.AspNetCore.Identity;
using Tasker.Application.DTOs;
using Tasker.Application.DTOs.Auth;
using Tasker.Application.Interfaces;
using Tasker.Application.Interfaces.Repositories;
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

    public async Task<IdentityOperationResult> LoginUserAsync(LoginModel model)
    {
        var result = new IdentityOperationResult();

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

    public async Task<IdentityOperationResult> UpdatePasswordAsync(PasswordUpdateModel model)
    {
        var result = new IdentityOperationResult();

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
        {
            result.AddError($"The user with email {model.Email} doesn't exist");

            return result;
        }

        var passwordCheck =
            _userManager.PasswordHasher.VerifyHashedPassword(user!, user.PasswordHash!, model.OldPassword);

        if (passwordCheck is PasswordVerificationResult.Failed)
        {
            result.AddError("The old password is incorrect");

            return result;
        }
        
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);

        var updateResult = await _userManager.UpdateAsync(user);

        if (updateResult.Succeeded)
        {
            result.Token = await _tokenService.GenerateTokensPairAsync(user);
        }
        else
        {
            result.AddError("Invalid password change attempt.");
        }
        
        return result;
    }

    #region Private Methods

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

    #endregion"
}