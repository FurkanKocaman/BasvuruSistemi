using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace BasvuruSistemi.Server.Infrastructure.Services;
internal sealed class UserService(UserManager<AppUser> userManager) : IUserService
{
    public async Task<Guid> CreateUserAsync(string? firstName, string? lastName, bool isEmployer, string email, string password)
    {
        var userExist = await userManager.FindByEmailAsync(email);

        if (userExist is not null)
            return Guid.Empty;

        string username = "";

        if (isEmployer)
        {
            username = email;
        }
        else
        {
            string baseUserName = $"{firstName}.{lastName}".ToLower().Replace(" ", "");
            string userName = baseUserName;
            int counter = 1;

            while (await userManager.FindByNameAsync(userName) != null)
            {
                userName = $"{baseUserName}{counter}";
                counter++;
            }
            username = userName;
        }
            
        AppUser appUser = new()
        {
            UserName = username,
            Email = email,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(appUser,password);

        if (!result.Succeeded)
            return Guid.Empty;
        
        return appUser.Id;
    }
}
