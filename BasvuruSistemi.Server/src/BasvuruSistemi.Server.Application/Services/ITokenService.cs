using TS.Result;

namespace BasvuruSistemi.Server.Application.Services;
public interface ITokenService
{
    Task<Result<string>> SendInvitationAsync(
                                           Guid inviteeId,
                                           Guid roleId,
                                           Guid? unitId,
                                           CancellationToken ct);
    Task<Result<string>> VerifyInvitationTokenAsync(string token, CancellationToken ct);
}
