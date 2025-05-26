using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class EvaluationStageRepository : Repository<EvaluationStage, ApplicationDbContext>, IEvaluationStageRepository
{
    public EvaluationStageRepository(ApplicationDbContext context) : base(context)
    {
    }
}

