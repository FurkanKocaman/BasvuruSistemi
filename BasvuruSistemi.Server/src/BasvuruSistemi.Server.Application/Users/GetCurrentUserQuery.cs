using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Addresses;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.Users;
using BasvuruSistemi.Server.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record GetCurrentUserQuery(
    ) : IRequest<GetCurrentUserQueryResponse?>;

public sealed class GetCurrentUserQueryResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => FirstName + " " + LastName;

    public string? AvatarUrl { get; set; }
    public string? Nationality { get; set; }
    public string? TCKN { get; set; }
    public ProfileStatus ProfileStatus { get;  set; }

    //public AddressDto Address { get; set; } = default!;
    public Contact Contact { get; set; } = default!;
}

internal sealed class GetCurrentUserQueryHandler(
    ICurrentUserService currentUserService,
    IAddressRepository addressRepository,
    UserManager<AppUser> userManager,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetCurrentUserQuery, GetCurrentUserQueryResponse?>
{
    public async Task<GetCurrentUserQueryResponse?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if (userId is null)
            return null;

        var user = await userManager.FindByIdAsync(userId!.Value.ToString());

        if(user is null)
            return null;

        var address = await addressRepository.Where(p => p.UserId == user.Id).FirstOrDefaultAsync();

        var context = httpContextAccessor.HttpContext?.Request;

        return new GetCurrentUserQueryResponse
        {
            Id = user!.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,

            AvatarUrl = $"{context?.Scheme}://{context?.Host}/{user.AvatarUrl}",

            Nationality = user.Nationality,
            TCKN = user.TCKN,
            ProfileStatus = user.ProfileStatus,

            //Address = new(address?.Id, address?.Country,address?.City,address?.District,address?.Street,address?.FullAddress,address?.PostalCode),
            Contact = user.Contact,
        };

    }
}
