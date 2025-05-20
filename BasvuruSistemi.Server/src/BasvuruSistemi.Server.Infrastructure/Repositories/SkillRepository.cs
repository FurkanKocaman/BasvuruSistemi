using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class SkillRepository : Repository<Skill, ApplicationDbContext>, ISkillRepository
{
    public SkillRepository(ApplicationDbContext context) : base(context)
    {
    }
}
