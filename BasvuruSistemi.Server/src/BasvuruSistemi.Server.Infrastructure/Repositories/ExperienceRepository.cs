using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class ExperienceRepository : Repository<Experience, ApplicationDbContext>, IExperienceRepository
{
    public ExperienceRepository(ApplicationDbContext context) : base(context)
    {
    }
}
