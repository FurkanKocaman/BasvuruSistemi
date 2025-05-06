using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class FormTemplateRepository : Repository<ApplicationFormTemplate, ApplicationDbContext>, IFormTemplateRepository
{
    public FormTemplateRepository(ApplicationDbContext context) : base(context)
    {
    }
}
