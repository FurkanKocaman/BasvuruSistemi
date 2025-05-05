using BasvuruSistemi.Server.Domain.Users;

namespace BasvuruSistemi.Server.Application.Services;

public interface IJwtProvider
{
    public Task<string> CreateTokenAsync(Appuser user, CancellationToken cancellationToken = default);
}
