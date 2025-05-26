using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class EvaluationFormRepository : Repository<EvaluationForm, ApplicationDbContext>, IEvaluationFormRepository
{
    public EvaluationFormRepository(ApplicationDbContext context) : base(context)
    {
    }
}

