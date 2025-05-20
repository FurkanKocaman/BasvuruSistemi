using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class EducationRepository : Repository<Education, ApplicationDbContext>, IEducationRepository
{
    public EducationRepository(ApplicationDbContext context) : base(context)
    {
    }
}
