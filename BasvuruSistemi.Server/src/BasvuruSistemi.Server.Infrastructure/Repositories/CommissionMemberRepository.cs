using BasvuruSistemi.Server.Domain.Comissions;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class CommissionMemberRepository : Repository<CommissionMember, ApplicationDbContext>, ICommissionMemberRepository
{
    public CommissionMemberRepository(ApplicationDbContext context) : base(context)
    {
    }
}
