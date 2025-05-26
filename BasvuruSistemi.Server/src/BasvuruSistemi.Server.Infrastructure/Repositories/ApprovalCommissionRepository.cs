using BasvuruSistemi.Server.Domain.Comissions;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class ApprovalCommissionRepository : Repository<ApprovalCommission, ApplicationDbContext>, IApprovalCommissionRepository
{
    public ApprovalCommissionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
