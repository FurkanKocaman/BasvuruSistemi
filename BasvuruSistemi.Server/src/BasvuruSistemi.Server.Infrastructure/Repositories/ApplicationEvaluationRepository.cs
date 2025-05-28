using BasvuruSistemi.Server.Domain.ApplicationEvaluations;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class ApplicationEvaluationRepository : Repository<ApplicationEvaluation, ApplicationDbContext>, IApplicationEvaluationRepository
{
    public ApplicationEvaluationRepository(ApplicationDbContext context) : base(context)
    {
    }
}
