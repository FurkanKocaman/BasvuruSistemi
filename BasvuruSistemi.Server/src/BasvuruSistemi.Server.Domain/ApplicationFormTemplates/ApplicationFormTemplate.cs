using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Employers;
using BasvuruSistemi.Server.Domain.FormFieldDefinitions;
using System.Reflection.Metadata;

namespace BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
public sealed class ApplicationFormTemplate : Entity
{
    public Guid EmployerId { get; private set; }
    public Employer Employer { get; private set; } = default!;

    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    public ICollection<FormFieldDefinition> FieldDefinitions { get; private set; } = new List<FormFieldDefinition>();
    private ApplicationFormTemplate() { }
    public ApplicationFormTemplate(Guid employerId, string name)
    {
        EmployerId = employerId;
        Name = name;
        //FieldDefinitions = new List<FormFieldDefinition>();
    }
}
