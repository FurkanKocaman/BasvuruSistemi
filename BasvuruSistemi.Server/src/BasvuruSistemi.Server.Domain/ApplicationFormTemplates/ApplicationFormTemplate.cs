using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.FormFieldDefinitions;
using BasvuruSistemi.Server.Domain.Tenants;

namespace BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
public sealed class ApplicationFormTemplate : Entity
{
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; } = default!; 

    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    public ICollection<FormFieldDefinition> FieldDefinitions { get; private set; } = new List<FormFieldDefinition>();
    private ApplicationFormTemplate() { }
    public ApplicationFormTemplate(Guid tenantId, string name, string? description)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
    }
}
