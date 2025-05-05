using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.FormFieldDefinitions;
public sealed class FormFieldDefinition : Entity
{
    public Guid TemplateId { get; private set; }
    public string Label { get; private set; } = default!;
    public FieldTypeEnum Type { get; private set; }
    public bool IsRequired { get; private set; }
    public string? OptionsJson { get; private set; }

    public ApplicationFormTemplate Template { get; private set; } = default!;

    private FormFieldDefinition() { }
    public FormFieldDefinition(Guid templateId, string label, FieldTypeEnum type, bool isRequired, string? optionsJson = null)
    {
        TemplateId = templateId;
        Label = label;
        Type = type;
        IsRequired = isRequired;
        OptionsJson = optionsJson;
    }
}
