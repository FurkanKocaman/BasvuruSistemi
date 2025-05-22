using BasvuruSistemi.Server.Application.Services;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.RoleAssignments;
public sealed record CreateRoleAssignmentCommand(
    Guid InviteeId,
    Guid RoleId,
    Guid? UnitId
    ) : IRequest<Result<string>>;

internal sealed class CreateRoleAssignmentCommandHandler(
    ITokenService tokenService
    ) : IRequestHandler<CreateRoleAssignmentCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateRoleAssignmentCommand request, CancellationToken cancellationToken)
    {
        var res = await tokenService.SendInvitationAsync(request.InviteeId, request.RoleId, request.UnitId,cancellationToken);

        return res;
    }
}
