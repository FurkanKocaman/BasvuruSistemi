using BasvuruSistemi.Server.Domain.Tokens;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class InvitationTokenRepository : Repository<InvitationToken, ApplicationDbContext>, IInvitationTokenRepository
{
    public InvitationTokenRepository(ApplicationDbContext context) : base(context)
    {
    }
}
