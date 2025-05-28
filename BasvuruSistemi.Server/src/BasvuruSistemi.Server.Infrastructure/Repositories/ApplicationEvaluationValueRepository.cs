using BasvuruSistemi.Server.Domain.ApplicationEvaluations;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class ApplicationEvaluationValueRepository : Repository<ApplicationEvaluationValue, ApplicationDbContext>, IApplicationEvaluationValueRepository
{
    public ApplicationEvaluationValueRepository(ApplicationDbContext context) : base(context)
    {
    }
}
