using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class EvaluationFormFieldDefinitionRepository : Repository<EvaluationFormFieldDefinition, ApplicationDbContext>, IEvaluationFormFieldDefinitionRepository
{
    public EvaluationFormFieldDefinitionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
