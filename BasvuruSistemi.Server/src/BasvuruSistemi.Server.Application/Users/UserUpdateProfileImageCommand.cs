using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record UserUpdateProfileImageCommand(
    IFormFile file
    ) : IRequest<Result<string>>;
 
internal sealed class UserUpdateProfileImageCommandHandler(
    IWebHostEnvironment webHostEnvironment,
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager
    ) : IRequestHandler<UserUpdateProfileImageCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserUpdateProfileImageCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(404,"User not found");

        var user = await userManager.FindByIdAsync(userId.Value.ToString());
        if (user is null)
            return Result<string>.Failure(404,"User not found");

        var allowedExtensions = new List<string>()
        {
             ".jpg", ".jpeg", ".png"
        };
        var extension = Path.GetExtension(request.file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
            return Result<string>.Failure("İnvalid file extension.");

        var wwwrootPath = webHostEnvironment.WebRootPath;
        if (!Directory.Exists(wwwrootPath))
            Directory.CreateDirectory(wwwrootPath);

        var uploadsRoot = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "profile_images", userId.Value.ToString());

        if (!Directory.Exists(uploadsRoot))
            Directory.CreateDirectory(uploadsRoot);

        using var image = Image.Load(request.file.OpenReadStream());

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(1200, 0)
        }));

        var webpEncoder = new WebpEncoder { Quality = 75 };

        var uniqueFileName = $"{Guid.CreateVersion7()}.webp";
        var filePath = Path.Combine(uploadsRoot, uniqueFileName);

        await image.SaveAsync(filePath, webpEncoder, cancellationToken);

        if (!string.IsNullOrWhiteSpace(user.AvatarUrl))
        {
            var oldRelative = user.AvatarUrl.TrimStart('/');
            var oldFullPath = Path.Combine(wwwrootPath, oldRelative.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(oldFullPath))
            {
                try
                {
                    File.Delete(oldFullPath);
                }
                catch
                {
                    // Log hata, ama işlemi engelleme
                }
            }
        }

        var relativePath = $"/uploads/profile_images/{userId.Value.ToString()}/{uniqueFileName}";

        user.setAvatar(relativePath);

        await userManager.UpdateAsync(user);

        return Result<string>.Succeed("Profile image updated successfully.");
    }
}
