using BasvuruSistemi.Server.Domain.FormFieldDefinitions;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class FormFieldDefinitionRepository : Repository<FormFieldDefinition, ApplicationDbContext>, IFormFieldDefinitionRepository
{
    public FormFieldDefinitionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
