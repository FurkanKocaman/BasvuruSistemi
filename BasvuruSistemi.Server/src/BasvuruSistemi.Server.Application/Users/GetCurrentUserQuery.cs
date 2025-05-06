using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.Users;
using BasvuruSistemi.Server.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record GetCurrentUserQuery(
    ) : IRequest<GetCurrentUserQueryResponse?>;

public sealed class GetCurrentUserQueryResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => FirstName + " " + LastName;

    public string? Nationality { get; set; }
    public string? TCKN { get; set; }
    public ProfileStatus ProfileStatus { get;  set; }

    public Address Address { get; set; } = default!;
    public Contact Contact { get; set; } = default!;
}

internal sealed class GetCurrentUserQueryHandler(
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager
    ) : IRequestHandler<GetCurrentUserQuery, GetCurrentUserQueryResponse?>
{
    public async Task<GetCurrentUserQueryResponse?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if (userId is null)
            return null;
            //User is null

        var user = await userManager.FindByIdAsync(userId!.Value.ToString());

        if(user is null) { }

        return new GetCurrentUserQueryResponse
        {
            Id = user!.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,

            Nationality = user.Nationality,
            TCKN = user.TCKN,
            ProfileStatus = user.ProfileStatus,

            Address = user.Address,
            Contact = user.Contact,
        };

    }
}
