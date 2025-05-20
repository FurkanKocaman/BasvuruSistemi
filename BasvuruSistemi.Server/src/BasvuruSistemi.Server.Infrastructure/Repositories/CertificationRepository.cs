using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class CertificationRepository : Repository<Certification, ApplicationDbContext>, ICertificationRepository
{
    public CertificationRepository(ApplicationDbContext context) : base(context)
    {
    }
}
