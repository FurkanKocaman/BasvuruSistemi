using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class ApplicationRepository : Repository<Domain.Applications.Application, ApplicationDbContext>, IApplicationRepository
{
    public ApplicationRepository(ApplicationDbContext context) : base(context)
    {
    }
}
