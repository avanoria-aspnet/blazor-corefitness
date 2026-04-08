using Application.Abstractions.Identity;
using Application.Dtos.Results;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services;

public class IdentityAuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager) : IAuthService
{
    public Task<AuthResult> AlreadyExistsAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<AuthResult> SignUpUserAsync(string email, string password, string? roleName = null)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthResult> SignInUserAsync(string email, string password, bool rememberMe = false)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return AuthResult.Failed("Incorrect email address or password");

        var result = await signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        if (result.IsLockedOut)
            return AuthResult.Failed("This user is temporary locked out");

        if (result.IsNotAllowed)
            return AuthResult.Failed("This user is not allowed to login");

        if (result.RequiresTwoFactor)
            return AuthResult.Failed("This user requires two-factor authentication");

        if (!result.Succeeded)
            return AuthResult.Failed("Incorrect email address or password");

        return AuthResult.Ok();
    }

    public Task SignOutUserAsync() => signInManager.SignOutAsync();
}