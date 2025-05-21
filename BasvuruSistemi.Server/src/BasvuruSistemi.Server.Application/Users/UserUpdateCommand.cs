using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record UserUpdateCommand(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly? BirthOfDate,
    string? nationality,
    string Email,
    string? Phone,
    string? TCKN
    ) : IRequest<Result<string>>;

internal sealed class UserUpdateCommandHandler(
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager
    ) : IRequestHandler<UserUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(404,"User not found");

        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
            return Result<string>.Failure(404,"User not found");

        user.Update(request.FirstName, request.LastName, request.BirthOfDate, request.nationality, request.TCKN, new Domain.ValueObjects.Contact(request.Email, request.Phone));
        await userManager.UpdateAsync(user);
        return Result<string>.Succeed("User updated successfully");
    }
}
