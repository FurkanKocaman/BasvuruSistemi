using BasvuruSistemi.Server.Application.Auth;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Users;
using BasvuruSistemi.Server.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record UserCreateCommand(
    string firstName,
    string lastName,
    string email,
    string password,
    string? nationality,
    string? tckn,
    DateTimeOffset birthOfDate,
    AddressDto address,
    Contact contact
    ) : IRequest<Result<LoginCommandResponse>>;

internal sealed class UserCreateCommandHandler(
    UserManager<AppUser> userManager,
    ISender sender 
    ) : IRequestHandler<UserCreateCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var isEmilExist = await userManager.FindByEmailAsync(request.email);

        if (isEmilExist is not null)
            return Result<LoginCommandResponse>.Failure("User with this email already exist");

        if(request.tckn is not null)
        {
            var isTcknExist = await userManager.Users.Where(p => p.TCKN == request.tckn).FirstOrDefaultAsync();
            if (isTcknExist is not null)
                return Result<LoginCommandResponse>.Failure("User with this TCKN is already exist");
        }

        AppUser user = new(request.firstName, request.lastName, new DateOnly(request.birthOfDate.Year, request.birthOfDate.Month, request.birthOfDate.Day), request.nationality, request.tckn, request.contact);

        user.EmailConfirmed = true;

        string baseUserName = $"{request.firstName}.{request.lastName}".ToLower().Replace(" ", "");
        string userName = baseUserName;
        int counter = 1;

        while (await userManager.FindByNameAsync(userName) != null)
        {
            userName = $"{baseUserName}{counter}";
            counter++;
        }
        user.UserName = userName;
        user.Email = request.email;

        IdentityResult result = await userManager.CreateAsync(user, request.password);

        await userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return Result<LoginCommandResponse>.Failure("User creation failed :" + result.Errors.ToArray()[0]);

        LoginCommand loginCommand = new(user.UserName, request.password);
        var res = await sender.Send(loginCommand);

        return res;
    }
}