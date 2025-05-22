using BasvuruSistemi.Server.Application.Services;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.RoleAssignments;
public sealed record VerifyInvitationCommand(
    string Token
    ): IRequest<Result<string>>;

internal sealed class VerifyInvitationCommandhandler(
    ITokenService tokenService
    ) : IRequestHandler<VerifyInvitationCommand, Result<string>>
{
    public async Task<Result<string>> Handle(VerifyInvitationCommand request, CancellationToken cancellationToken)
    {
        var res = await tokenService.VerifyInvitationTokenAsync(request.Token, cancellationToken);
        return res;
    }
}
