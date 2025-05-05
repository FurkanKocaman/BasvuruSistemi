using BasvuruSistemi.Server.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace BasvuruSistemi.Server.WebAPI;

public static class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using(var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<Appuser>>();
            if(!userManager.Users.Any(p => p.UserName == "admin"))
            {
                Appuser user = new()
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "Furkan",
                    LastName = "Kocaman",
                    EmailConfirmed = true,
                    CreatedAt = DateTimeOffset.Now,
                };
                user.CreateUserId = user.Id;
                userManager.CreateAsync(user, "1").Wait();
            }
        }
    }
}
