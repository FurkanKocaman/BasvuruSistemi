using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        var uploadsRoot = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "profile_images", userId.Value.ToString());

        if (!Directory.Exists(uploadsRoot))
            Directory.CreateDirectory(uploadsRoot);

        var uniqueFileName = $"{Guid.CreateVersion7()}";
        var filePath = Path.Combine(uploadsRoot, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.file.CopyToAsync(stream, cancellationToken);
        }

        var relativePath = $"/uploads/profile_images/{userId.Value.ToString()}/{uniqueFileName}";

        user.setAvatar(relativePath);

        await userManager.UpdateAsync(user);

        return Result<string>.Succeed("Profile image updated successfully.");
    }
}
