using BasvuruSistemi.Server.Domain.Employers;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class EmployerRepository : Repository<Employer, ApplicationDbContext>, IEmployerRepository
{
    public EmployerRepository(ApplicationDbContext context) : base(context)
    {
    }
}
