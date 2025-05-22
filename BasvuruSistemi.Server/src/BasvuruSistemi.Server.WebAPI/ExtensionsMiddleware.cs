namespace BasvuruSistemi.Server.WebAPI;

public static class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using(var scoped = app.Services.CreateScope())
        {
            //var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            //if(!userManager.Users.Any(p => p.UserName == "admin"))
            //{
            //    AppUser user = new("admin", "", null, null, null, new("admin@admin.com", null));
            //    user.CreateUserId = user.Id;
            //    userManager.CreateAsync(user, "1").Wait();
            //}
        }
    }
}
