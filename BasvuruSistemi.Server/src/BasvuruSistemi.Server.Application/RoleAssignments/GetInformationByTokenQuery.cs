using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Tokens;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.RoleAssignments;
public sealed record GetInformationByTokenQuery(
    string Token
    ) : IRequest<Result<GetInformationByTokenQueryResponse>>;

public sealed class GetInformationByTokenQueryResponse
{
    public string InviterName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string? UnitName { get; set; }
}

internal sealed class GetInformationByTokenQueryHandler(
    IInvitationTokenRepository invitationTokenRepository,
    ICurrentUserService currentUserService
    ) : IRequestHandler<GetInformationByTokenQuery, Result<GetInformationByTokenQueryResponse>>
{
    public async Task<Result<GetInformationByTokenQueryResponse>> Handle(GetInformationByTokenQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if(!userId.HasValue)
            return Result<GetInformationByTokenQueryResponse>.Failure(404, "User not found.");

        var token = await invitationTokenRepository.Where(p => p.Token == request.Token && !p.IsDeleted).Include(p => p.Inviter).Include(p => p.Role).Include(p => p.Unit).FirstOrDefaultAsync();
        if (token is null)
            return Result<GetInformationByTokenQueryResponse>.Failure(404, "Token not found.");

        if(token.IsUsed)
            return Result<GetInformationByTokenQueryResponse>.Failure(404, "Token already used.");

        if(token.ExpiresAt < DateTimeOffset.Now)
            return Result<GetInformationByTokenQueryResponse>.Failure(404, "Token expired.");

        if (token.InviteeId != userId)
            return Result<GetInformationByTokenQueryResponse>.Failure(404, "You are not the invitee.");

        var response = new GetInformationByTokenQueryResponse
        {
            InviterName = token.Inviter.FullName,
            RoleName = token.Role.Name ?? "",
            UnitName = token.Unit?.Name
        };

        return Result<GetInformationByTokenQueryResponse>.Succeed(response);
    }
}
