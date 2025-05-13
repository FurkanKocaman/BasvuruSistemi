using BasvuruSistemi.Server.Domain.ApplicationFieldValues;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class ApplicationFieldValueRepository : Repository<ApplicationFieldValue, ApplicationDbContext>, IApplicationFieldValueRepository
{
    public ApplicationFieldValueRepository(ApplicationDbContext context) : base(context)
    {
    }
}
